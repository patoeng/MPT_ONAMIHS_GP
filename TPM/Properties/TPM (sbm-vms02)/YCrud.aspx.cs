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
        public List<string> Field = new List<string>();
        TPMHelper Helper = new TPMHelper();
        MySessions session;
        protected void Page_Load(object sender, EventArgs e)
        {
            session = new MySessions();
            if (!IsPostBack)
            {
                TableName = Request.QueryString["table"];
                defaults = Request.QueryString["d"]!=null? Request.QueryString["d"].ToString():"";
                defaulti = Request.QueryString["v"] != null ? Request.QueryString["v"].ToString() : "";
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
                    Server.Transfer(session.redirection);
                }
            }

        }
        protected void prepareCRUD()
        {
            // get table info
            DataSet ds;
            DataTable dt = new DataTable();
            if (isBOMtable == true)
            {
                ds = MySqlHelper.ExecuteDataset(Helper.TInventoryConnection(), "Select id,name as Descriptions,code as Inventory_Code from product");
                dt = ds.Tables[0];
            }
            SqlParameter sql = new SqlParameter("@ForeignColNum", 0)
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
            DataTable Fields = AssetDS.Tables[0];
            if (Fields.Rows.Count > 0)
            {
                ImageButton img = new ImageButton();
                img.ID = "BtnCloseForm";
                img.ClientIDMode = ClientIDMode.Static;
                img.ImageUrl = "./Images/delete.png";
                img.OnClientClick = "return false";
                img.Width = 15;
                img.BorderWidth = 0;
                img.ToolTip = "Close the Form";


                Label head = new Label();
                head.ID = "FormHeader";
                head.ClientIDMode = ClientIDMode.Static;
                head.Text = "Form Header";
                tr = new TableRow();
                tr.ID = "trhead";
                tr.ClientIDMode = ClientIDMode.Static;
                tc = new TableCell();
                tc.ColumnSpan = 2;
                tc.Controls.Add(img);
                tc.Controls.Add(head);
                tr.Cells.Add(tc);
                FormTbl.Rows.Add(tr);

                HiddenField hide = new HiddenField();
                hide.ID = "TableName";
                hide.Value = TableName;
                hide.ClientIDMode = ClientIDMode.Static;
                form1.Controls.Add(hide);

                hide = new HiddenField();
                hide.ID = "done_by";
                hide.Value = new MySessions().EmployeeNo;
                hide.ClientIDMode = ClientIDMode.Static;
                form1.Controls.Add(hide);
                hide.Dispose();

                foreach (DataRow dr in Fields.Rows)
                {
                    string s = dr["name"].ToString() as string;
                    Field.Add(s);
                    bool is_foreign = false;
                    if (dr["PK_Column"].ToString() != string.Empty)
                    {
                        is_foreign = true;
                    }
                    if (s.ToUpper() == "ID")
                    {
                        HiddenField hides = new HiddenField();
                        hides.ID = s;
                        hides.ClientIDMode = ClientIDMode.Static;
                        hides.Value = "";
                        form1.Controls.Add(hides);
                    }
                    else
                    {
                        tr = new TableRow();
                        if ((defaults!="")&&(s.ToUpper() == defaults.ToUpper())) { tr.Style.Add("display","none"); }
                        tc = new TableCell();
                        tc.Text = s.Replace('_', ' ');
                        if (is_foreign)
                        {
                            tc.Text = tc.Text.Remove(tc.Text.Length - 3);
                        }
                        tr.Cells.Add(tc);

                        if (!is_foreign)
                        {
                            if (s.ToUpper() == "ACTIVE")
                            {
                                DropDownList ddla = new DropDownList();
                                ListItem itm = new ListItem();
                                itm.Text = "True";
                                itm.Value = "1";
                                ddla.Items.Add(itm);
                                itm = new ListItem();
                                itm.Text = "False";
                                itm.Value = "0";
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
                                    DropDownList ddl1 = new DropDownList();
                                    ddl1.ID = s;
                                    ddl1.ClientIDMode = ClientIDMode.Static;
                                    ddl1.CssClass = "onchange";
                                    ListItem li = new ListItem();
                                    li.Text = "Please Select";
                                    li.Value = "";
                                    ddl1.Items.Add(li);
                                    foreach (DataRow drow in dt.Rows)
                                    {
                                        li = new ListItem();
                                        li.Text = drow[s].ToString();
                                        li.Value = drow["inventory_code"].ToString();
                                        ddl1.Items.Add(li);
                                    }
                                    tc = new TableCell();
                                    tc.Controls.Add(ddl1);
                                    tr.Cells.Add(tc);
                                    FormTbl.Rows.Add(tr);
                                }
                                else
                                {
                                    TextBox txt = new TextBox();
                                    txt.ID = s;
                                    txt.ClientIDMode = ClientIDMode.Static;

                                    if((bool)dr["is_nullable"]==false){txt.CssClass = "{required:true}";};
                                    if (dr[1].ToString() == "56")
                                    {
                                        txt.CssClass = "required digits";
                                    }
                                    else
                                    {
                                        if (dr[1].ToString() == "61")
                                        {
                                            txt.CssClass = "required datetime datepicker";
                                        }
                                        else
                                        {
                                            int t = Convert.ToInt32(dr[3].ToString());
                                            if (t > 0)
                                            {
                                                txt.MaxLength = t;
                                            }
                                        }
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
                            DropDownList ddl = new DropDownList();
                            ddl.ID = s;
                            ddl.ClientIDMode = ClientIDMode.Static;
                            ListItem l = new ListItem();
                            l.Text = "Please Select";
                            l.Value = string.Empty;
                            ddl.Items.Add(l);
                            foreach (DataRow ddr in AssetDS.Tables[foreignindex].Rows)
                            {
                                l = new ListItem();
                                l.Text = ddr["Descriptions"].ToString();
                                l.Value = ddr["Id"].ToString();
                                ddl.Items.Add(l);
                            }
                            ddl.CssClass = "{required:true}";

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
                tc = new TableCell();
                tc.Text = "&nbsp;";
                tr.Cells.Add(tc);


                Button SubmitBtn = new Button();
                SubmitBtn.ID = "BtnSubmit";
                SubmitBtn.ClientIDMode = ClientIDMode.Static;
                SubmitBtn.Text = "Submit";
                SubmitBtn.CssClass = "btn btn-primary";
                tc = new TableCell();
                tc.Controls.Add(SubmitBtn);
                tr.Cells.Add(tc);
                FormTbl.Rows.Add(tr);
            } //end of create form

            //start create list table
            DataTable Dtb = AssetDS.Tables[ForeignColNum + 1];
            //thead
            tr = new TableRow();
            tr.TableSection = TableRowSection.TableHeader;
            for (int column = 0; column < Dtb.Columns.Count; column++)
            {
                thc = new TableHeaderCell();
                thc.Text = Dtb.Columns[column].ColumnName.ToString().Replace('_', ' ');
                // tc.BorderWidth = 1;
                tr.Cells.Add(thc);
            }
            //

            CrudTbl.Rows.Add(tr);
            if (Dtb.Rows.Count > 0)
            {
                foreach (DataRow dr in Dtb.Rows)
                {
                    tr = new TableRow();

                    tr.TableSection = TableRowSection.TableBody;

                    for (int column = 0; column < Dtb.Columns.Count; column++)
                    {

                        tc = new TableCell();

                        tc.Text = dr[column].ToString();

                        tr.Cells.Add(tc);

                    }
                    CrudTbl.Rows.Add(tr);
                }

            }
        }
    }
}