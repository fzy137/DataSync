using DataSync.util;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace DataSync
{
    public partial class TableSpaceView : Form
    {
        public TableSpaceView()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 所有的数据库连接
        /// </summary>
        private List<DataBaseModel> _srcDataBaseModels = new List<DataBaseModel>();


        /// <summary>
        /// 选中的原数据库连接
        /// </summary>
        private DataBaseModel _srcSelectDatabase = new DataBaseModel();


        /// <summary>
        /// 选中的原数据库（ORACLE）连接
        /// </summary>
        private OracleConnection connSrcOracle = null;

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
                string srcDataBasepath = Application.StartupPath + @"\xml\oracleDataBase.xml";//数据库连接配置
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
                    _srcDataBaseModels.Add(model);//数据库连接对象
                    cboDataBase.Items.Add(model.key);//数据库连接名称

                }
            }
            catch (Exception e1)
            {
                MessageBox.Show(e1.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

           
           
        }
        /// <summary>
        /// ORACLE数据库选择
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CboDataBase_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                ComboBox cbox = (ComboBox)sender;
                if (cbox.SelectedItem == null)
                {
                    return;
                }
                splitContainer.Enabled = false;//禁用主界面控件，为了等待数据库连接完成
                 
                string key = cboDataBase.SelectedItem.ToString();
                DataBaseModel model = _srcDataBaseModels.First(item => item.key.Equals(key));
                _srcSelectDatabase = model;

                string selectTableSpaceSql = @"SELECT a.tablespace_name,
                                            round(total / (1024 * 1024 * 1024),2) total, 
                                            round(free / (1024 * 1024 * 1024),2) free, 
                                            round((total - free) / (1024 * 1024 * 1024),2) tableSpaceUseSize,
                                            round((total - free) / total, 4) * 100 usePercent 
                                            FROM (SELECT tablespace_name, SUM(bytes) free 
                                            FROM dba_free_space 
                                            GROUP BY tablespace_name) a, 
                                            (SELECT tablespace_name, SUM(bytes) total 
                                            FROM dba_data_files 
                                            GROUP BY tablespace_name) b 
                                            WHERE a.tablespace_name = b.tablespace_name";

                //查询表空间使用情况
                var dt = OracleConn.Search(selectTableSpaceSql, _srcSelectDatabase.conntext);
                dgvList.DataSource = dt;

                for (int i = 0; i < dgvList.Rows.Count; i++)//行
                {
                    //for (int j = 0; j < this.dgvList.Columns.Count; j++)//列
                    //{
                    //第5列大于80 背景标红
                    string usePercent = dgvList.Rows[i].Cells[4].Value.ToString();
                    double percent = double.Parse(usePercent);
                    if (percent>=80.00)
                    {
                        dgvList[4, i].Style.BackColor = Color.Red;
                    }
                  

                    //}
                }
                 

            }
            catch (Exception e1)
            {
                MessageBox.Show(e1.Message, "程序异常", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            splitContainer.Enabled = true;//启用主界面控件
        }
    }
 
    }
