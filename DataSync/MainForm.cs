using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using System.IO;
using DataSync.util;
using System.Collections;
using Newtonsoft.Json;
using Oracle.ManagedDataAccess.Client;
using MySql.Data.MySqlClient;
using System.Threading;
using System.Configuration;
using System.Threading.Tasks;
/// <summary>
/// .net 4依赖环境
///  mysql.data F:\vs_workspace\DataSync\packages\MySql.Data.6.8.8\lib\net40\Mysql.Data.dll
///  Newtonsoft.Json 8.0.0.0  F:\vs_workspace\DataSync\packages\Newtonsoft.Json.8.0.3\lib\net40\Newtonsoft.Json.dll
///  log4net  2.0.7.0 F:\vs_workspace\DataSync\packages\log4net.2.0.7\lib\net40-full\log4net.dll
///  
/// .net 4.5依赖环境
/// mysql.data 8.0.12
/// Newtonsoft.Json 11.0.0.0  F:\vs_workspace\DataSync\packages\Newtonsoft.Json.11.0.2\lib\net45\Newtonsoft.Json.dll
/// log4net log4net F:\vs_workspace\DataSync\packages\log4net.2.0.8\lib\net45-full\log4net.dll
/// </summary>
namespace DataSync
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }
        log4net.ILog loginfo = log4net.LogManager.GetLogger(typeof(MainForm));

        /// <summary>
        /// 数据库连接配置
        /// </summary>
        List<DataBaseModel> _srcDataBaseModels = new List<DataBaseModel>();

        /// <summary>
        /// 选中的原数据库
        /// </summary>
        DataBaseModel _srcSelectDatabase = new DataBaseModel();

        /// <summary>
        /// 选中的目标数据库
        /// </summary>
        DataBaseModel _dstSelectDatabase = new DataBaseModel();

        /// <summary>
        /// 选中的表的字段名（交集）
        /// </summary>
        List<string> selectTabColumnNames = new List<string>();

        /// <summary>
        /// 存储源数据库所有的表
        /// </summary>
        List<string> dsSrcTab = new List<string>();

        /// <summary>
        /// 存储目标数据库所有的表
        /// </summary>
        List<string> dsDstTab = new List<string>();

        /// <summary>
        /// 原始库和目标库表的交集
        /// </summary>
        List<string> intersectTab = new List<string>();



        /// <summary>
        /// 同步的表名
        /// </summary>
        string _syncTabName = "";

        /// <summary>
        /// 需要更新的数量
        /// </summary>
        int _syncUpdateNum = 0;

        /// <summary>
        /// 需要新增数量
        /// </summary>
        int _sysnInsertNum = 0;

        /// <summary>
        /// 需要删除的数量
        /// </summary>
        int _syncDeleteNum = 0;

        /// <summary>
        /// 更新的SQL语句集合
        /// </summary>
        List<string> _updateSqlList = new List<string>();

        /// <summary>
        /// 更新语句的参数值集合  Oracle
        /// </summary>
        List<OracleParameter[]> _oracleParametersUpdateList = new List<OracleParameter[]>();

        /// <summary>
        /// 更新语句的参数值集合  Mysql
        /// </summary>
        List<MySqlParameter[]> _mysqlParametersUpdateList = new List<MySqlParameter[]>();

        /// <summary>
        /// 新增的SQL语句集合
        /// </summary>
        List<string> _insertSqlList = new List<string>();

        /// <summary>
        /// 新增语句的参数值集合 Oracle
        /// </summary>
        List<OracleParameter[]> _oracleParametersInsertList = new List<OracleParameter[]>();

        /// <summary>
        /// 新增语句的参数值集合 Mysql
        /// </summary>
        List<MySqlParameter[]> _mysqlParametersInsertList = new List<MySqlParameter[]>();

        ///// <summary>
        ///// 删除的SQL语句集合
        ///// </summary>
        //List<string> _deleteSqlList = new List<string>();


        /// <summary>
        /// 删除的SQL语句与参数队列
        /// </summary>
        Queue<ExcuteSqlObj> _deleteSqlQueue = new Queue<ExcuteSqlObj>();

        ///// <summary>
        ///// 删除语句的参数值集合 Oracle
        ///// </summary>
        //List<OracleParameter[]> _oracleParametersDeleteList = new List<OracleParameter[]>();

        ///// <summary>
        ///// 删除语句的参数值集合 Mysql
        ///// </summary>
        //List<MySqlParameter[]> _mysqlParametersDeleteList = new List<MySqlParameter[]>();


        /// <summary>
        /// 删除语句的参数值集合 Oracle
        /// </summary>
        Queue<OracleParameter[]> _oracleParametersDeleteQueue = new Queue<OracleParameter[]>();

        /// <summary>
        /// 删除语句的参数值集合 Mysql
        /// </summary>
        Queue<MySqlParameter[]> _mysqlParametersDeleteQueue = new Queue<MySqlParameter[]>();

        /// <summary>
        /// 停止状态
        /// </summary>
        bool isStop = false;

        /// <summary>
        /// 点击交换
        /// </summary>
        bool isChange = false;

        /// <summary>
        /// 选中的原数据库（MYSQL）连接
        /// </summary>
        MySqlConnection connSrcMysql = null;

        /// <summary>
        /// 选中的目标数据库（MYSQL）连接
        /// </summary>
        MySqlConnection connDstMysql = null;

        /// <summary>
        /// 选中的原数据库（ORACLE）连接
        /// </summary>
        OracleConnection connSrcOracle = null;

        /// <summary>
        /// 选中的目标数据库（ORACLE）连接
        /// </summary>
        OracleConnection connDstOracle = null;

        /// <summary>
        /// 新增数据的集合
        /// </summary>
        DataTable _insertDataTable = new DataTable();

        /// <summary>
        /// 查询目标库ORACLE的语句
        /// </summary>
        string oracleSelectSql = "";

        /// <summary>
        /// 查询目标库MYSQL的语句
        /// </summary>
        string mysqlSelectSql = "";
        
        /// <summary>
        /// 可选同步的字段
        /// </summary>
        List<string> fourFields = new List<string>() { "USEC", "CDAT" , "USEU", "LSTU" };


        int isSuccessDel = 0;//删除成功的数量
        int isFailDel = 0;//删除失败的数量

        int isSuccessUpdate = 0;//更新成功的数量
        int isFailUpdate = 0;//更新失败的数量

        int isSuccessInsert = 0;//新增成功的数量
        int isFailInsert = 0;//新增失败的数量


        /// <summary>
        /// 程序运行从xml文件加载数据库连接配置
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainForm_Load(object sender, EventArgs e)
        {
            try
            {
                //加载数据库连接配置
                string srcDataBasepath = Application.StartupPath + @"\xml\srcDataBase.xml";//数据库连接配置
                XmlDocument xmlDocSrc = new XmlDocument();//声明读取xml的对象
                xmlDocSrc.Load(srcDataBasepath);//获取xml文件
                XmlNode xmlNodeSrc = xmlDocSrc.SelectSingleNode("Connections");//获取根节点
                foreach (XmlNode xmlElement in xmlNodeSrc.ChildNodes)
                {
                    DataBaseModel model = new DataBaseModel();
                    XmlElement item = (XmlElement)xmlElement;
                    model.key = item.GetAttribute("key");
                    model.conntext = item.GetAttribute("value");
                    string dataBaseName = item.GetAttribute("dataBaseName");
                    model.dataBaseName = dataBaseName;
                    string type = item.GetAttribute("type");
                    if (type.ToLower().Equals("mysql"))
                    {
                        model.type = DataBaseTypeEnum.Mysql;
                    }
                    else if (type.ToLower().Equals("oracle"))
                    {
                        model.type = DataBaseTypeEnum.Oracle;
                    }
                    _srcDataBaseModels.Add(model);
                    srcDataBaseCbo.Items.Add(model.key);
                    dstDataBaseCbo.Items.Add(model.key);
                }
            }
            catch (Exception e1)
            {
                MessageBox.Show(e1.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }




        }

        /// <summary>
        /// 选中原数据库连接后，加载所有表
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SrcDataBaseCbo_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                ComboBox cbox = (ComboBox)sender;
                if (cbox.SelectedItem == null)
                {
                    return;
                }
                gbMainOfOnToOne.Enabled = false;//禁用主界面控件，为了等待数据库连接完成
                if (isChange)//点击的是交换,只要将选中的原数据库进行重新加载，其他的数据都不用修改
                {
                    string key = srcDataBaseCbo.SelectedItem.ToString();
                    DataBaseModel model = _srcDataBaseModels.First(item => item.key.Equals(key));
                    _srcSelectDatabase = model;// 选中的原数据库
                    //获取原数据库连接
                    if (model.type == DataBaseTypeEnum.Oracle)
                    {
                        //首先关闭原有的连接
                        OracleConn.CloseOracleConnection(connSrcOracle);
                        //打开新连接
                        connSrcOracle = OracleConn.GetOracleConnection(model.conntext);
                    }
                    else if (model.type == DataBaseTypeEnum.Mysql)
                    {
                        //首先关闭原有的连接
                        MySqlConn.CloseMySqlConnection(connSrcMysql);
                        //打开新连接
                        connSrcMysql = MySqlConn.GetMySqlConnection(model.conntext);
                    }
                    else
                    {
                        loginfo.Info("不支持此数据库的同步：" + model.type);
                    }

                    List<string> srcDataTabs = GetTabsByDataBase(model, connSrcOracle, connSrcMysql);
                    dsSrcTab = srcDataTabs;//原数据库中所有的表
                    syncTabCbo.Enabled = true;
                }
                else
                {
                    if (SelectSame())
                    {
                        cbox.SelectedItem = null;
                        //将同步的表和字段也置空
                        syncTabCbo.Items.Clear();
                        ckbFileds.Items.Clear();
                        gbMainOfOnToOne.Enabled = true;//启用主界面控件
                        return;
                    }

                    //先清空原有的表数据
                    syncTabCbo.Items.Clear();
                    //清空两个库中表的交集
                    intersectTab.Clear();
                    string key = srcDataBaseCbo.SelectedItem.ToString();
                    DataBaseModel model = _srcDataBaseModels.First(item => item.key.Equals(key));
                    _srcSelectDatabase = model;// 选中的原数据库
                    //获取原数据库连接
                    if (model.type == DataBaseTypeEnum.Oracle)
                    {
                        //首先关闭原有的连接
                        OracleConn.CloseOracleConnection(connSrcOracle);
                        //打开新连接
                        connSrcOracle = OracleConn.GetOracleConnection(model.conntext);
                    }
                    else if (model.type == DataBaseTypeEnum.Mysql)
                    {
                        //首先关闭原有的连接
                        MySqlConn.CloseMySqlConnection(connSrcMysql);
                        //打开新连接
                        connSrcMysql = MySqlConn.GetMySqlConnection(model.conntext);
                    }
                    else
                    {
                        loginfo.Info("不支持此数据库的同步：" + model.type);
                    }
                    List<string> srcDataTabs = GetTabsByDataBase(model, connSrcOracle, connSrcMysql);
                    dsSrcTab = srcDataTabs;//原数据库中所有的表


                    //判断目标库是否已经选中，若已经选中，则计算两个集合的交集
                    if (null == dsDstTab || dsDstTab.Count == 0)
                    {
                        syncTabCbo.Items.AddRange(srcDataTabs.ToArray().OrderBy(item => item).ToArray());
                        loginfo.Info("目标数据库还没有选中");
                    }
                    else
                    {

                        //求两个集合的交集
                        intersectTab = dsSrcTab.Intersect(dsDstTab, new myEqualityComparer()).ToList();
                        if (null == intersectTab || intersectTab.Count == 0)
                        {
                            loginfo.Info("原数据库和目标数据库没有共同的表");
                            MessageBox.Show("原数据库和目标数据库没有共同的表");
                        }
                        else
                        {
                            syncTabCbo.Items.AddRange(intersectTab.ToArray().OrderBy(item => item).ToArray());
                            syncTabCbo.Enabled = true;
                        }



                    }
                    ckbFileds.Items.Clear();



                }

            }
            catch (Exception e1)
            {
                MessageBox.Show(e1.Message, "程序异常", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            isChange = false;
            gbMainOfOnToOne.Enabled = true;//启用主界面控件
        }

        /// <summary>
        /// 检查选择的原数据库和目标数据库是否一致
        /// </summary>
        /// <returns></returns>
        private bool SelectSame()
        {
            if (srcDataBaseCbo.SelectedItem == null || dstDataBaseCbo.SelectedItem == null)
            {
                return false;
            }
               
            if (srcDataBaseCbo.SelectedItem.ToString().Equals(dstDataBaseCbo.SelectedItem.ToString()))
            {
                MessageBox.Show("请不要选择同一个数据库");
                cboFourField.Checked = false;
                return true;
            }
            return false;
        }

     

        /// <summary>
        /// 根据数据库连接获取所有的表名
        /// </summary>
        /// <param name="model"></param>
        /// <param name="oracleConnection"></param>
        /// <param name="mySqlConnection"></param>
        /// <returns></returns>
        private List<string> GetTabsByDataBase(DataBaseModel model, OracleConnection oracleConnection, MySqlConnection mySqlConnection)
        {
            List<string> res = new List<string>();
            try
            {
                //对两种数据库类型做不同的查所有表处理
                if (model.type == DataBaseTypeEnum.Mysql)
                {
                    //加载数据库中所有的表   and table_type='base table'
                    //使用已经有的连接
                    DataSet ds = MySqlConn.GetDataSet(mySqlConnection, model.conntext, CommandType.Text, "select table_name from information_schema.tables where table_schema='" + model.dataBaseName + "'", null);
                    if (null == ds || ds.Tables.Count == 0)
                    {
                        loginfo.Info("选中的数据库中没有表");
                        MessageBox.Show("原数据库中没有表");
                    }
                    else
                    {
                        res = ds.Tables[0].AsEnumerable().Select(row => row["table_name"].ToString().ToUpper()).ToList();

                    }

                }
                else if (model.type == DataBaseTypeEnum.Oracle)
                {
                    //加载数据库中所有的表
                    DataSet ds = OracleConn.Query(oracleConnection, model.conntext, "select table_name from user_tables");
                    if (null == ds || ds.Tables.Count == 0)
                    {
                        loginfo.Info("选中的数据库中没有表");
                        MessageBox.Show("原数据库中没有表");
                    }
                    else
                    {

                        res = ds.Tables[0].AsEnumerable().Select(row => row["table_name"].ToString().ToUpper()).ToList();

                    }
                }
                else
                {
                    loginfo.Info("不支持此类数据库同步：" + model.type);
                    MessageBox.Show("不支持此类数据库同步:" + model.type);

                }
            }
            catch (OracleException ex)
            {
                throw new Exception(ex.Message);
            }


            return res;
        }

        /// <summary>
        /// 当选中表后，在下方的显示框中加载所有的字段名
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SyncTabCbo_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                //将开始和停止按钮禁用
                btnStart.Enabled = false;
                btnStop.Enabled = false;

                if (syncTabCbo.SelectedItem == null)
                    return;

                //txWhere.Text = "where";//清空where条件
                ckbFileds.Items.Clear();//清空选中的字段

                string selectTabName = syncTabCbo.SelectedItem.ToString();//选中的表名

                if (null == selectTabName || selectTabName.Length == 0)
                {
                    MessageBox.Show("请选择要同步的表");
                    return;
                }
                _syncTabName = selectTabName;
                //根据数据库类型来进行查表的字段
                List<string> srcSelectTabColumnNames = new List<string>();//同步的原始表所有字段
                List<string> dstSelectTabColumnNames = new List<string>();//同步的目标表所有字段

                srcSelectTabColumnNames = GetTabColumnNamesByTabName(_srcSelectDatabase, selectTabName, connSrcOracle, connSrcMysql);
                dstSelectTabColumnNames = GetTabColumnNamesByTabName(_dstSelectDatabase, selectTabName, connDstOracle, connDstMysql);

                //计算两个列集合的交集
                if (srcSelectTabColumnNames.Count == 0 && dstSelectTabColumnNames.Count == 0)
                {
                    loginfo.Info("两个表都没有列");
                    return;
                }
                else
                {

                    selectTabColumnNames = srcSelectTabColumnNames.Intersect(dstSelectTabColumnNames, new myEqualityComparer()).ToList();
                    loginfo.Info("两个表指定的交集：" + JsonConvert.SerializeObject(selectTabColumnNames));
                    //检查是否有ID列
                    if (!selectTabColumnNames.Contains("ID") && !selectTabColumnNames.Contains("id"))
                    {
                        MessageBox.Show("该表【" + selectTabName + "】不支持同步，原因是没有ID列");
                        syncTabCbo.SelectedIndex = -1;
                        return;
                    }
                    ckbFileds.Items.Clear();
                    ckbFileds.Items.AddRange(selectTabColumnNames.ToArray());
                    //选中所有字段
                    for (int i = 0; i < ckbFileds.Items.Count; i++)
                    {
                        ckbFileds.SetItemCheckState(i, CheckState.Checked);
                    }

                    //20181026 如果上次选中了“不选USEC、CDAT、USEU、LSTU”，则这里也做下不选处理
                    if (cboFourField.Checked)
                    {
                        bool haveCancelField = false;//要取消的字段
                        for (int j = 0; j < ckbFileds.Items.Count; j++)
                        {
                            string field = ckbFileds.Items[j].ToString();
                            if (fourFields.Contains(field))
                            {
                                //取消选中
                                ckbFileds.SetItemChecked(j, false);
                                haveCancelField = true;
                            }

                        }

                        //将全选按钮取消，但是原来选中的不能取消
                        if (haveCancelField)
                        {
                            cboAllField.Checked = false;//将全选勾去掉
                                                        //选择非 fourFields 的字段
                            for (int j = 0; j < ckbFileds.Items.Count; j++)
                            {
                                string field = ckbFileds.Items[j].ToString();
                                if (!fourFields.Contains(field))
                                {
                                    //选中
                                    ckbFileds.SetItemChecked(j, true);
                                }
                            }
                        }
                    }


                }

            }
            catch (Exception e1)
            {
                MessageBox.Show(e1.Message, "程序异常", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }


        }

        /// <summary>
        /// 根据表名获取表的所有字段
        /// </summary>
        /// <param name="srcSelectTabColumnNames"></param>
        /// <param name="selectTabName"></param>
        /// <returns></returns>
        private List<string> GetTabColumnNamesByTabName(DataBaseModel selectDatabase, string selectTabName, OracleConnection oracleConnection, MySqlConnection mySqlConnection)
        {
            List<string> srcSelectTabColumnNames = new List<string>();//表的所有列
            try
            {
                //原数据库的类型
                if (selectDatabase.type == DataBaseTypeEnum.Oracle)
                {
                    //oracle 根据表名查所有字段 ：select column_name from user_col_comments  where table_name = 'TABLE_NAME'    
                    DataSet ds = OracleConn.Query(oracleConnection, selectDatabase.conntext, "select column_name from user_col_comments  where table_name = '" + selectTabName.ToUpper() + "'");
                    if (null == ds || ds.Tables.Count == 0)
                    {
                        loginfo.Info("选中的表中没有列");
                        MessageBox.Show("选中的表中没有列");
                    }
                    else
                    {
                        srcSelectTabColumnNames = ds.Tables[0].AsEnumerable().Select(row => row["column_name"].ToString()).ToList();
                    }

                }
                else if (selectDatabase.type == DataBaseTypeEnum.Mysql)
                {
                    string srcDataBaseName = selectDatabase.dataBaseName;//选中mysql数据库名
                                                                         //mysql 根据表名查所有字段 ：select COLUMN_NAME from information_schema.COLUMNS where table_name = 'TABLE_NAME' and table_schema = 'SID'
                    DataSet ds = MySqlConn.GetDataSet(mySqlConnection, selectDatabase.conntext, CommandType.Text, "select COLUMN_NAME from information_schema.COLUMNS where table_name = '" + selectTabName + "' and table_schema = '" + srcDataBaseName + "'", null);
                    if (null == ds || ds.Tables.Count == 0)
                    {
                        loginfo.Info("选中的表中没有列");
                        MessageBox.Show("选中的表中没有列");
                    }
                    else
                    {
                        srcSelectTabColumnNames = ds.Tables[0].AsEnumerable().Select(row => row["column_name"].ToString()).ToList();
                    }
                }

                loginfo.Info("选中表的所有列：" + JsonConvert.SerializeObject(srcSelectTabColumnNames));
            }
            catch (OracleException ex)
            {
                throw new Exception(ex.Message);
            }



            return srcSelectTabColumnNames;
        }

        /// <summary>
        /// 选中目标数据库后，计算目标和原始库都有的表
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DstDataBaseCbo_SelectedIndexChanged(object sender, EventArgs e)
        {

            try
            {
                ComboBox cbox = (ComboBox)sender;
                if (cbox.SelectedItem == null)
                {
                    return;
                }
                gbMainOfOnToOne.Enabled = false;//等待数据库连接成功
                if (isChange)//点击交换数据源，只要将目标数据库的数据进行加载，其他数据项都不变
                {
                    string key = dstDataBaseCbo.SelectedItem.ToString();
                    DataBaseModel model = _srcDataBaseModels.First(item => item.key.Equals(key));
                    //选中的目标库
                    _dstSelectDatabase = model;
                    //获取原数据库连接
                    if (model.type == DataBaseTypeEnum.Oracle)
                    {
                        //首先关闭原有的连接
                        OracleConn.CloseOracleConnection(connDstOracle);
                        //打开新连接
                        connDstOracle = OracleConn.GetOracleConnection(model.conntext);
                    }
                    else if (model.type == DataBaseTypeEnum.Mysql)
                    {
                        //首先关闭原有的连接
                        MySqlConn.CloseMySqlConnection(connDstMysql);
                        //打开新连接
                        connDstMysql = MySqlConn.GetMySqlConnection(model.conntext);
                    }
                    else
                    {
                        loginfo.Info("不支持此数据库的同步：" + model.type);
                    }


                    List<string> dstDataTabs = GetTabsByDataBase(model, connDstOracle, connDstMysql);
                    //目标库中所有的表
                    dsDstTab = dstDataTabs;
                    syncTabCbo.Enabled = true;
                }
                else
                {
                    if (SelectSame())
                    {
                        cbox.SelectedItem = null;
                        //将同步的表和字段也置空
                        syncTabCbo.Items.Clear();
                        syncTabCbo.SelectedItem = null;
                        ckbFileds.Items.Clear();
                        gbMainOfOnToOne.Enabled = true;//启用主界面控件
                        return;
                    }

                    //先清空原有的表数据
                    syncTabCbo.Items.Clear();
                    //清空两个库中表的交集
                    intersectTab.Clear();
                    string key = dstDataBaseCbo.SelectedItem.ToString();
                    DataBaseModel model = _srcDataBaseModels.First(item => item.key.Equals(key));
                    //选中的目标库
                    _dstSelectDatabase = model;
                    //获取原数据库连接
                    if (model.type == DataBaseTypeEnum.Oracle)
                    {
                        //首先关闭原有的连接
                        OracleConn.CloseOracleConnection(connDstOracle);
                        //打开新连接
                        connDstOracle = OracleConn.GetOracleConnection(model.conntext);
                    }
                    else if (model.type == DataBaseTypeEnum.Mysql)
                    {
                        //首先关闭原有的连接
                        MySqlConn.CloseMySqlConnection(connDstMysql);
                        //打开新连接
                        connDstMysql = MySqlConn.GetMySqlConnection(model.conntext);
                    }
                    else
                    {
                        loginfo.Info("不支持此数据库的同步：" + model.type);
                    }
                    List<string> dstDataTabs = GetTabsByDataBase(model, connDstOracle, connDstMysql);
                    //目标库中所有的表
                    dsDstTab = dstDataTabs;


                    //判断目标库是否已经选中，若已经选中，则计算两个集合的交集
                    if (null == dsSrcTab || dsSrcTab.Count == 0)
                    {
                        loginfo.Info("原始数据库还没有选中");
                        syncTabCbo.Items.AddRange(dstDataTabs.ToArray().OrderBy(item => item).ToArray());

                    }
                    else
                    {

                        //求两个集合的交集
                        intersectTab = dsSrcTab.Intersect(dsDstTab, new myEqualityComparer()).ToList();
                        if (null == intersectTab || intersectTab.Count == 0)
                        {
                            loginfo.Info("原数据库和目标数据库没有共同的表");
                            MessageBox.Show("原数据库和目标数据库没有共同的表");
                        }
                        else
                        {
                            ckbFileds.Items.Clear();
                            syncTabCbo.Items.AddRange(intersectTab.ToArray().OrderBy(item => item).ToArray());
                            //20181026 将上次选中的要同步的表保留，用于用户在只是更改目标库而不是同步的表时不用再重新选择表
                            if (!"".Equals(_syncTabName))
                            {
                                //有上次选中的表
                                if (syncTabCbo.Items.Contains(_syncTabName))
                                {
                                    //使用上次选中的表
                                    int index = this.syncTabCbo.FindString(_syncTabName);
                                    syncTabCbo.SelectedIndex = index;
                                }
                            }
                            syncTabCbo.Enabled = true;
                        }
 
                    }
                   
                }


            }
            catch (Exception e1)
            {
                MessageBox.Show(e1.Message, "程序异常", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            isChange = false;
            gbMainOfOnToOne.Enabled = true;//启用主界面控件

        }


        /// <summary>
        /// 对比两个库中表的数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnCompare_Click(object sender, EventArgs e)
        {


            //增删改成功数目归零
            isSuccessDel = 0;
            isSuccessUpdate = 0;
            isSuccessInsert = 0;

            //增删改失败数目归零
            isFailDel = 0;
            isFailUpdate = 0;
            isFailInsert = 0;

            gbMainOfOnToOne.Enabled = false;//禁用主界面控件
            btnStart.Enabled = false;//开始按钮禁用
            btnStop.Enabled = true;//停止按钮启用
            try
            {
                isStop = true;//正常运行

                //清空 需要更新的数量
                _syncUpdateNum = 0;

                //清空 需要新增数量
                _sysnInsertNum = 0;

                //清空 需要删除的数量
                _syncDeleteNum = 0;

                //清空 更新的SQL语句集合
                _updateSqlList = new List<string>();

                //清空 新增的SQL语句集合
                _insertSqlList = new List<string>();

                //清空 删除的SQL语句集合
                //_deleteSqlList = new List<string>();
                _deleteSqlQueue= new Queue<ExcuteSqlObj>();


                _oracleParametersUpdateList.Clear();
                _oracleParametersDeleteQueue.Clear();
                _oracleParametersInsertList.Clear();

                _mysqlParametersUpdateList.Clear();
                _mysqlParametersDeleteQueue.Clear();
                _mysqlParametersInsertList.Clear();

                //清除失败的数量
                lblDelFail.Text = "";
                lblInsertFail.Text = "";
                lblUpdateFail.Text = "";

                //清除历史记录
                ckbDelete.Text = "";
                ckbUpdate.Text = "";
                ckbInsert.Text = "";
 
                //检查要同步的表是否已经选中
                if (syncTabCbo.SelectedIndex == -1)
                {
                    MessageBox.Show("请选中同步的表");
                    gbMainOfOnToOne.Enabled = true;
                    return;
                }

                //要同步表选中的同步字段
                List<string> selectFieldNamesList = SelectFieldNames();
                if (selectFieldNamesList.Count == 0)
                {
                    MessageBox.Show("请选中要同步的字段");
                    gbMainOfOnToOne.Enabled = true;
                    return;
                }
                else
                {
                    //同步的列一定要有ID，下面是根据id来做查询比较的
                    if (!selectFieldNamesList.Contains("ID"))
                    {
                        MessageBox.Show("请选中同步字段的ID列");
                        gbMainOfOnToOne.Enabled = true;
                        btnStop.Enabled = false;
                        return;
                    }
                    if (selectFieldNamesList.Count == 1 && selectFieldNamesList[0] == "ID")
                    {
                        MessageBox.Show("不能只选ID列");
                        btnStop.Enabled = false;
                        gbMainOfOnToOne.Enabled = true;
                        return;
                    }

                    loginfo.Info("要同步的字段有:" + JsonConvert.SerializeObject(selectFieldNamesList));

                    //执行对比的耗时操作交给子线程去做
                    ThreadPool.QueueUserWorkItem(obj =>
                    {

                        try
                        {
                            //查原数据的数据量  
                            int countSrc = GetCountQueryResult(selectFieldNamesList, _srcSelectDatabase, connSrcOracle, connSrcMysql);
                            //若原始数据大于XX条，则不进行对比
                            string sqlCountSrcStr = ConfigurationManager.AppSettings["sqlcount"];
                            int sqlCountSrc = Convert.ToInt32(sqlCountSrcStr);
                            if (countSrc > sqlCountSrc)
                            {
                                MessageBox.Show("原库数据量 " + countSrc + " 大于 " + sqlCountSrc + " 条，不进行对比");
                                this.Invoke(new Action(() =>
                                {
                                    gbMainOfOnToOne.Enabled = true;
                                    btnStop.Enabled = false;
                                }));
                                return;

                            }
                            //查目标数据的数据量  
                            int countDst = GetCountQueryResult(selectFieldNamesList, _dstSelectDatabase, connDstOracle, connDstMysql);
                            //若原始数据大于XX条，则不进行对比
                            string sqlCountDstStr = ConfigurationManager.AppSettings["sqlcount"];
                            int sqlCountDst = Convert.ToInt32(sqlCountDstStr);
                            if (countDst > sqlCountDst)
                            {
                                MessageBox.Show("目标库数据量 " + countDst + " 大于 " + sqlCountDst + " 条，不进行对比");
                                this.Invoke(new Action(() =>
                                {
                                    gbMainOfOnToOne.Enabled = true;
                                    btnStop.Enabled = false;

                                }));
                                return;
                            }

                            //原数据库数据
                            DataSet dsSrcBaseData = GetQueryResult(selectFieldNamesList, _srcSelectDatabase, connSrcOracle, connSrcMysql);

                            //目标数据库数据
                            DataSet dsDstBaseData = GetQueryResult(selectFieldNamesList, _dstSelectDatabase, connDstOracle, connDstMysql);


                            //对比数据 新增 修改 删除
                            //根据从原数据库查出来的数据的id对目标数据字段进行一一对比
                            DataRowCollection dsSrcDataRows = dsSrcBaseData.Tables[0].Rows;//源表

                            DataRowCollection dsDstDataRows = dsDstBaseData.Tables[0].Rows;//目标表

                          

                            for (int i = dsSrcDataRows.Count - 1; i >= 0; i--)
                            {

                                DataRow srcRow = dsSrcDataRows[i];
                                string dsSrcId = srcRow["ID"].ToString();//ID为比较的基准
                                for (int j = dsDstDataRows.Count - 1; j >= 0; j--)
                                {

                                    if (!isStop)
                                    {
                                        this.Invoke(new Action(() =>
                                        {
                                            MessageBox.Show("任务已停止");
                                            gbMainOfOnToOne.Enabled = true;
                                            btnStop.Enabled = false;
                                        }));
                                        return;
                                    }

                                    DataRow dstRow = dsDstDataRows[j];
                                    string dsDstId = dstRow["ID"].ToString();
                                    //更新
                                    if (dsSrcId.Equals(dsDstId))
                                    {
                                        //比较两条字段的值是否一样
                                        bool isNeedUpdate = ColumnValueCompare(srcRow, dstRow, selectFieldNamesList);
                                        if (isNeedUpdate)//说明此条数据要更新
                                        {
                                            //将更新的数量加 1
                                            _syncUpdateNum += 1;
                                            //拼接好更新的SQL
                                            string updateSql = GetExcuteUpdateSql(srcRow, selectFieldNamesList, _syncTabName, _dstSelectDatabase);
                                            //将拼接好的SQL存储起来
                                            _updateSqlList.Add(updateSql);

                                        }
                                        //将该条数据从目标库移除
                                        dsDstDataRows.Remove(dstRow);
                                        srcRow = null;//原始数据已经处理过
                                        break;
                                    }
                                }


                                if (null != srcRow)//说明该条数据没有在目标库中找到，需要新增
                                {
                                    //将新增的数量加 1
                                    _sysnInsertNum += 1;
                                    //拼接新增的SQL
                                    string insertSql = GetExcuteInsertSql(srcRow, selectFieldNamesList, _syncTabName, _dstSelectDatabase);
                                    //将新增的SQL放入集合
                                    _insertSqlList.Add(insertSql);

                                    //考虑到新增的速度太慢，这里采用datatable直接入库的方式
                                    //经过测试，采用datatable入库的时间比逐条入库的时间慢，所以还是用原有的逐条进行入库
                                    //DataRow row = _insertDataTable.NewRow(); //创建一个行
                                    //row.ItemArray = srcRow.ItemArray;
                                    //_insertDataTable.Rows.Add(row);


                                }



                            }

                            //全部循环完成后，看目标库集合还有多少条记录，说明这些记录都是要被删除的
                            if (dsDstDataRows.Count > 0)
                            {
                                //拼接需要删除的SQL
                                _syncDeleteNum = dsDstDataRows.Count;
                                //_deleteSqlList = GetExcuteDeleteSql(dsDstDataRows, _syncTabName, _dstSelectDatabase);
                                _deleteSqlQueue = GetExcuteDeleteSql(dsDstDataRows, _syncTabName, _dstSelectDatabase);
                            }
                        }
                        catch (Exception e1)
                        {

                            this.Invoke(new Action(() =>
                            {
                                gbMainOfOnToOne.Enabled = true;
                                btnStop.Enabled = false;
                                MessageBox.Show(e1.Message, "程序异常", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }));
                            return;

                        }
                        //子线程做完任务后，将要更改界面上控件值的交给主线程去做
                        this.Invoke(new Action(() => {

                            loginfo.Info("本次数据对比结果：");
                            loginfo.Info("需要新增的数量：" + _sysnInsertNum);
                            loginfo.Info("需要更新的数量：" + _syncUpdateNum);
                            loginfo.Info("需要删除的数量：" + _syncDeleteNum);
                            ckbInsert.Text = string.Format("新增 0/{0}", _sysnInsertNum);
                            ckbUpdate.Text = string.Format("更新 0/{0}", _syncUpdateNum);
                            ckbDelete.Text = string.Format("删除 0/{0}", _syncDeleteNum);
                           
                            gbMainOfOnToOne.Enabled = true;
                            //如果比较后数据是不需要同步的，不用将开始按钮启用
                            if (_sysnInsertNum==0 && _syncUpdateNum ==0 && _syncDeleteNum==0)
                            {
                                MessageBox.Show("对比完成，不需要进行同步");
                            }
                            else
                            {
                                MessageBox.Show("对比完成，需要进行同步");
                                btnStart.Enabled = true;//开始按钮启用
                            }
                            
                            btnStop.Enabled = false;//停止按钮禁用
                        }));
                    });
                }
            }
            catch (Exception e1)
            {
                MessageBox.Show(e1.Message, "程序异常", MessageBoxButtons.OK, MessageBoxIcon.Error);
                gbMainOfOnToOne.Enabled = true;
                btnStop.Enabled = false;
            }


        }

        /// <summary>
        /// 查询原数据的数据量
        /// </summary>
        /// <param name="selectFieldNamesList"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        private int GetCountQueryResult(List<string> selectFieldNamesList, DataBaseModel model, OracleConnection oracleConnection, MySqlConnection mySqlConnection)
        {

            int count = 0;//查询的结果数量
            try
            {

                //拼接查询SQL
                if (model.type == DataBaseTypeEnum.Mysql)
                {
                    StringBuilder sbMysqlSelectSql = new StringBuilder();
                    sbMysqlSelectSql.Append("select ");
                    for (int i = 0; i < selectFieldNamesList.Count; i++)
                    {
                        sbMysqlSelectSql.Append("`" + selectFieldNamesList[i] + "`,");
                    }
                    string mysqlSelectSql = sbMysqlSelectSql.ToString();
                    //去掉最后的一个 ","
                    mysqlSelectSql = mysqlSelectSql.Substring(0, mysqlSelectSql.Length - 1);
                    mysqlSelectSql = mysqlSelectSql + " from " + _syncTabName;//拼接表名

                    //拼接where条件
                    string txWhereStr = txWhere.Text;
                    if ("".Equals(txWhereStr.Trim()) || "where".Equals(txWhereStr.Trim().ToLower()))
                    {
                        loginfo.Info("没有写where条件");
                        txWhereStr = "";

                    }
                    mysqlSelectSql = mysqlSelectSql + " " + txWhereStr;
                    loginfo.Info("mysql库拼接的查询sql是：" + mysqlSelectSql);
                    //拼接count语句
                    string sqlCount = GetCountSql(mysqlSelectSql);
                    Object obj = MySqlConn.ExecuteScalar(mySqlConnection, model.conntext, CommandType.Text, sqlCount, null);
                    count = Convert.ToInt32(obj);

                }
                else if (model.type == DataBaseTypeEnum.Oracle)
                {
                    StringBuilder sbOracleSelectSql = new StringBuilder();
                    sbOracleSelectSql.Append("select ");
                    for (int i = 0; i < selectFieldNamesList.Count; i++)
                    {
                        sbOracleSelectSql.Append("\"" + selectFieldNamesList[i] + "\",");
                    }
                    string oracleSelectSql = sbOracleSelectSql.ToString();
                    //去掉最后的一个 ","
                    oracleSelectSql = oracleSelectSql.Substring(0, oracleSelectSql.Length - 1);
                    oracleSelectSql = oracleSelectSql + " from " + _syncTabName;//拼接表名

                    //拼接where条件
                    string txWhereStr = txWhere.Text;
                    if ("".Equals(txWhereStr.Trim()) || "where".Equals(txWhereStr.Trim().ToLower()))
                    {
                        loginfo.Info("没有写where条件");
                        txWhereStr = "";

                    }
                    oracleSelectSql = oracleSelectSql + " " + txWhereStr;
                    loginfo.Info("oracle库拼接的查询sql是：" + oracleSelectSql);

                    //拼接count语句
                    string sqlCount = GetCountSql(oracleSelectSql);
                    Object obj = OracleConn.ExecuteScalar(oracleConnection, model.conntext, CommandType.Text, sqlCount, null);
                    count = Convert.ToInt32(obj);


                }
                loginfo.Info("数据量为：" + count);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }


            return count;
        }

        /// <summary>
        /// 拼接查询表数据量count的语句
        /// </summary>
        /// <param name="selectSql"></param>
        /// <returns></returns>
        private string GetCountSql(string selectSql)
        {
            string sqlCount = "select count(*) from (" + selectSql + ") sqlCount";
            return sqlCount;

        }

        ///// <summary>
        ///// 生成删除目标数据的SQL
        ///// </summary>
        ///// <param name="dsDstDataRows"></param>
        ///// <param name="tabName"></param>
        ///// <param name="dataBaseModel"></param>
        ///// <returns></returns>
        //private List<string> GetExcuteDeleteSql(DataRowCollection dsDstDataRows, string tabName, DataBaseModel dataBaseModel)
        //{
        //    List<string> deleteSqlList = new List<string>();
        //    try
        //    {

        //        if (dataBaseModel.type == DataBaseTypeEnum.Mysql)
        //        {
        //            foreach (DataRow dr in dsDstDataRows)
        //            {
        //                MySqlParameter[] mysqlParameters = new MySqlParameter[1];//拼接ID
        //                string deleteSql = "delete from " + tabName + " where id=?ID";
        //                deleteSqlList.Add(deleteSql);
        //                string idValue = dr["ID"].ToString();
        //                mysqlParameters[0] = new MySqlParameter("?ID", idValue);//参数名称，对象值
        //                _mysqlParametersDeleteList.Add(mysqlParameters);

        //            }
        //        }
        //        else if (dataBaseModel.type == DataBaseTypeEnum.Oracle)
        //        {
        //            foreach (DataRow dr in dsDstDataRows)
        //            {
        //                OracleParameter[] oraParameters = new OracleParameter[1];
        //                string deleteSql = "delete from " + tabName + " where id=:ID";
        //                deleteSqlList.Add(deleteSql);
        //                string idValue = dr["ID"].ToString();
        //                oraParameters[0] = new OracleParameter(":ID", idValue);//参数名称，对象值
        //                _oracleParametersDeleteList.Add(oraParameters);

        //            }
        //        }
        //        else
        //        {
        //            loginfo.Info("不支持此类数据库的同步：" + dataBaseModel.type);
        //        }
        //    }
        //    catch (Exception ex) {
        //        throw new Exception(ex.Message);
        //    }


        //    return deleteSqlList;
        //}

        /// <summary>
        /// 生成删除目标数据的SQL
        /// </summary>
        /// <param name="dsDstDataRows"></param>
        /// <param name="tabName"></param>
        /// <param name="dataBaseModel"></param>
        /// <returns></returns>
        private Queue<ExcuteSqlObj> GetExcuteDeleteSql(DataRowCollection dsDstDataRows, string tabName, DataBaseModel dataBaseModel)
        {
            Queue<ExcuteSqlObj> deleteExcuteSqlObj = new Queue<ExcuteSqlObj>();
            try
            {

                if (dataBaseModel.type == DataBaseTypeEnum.Mysql)
                {
                    foreach (DataRow dr in dsDstDataRows)
                    {
                        MySqlParameter[] mysqlParameters = new MySqlParameter[1];//拼接ID
                        string deleteSql = "delete from " + tabName + " where id=?ID";
                        ExcuteSqlObj excuteSqlObj = new ExcuteSqlObj();
                        excuteSqlObj.sql = deleteSql;
                        excuteSqlObj.type = DataBaseTypeEnum.Mysql;
                        string idValue = dr["ID"].ToString();

                        mysqlParameters[0] = new MySqlParameter("?ID", idValue);//参数名称，对象值
                        excuteSqlObj.mySqlParameters = mysqlParameters;
                        deleteExcuteSqlObj.Enqueue(excuteSqlObj);
                      
                    }
                }
                else if (dataBaseModel.type == DataBaseTypeEnum.Oracle)
                {
                    foreach (DataRow dr in dsDstDataRows)
                    {
                        OracleParameter[] oraParameters = new OracleParameter[1];
                        string deleteSql = "delete from " + tabName + " where id=:ID";
                        ExcuteSqlObj excuteSqlObj = new ExcuteSqlObj();
                        excuteSqlObj.sql = deleteSql;
                        excuteSqlObj.type = DataBaseTypeEnum.Oracle;
                        string idValue = dr["ID"].ToString();
                        oraParameters[0] = new OracleParameter(":ID", idValue);//参数名称，对象值
                        excuteSqlObj.oracleParameters = oraParameters;
                        deleteExcuteSqlObj.Enqueue(excuteSqlObj);

                    }
                }
                else
                {
                    loginfo.Info("不支持此类数据库的同步：" + dataBaseModel.type);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }


            return deleteExcuteSqlObj;
        }

        /// <summary>
        /// 生成新增目标数据的SQL
        /// </summary>
        /// <param name="srcRow"></param>
        /// <param name="selectFieldNamesList"></param>
        /// <param name="tabName"></param>
        /// <param name="dataBaseModel"></param>
        /// <returns></returns>
        private string GetExcuteInsertSql(DataRow srcRow, List<string> selectFieldNamesList, string tabName, DataBaseModel dataBaseModel)
        {
            string insertSql = "";
            try
            {

                StringBuilder sbInsertSql = new StringBuilder();
                sbInsertSql.Append("insert into " + tabName + " (");
                //拼接新增数据的所有列名
                if (dataBaseModel.type == DataBaseTypeEnum.Mysql)
                {
                    MySqlParameter[] parameters = new MySqlParameter[selectFieldNamesList.Count];//存储参数值的数组
                    for (int i = 0; i < selectFieldNamesList.Count; i++)
                    {
                        string fieldName = selectFieldNamesList[i];
                        sbInsertSql.Append(" `" + fieldName + "`,");
                    }

                    //移掉最后一个逗号 
                    insertSql = sbInsertSql.ToString();
                    if (insertSql.EndsWith(","))
                    {
                        insertSql = insertSql.Substring(0, insertSql.Length - 1);
                    }
                    //拼接列的值
                    insertSql += ") values (";

                    for (int i = 0; i < selectFieldNamesList.Count; i++)
                    {
                        string fieldName = selectFieldNamesList[i];
                        insertSql += " ?" + fieldName + ",";
                        string fieldValue = Convert.ToString(srcRow[fieldName]);
                        parameters[i] = new MySqlParameter("?" + fieldName, fieldValue);//参数名称，对象值

                    }
                    if (insertSql.EndsWith(","))
                    {
                        insertSql = insertSql.Substring(0, insertSql.Length - 1);
                    }
                    //拼接最后的一个右括号
                    insertSql += ")";
                    _mysqlParametersInsertList.Add(parameters);

                }
                else if (dataBaseModel.type == DataBaseTypeEnum.Oracle)
                {
                    OracleParameter[] parameters = new OracleParameter[selectFieldNamesList.Count];//存储参数值的数组.最后要拼接上ID
                    for (int i = 0; i < selectFieldNamesList.Count; i++)
                    {
                        string fieldName = selectFieldNamesList[i];
                        sbInsertSql.Append(" \"" + fieldName + "\",");
                    }

                    //移掉最后一个逗号 
                    insertSql = sbInsertSql.ToString();
                    if (insertSql.EndsWith(","))
                    {
                        insertSql = insertSql.Substring(0, insertSql.Length - 1);
                    }
                    //拼接列的值
                    insertSql += ") values (";
                    for (int i = 0; i < selectFieldNamesList.Count; i++)
                    {
                        string fieldName = selectFieldNamesList[i];
                        insertSql += " :" + fieldName + ",";
                        string fieldValue = Convert.ToString(srcRow[fieldName]);
                        parameters[i] = new OracleParameter(":" + fieldName, fieldValue);//参数名称，对象值

                    }
                    if (insertSql.EndsWith(","))
                    {
                        insertSql = insertSql.Substring(0, insertSql.Length - 1);
                    }
                    //拼接最后的一个右括号
                    insertSql += ")";
                    _oracleParametersInsertList.Add(parameters);
                }
                else
                {
                    loginfo.Info("不支持此类数据库的同步：" + dataBaseModel.type);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }


            return insertSql;
        }



        /// <summary>
        /// 生成要更新目标数据的SQL
        /// </summary>
        /// <param name="srcRow"></param>
        /// <param name="selectFieldNamesList"></param>
        /// <param name="tabName"></param>
        /// <param name="dataBaseModel"></param>
        /// <returns></returns>
        private string GetExcuteUpdateSql(DataRow srcRow, List<string> selectFieldNamesList, string tabName, DataBaseModel dataBaseModel)
        {
            string updateSql = "";
            try
            {

                StringBuilder sbUpdateSql = new StringBuilder();
                selectFieldNamesList.Remove("ID");//将ID列移除
                sbUpdateSql.Append("update " + tabName + " set ");
                //根据目标库类型来生成SQL
                if (dataBaseModel.type == DataBaseTypeEnum.Mysql)
                {
                    MySqlParameter[] parameters = new MySqlParameter[selectFieldNamesList.Count + 1];//存储参数值的数组.最后要拼接上ID
                    for (int i = 0; i < selectFieldNamesList.Count; i++)
                    {
                        string fieldName = selectFieldNamesList[i];
                        //考虑到特殊字符要转义，使用传参的方式进行 
                        sbUpdateSql.Append(" `" + fieldName + "`=?" + fieldName + ",");
                        string fieldValue = Convert.ToString(srcRow[fieldName]);
                        parameters[i] = new MySqlParameter("?" + fieldName, fieldValue);//参数名称，对象值
                    }

                    updateSql = sbUpdateSql.ToString();
                    if (updateSql.EndsWith(","))
                    {
                        updateSql = updateSql.Substring(0, updateSql.Length - 1);//移掉最后一个逗号 
                    }

                    //拼接where条件
                    updateSql = updateSql + " where id=?ID";
                    parameters[parameters.Length - 1] = new MySqlParameter("?ID", srcRow["ID"].ToString());
                    _mysqlParametersUpdateList.Add(parameters);
                }
                else if (dataBaseModel.type == DataBaseTypeEnum.Oracle)
                {
                    OracleParameter[] parameters = new OracleParameter[selectFieldNamesList.Count + 1];//存储参数值的数组
                    for (int i = 0; i < selectFieldNamesList.Count; i++)
                    {
                        string fieldName = selectFieldNamesList[i];
                        sbUpdateSql.Append(" \"" + fieldName + "\"=:" + fieldName + ",");
                        string fieldValue = Convert.ToString(srcRow[fieldName]);
                        parameters[i] = new OracleParameter(":" + fieldName, fieldValue);//参数名称，对象值
                    }
                    updateSql = sbUpdateSql.ToString();
                    if (updateSql.EndsWith(","))
                    {
                        updateSql = updateSql.Substring(0, updateSql.Length - 1);//移掉最后一个逗号 
                    }

                    //拼接where条件
                    updateSql = updateSql + " where id=:ID";
                    parameters[parameters.Length - 1] = new OracleParameter(":ID", srcRow["ID"].ToString());
                    _oracleParametersUpdateList.Add(parameters);
                }
                else
                {
                    loginfo.Info("不支持此类数据库的同步：" + dataBaseModel.type);
                }

                selectFieldNamesList.Add("ID");//将ID列加上去
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }



            return updateSql;
        }

        /// <summary>
        /// 列值比较
        /// </summary>
        /// <param name="srcRow"></param>
        /// <param name="dstRow"></param>
        /// <param name="selectFieldNamesList"></param>
        /// <returns></returns>
        private bool ColumnValueCompare(DataRow srcRow, DataRow dstRow, List<string> selectFieldNamesList)
        {
            //分别从两个集合中取出要对比的字段
            foreach (string fieldName in selectFieldNamesList)
            {
                //ID列不用比较
                if ("ID".Equals(fieldName))
                {
                    continue;
                }
                string srcFieldValue = Convert.ToString(srcRow[fieldName]);
                string dstFieldValue = Convert.ToString(dstRow[fieldName]);
                if (!srcFieldValue.Equals(dstFieldValue))
                {
                    loginfo.Info("要比较的字段：" + fieldName);
                    loginfo.Info("值不一样：原库：" + srcFieldValue + " 目标库：" + dstFieldValue);
                    return true;
                }
            }
            return false;
        }


        /// <summary>
        ///  根据表名查询数据
        /// </summary>
        /// <param name="selectFieldNamesList"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        private DataSet GetQueryResult(List<string> selectFieldNamesList, DataBaseModel model, OracleConnection oracleConnection, MySqlConnection mySqlConnection)
        {
            DataSet dataSet = new DataSet();//存储查询的结果集
            try
            {

                //拼接查询SQL
                if (model.type == DataBaseTypeEnum.Mysql)
                {
                    StringBuilder sbMysqlSelectSql = new StringBuilder();
                    sbMysqlSelectSql.Append("select ");
                    for (int i = 0; i < selectFieldNamesList.Count; i++)
                    {
                        sbMysqlSelectSql.Append("`" + selectFieldNamesList[i] + "`,");
                    }
                    mysqlSelectSql = sbMysqlSelectSql.ToString();
                    //去掉最后的一个 ","
                    mysqlSelectSql = mysqlSelectSql.Substring(0, mysqlSelectSql.Length - 1);
                    mysqlSelectSql = mysqlSelectSql + " from " + _syncTabName;//拼接表名

                    //拼接where条件
                    string txWhereStr = txWhere.Text;
                    if ("".Equals(txWhereStr.Trim()) || "where".Equals(txWhereStr.Trim().ToLower()))
                    {
                        loginfo.Info("没有写where条件");
                        txWhereStr = "";

                    }
                    mysqlSelectSql = mysqlSelectSql + " " + txWhereStr;
                    loginfo.Info("mysql库拼接的查询sql是：" + mysqlSelectSql);
                    dataSet = MySqlConn.GetDataSet(mySqlConnection, model.conntext, CommandType.Text, mysqlSelectSql, null);

                }
                else if (model.type == DataBaseTypeEnum.Oracle)
                {
                    StringBuilder sbOracleSelectSql = new StringBuilder();
                    sbOracleSelectSql.Append("select ");
                    for (int i = 0; i < selectFieldNamesList.Count; i++)
                    {
                        sbOracleSelectSql.Append("\"" + selectFieldNamesList[i] + "\",");
                    }
                    oracleSelectSql = sbOracleSelectSql.ToString();
                    //去掉最后的一个 ","
                    oracleSelectSql = oracleSelectSql.Substring(0, oracleSelectSql.Length - 1);
                    oracleSelectSql = oracleSelectSql + " from " + _syncTabName;//拼接表名

                    //拼接where条件
                    string txWhereStr = txWhere.Text;
                    if ("".Equals(txWhereStr.Trim()) || "where".Equals(txWhereStr.Trim().ToLower()))
                    {
                        loginfo.Info("没有写where条件");
                        txWhereStr = "";

                    }
                    oracleSelectSql = oracleSelectSql + " " + txWhereStr;
                    loginfo.Info("oracle库拼接的查询sql是：" + oracleSelectSql);

                    dataSet = OracleConn.Query(oracleConnection, model.conntext, oracleSelectSql);


                }
            }
            catch (Exception e1)
            {
                throw new Exception(e1.Message);
            }


            return dataSet;
        }






        /// <summary>
        /// 全部选中要同步的字段
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CboAllField_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (ckbFileds.Items.Count == 0)
                {
                    MessageBox.Show("没有要同步的字段");
                    return;
                }
                if (cboAllField.Checked)//全选
                {
                    for (int j = 0; j < ckbFileds.Items.Count; j++)
                    {
                        ckbFileds.SetItemChecked(j, true);
                    }
                    //将右边的“不选USEC ...”去掉勾选
                    cboFourField.Checked = false;

                }
                else//反选
                {
                    for (int j = 0; j < ckbFileds.Items.Count; j++)
                    {
                        ckbFileds.SetItemChecked(j, false);
                    }

                }
            }
            catch (Exception e1)
            {
                MessageBox.Show(e1.Message, "程序异常", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }


        /// <summary>
        /// 获取选中的字段集合
        /// </summary>
        /// <returns></returns>
        private List<string> SelectFieldNames()
        {
            List<string> selectFieldNameList = new List<string>();
            for (int i = 0; i < ckbFileds.Items.Count; i++)
            {
                if (ckbFileds.GetItemChecked(i))
                {
                    selectFieldNameList.Add(ckbFileds.GetItemText(ckbFileds.Items[i]));
                }
            }
            return selectFieldNameList;
        }

        /// <summary>
        /// 点击开始的动作
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnStart_Click(object sender, EventArgs e)
        {

            gbMainOfOnToOne.Enabled = false;//禁用主界面按钮
            btnStop.Enabled = true;//启用停止按钮
            try
            {

                isStop = true;//正常运行
                //清除失败的数量
                lblDelFail.Text = "";
                lblInsertFail.Text = "";
                lblUpdateFail.Text = "";

                //增删改成功数目归零
                isSuccessDel = 0;
                isSuccessUpdate = 0;
                isSuccessInsert = 0;

                //增删改失败数目归零
                isFailDel = 0;
                isFailUpdate = 0;
                isFailInsert = 0;

                //检查用户是否选中要执行的操作
                //获取要执行操作
                bool isNeedInsert = ckbInsert.Checked;
                bool isNeedUpdate = ckbUpdate.Checked;
                bool isNeedDelete = ckbDelete.Checked;
                if (!isNeedInsert && !isNeedUpdate && !isNeedDelete)
                {
                    MessageBox.Show("请选择要执行同步的操作");
                    gbMainOfOnToOne.Enabled = true;
                    return;
                }

                //执行数据库SQL的耗时操作交给子线程去做
                ThreadPool.QueueUserWorkItem(obj =>
                {

                    //执行删除
                    if (isNeedDelete)
                    {
                        this.Invoke(new Action(() =>
                        {
                            btnStop.Enabled = true;//启用停止按钮
                        }));

                        //ThreadPool.SetMaxThreads(5, 5);//五个线程处理

                        //Boolean excuteResultThread1 = false;
                        //ThreadPool.QueueUserWorkItem(obj1 =>
                        //{
                        //    excuteResultThread1 = ExcuteDelTask(_deleteSqlQueue);

                        //});

                        bool excuteResultThread1 = ExcuteDelTask(_deleteSqlQueue);
                        //for (int i = 0; i < _deleteSqlList.Count; i++)
                        //{
                        //    try
                        //    {
                        //        if (!isStop)//停止
                        //        {
                        //            this.Invoke(new Action(() =>
                        //            {
                        //                gbMainOfOnToOne.Enabled = true;
                        //                btnStop.Enabled = false;//禁用停止按钮

                        //            }));
                        //            return;

                        //        }

                        //        string delsql = _deleteSqlList[i];
                        //        if (_dstSelectDatabase.type == DataBaseTypeEnum.Oracle)
                        //        {
                        //            loginfo.Info("执行删除的SQL：" + delsql);
                        //            loginfo.Info("删除的SQL的参数：" + string.Join(",", _oracleParametersDeleteList[i].Select(o => Convert.ToString(o.Value)).ToArray()));
                        //            OracleConn.ExecuteReader(connDstOracle, _dstSelectDatabase.conntext, CommandType.Text, delsql, _oracleParametersDeleteList[i]);
                        //            isSuccessDel++;//删除成功的数量 +1

                        //            //子线程做完任务后，将要更改界面上控件值的交给主线程去做
                        //            this.Invoke(new Action(() =>
                        //            {
                        //                ckbDelete.Text = $"删除 {isSuccessDel}/{_syncDeleteNum}";
                        //                // string.Format("删除 {0}/{1}", isSuccess, _syncDeleteNum);
                        //            }));

                        //        }
                        //        else if (_dstSelectDatabase.type == DataBaseTypeEnum.Mysql)
                        //        {
                        //            loginfo.Info("执行删除的SQL：" + delsql);
                        //            loginfo.Info("删除的SQL的参数：" + JsonConvert.SerializeObject(_mysqlParametersDeleteList[i]));
                        //            MySqlConn.ExecuteNonQuery(connDstMysql, _dstSelectDatabase.conntext, CommandType.Text, delsql, _mysqlParametersDeleteList[i]);
                        //            isSuccessDel++;//删除成功的数量 +1
                        //                           //子线程做完任务后，将要更改界面上控件值的交给主线程去做
                        //            this.Invoke(new Action(() =>
                        //            {
                        //                ckbDelete.Text = $"删除 {isSuccessDel}/{_syncDeleteNum}";
                        //            }));
                        //        }
                        //        else
                        //        {
                        //            loginfo.Info("不支持同步的数据库类型：" + _dstSelectDatabase.type);
                        //        }

                        //    }
                        //    catch (Exception e1)
                        //    {
                        //        loginfo.Info("执行SQL异常：" + e1.Message);
                        //        //失败数量 +1
                        //        isFailDel++;
                        //        //子线程做完任务后，将要更改界面上控件值的交给主线程去做
                        //        this.Invoke(new Action(() =>
                        //        {
                        //            lblDelFail.Text = $"失败 {isFailDel}";
                        //        }));
                        //        continue;
                        //    }



                        //}
                    }



                    //执行修改
                    if (isNeedUpdate)
                    {
                        this.Invoke(new Action(() =>
                        {
                            btnStop.Enabled = true;//启用停止按钮
                        }));
                        int isSuccess = 0;
                        int isFail = 0;
                        for (int i = 0; i < _updateSqlList.Count; i++)
                        {
                            try
                            {
                                if (!isStop)
                                {
                                    this.Invoke(new Action(() =>
                                    {
                                        gbMainOfOnToOne.Enabled = true;
                                        btnStop.Enabled = false;//禁用停止按钮

                                    }));
                                    return;
                                }
                                string updatesql = _updateSqlList[i];
                                if (_dstSelectDatabase.type == DataBaseTypeEnum.Oracle)
                                {
                                    loginfo.Info("执行更新的SQL：" + updatesql);
                                    loginfo.Info("更新的SQL的参数：：" + JsonConvert.SerializeObject(_oracleParametersUpdateList[i]));
                                    OracleConn.ExecuteReader(connDstOracle, _dstSelectDatabase.conntext, CommandType.Text, updatesql, _oracleParametersUpdateList[i]);
                                    isSuccess++;//修改成功的数量 +1
                                    //子线程做完任务后，将要更改界面上控件值的交给主线程去做
                                    this.Invoke(new Action(() =>
                                    {
                                        ckbUpdate.Text = $"更新 {isSuccess}/{_syncUpdateNum}";
                                    }));
                                }
                                else if (_dstSelectDatabase.type == DataBaseTypeEnum.Mysql)
                                {
                                    loginfo.Info("执行更新的SQL：" + updatesql);
                                    loginfo.Info("更新的SQL的参数：" + JsonConvert.SerializeObject(_mysqlParametersUpdateList[i]));
                                    MySqlConn.ExecuteNonQuery(connDstMysql, _dstSelectDatabase.conntext, CommandType.Text, updatesql, _mysqlParametersUpdateList[i]);
                                    isSuccess++;//修改成功的数量 +1
                                    //子线程做完任务后，将要更改界面上控件值的交给主线程去做
                                    this.Invoke(new Action(() =>
                                    {
                                        ckbUpdate.Text = $"更新 {isSuccess}/{_syncUpdateNum}";
                                    }));
                                }
                                else
                                {
                                    loginfo.Info("不支持同步的数据库类型：" + _dstSelectDatabase.type);
                                }
                            }
                            catch (Exception e1)
                            {
                                loginfo.Info("执行SQL异常：" + e1.Message);
                                //失败数量 +1
                                isFail++;
                                //子线程做完任务后，将要更改界面上控件值的交给主线程去做
                                this.Invoke(new Action(() =>
                                {
                                    lblUpdateFail.Text = $"失败 {isFail}";
                                }));
                                continue;
                            }

                        }
                    }


                    //执行新增
                    if (isNeedInsert)
                    {
                        //采用datatable的入库方式的速度不及逐条入库
                        //this.Invoke(new Action(() =>
                        //{
                        //    btnStop.Enabled = false;//禁用停止按钮
                        //}));
                        //try
                        //{
                        //    if (_dstSelectDatabase.type == DataBaseTypeEnum.Oracle)
                        //    {
                        //        //这里使用dataTable的方式新增数据
                        //        OracleConn.insertValueWithDt(connDstOracle, _dstSelectDatabase.conntext, _insertDataTable,oracleSelectSql);
                        //        //子线程做完任务后，将要更改界面上控件值的交给主线程去做
                        //        //20180829 动态显示进度
                        //        this.Invoke(new Action(() =>
                        //        {
                        //            ckbInsert.Text = $"新增 {_sysnInsertNum}/{_sysnInsertNum}";
                        //        }));

                        //    }
                        //    else if (_dstSelectDatabase.type == DataBaseTypeEnum.Mysql)
                        //    {

                        //        //子线程做完任务后，将要更改界面上控件值的交给主线程去做
                        //        this.Invoke(new Action(() =>
                        //        {
                        //            ckbInsert.Text = $"新增 {_sysnInsertNum}/{_sysnInsertNum}";
                        //        }));
                        //    }
                        //    else
                        //    {
                        //        loginfo.Info("不支持同步的数据库类型：" + _dstSelectDatabase.type);
                        //    }
                        //}
                        //catch (Exception e1)
                        //{
                        //    loginfo.Info("新增数据异常：" + e1.Message);
                        //    this.Invoke(new Action(() =>
                        //    {
                        //        lblInsertFail.Text = "新增失败";
                        //    }));

                        //}


                        int isSuccess = 0;
                        int isFail = 0;
                        for (int i = 0; i < _insertSqlList.Count; i++)
                        {
                            try
                            {
                                if (!isStop)
                                {
                                    this.Invoke(new Action(() =>
                                    {
                                        gbMainOfOnToOne.Enabled = true;
                                        btnStop.Enabled = false;//禁用停止按钮
                                    }));
                                    return;

                                }
                                string insertsql = _insertSqlList[i];
                                if (_dstSelectDatabase.type == DataBaseTypeEnum.Oracle)
                                {
                                    loginfo.Info("执行新增的SQL：" + insertsql);
                                    loginfo.Info("新增的SQL的参数：" + JsonConvert.SerializeObject(_oracleParametersInsertList[i]));
                                    OracleConn.ExecuteReader(connDstOracle, _dstSelectDatabase.conntext, CommandType.Text, insertsql, _oracleParametersInsertList[i]);
                                    isSuccess++;//新增成功的数量 +1
                                    //子线程做完任务后，将要更改界面上控件值的交给主线程去做
                                    this.Invoke(new Action(() =>
                                    {
                                        ckbInsert.Text = $"新增 {isSuccess}/{_sysnInsertNum}";
                                    }));

                                }
                                else if (_dstSelectDatabase.type == DataBaseTypeEnum.Mysql)
                                {
                                    loginfo.Info("执行新增的SQL：" + insertsql);
                                    loginfo.Info("新增的SQL的参数：" + JsonConvert.SerializeObject(_mysqlParametersInsertList[i]));
                                    MySqlConn.ExecuteNonQuery(connDstMysql, _dstSelectDatabase.conntext, CommandType.Text, insertsql, _mysqlParametersInsertList[i]);
                                    isSuccess++;//新增成功的数量 +1
                                    //子线程做完任务后，将要更改界面上控件值的交给主线程去做
                                    this.Invoke(new Action(() =>
                                    {
                                        ckbInsert.Text = $"新增 {isSuccess}/{_sysnInsertNum}";
                                    }));
                                }
                                else
                                {
                                    loginfo.Info("不支持同步的数据库类型：" + _dstSelectDatabase.type);
                                }
                            }
                            catch (Exception e1)
                            {
                                loginfo.Info("执行SQL异常：" + e1.Message);
                                //失败数量 +1
                                isFail++;
                                //子线程做完任务后，将要更改界面上控件值的交给主线程去做
                                this.Invoke(new Action(() =>
                                {
                                    lblInsertFail.Text = $"失败 {isFail}";
                                }));
                                continue;

                            }


                        }
                    }

                    //子线程做完任务后，将要更改界面上控件值的交给主线程去做
                    if (isStop)//没有点击停止，点击停止弹出任务已经停止
                    {
                        this.Invoke(new Action(() =>
                        {
                            MessageBox.Show("同步完成");
                            btnStop.Enabled = false;//禁用停止按钮
                            gbMainOfOnToOne.Enabled = true;
                        }));
                    }
                    else
                    {
                        this.Invoke(new Action(() =>
                        {
                            MessageBox.Show("任务已停止");
                            btnStop.Enabled = false;//禁用停止按钮
                            gbMainOfOnToOne.Enabled = true;
                        }));
                    }
                   

                });

            }
            catch (Exception e1)
            {
                MessageBox.Show(e1.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                btnStop.Enabled = false;//禁用停止按钮
                gbMainOfOnToOne.Enabled = true;
            }


        }

        /// <summary>
        /// 执行删除SQL任务
        /// </summary>
        /// <param name="sqlQueue"></param>
        public Boolean ExcuteDelTask(Queue<ExcuteSqlObj> sqlQueue)
        {

            if (null== sqlQueue|| sqlQueue.Count==0)
            {
                loginfo.Info("队列深度为0，不用执行删除任务");
                return true;
            }
            while(sqlQueue.Count>0)
            {
                ExcuteSqlObj delsql = sqlQueue.Dequeue();//移除并返回在 Queue 的开头的对象。
                try
                {
                    if (!isStop)
                    {
                        this.Invoke(new Action(() =>
                        {
                            gbMainOfOnToOne.Enabled = true;
                            btnStop.Enabled = false;//禁用停止按钮
                        }));
                        return true;

                    }
                    if (_dstSelectDatabase.type == DataBaseTypeEnum.Oracle)
                    {
                        OracleParameter[] oracleParametersDelete = delsql.oracleParameters;//移除并返回在 Queue 的开头的对象。
                        loginfo.Info("执行删除的SQL：" + delsql.sql);
                        loginfo.Info("删除的SQL的参数：" + string.Join(",", oracleParametersDelete.Select(o => Convert.ToString(o.Value)).ToArray()));
                        OracleConn.ExecuteReader(connDstOracle, _dstSelectDatabase.conntext, CommandType.Text, delsql.sql, oracleParametersDelete);
                        isSuccessDel++;//删除成功的数量 +1

                        //子线程做完任务后，将要更改界面上控件值的交给主线程去做
                        this.Invoke(new Action(() =>
                        {
                            ckbDelete.Text = $"删除 {isSuccessDel}/{_syncDeleteNum}";
                        }));

                    }
                    else if (_dstSelectDatabase.type == DataBaseTypeEnum.Mysql)
                    {
                        MySqlParameter[] mysqlParametersDelete = delsql.mySqlParameters;//移除并返回在 Queue 的开头的对象。
                        loginfo.Info("执行删除的SQL：" + delsql.sql);
                        loginfo.Info("删除的SQL的参数：" + JsonConvert.SerializeObject(mysqlParametersDelete));
                        MySqlConn.ExecuteNonQuery(connDstMysql, _dstSelectDatabase.conntext, CommandType.Text, delsql.sql, mysqlParametersDelete);
                        isSuccessDel++;//删除成功的数量 +1
                                       //子线程做完任务后，将要更改界面上控件值的交给主线程去做
                        this.Invoke(new Action(() =>
                        {
                            ckbDelete.Text = $"删除 {isSuccessDel}/{_syncDeleteNum}";
                        }));
                    }
                    else
                    {
                        loginfo.Info("不支持同步的数据库类型：" + _dstSelectDatabase.type);
                    }

                }
                catch (Exception e1)
                {
                    loginfo.Info("执行SQL异常：" + e1.Message);
                    //失败数量 +1
                    isFailDel++;
                    //子线程做完任务后，将要更改界面上控件值的交给主线程去做
                    this.Invoke(new Action(() =>
                    {
                        lblDelFail.Text = $"失败 {isFailDel}";
                    }));
                }
            }

            return true;


        }



        #region 把一个集合平均分组后，最后不满一组的放到最后一个分组中
        /// <summary>
        /// 把一个集合平均分组，最后不满一组的放到最后一个分组中
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="Lists"></param>
        /// <param name="num"></param>
        /// <returns></returns>
        public static List<List<T>> SplitList<T>(List<T> Lists, int num) //where T:class
        {
            List<List<T>> fz = new List<List<T>>();
            //元素数量大于等于 分组数量
            if (Lists.Count >= num)
            {
                int avg = Lists.Count / num; //每组数量
                int vga = Lists.Count % num; //余数
                for (int i = 0; i < num; i++)
                {
                    List<T> cList = new List<T>();
                    if (i + 1 == num)
                    {
                        cList = Lists.Skip(avg * i).ToList<T>();
                    }
                    else
                    {
                        cList = Lists.Skip(avg * i).Take(avg).ToList<T>();
                    }
                    fz.Add(cList);
                }
            }
            else
            {
                fz.Add(Lists);//元素数量小于分组数量
            }
            return fz;
        }
        #endregion

        /// <summary>
        /// 停止按钮：停止正在进行的操作
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnStop_Click(object sender, EventArgs e)
        {
            isStop = false;
            //MessageBox.Show("任务已停止");
        }

        /// <summary>
        /// 点击交换，代表数据源和目标源进行切换
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnChange(object sender, EventArgs e)
        {
            //必须要两个都选了，才可以交换
            int srcSelectIndex=srcDataBaseCbo.SelectedIndex;//首先获取到源数据库的索引
            int dstSelectIndex = dstDataBaseCbo.SelectedIndex;//获取目标数据库的索引

            if (srcSelectIndex==-1 || dstSelectIndex==-1)
            {
                MessageBox.Show("源数据库或目标数据库没有选中");
                return;
            }

            //交换数据源不需要检查是否为同一个数据库
            isChange = true;

            //将两个索引进行交换即可
            srcDataBaseCbo.SelectedIndex = dstSelectIndex;

            //交换数据源不需要检查是否为同一个数据库
            isChange = true;
            dstDataBaseCbo.SelectedIndex = srcSelectIndex;

            




        }

        /// <summary>
        /// 不选非重要更新的字段（USEC、CDAT、USEU、LSTU）
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CboFourField_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
               
                if (ckbFileds.Items.Count == 0)
                {
                    MessageBox.Show("没有要同步的字段");
                    return;
                }
                if (cboFourField.Checked)//选中
                {
                    bool haveCancelField = false;//要取消的字段
                    for (int j = 0; j < ckbFileds.Items.Count; j++)
                    {
                        string field = ckbFileds.Items[j].ToString();
                        if (fourFields.Contains(field))
                        {
                            //取消选中
                            ckbFileds.SetItemChecked(j, false);
                            haveCancelField = true;
                        }
                       
                    }

                    //将全选按钮取消，但是原来选中的不能取消
                    if (haveCancelField)
                    {
                        cboAllField.Checked = false;//将全选勾去掉
                        //选择非 fourFields 的字段
                        for (int j = 0; j < ckbFileds.Items.Count; j++)
                        {
                            string field = ckbFileds.Items[j].ToString();
                            if (!fourFields.Contains(field))
                            {
                                //选中
                                ckbFileds.SetItemChecked(j, true);
                            }
                        }
                    }

                }

            }
            catch (Exception e1)
            {
                MessageBox.Show(e1.Message, "程序异常", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 锁定原数据库与目标数据库的选择
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CboLock_CheckedChanged(object sender, EventArgs e)
        {
            if (cboLock.Checked)
            {
                srcDataBaseCbo.Enabled = false;
                dstDataBaseCbo.Enabled = false;
            }
            else
            {
                srcDataBaseCbo.Enabled = true;
                dstDataBaseCbo.Enabled = true;
            }
        }

      
    }



    /// <summary>
    /// 数据库连接model
    /// </summary>
    public class DataBaseModel {
        public string key;
        public DataBaseTypeEnum type;
        public string dataBaseName;
        public string conntext;
    }

    /// <summary>
    /// 数据库连接model
    /// </summary>
    public class ExcuteSqlObj
    {
        public string sql;
        public DataBaseTypeEnum type;
        public OracleParameter[] oracleParameters ;
        public MySqlParameter[] mySqlParameters ;
    }

    /// <summary>
    /// 数据库类型枚举
    /// </summary>
    public enum DataBaseTypeEnum {
        Oracle=1,
        Mysql=2
    }


    /// <summary>
    /// 将两个字符串全部转成小写进行比较
    /// </summary>
    class myEqualityComparer : IEqualityComparer<string>
    {
        public bool Equals(string x, string y)
        {
            return x.ToLower().Equals(y.ToLower());
        }

        public int GetHashCode(string obj)
        {
            return base.GetHashCode();
        }
    }

}
