using System;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Microsoft.ApplicationBlocks.Data;
using TPM.Classes;

namespace TPM
{
    public partial class Default : System.Web.UI.Page
    {

        public string paramId = "";
        public string paramText = "";
        public string m;
        protected void Page_Load(object sender, EventArgs e)
        {
          
            if (!IsPostBack)
            {
                paramId = Request.QueryString["v"] ?? "";
                paramText = Request.QueryString["t"] ?? "";
                m = Request.QueryString["m"] ?? "";
                Prepare();

            }
          
        }
        protected void Prepare()
        {
            if (m != "")
            {
                const char depid = '0';
                var ss = paramId.Split('_');

                var tbl = new Table
                {
                    ID = "jsonTable",
                    ClientIDMode = ClientIDMode.Static,
                    CssClass = "table table-bordered table-striped"
                };
                TableCell tc;
                DataSet ds = null;
                switch (ss[0])
                {
                    case "wo":
                        ds = SqlHelper.ExecuteDataset(TPMHelper.DBTPMstring, CommandType.StoredProcedure,
                                                      "usp_MWorkOrdersSelect_byStatus",
                                                      new SqlParameter("@status_id", ss[1]),
                                                      new SqlParameter("@dept", depid));
                        break;
                    case "dp":
                        ds = SqlHelper.ExecuteDataset(TPMHelper.DBTPMstring, CommandType.StoredProcedure,
                                                      "usp_mainpage_DailyPromptStatus", new SqlParameter("@option", ss[1]),
                                                      new SqlParameter("@dept", depid));
                        break;
                    case "pm":
                        ds = SqlHelper.ExecuteDataset(TPMHelper.DBTPMstring, CommandType.StoredProcedure,
                                                      "usp_mainpage_PMStatus", new SqlParameter("@option", ss[1]),
                                                      new SqlParameter("@dept", depid));
                        break;
                    case "bom":
                        ds = SqlHelper.ExecuteDataset(TPMHelper.DBTPMstring, CommandType.StoredProcedure,
                                                      "usp_mainpage_AllAssetModel");
                        break;
                    case "iwo":
                        ds = SqlHelper.ExecuteDataset(TPMHelper.DBTPMstring, CommandType.StoredProcedure,
                                                      "usp_mainpage_iwostatus", new SqlParameter("@option", ss[1]),
                                                      new SqlParameter("@dept", depid));
                        break;
                    case "bl":
                        AssetModels.AssetModelCode = ss[1];
                        ds = AssetModels.BomList;
                        break;
                    case "bon":
                        AssetModels.AssetModelCode = ss[1];
                        ds = AssetModels.BomListLow;
                        break;
                    case "bos":
                        AssetModels.AssetModelCode = ss[1];
                        ds = AssetModels.BomListLowAll;
                        break;
                    case "bol":
                        AssetModels.AssetModelCode = ss[1];
                        ds = AssetModels.SparePart;
                        break;
                    case "sum":
                        ds = SqlHelper.ExecuteDataset(TPMHelper.DBTPMstring, CommandType.StoredProcedure,
                                                      "usp_mainpage_DownTime", new SqlParameter("@option", ss[1]));
                        break;
                }
                var tr = new TableRow { TableSection = TableRowSection.TableHeader };
                var dt = ds != null ? ds.Tables[0] : new DataTable();
                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    tc = new TableHeaderCell { Text = dt.Columns[i].ColumnName };
                    tr.Cells.Add(tc);
                }
                tbl.Rows.Add(tr);


                foreach (DataRow dr in dt.Rows)
                {
                    tr = new TableRow();
                    for (int i = 0; i < dt.Columns.Count; i++)
                    {
                        tc = new TableCell
                        {
                            Text =
                                dt.Columns[i].DataType == Type.GetType("System.DateTime")
                                    ? ((DateTime)(dr[i])).ToString("d MMMM yyyy")
                                    : dr[i].ToString()
                        };
                        tr.Cells.Add(tc);
                    }
                    tbl.Rows.Add(tr);
                }
               
                var htm = new HtmlGenericControl("h4") { InnerText = "Export To Excel" };
                htm.Attributes.Add("class", "btn btn-primary");
                htm.Attributes.Add("id", paramId + "_" + paramText);
                tableContainer.Controls.Clear();
                tableContainer.Controls.Add(htm);
                
                //
                if (ss[0] == "pm" || ss[0] == "wo" || ss[0] == "dp" || ss[0] == "iwo")
                {
                    
                    var ddlDept = new DropDownList
                    {
                        ClientIDMode = ClientIDMode.Static,
                        ID = "ddlDept",
                        CssClass = "ddl"
                    };
                    ddlDept.Items.Add(new ListItem("ALL", "0"));
                    var ds2 = SqlHelper.ExecuteDataset(TPMHelper.DBTPMstring, CommandType.Text,
                                                           "select id,descriptions from mdepartments order by descriptions");
                    if (ds2.Tables.Count > 0)
                    {
                        foreach (DataRow row in ds2.Tables[0].Rows)
                        {
                            ddlDept.Items.Add(new ListItem(row[1].ToString(), row[0].ToString()));
                        }
                    }
                   
                    //  
                }
                htm = new HtmlGenericControl("hr");
                tableContainer.Controls.Add(htm);  
                tableContainer.Controls.Add(tbl); 
            }
        }
    }
}