using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using TPM.Classes;
using Microsoft.ApplicationBlocks.Data;
using System.Data;
using System.Data.SqlClient;
using MySql.Data.MySqlClient;

namespace TPM
{
    public partial class YCrud : System.Web.UI.Page
    {
        public string TableName;
        bool isBOMtable = false;
        public int ForeignColNumS;
        public string defaults = "";
        public string defaultv = "";
        public string defaulti = "";
        public string form = "";
        public List<string> Field = new List<string>();
        public string tittle = "";
        TPMHelper Helper = new TPMHelper();
        MySessions session;
        protected void Page_Load(object sender, EventArgs e)
        {
            session = new MySessions();
            if (!IsPostBack)
            {
                tittle = Request.QueryString["t"] ?? "";
                form = Request.QueryString["f"] ?? "";
                TableName = Request.QueryString["table"];
                defaults = Request.QueryString["d"] ?? "";
                defaulti = Request.QueryString["v"] ?? "";
                if (TableName.ToUpper() == "MBOMS")
                {
                    isBOMtable = true;//special table that referenced to external MYSql Server.
                }
                if (session.IsAdministrator)
                {
                    prepareCRUD();
                }
                else
                {
                    Server.Transfer(session.Redirection);
                }
            }

        }
        protected void prepareCRUD()
        {
            // get table info
            DataSet ds;
            var dt = new DataTable();
            if (isBOMtable == true)
            {
                ds = MySqlHelper.ExecuteDataset(Helper.TInventoryConnection(), "Select id,name as Descriptions,code as Inventory_Code from product");
                dt = ds.Tables[0];
            }
            var sql = new SqlParameter("@ForeignColNum", 0)
            {
                Direction = ParameterDirection.Output
            };
            SqlParameter[] sqlparams = {
                                        new SqlParameter("@Id",DBNull.Value),
                                        new SqlParameter("@TableName",TableName),
                                        new SqlParameter("@Active",DBNull.Value),
                                        sql
                                       };
            DataSet AssetDS = SqlHelper.ExecuteDataset(Helper.TPMDBConnection(), CommandType.StoredProcedure, "usp_SelectPrepareCRUD", sqlparams);
            string ForeignColNumSS = sql.SqlValue.ToString();
            int ForeignColNum = Convert.ToInt16(ForeignColNumSS);
            ForeignColNumS = ForeignColNum;
            //get the fields and create form and crudtable
            TableRow tr = null;
            TableCell tc = null;
            TableHeaderCell thc = null;
            int foreignindex = 0;
            var Fields = AssetDS.Tables[0];
            if (Fields.Rows.Count > 0)
            {
                var img = new ImageButton
                    {
                        ID = "BtnCloseForm",
                        ClientIDMode = ClientIDMode.Static,
                        ImageUrl = "./Images/delete.png",
                        OnClientClick = "return false",
                        Width = 15,
                        BorderWidth = 0,
                        ToolTip = "Close the Form"
                    };


                var head = new Label {ID = "FormHeader", ClientIDMode = ClientIDMode.Static, Text = "Form Header"};
                tr = new TableRow {ID = "trhead", ClientIDMode = ClientIDMode.Static};
                tc = new TableCell {ColumnSpan = 2};
                tc.Controls.Add(img);
                tc.Controls.Add(head);
                tr.Cells.Add(tc);
                FormTbl.Rows.Add(tr);

                var hide = new HiddenField {ID = "TableName", Value = TableName, ClientIDMode = ClientIDMode.Static};
                form1.Controls.Add(hide);

                hide = new HiddenField
                    {
                        ID = "done_by",
                        Value = new MySessions().EmployeeName,
                        ClientIDMode = ClientIDMode.Static
                    };
                form1.Controls.Add(hide);
                hide.Dispose();

                foreach (DataRow dr in Fields.Rows)
                {
                    var s = dr["name"].ToString();
                    Field.Add(s);
                    var is_foreign = dr["PK_Column"].ToString() != string.Empty;
                    if (s.ToUpper() == "ID")
                    {
                        var hides = new HiddenField {ID = s, ClientIDMode = ClientIDMode.Static, Value = ""};
                        form1.Controls.Add(hides);
                    }
                    else
                    {
                        tr = new TableRow();
                        if ((defaults!="")&&(s.ToUpper() == defaults.ToUpper())) { tr.Style.Add("display","none"); }
                        tc = new TableCell {Text = s.Replace('_', ' ')};
                        if (is_foreign)
                        {
                            var sss= tc.Text.Remove(tc.Text.Length - 3);
                            tc.Text = sss.ToUpper().Replace("ASSET", "MACHINE");

                        }
                        tr.Cells.Add(tc);

                        if (!is_foreign)
                        {
                            if (s.ToUpper() == "ACTIVE")
                            {
                                var ddla = new DropDownList();
                                var itm = new ListItem {Text = "True", Value = "1"};
                                ddla.Items.Add(itm);
                                itm = new ListItem {Text = "False", Value = "0"};
                                ddla.Items.Add(itm);
                                ddla.SelectedIndex = 0;
                                ddla.ID = s;
                                ddla.ClientIDMode = ClientIDMode.Static;
                                tc = new TableCell();
                                tc.Controls.Add(ddla);
                                tr.Cells.Add(tc);
                                FormTbl.Rows.Add(tr);

                            }
                            else
                            {
                                if ((isBOMtable == true) && ((s.ToUpper() == "INVENTORY_CODE") || (s.ToUpper() == "DESCRIPTIONS")))
                                {
                                    var ddl1 = new DropDownList
                                        {
                                            ID = s,
                                            ClientIDMode = ClientIDMode.Static,
                                            CssClass = "onchange"
                                        };
                                    var li = new ListItem {Text = "Please Select", Value = ""};
                                    ddl1.Items.Add(li);
                                    foreach (DataRow drow in dt.Rows)
                                    {
                                        li = new ListItem
                                            {
                                                Text = drow[s].ToString(),
                                                Value = drow["inventory_code"].ToString()
                                            };
                                        ddl1.Items.Add(li);
                                    }
                                    tc = new TableCell();
                                    tc.Controls.Add(ddl1);
                                    tr.Cells.Add(tc);
                                    FormTbl.Rows.Add(tr);
                                }
                                else
                                {
                                    var txt = new TextBox {ID = s, ClientIDMode = ClientIDMode.Static};

                                    if((bool)dr["is_nullable"]==false){txt.CssClass = "{required:true}";};
                                    switch (dr[1].ToString())
                                    {
                                        case "56":
                                            txt.CssClass = (bool)dr["is_nullable"] == false ? "required digits" : "digits";
                                            break;
                                        case "61":
                                            txt.CssClass = (bool)dr["is_nullable"] == false ? "required datetime datepicker" : "datetime datepicker";
                                            break;
                                        default:
                                            {
                                                int t = Convert.ToInt32(dr[3].ToString());
                                                if (t > 0)
                                                {
                                                    txt.MaxLength = t;
                                                }
                                            }
                                            break;
                                    }

                                    tc = new TableCell();
                                    tc.Controls.Add(txt);
                                    tr.Cells.Add(tc);
                                    FormTbl.Rows.Add(tr);
                                }
                            }

                        }
                        else
                        {

                            foreignindex++;
                            var ddl = new DropDownList {ID = s, ClientIDMode = ClientIDMode.Static};
                            var l = new ListItem {Text = "Please Select", Value = string.Empty};
                            ddl.Items.Add(l);
                            foreach (DataRow ddr in AssetDS.Tables[foreignindex].Rows)
                            {
                                l = new ListItem {Text = ddr["Descriptions"].ToString(), Value = ddr["Id"].ToString()};
                                ddl.Items.Add(l);
                            }
                            ddl.CssClass = TableName.ToUpper()=="DAILYPROMPTPOPUP"? "": "{required:true}";

                            if ((defaults != "") && (s.ToUpper() == defaults.ToUpper())) {
                                if (defaulti!="")
                                ddl.Items.FindByValue(defaulti).Selected = true;
                                defaultv=ddl.SelectedItem.Text;
                            } 
                            tc = new TableCell();
                            tc.Controls.Add(ddl);
                            tr.Cells.Add(tc);

                            FormTbl.Rows.Add(tr);
                        }

                    }
                }
                //add submit button
                tr = new TableRow();
                tc = new TableCell {Text = "&nbsp;"};
                tr.Cells.Add(tc);


                var SubmitBtn = new Button
                    {
                        ID = "BtnSubmit",
                        ClientIDMode = ClientIDMode.Static,
                        Text = "Submit",
                        CssClass = "btn btn-primary"
                    };
                tc = new TableCell();
                tc.Controls.Add(SubmitBtn);
                tr.Cells.Add(tc);
                FormTbl.Rows.Add(tr);
            } //end of create form

            //start create list table
            var Dtb = AssetDS.Tables[ForeignColNum + 1];
            //thead
            tr = new TableRow {TableSection = TableRowSection.TableHeader};
            for (var column = 0; column < Dtb.Columns.Count; column++)
            {
                var jj = Dtb.Columns[column].ColumnName.Replace('_', ' ').ToUpper();
                jj = TableName.ToUpper()=="DAILYPROMPTPOPUP"? jj.Replace("ASSET","Machine Name"): jj;
                thc = new TableHeaderCell {Text =jj};
                // tc.BorderWidth = 1;
                tr.Cells.Add(thc);
            }
            //

            CrudTbl.Rows.Add(tr);
            if (Dtb.Rows.Count > 0)
            {
                foreach (DataRow dr in Dtb.Rows)
                {
                    tr = new TableRow {TableSection = TableRowSection.TableBody};

                    for (int column = 0; column < Dtb.Columns.Count; column++)
                    {

                        tc = new TableCell {Text = dr[column].ToString()};

                        tr.Cells.Add(tc);

                    }
                    CrudTbl.Rows.Add(tr);
                }

            }
        }
    }
}