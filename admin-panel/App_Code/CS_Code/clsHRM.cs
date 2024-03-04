using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.IO;
using System.Data.SqlClient;
using System.Net.Mail;
using System.Data.OleDb;
using System.Collections;
using System.Reflection;



/// <summary>
/// Summary description for clsHRM
/// </summary>
public class clsHRM
{
    #region set--SqlConnection
    //############################################
    private SqlTransaction dbTransaction = null;
    string cn = ConfigurationManager.ConnectionStrings["sqlConString"].ConnectionString;
    SqlConnection con;
    //############################################
    #endregion
    public clsHRM()
    {

    }


    #region DEPARTMENT
    public string AddDepartment(
                                    string strBr_ID,
                                    string strDPT_NAME,
                                    string strDPT_DETAILS)
    {
        string strSql;

        strSql = "select CMP_BRANCH_ID, DPT_NAME,DPT_DETAILS from PR_DEPARTMENT ";

        try
        {
            DataRow oOrderRow;
            con = new SqlConnection(cn);
            con.Open();
            dbTransaction = con.BeginTransaction();
            DataSet oDS = new DataSet();
            SqlDataAdapter oOrdersDataAdapter = new SqlDataAdapter(new SqlCommand(strSql, con, dbTransaction));
            SqlCommandBuilder oOrdersCmdBuilder = new SqlCommandBuilder(oOrdersDataAdapter);
            oOrdersDataAdapter.FillSchema(oDS, SchemaType.Source, "PR_DEPARTMENT");
            oOrderRow = oDS.Tables["PR_DEPARTMENT"].NewRow();
            oOrderRow["CMP_BRANCH_ID"] = strBr_ID;
            oOrderRow["DPT_NAME"] = strDPT_NAME;
            oOrderRow["DPT_DETAILS"] = strDPT_DETAILS;
            oDS.Tables["PR_DEPARTMENT"].Rows.Add(oOrderRow);
            oOrdersDataAdapter.Update(oDS, "PR_DEPARTMENT");
            dbTransaction.Commit();
            //con.Close();
            return "Saved successfully";

        }
        catch (Exception ex)
        {
            return ex.Message.ToString();
        }
        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }

    }
    #endregion

    #region Organogram

    //-----------CREATED BY PRISCILA-----------------//
    public DataSet GetParentDesignationI(
                                            string strType,
                                            string strBranchID,
                                            string strLevel,
                                            string strParentCode)
    {
        string strSql = "";

        if (!strBranchID.Equals("-1")) //when not all branch
        {
            strSql = " AND DE.CMP_BRANCH_ID = C.CMP_BRANCH_ID AND ( C.CMP_BRANCH_ID ='" + strBranchID + "' OR C.CMP_BRANCH_TYPE_ID = 'A') ";
        }
        else //when all branch
        {
            strSql = " AND DE.CMP_BRANCH_ID = C.CMP_BRANCH_ID ";
        }

        string strQuery = "SELECT DE.CMP_BRANCH_ID, DE.DPT_ID, DSG_ID, SET_TYPE, SET_CODE, SET_LEVEL, PARENT_CODE, "
                        + "CASE "
                        + " WHEN SET_CODE IS NULL THEN DSG_TITLE  "
                        + "ELSE '  ' + CAST(SET_CODE AS VARCHAR) + ' - ' + CAST(DSG_TITLE AS VARCHAR)  "
                        + "END DNAME "
                        + "FROM PR_DESIGNATION DE, CM_CMP_BRANCH C "
                        + "WHERE DE.SET_TYPE='" + strType + "' AND DE.SET_LEVEL='" + strLevel + "' "
                        + "AND DE.PARENT_CODE='" + strParentCode + "' " + strSql + " "
                        + "ORDER BY CONVERT(NUMERIC(8, 2), SET_CODE) ";


        try
        {
            con = new SqlConnection(cn);
            con.Open();
            DataSet oDS = new DataSet();
            SqlDataAdapter odaData = new SqlDataAdapter(new SqlCommand(strQuery, con));
            odaData.Fill(oDS, "PR_DESIGNATION");
            return oDS;
        }
        catch (Exception ex)
        {
            return null;
        }
        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }
    }

    public DataSet GetParentDesignationF(
                                        string strType,
                                        string strBranchID,
                                        string strLevel,
                                        string strParentCode)
    {
        string strSql = "";

        if (!strBranchID.Equals("-1"))
        {

            strSql = " AND DE.CMP_BRANCH_ID = C.CMP_BRANCH_ID AND ( C.CMP_BRANCH_ID ='" + strBranchID + "' OR C.CMP_BRANCH_TYPE_ID = 'A') ";
        }
        else
        {
            strSql = " AND DE.CMP_BRANCH_ID = C.CMP_BRANCH_ID ";
        }
        //ASAD
        string strQuery = "SELECT DE.CMP_BRANCH_ID, DE.DPT_ID, DSG_ID, SET_TYPE, SET_CODE, SET_LEVEL, PARENT_CODE, "
                        + " CASE "
                        + " WHEN SET_CODE IS NULL THEN DSG_TITLE "
                        + " ELSE '  ' + CAST(SET_CODE AS VARCHAR) + ' - ' + CAST(DSG_TITLE AS VARCHAR)  "
                        + " END DNAME "
                        + "FROM PR_DESIGNATION DE, CM_CMP_BRANCH C "
                        + "WHERE DE.SET_TYPE='" + strType + "' AND DE.SET_LEVEL='" + strLevel + "' "
                        + "AND DE.PARENT_CODE='" + strParentCode + "' " + strSql + " "
                        + "ORDER BY CMP_BRANCH_ID, CONVERT(NUMERIC(8, 2), SET_LEVEL), CONVERT(NUMERIC(8, 2), PARENT_CODE), SET_TYPE, SET_CODE";

        try
        {
            con = new SqlConnection(cn);
            con.Open();
            DataSet oDS = new DataSet();
            SqlDataAdapter odaData = new SqlDataAdapter(new SqlCommand(strQuery, con));
            odaData.Fill(oDS, "PR_DESIGNATION");
            return oDS;
        }
        catch (Exception ex)
        {
            return null;
        }
        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }
    }



    public DataSet GetChildGradeI(
                                    string strType,
                                    string strBranchID,
                                    string strPID)
    {
        string strQuery = "SELECT DE.CMP_BRANCH_ID, DSG_ID, SET_TYPE, SET_LEVEL, SET_CODE, PARENT_CODE, "
                        + "CASE   WHEN SET_CODE IS NULL THEN DSG_TITLE  ELSE '  ' + CAST(SET_CODE AS VARCHAR) + ' - ' + CAST(DSG_TITLE AS VARCHAR)  END DNAME "
                        + "FROM PR_DESIGNATION DE, CM_CMP_BRANCH C "
                        + "WHERE DE.CMP_BRANCH_ID = C.CMP_BRANCH_ID  AND DE.SET_TYPE='" + strType + "' "
                        + "AND DE.PARENT_CODE='" + strPID + "' AND C.CMP_BRANCH_ID ='" + strBranchID + "' "
                        + "ORDER BY CONVERT(NUMERIC(8, 2), SET_CODE)";
        //+ "ORDER BY DE.CMP_BRANCH_ID, to_number(SET_LEVEL), to_number(PARENT_CODE), SET_TYPE, SET_CODE";


        try
        {
            con = new SqlConnection(cn);
            con.Open();
            DataSet oDS = new DataSet();
            SqlDataAdapter odaData = new SqlDataAdapter(new SqlCommand(strQuery, con));
            odaData.Fill(oDS, "PR_CHILD_GRADE");
            return oDS;
        }
        catch (Exception ex)
        {
            return null;
        }
        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }
    }

    public DataSet GetChildGradeF(
                                string strType,
                                string strBranchID,
                                string strPID)
    {
        //string strQuery = "SELECT DE.CMP_BRANCH_ID, DSG_ID, SET_TYPE, SET_LEVEL, SET_CODE, PARENT_CODE, "
        //                + "DECODE(SET_CODE,'', DSG_TITLE, '  ' || SET_CODE || ' - ' || DSG_TITLE) DNAME "
        //                + "FROM PR_DESIGNATION DE, CM_CMP_BRANCH C "
        //                + "WHERE DE.CMP_BRANCH_ID = C.CMP_BRANCH_ID AND DE.SET_TYPE='" + strType + "' "
        //                + "AND DE.PARENT_CODE='" + strPID + "' AND C.CMP_BRANCH_ID ='" + strBranchID + "' "
        //                + "ORDER BY DE.CMP_BRANCH_ID, to_number(SET_LEVEL), to_number(PARENT_CODE), SET_TYPE, SET_CODE";


        //ASAD

        string strQuery = "SELECT DE.CMP_BRANCH_ID, DSG_ID, SET_TYPE, SET_LEVEL, SET_CODE, PARENT_CODE, "
                    + " CASE  "
                    + " WHEN SET_CODE IS NULL THEN DSG_TITLE  "
                    + " ELSE '  ' + CAST(SET_CODE AS VARCHAR) + ' - ' + CAST(DSG_TITLE AS VARCHAR)  "
                    + " END DNAME "
                   + "FROM PR_DESIGNATION DE, CM_CMP_BRANCH C "
                   + "WHERE DE.CMP_BRANCH_ID = C.CMP_BRANCH_ID AND DE.SET_TYPE='" + strType + "' "
                   + "AND DE.PARENT_CODE='" + strPID + "' AND C.CMP_BRANCH_ID ='" + strBranchID + "' "
                   + "ORDER BY DE.CMP_BRANCH_ID,  CONVERT(NUMERIC(8, 2), SET_LEVEL), CONVERT(NUMERIC(8, 2), PARENT_CODE), SET_TYPE, SET_CODE";

        try
        {
            con = new SqlConnection(cn);
            con.Open();
            DataSet oDS = new DataSet();
            SqlDataAdapter odaData = new SqlDataAdapter(new SqlCommand(strQuery, con));
            odaData.Fill(oDS, "PR_CHILD_GRADE");
            return oDS;
        }
        catch (Exception ex)
        {
            return null;
        }
        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }
    }



    #endregion

    #region TAX SLAB

    public double GetTaxSlabAmt(double amtFrom, double amtTo, string gender)
    {
        double tax_percent = 0;

        string strQuery = "SELECT TAX_PERCENTAGE FROM PR_TAX_SLAV WHERE AMOUNT_FROM='" + amtFrom + "' AND AMOUNT_TO='" + amtTo + "' AND SLAB_FOR='" + gender + "'";

        try
        {
            con = new SqlConnection(cn);
            con.Open();
            DataSet oDS = new DataSet();
            SqlDataAdapter odaData = new SqlDataAdapter(new SqlCommand(strQuery, con));
            odaData.Fill(oDS, "PR_TAX_SLAV");

            foreach (DataRow prow in oDS.Tables["PR_TAX_SLAV"].Rows)
            {
                tax_percent = Convert.ToDouble(prow["TAX_PERCENTAGE"].ToString());
            }

            return tax_percent;
        }
        catch (Exception ex)
        {
            return 0;
        }
        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }

    }

    public DataSet GetTaxSlabDetails(string gender)
    {
        double tax_percent = 0;

        string strQuery = "SELECT AMOUNT_FROM, AMOUNT_TO, TAX_PERCENTAGE, SLAB_FOR "
                        + "FROM PR_TAX_SLAV "
                        + "WHERE SLAB_FOR='" + gender + "' "

                      + "ORDER BY AMOUNT_FROM";

        try
        {
            con = new SqlConnection(cn);
            con.Open();
            DataSet oDS = new DataSet();
            SqlDataAdapter odaData = new SqlDataAdapter(new SqlCommand(strQuery, con));
            odaData.Fill(oDS, "PR_TAX_SLAV");
            return oDS;
        }
        catch (Exception ex)
        {
            return null;
        }
        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }
    }

    public DataSet GetTotalTaxIncome(string bId, string gId, string strGender, string strEtYp, string allBranchIndex)
    {
        string strSQL = "";


        //if (bId != "100310001") // when not all branch
        //{
        strSQL = " (CMP_BRANCH_ID='" + bId + "' OR CMP_BRANCH_ID = '" + allBranchIndex + "')";
        //}
        //else
        //{
        //    strSQL = " CMP_BRANCH_ID='" + bId + "' ";
        //}

        string strQuery = "SELECT TOT_TAX, TAX_PER_MONTH, TOT_INCOME FROM PR_TAX_CALCULATION "
                            + "WHERE "
                            + " " + strSQL + " "
                            + "AND DSG_ID='" + gId + "' AND EMP_GENDER='" + strGender + "' AND TYP_CODE='" + strEtYp + "'";

        try
        {
            con = new SqlConnection(cn);
            con.Open();
            DataSet oDS = new DataSet();
            SqlDataAdapter odaData = new SqlDataAdapter(new SqlCommand(strQuery, con));
            odaData.Fill(oDS, "PR_TAX_CALCULATION");
            return oDS;
        }
        catch (Exception ex)
        {
            return null;
        }
        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }
    }

    public DataSet GetTotalTaxIncome(string bId, string gId, string dId, string strGender, string strEtYp, string allbranch)
    {
        string strSQL = "";

        //if (bId != "100310001") // when not all branch
        //{
        strSQL = " (CMP_BRANCH_ID='" + bId + "' OR CMP_BRANCH_ID = '" + allbranch + "')";
        //}
        //else
        //{
        //    strSQL = " CMP_BRANCH_ID='" + bId + "' ";
        //}

        string strQuery = "SELECT TOT_TAX, TAX_PER_MONTH, TOT_INCOME FROM PR_TAX_CALCULATION "
                            + "WHERE "
                            + " " + strSQL + " "
                            + "AND DSG_ID='" + gId + "' AND DPT_ID='" + dId + "' "
                            + "AND EMP_GENDER='" + strGender + "' AND TYP_CODE='" + strEtYp + "'";

        try
        {
            con = new SqlConnection(cn);
            con.Open();
            DataSet oDS = new DataSet();
            SqlDataAdapter odaData = new SqlDataAdapter(new SqlCommand(strQuery, con));
            odaData.Fill(oDS, "PR_TAX_CALCULATION");
            return oDS;
        }
        catch (Exception ex)
        {
            return null;
        }
        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }
    }

    #endregion


    public DataSet GetEmployeeList(string strParam)
    {

        string strQuery = "SELECT CAST(e.EMP_NAME AS VARCHAR) + ' (' + CAST(e.EMP_CODE AS VARCHAR)+')' emp , EMP_ID,DPT_ID "
                          + "FROM PR_EMPLOYEE_LIST e, CM_CMP_BRANCH c "
                          + "WHERE e.CMP_BRANCH_ID = c.CMP_BRANCH_ID  "
                          + " " + strParam + "  ";

        try
        {
            con = new SqlConnection(cn);
            con.Open();
            DataSet oDS = new DataSet();
            SqlDataAdapter odaData = new SqlDataAdapter(new SqlCommand(strQuery, con));
            odaData.Fill(oDS, "PR_EMPLOYEE_LIST");
            return oDS;
        }
        catch (Exception e)
        {
            return null;
        }
        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }

    }
    //not necessary
    //public DataSet GetAttendance(string strParam)
    //{
    //    string strQuery = "select FNC_EMPATTENDENCE(" + strParam + ") ATT_TYPE from DUAL ";

    //    try
    //    {
    //        con = new SqlConnection(cn);
    //        con.Open();
    //        DataSet oDS = new DataSet();
    //        SqlDataAdapter odaData = new SqlDataAdapter(new SqlCommand(strQuery, con));
    //        odaData.Fill(oDS, "PR_EMP_ATTENDENCE");
    //        return oDS;
    //    }
    //    catch (Exception ex)
    //    {
    //        return null;
    //    }
    //    finally
    //    {
    //        con.Close();
    //        con = null;
    //    }
    //}

    public string AssignTask(string strTaskId, string strEmpId)
    {
        string strSql;
        strSql = "select ASGN_ID,EMP_ID from PR_ASSIGNMENT_DETAIL ";

        try
        {
            DataRow oOrderRow;
            con = new SqlConnection(cn);
            con.Open();
            dbTransaction = con.BeginTransaction();
            DataSet oDS = new DataSet();
            SqlDataAdapter oOrdersDataAdapter = new SqlDataAdapter(new SqlCommand(strSql, con, dbTransaction));
            SqlCommandBuilder oOrdersCmdBuilder = new SqlCommandBuilder(oOrdersDataAdapter);
            //oOrdersDataAdapter.MissingSchemaAction = MissingSchemaAction.AddWithKey;
            //oOrdersDataAdapter.Fill(oDS, "PR_ASSIGNMENT_DETAIL");

            oOrdersDataAdapter.FillSchema(oDS, SchemaType.Source, "PR_ASSIGNMENT_DETAIL");
            oOrderRow = oDS.Tables["PR_ASSIGNMENT_DETAIL"].NewRow();

            oOrderRow["ASGN_ID"] = strTaskId;
            oOrderRow["EMP_ID"] = strEmpId;

            oDS.Tables["PR_ASSIGNMENT_DETAIL"].Rows.Add(oOrderRow);
            oOrdersDataAdapter.Update(oDS, "PR_ASSIGNMENT_DETAIL");
            dbTransaction.Commit();
            // con.Close();
            return "Saved successfully";
        }
        catch (Exception ex)
        {
            return ex.Message.ToString();
        }
        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }
    }

    public bool CheckTaskAreadyAssigned(string strTaskId, string strEmpId)
    {
        string strSql;
        strSql = "select ASGN_ID,EMP_ID from PR_ASSIGNMENT_DETAIL WHERE ASGN_ID = '" + strTaskId + "' AND EMP_ID = '" + strEmpId + "'";

        try
        {
            //DataRow oOrderRow;

            con = new SqlConnection(cn);
            con.Open();
            DataSet oDS = new DataSet();
            SqlDataAdapter oOrdersDataAdapter = new SqlDataAdapter(new SqlCommand(strSql, con));
            SqlCommandBuilder oOrdersCmdBuilder = new SqlCommandBuilder(oOrdersDataAdapter);
            oOrdersDataAdapter.MissingSchemaAction = MissingSchemaAction.AddWithKey;
            oOrdersDataAdapter.Fill(oDS, "PR_ASSIGNMENT_DETAIL");

            DataTable tbl_AD = oDS.Tables["PR_ASSIGNMENT_DETAIL"];

            // already exists
            if (tbl_AD.Rows.Count > 0) { return true; }
            else { return false; }
        }
        catch (Exception ex)
        {
            return false;
        }
        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }
    }
    public string CheckDuplicateLVapplication(string empId, string Brid, string FrmDt, string Todt)
    {
        string Exist = "0";
        string strSql;
        strSql = "SELECT Count(*) FROM PR_LEAVE pl WHERE  pl.EMP_ID='" + empId + "' and pl.CMP_BRANCH_ID='" + Brid + "' "
        + " AND  "
        + "( "
        + " convert(date, CONVERT(DATETIME, CONVERT(VARCHAR(23), pl.LVE_FROM_DATE)))  =  convert(date, CONVERT(DATETIME, CONVERT(VARCHAR(23), '" + FrmDt + "'))) "
        + " OR "
        + "( "
        + " convert(date, CONVERT(DATETIME, CONVERT(VARCHAR(23), pl.LVE_FROM_DATE))) = '" + FrmDt + "' "
        + " AND "
        + " convert(date, CONVERT(DATETIME, CONVERT(VARCHAR(23), pl.LVE_TO_DATE))) = '" + Todt + "' "
        + ") "
        + " ) "
        + " GROUP BY pl.EMP_ID ";

        try
        {
            //DataRow oOrderRow;

            con = new SqlConnection(cn);
            con.Open();
            DataSet oDS = new DataSet();
            SqlDataAdapter oOrdersDataAdapter = new SqlDataAdapter(new SqlCommand(strSql, con));
            SqlCommandBuilder oOrdersCmdBuilder = new SqlCommandBuilder(oOrdersDataAdapter);
            oOrdersDataAdapter.MissingSchemaAction = MissingSchemaAction.AddWithKey;
            oOrdersDataAdapter.Fill(oDS, "PR_LEAVE");

            DataTable tbl_AD = oDS.Tables["PR_LEAVE"];

            // already exists
            if (tbl_AD.Rows.Count > 0)
            {
                Exist = "1";
            }

        }
        catch (Exception ex)
        {
            return "";
        }
        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }
        return Exist;
    }
    public string RemoveTask(string strTaskId, string strEmpId)
    {
        string strSql;
        strSql = "DELETE from PR_ASSIGNMENT_DETAIL WHERE ASGN_ID = '" + strTaskId + "' AND EMP_ID = '" + strEmpId + "'";

        try
        {

            con = new SqlConnection(cn);
            con.Open();
            dbTransaction = con.BeginTransaction();
            SqlCommand olcmd = new SqlCommand(strSql, con, dbTransaction);
            olcmd.Connection = con;
            olcmd.ExecuteNonQuery();
            dbTransaction.Commit();
            //con.Close();

            return "Saved successfully";
        }
        catch (Exception ex)
        {
            return ex.Message.ToString();
        }
        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }
    }

    #region Leave Deduct

    //public string CalcLeaveAmt(string bId, string eId, string gId, double totLeave)  
    //{
    //    string strQuery = "SELECT  EL.EMP_GENDER, PG.PRST_AMT, EL.EMP_NAME, PR_LEPO_ID, PR_LEPO_LEAVE_DEDUCT, " 
    //                        + "PP.CMP_BRANCH_ID, PR_LEPO_MAX_LATE_PERMIT, PR_LEPO_LATE_SALARY_DAYS, PR_LEPO_MONTH_DAY " 
    //                        + "FROM PR_LEAVE_POLICY_DEDUCT PP, PR_PAYROLL_GENERUL PG, PR_EMPLOYEE_LIST EL "
    //                        + "WHERE EL.CMP_BRANCH_ID='" + bId + "' AND EL.CMP_BRANCH_ID=PG.CMP_BRANCH_ID AND "
    //                        + "PG.PR_ITEM_ID=PP.PR_LEPO_LEAVE_DEDUCT AND EL.EMP_ID=PG.EMP_ID "
    //                        + "AND EL.EMP_ID='" + eId + "' AND EL.DSG_ID='" + gId + "'";

    //    try
    //    {
    //        SqlConnection
    //        con.Open();
    //        dbTransaction = con.BeginTransaction();
    //        DataSet oDS = new DataSet();
    //        SqlDataAdapter odaData = new SqlDataAdapter(new SqlCommand(strQuery, con,dbTransaction));
    //        odaData.Fill(oDS, "PR_LEAVE_POLICY");
    //        //return oDS;

    //        double leaveDeductAmt = 0;
    //        double monthDays = 0;
    //        double maxLimit = 0;
    //        double salaryDeduct = 0;
    //        double itemAmt = 0;
    //        double perCycleAmt = 0;
    //        int deductCycle = 0;

    //        if (oDS.Tables["PR_LEAVE_POLICY"].Rows.Count > 0)
    //        {
    //            DataRow dRow = oDS.Tables["PR_LEAVE_POLICY"].Rows[0];

    //            monthDays = Convert.ToDouble(dRow["PR_LEPO_MONTH_DAY"].ToString());
    //            maxLimit = Convert.ToDouble(dRow["PR_LEPO_MAX_LATE_PERMIT"].ToString());
    //            salaryDeduct = Convert.ToDouble(dRow["PR_LEPO_LATE_SALARY_DAYS"].ToString());
    //            itemAmt = Convert.ToDouble(dRow["PRST_AMT"].ToString());

    //            if (maxLimit > 0)
    //            {
    //                perCycleAmt = (itemAmt / monthDays) * salaryDeduct;
    //                deductCycle = Convert.ToInt32(Math.Floor(totLeave / maxLimit));

    //                leaveDeductAmt = perCycleAmt * deductCycle;
    //            }
    //            //monthDays = Convert.ToDouble(dRow["TOTAL_LATE"].ToString());
    //        }

    //        dbTransaction.Commit();
    //        con.Close();
    //        return leaveDeductAmt.ToString();

    //    }
    //    catch (Exception ex)
    //    {
    //        //return null;
    //        return ex.Message.ToString();
    //    }
    //}

    public string CalcLeaveAmt(string bId, string eId, string gId, double totLeave)
    {
        string strQuery = "SELECT  EL.EMP_GENDER, PG.PRST_AMT, EL.EMP_NAME, PR_LEPO_ID, PR_LEPO_LEAVE_DEDUCT, "
                            + "PP.CMP_BRANCH_ID, PR_LEPO_MAX_LATE_PERMIT, PR_LEPO_LATE_SALARY_DAYS, PR_LEPO_MONTH_DAY "
                            + "FROM PR_LEAVE_POLICY_DEDUCT PP, PR_PAYROLL_GENERUL PG, PR_EMPLOYEE_LIST EL "
                            + "WHERE EL.CMP_BRANCH_ID='" + bId + "' AND EL.CMP_BRANCH_ID=PG.CMP_BRANCH_ID AND "
                            + "PG.PR_ITEM_ID=PP.PR_LEPO_LEAVE_DEDUCT AND EL.EMP_ID=PG.EMP_ID "
                            + "AND EL.EMP_ID='" + eId + "' AND EL.DSG_ID='" + gId + "'";


        try
        {
            con = new SqlConnection(cn);
            con.Open();
            DataSet oDS = new DataSet();
            SqlDataAdapter odaData = new SqlDataAdapter(new SqlCommand(strQuery, con));
            odaData.Fill(oDS, "PR_LEAVE_POLICY");

            double leaveDeductAmt = 0;
            double monthDays = 0;
            double maxLimit = 0;
            double salaryDeduct = 0;
            double itemAmt = 0;
            double perCycleAmt = 0;
            int deductCycle = 0;

            if (oDS.Tables["PR_LEAVE_POLICY"].Rows.Count > 0)
            {
                DataRow dRow = oDS.Tables["PR_LEAVE_POLICY"].Rows[0];

                monthDays = Convert.ToDouble(dRow["PR_LEPO_MONTH_DAY"].ToString());
                maxLimit = Convert.ToDouble(dRow["PR_LEPO_MAX_LATE_PERMIT"].ToString());
                salaryDeduct = Convert.ToDouble(dRow["PR_LEPO_LATE_SALARY_DAYS"].ToString());
                itemAmt = Convert.ToDouble(dRow["PRST_AMT"].ToString());

                perCycleAmt = (itemAmt / monthDays) * salaryDeduct;
                deductCycle = Convert.ToInt32(Math.Floor(totLeave / maxLimit));

                leaveDeductAmt = Math.Round((perCycleAmt * deductCycle), 2);
            }

            return leaveDeductAmt.ToString();
        }
        catch (Exception ex)
        {
            //return null;
            return ex.Message.ToString();
        }
        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }
    }
    //sydur 24-02-2014
    public string GetTotalLeave(string strEmpId, string month)
    {
        //string strQuery = "SELECT nvl(SUM(LVE_DURATION),0) TOTAL_LEAVE FROM PR_LEAVE WHERE EMP_ID ='" + strEmpId + "' "
        //                    + "AND APPLY_LEAVE_DEDUCT='Y' AND LVE_STATUS='R' AND (TO_CHAR(LVE_FROM_DATE, 'MM/YYYY')='" + month + "')";
        string strQuery = "SELECT ISNULL(SUM(CONVERT(FLOAT, LVE_DURATION)), 0) TOTAL_LEAVE FROM  PR_LEAVE WHERE EMP_ID ='" + strEmpId + "' "
                            + "AND APPLY_LEAVE_DEDUCT='Y' AND LVE_STATUS='R' AND (CONVERT(VARCHAR(23), LVE_FROM_DATE) = '" + month + "')";


        try
        {
            con = new SqlConnection(cn);
            con.Open();
            DataSet oDS = new DataSet();
            SqlDataAdapter odaData = new SqlDataAdapter(new SqlCommand(strQuery, con));
            odaData.Fill(oDS, "PR_EMP_LEAVE");
            //return oDS;

            DataRow dRow = oDS.Tables["PR_EMP_LEAVE"].Rows[0];
            return dRow["TOTAL_LEAVE"].ToString();
        }
        catch (Exception ex)
        {
            //return null;
            return ex.Message.ToString();
        }
        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }
    }



    #endregion

    #region Late Deduct

    public string GetLatePolicyInfo(string strFilter)
    {

        try
        {
            con = new SqlConnection(cn);
            con.Open();
            string strSql = "SELECT * FROM PR_PAYROLL_POLICY WHERE " + strFilter;
            DataSet oDs = new DataSet();
            SqlDataAdapter oOrdersDataAdapter = new SqlDataAdapter(new SqlCommand(strSql, con));
            SqlCommandBuilder oOrdersCmdBuilder = new SqlCommandBuilder(oOrdersDataAdapter);
            oOrdersDataAdapter.Fill(oDs, "PR_PAYROLL_POLICY");

            DataRow dRow = oDs.Tables["PR_PAYROLL_POLICY"].Rows[0];
            return dRow["PR_DEDUCTION"].ToString();
        }
        catch (Exception ex)
        {

            return ex.Message.ToString();
        }
        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }
    }

    //public string CalcLateAmt(string bId, string eId, string gId, double totLate) //string tCode, string gender, 
    //{
    //    string strQuery = "SELECT  EL.EMP_GENDER, PG.PRST_AMT, EL.EMP_NAME, PR_POLICY_ID, PR_POLICY_LATE_DEDUCT, " 
    //                        + "PP.CMP_BRANCH_ID, PR_MAX_LATE_PERMIT, PR_LATE_SALARY_DAYS, PR_MONTH_DAY " 
    //                        + "FROM PR_PAYROLL_POLICY PP, PR_PAYROLL_GENERUL PG, PR_EMPLOYEE_LIST EL "
    //                        + "WHERE EL.CMP_BRANCH_ID='"+bId+"' AND EL.CMP_BRANCH_ID=PG.CMP_BRANCH_ID AND "
    //                        + "PG.PR_ITEM_ID=PP.PR_POLICY_LATE_DEDUCT AND EL.EMP_ID=PG.EMP_ID " 
    //                        + "AND EL.EMP_ID='"+eId+"' AND EL.DSG_ID='"+gId+"'";

    //    try
    //    {
    //        SqlConnection
    //        DataSet oDS = new DataSet();
    //        SqlDataAdapter odaData = new SqlDataAdapter(new SqlCommand(strQuery, con));
    //        odaData.Fill(oDS, "PR_LATE_POLICY");
    //        //return oDS;

    //        double lateDeductAmt = 0;
    //        double monthDays = 0;
    //        double maxLimit = 0;
    //        double salaryDeduct = 0;
    //        double itemAmt = 0;
    //        double perCycleAmt = 0;
    //        int deductCycle = 0;

    //        if (oDS.Tables["PR_LATE_POLICY"].Rows.Count > 0)
    //        {
    //            DataRow dRow = oDS.Tables["PR_LATE_POLICY"].Rows[0];

    //            monthDays = Convert.ToDouble(dRow["PR_MONTH_DAY"].ToString());
    //            maxLimit = Convert.ToDouble(dRow["PR_MAX_LATE_PERMIT"].ToString());
    //            salaryDeduct = Convert.ToDouble(dRow["PR_LATE_SALARY_DAYS"].ToString());
    //            itemAmt = Convert.ToDouble(dRow["PRST_AMT"].ToString());

    //            if (maxLimit > 0)
    //            {
    //                perCycleAmt = (itemAmt / monthDays) * salaryDeduct;
    //                deductCycle = Convert.ToInt32(Math.Floor(totLate / maxLimit));

    //                lateDeductAmt = perCycleAmt * deductCycle;
    //            }
    //            //monthDays = Convert.ToDouble(dRow["TOTAL_LATE"].ToString());
    //        }

    //        return lateDeductAmt.ToString();
    //    }
    //    catch (Exception ex)
    //    {
    //        //return null;
    //        return ex.Message.ToString();
    //    }
    //}

    public string CalcLateAmt(string bId, string eId, string gId, double totLate) //string tCode, string gender, 
    {
        string strQuery = "SELECT  EL.EMP_GENDER, PG.PRST_AMT, EL.EMP_NAME, PR_POLICY_ID, PR_POLICY_LATE_DEDUCT, "
                            + "PP.CMP_BRANCH_ID, PR_MAX_LATE_PERMIT, PR_LATE_SALARY_DAYS, PR_MONTH_DAY "
                            + "FROM PR_PAYROLL_POLICY PP, PR_PAYROLL_GENERUL PG, PR_EMPLOYEE_LIST EL "
                            + "WHERE EL.CMP_BRANCH_ID='" + bId + "' AND EL.CMP_BRANCH_ID=PG.CMP_BRANCH_ID AND "
                            + "PG.PR_ITEM_ID=PP.PR_POLICY_LATE_DEDUCT AND EL.EMP_ID=PG.EMP_ID "
                            + "AND EL.EMP_ID='" + eId + "' AND EL.DSG_ID='" + gId + "'";


        try
        {
            con = new SqlConnection(cn);
            con.Open();
            DataSet oDS = new DataSet();
            SqlDataAdapter odaData = new SqlDataAdapter(new SqlCommand(strQuery, con));
            odaData.Fill(oDS, "PR_LATE_POLICY");

            double lateDeductAmt = 0;
            double monthDays = 0;
            double maxLimit = 0;
            double salaryDeduct = 0;
            double itemAmt = 0;
            double perCycleAmt = 0;
            int deductCycle = 0;

            if (oDS.Tables["PR_LATE_POLICY"].Rows.Count > 0)
            {
                DataRow dRow = oDS.Tables["PR_LATE_POLICY"].Rows[0];

                monthDays = Convert.ToDouble(dRow["PR_MONTH_DAY"].ToString());
                maxLimit = Convert.ToDouble(dRow["PR_MAX_LATE_PERMIT"].ToString());
                salaryDeduct = Convert.ToDouble(dRow["PR_LATE_SALARY_DAYS"].ToString());
                itemAmt = Convert.ToDouble(dRow["PRST_AMT"].ToString());

                perCycleAmt = (itemAmt / monthDays) * salaryDeduct;
                deductCycle = Convert.ToInt32(Math.Floor(totLate / maxLimit));

                lateDeductAmt = Math.Round((perCycleAmt * deductCycle), 2);
            }

            return lateDeductAmt.ToString();
        }
        catch (Exception ex)
        {
            return ex.Message.ToString();
        }
        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }
    }

    //public string GetTotalLate(string strEmpId, string month)
    //{
    //    string strQuery = "SELECT COUNT(*) AS TOTAL_LATE FROM PR_EMP_ATTENDENCE WHERE EMP_ID ='" + strEmpId + "' "
    //                    + "AND APPLY_LATE_DEDUCT='Y' AND ATT_LATE_STATUS='YES' AND (TO_CHAR(ATT_DATE_TIME, 'MM/YYYY')='" + month + "')";

    //    try
    //    {
    //        con = new SqlConnection(cn);
    //        con.Open(); 
    //        DataSet oDS = new DataSet();
    //        SqlDataAdapter odaData = new SqlDataAdapter(new SqlCommand(strQuery, con));
    //        odaData.Fill(oDS, "PR_EMP_ATTENDENCE");
    //        //return oDS;

    //        DataRow dRow = oDS.Tables["PR_EMP_ATTENDENCE"].Rows[0];
    //        return dRow["TOTAL_LATE"].ToString();
    //    }
    //    catch (Exception ex)
    //    {
    //        //return null;
    //        return ex.Message.ToString();
    //    }
    //    finally
    //    {
    //        con.Close();
    //        con = null;
    //    }
    //}

    //sydur 24-02-2014

    public string GetTotalLate(string strEmpId, string month)
    {
        //string strQuery = "SELECT COUNT(*) AS TOTAL_LATE FROM PR_EMP_ATTENDENCE WHERE EMP_ID ='" + strEmpId + "' "
        //                + "AND APPLY_LATE_DEDUCT='Y' AND ATT_LATE_STATUS='YES' AND (TO_CHAR(ATT_DATE_TIME, 'MM/YYYY')='" + month + "')";
        string strQuery = "SELECT COUNT(*) AS TOTAL_LATE FROM PR_EMP_ATTENDENCE WHERE EMP_ID ='" + strEmpId + "' "
                        + "AND APPLY_LATE_DEDUCT='Y' AND ATT_LATE_STATUS='YES' AND (CONVERT(VARCHAR(23), ATT_DATE_TIME) = '" + month + "')";
        try
        {
            con = new SqlConnection(cn);
            con.Open();
            DataSet oDS = new DataSet();
            SqlDataAdapter odaData = new SqlDataAdapter(new SqlCommand(strQuery, con));
            odaData.Fill(oDS, "PR_EMP_ATTENDENCE");
            //return oDS;

            DataRow dRow = oDS.Tables["PR_EMP_ATTENDENCE"].Rows[0];
            return dRow["TOTAL_LATE"].ToString();
        }
        catch (Exception ex)
        {
            //return null;
            return ex.Message.ToString();
        }
        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }
    }



    //public string GetMonthYear(string mId)
    //{
    //    string strQuery = "SELECT TO_CHAR(PRM_MONTH, 'MM/YYYY') MONTH FROM PR_PAYROLL_MASTER WHERE PRM_ID = '" + mId + "'";

    //    try
    //    {
    //        con = new SqlConnection(cn);
    //        con.Open();
    //        DataSet oDS = new DataSet();
    //        SqlDataAdapter odaData = new SqlDataAdapter(new SqlCommand(strQuery, con));
    //        odaData.Fill(oDS, "PR_MONTH_YEAR");

    //        DataRow dRow = oDS.Tables["PR_MONTH_YEAR"].Rows[0];
    //        return dRow["MONTH"].ToString();
    //    }
    //    catch (Exception ex)
    //    {
    //        return ex.Message.ToString();
    //    }
    //    finally
    //    {
    //        con.Close();
    //        con = null;
    //    }
    //}

    //sydur 24-02-2014
    public string GetMonthYear(string mId)
    {
        string strQuery = "SELECT CONVERT(VARCHAR(23), PRM_MONTH) MONTH FROM  PR_PAYROLL_MASTER WHERE PRM_ID = '" + mId + "'";

        try
        {
            con = new SqlConnection(cn);
            con.Open();
            DataSet oDS = new DataSet();
            SqlDataAdapter odaData = new SqlDataAdapter(new SqlCommand(strQuery, con));
            odaData.Fill(oDS, "PR_MONTH_YEAR");

            DataRow dRow = oDS.Tables["PR_MONTH_YEAR"].Rows[0];
            return dRow["MONTH"].ToString();
        }
        catch (Exception ex)
        {
            return ex.Message.ToString();
        }
        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }
    }
    #endregion

    //asad
    public DataSet GetTotalOvertimeAmt(string strEmpId, string month)
    {
        string strQuery = " SELECT SUM(CONVERT(FLOAT, OT_AMT)) TOTAL_OAMT FROM  HR_EMP_OVERTIME "
                          + " WHERE EMPLOYEE_ID  = '" + strEmpId + "'  AND	(CONVERT(VARCHAR(23), OT_DATE)  = '" + month + "') ";

        try
        {
            con = new SqlConnection(cn);
            con.Open();
            DataSet oDS = new DataSet();
            SqlDataAdapter odaData = new SqlDataAdapter(new SqlCommand(strQuery, con));
            odaData.Fill(oDS, "HR_EMP_OVERTIME");
            return oDS;
        }
        catch (Exception ex)
        {
            return null;
        }
        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }

    }

    public DataSet GetTotalLeave()
    {
        string strQuery = "SELECT (SUM(PRLP_DAYS)/12) AS TOTAL_DAYS FROM PR_LEAVE_POLICY";

        try
        {
            con = new SqlConnection(cn);
            con.Open();
            DataSet oDS = new DataSet();
            SqlDataAdapter odaData = new SqlDataAdapter(new SqlCommand(strQuery, con));
            odaData.Fill(oDS, "PR_LEAVE_POLICY");
            return oDS;
        }
        catch (Exception ex)
        {
            return null;
        }
        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }
    }

    public DataSet GetTotalPresentAbsent(string strEmpId)
    {
        string strQuery = "SELECT SUM(LVE_DURATION) TOTAL_ABSENT, (30-8-SUM(LVE_DURATION)) TOTAL_PRESENT FROM PR_LEAVE WHERE EMP_ID ='" + strEmpId + "'";

        try
        {
            con = new SqlConnection(cn);
            con.Open();
            DataSet oDS = new DataSet();
            SqlDataAdapter odaData = new SqlDataAdapter(new SqlCommand(strQuery, con));
            odaData.Fill(oDS, "PR_LEAVE");
            return oDS;
        }
        catch (Exception ex)
        {
            return null;
        }
        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }
    }

    public DataSet GetPayrollStructureId(string strEmpId)
    {
        string strQuery = "SELECT PRST_ID FROM PR_STRUCTURE PS, PR_EMPLOYEE_LIST EL, PR_DESIGNATION D "
                            + "WHERE EL.DSG_ID=D.DSG_ID AND D.GRD_ID=PS.GRD_ID AND EMP_ID ='" + strEmpId + "'";

        try
        {
            con = new SqlConnection(cn);
            con.Open();
            DataSet oDS = new DataSet();
            SqlDataAdapter odaData = new SqlDataAdapter(new SqlCommand(strQuery, con));
            odaData.Fill(oDS, "PR_STRUCTURE");
            return oDS;
        }
        catch (Exception ex)
        {
            return null;
        }
        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }
    }

    //public string SaveEmployeeRecord(string empId, string bonusId, string payIn, string month, string loanDeduct, 
    //                                string loanInterest, string totalPresent, string totalAbsent, string totalLeave,
    //                                string leaveDeduct, string advanceDeduct, string totalLate, string lateDeduct,
    //                                string otherDeduct, string comments, string pStrucId, string netIncome) // PayRoll Sheet
    //{
    //    string strSql;

    //    strSql = "SELECT EMP_ID, PRB_ID, PRS_PAID_IN, MONTH_YEAR, PRS_LOAN_DEDUCT, PRS_LOAN_INTEREST, TOTAL_PRESENT, TOTAL_ABSENT, "
    //            + "TOTAL_LEAVE, PRS_LEAVE_DEDUCT, PRS_ADVANCE_DEDUCT, TOTAL_LATE, PRS_LATE_DEDUCT, PRS_OTHER_DEDUCT, "
    //            + "PRS_COMMENTS, PRST_ID, NET_AMOUNT FROM PR_SHEET";

    //    try
    //    {
    //        DataRow oOrderRow;
    //        SqlConnection
    //        con.Open();
    //        dbTransaction = con.BeginTransaction();
    //        DataSet oDS = new DataSet();
    //        SqlDataAdapter oOrdersDataAdapter = new SqlDataAdapter(new SqlCommand(strSql, con,dbTransaction));
    //        SqlCommandBuilder oOrdersCmdBuilder = new SqlCommandBuilder(oOrdersDataAdapter);
    //        oOrdersDataAdapter.MissingSchemaAction = MissingSchemaAction.AddWithKey;
    //        oOrdersDataAdapter.Fill(oDS, "PR_SHEET");



    //        oOrderRow = oDS.Tables["PR_SHEET"].NewRow();

    //        oOrderRow["EMP_ID"] = empId;
    //        oOrderRow["PRB_ID"] = bonusId;
    //        oOrderRow["PRS_PAID_IN"] = payIn;
    //        oOrderRow["MONTH_YEAR"] = month;
    //        oOrderRow["PRS_LOAN_DEDUCT"] = loanDeduct;
    //        oOrderRow["PRS_LOAN_INTEREST"] = loanInterest;
    //        oOrderRow["TOTAL_PRESENT"] = totalPresent;
    //        oOrderRow["TOTAL_ABSENT"] = totalAbsent;
    //        oOrderRow["TOTAL_LEAVE"] = totalLeave;
    //        oOrderRow["PRS_LEAVE_DEDUCT"] = leaveDeduct;
    //        oOrderRow["PRS_ADVANCE_DEDUCT"] = advanceDeduct;
    //        oOrderRow["TOTAL_LATE"] = totalLate;
    //        oOrderRow["PRS_LATE_DEDUCT"] = lateDeduct;
    //        oOrderRow["PRS_OTHER_DEDUCT"] = otherDeduct;
    //        oOrderRow["PRS_COMMENTS"] = comments;
    //        oOrderRow["PRST_ID"] = pStrucId;
    //        oOrderRow["NET_AMOUNT"] = netIncome;

    //        oDS.Tables["PR_SHEET"].Rows.Add(oOrderRow);
    //        oOrdersDataAdapter.Update(oDS, "PR_SHEET");
    //        dbTransaction.Commit();
    //        con.Close();
    //        return "Saved successfully";
    //    }
    //    catch (Exception ex)
    //    {
    //        return ex.Message.ToString();
    //    }

    //    return "Saved successfully";
    //}

    public string AddNewCV(string strCV_NAME, string fPath, string fileSize, string appNo)
    {
        string strSql;
        con = new SqlConnection(cn);
        con.Open();
        strSql = "select CV_NAME,CV_PATH, CV_SIZE, CV_APPLICANT_NO from PR_EMPLOYEE_CV ";

        try
        {
            DataRow oOrderRow;

            con.Open();
            dbTransaction = con.BeginTransaction();
            DataSet oDS = new DataSet();
            SqlDataAdapter oOrdersDataAdapter = new SqlDataAdapter(new SqlCommand(strSql, con, dbTransaction));
            SqlCommandBuilder oOrdersCmdBuilder = new SqlCommandBuilder(oOrdersDataAdapter);
            //oOrdersDataAdapter.MissingSchemaAction = MissingSchemaAction.AddWithKey;
            //oOrdersDataAdapter.Fill(oDS, "PR_EMPLOYEE_CV");

            oOrdersDataAdapter.FillSchema(oDS, SchemaType.Source, "PR_EMPLOYEE_CV");
            oOrderRow = oDS.Tables["PR_EMPLOYEE_CV"].NewRow();

            oOrderRow["CV_NAME"] = strCV_NAME;
            oOrderRow["CV_PATH"] = fPath;
            oOrderRow["CV_SIZE"] = fileSize;
            oOrderRow["CV_APPLICANT_NO"] = appNo;

            oDS.Tables["PR_EMPLOYEE_CV"].Rows.Add(oOrderRow);
            oOrdersDataAdapter.Update(oDS, "PR_EMPLOYEE_CV");
            dbTransaction.Commit();
            //con.Close();
            return "Saved successfully";

        }
        catch (Exception ex)
        {
            return ex.Message.ToString();
        }
        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }

    }

    //public DataSet GetSalaryStructure(string bId, string strgId, string strGender)
    //{
    //    string strQuery = "SELECT PRST_BASIC, PRST_HOUSE_RENT, PRST_CONVENYANCE, PRST_UTILITIES, PRST_MEDICAL, PRST_ENTERTAINMENT, "
    //                        + "PRST_PS_SUBSIDY, PRST_HOUSE_MTS, PRST_WAGES, PRST_DIRECTOR_FEES, PRST_LUMSUM, PRS_PROVIDENT_FUND, "
    //                        + "PRST_GRATUITY, PRST_FOOD_ALLOWANCE, PRST_SP_ALLOWANCES, "
    //                        + "PRST_ID, CMP_BRANCH_ID, DSG_ID FROM PR_STRUCTURE "
    //                        + "WHERE CMP_BRANCH_ID = '" + bId + "' AND DSG_ID = '" + strgId + "' AND GENDER = '" + strGender + "'";

    //    //string strQuery = "SELECT DISTINCT PRST_BASIC, PRST_HOUSE_RENT, PRST_CONVENYANCE, PRST_UTILITIES, PRST_MEDICAL, PRST_ENTERTAINMENT, "
    //    //                    + "PRST_PS_SUBSIDY, PRST_HOUSE_MTS, PRST_WAGES, PRST_DIRECTOR_FEES, PRST_LUMSUM, PRS_PROVIDENT_FUND, "
    //    //                    + "PRST_ID, PS.GRD_ID, TAX_SLAV_ID FROM PR_STRUCTURE PS, PR_DESIGNATION D "
    //    //                    + "WHERE PS.GRD_ID=D.GRD_ID AND D.DSG_ID='" + strDesigId + "'";
    //    try
    //    {
    //        SqlConnection
    //        DataSet oDS = new DataSet();
    //        SqlDataAdapter odaData = new SqlDataAdapter(new SqlCommand(strQuery, con));
    //        odaData.Fill(oDS, "PR_STRUCTURE");
    //        return oDS;
    //    }
    //    catch (Exception ex)
    //    {
    //        return null;
    //    }

    //}

    #region Tax Calculation

    public DataSet GetSalaryStructureDynamic(string bId, string strgId, string strGender, string strEmpT)
    {
        string strQuery = "SELECT PR_ITEM_TITLE, PI.PR_ITEM_ID, PR_ITEM_IS_TAXABLE, PR_ITNATURE_CODE, ACC_INT_ID, "
                            + "PI.CMP_BRANCH_ID, PRST_ID, DSG_ID, PRST_AMT, PS.TYP_CODE "
                            + "FROM PR_PAYROLL_ITEM  PI, PR_STRUCTURE PS "
                            + "WHERE PI.PR_ITEM_ID=PS.PR_ITEM_ID AND PI.CMP_BRANCH_ID=PS.CMP_BRANCH_ID "
                            + "AND PI.CMP_BRANCH_ID = '" + bId + "' AND DSG_ID = '" + strgId + "' AND GENDER = '" + strGender + "' AND PS.TYP_CODE = '" + strEmpT + "' "
                            + "ORDER BY PR_ITEM_TITLE ASC";

        //string strQuery = "SELECT DISTINCT PRST_BASIC, PRST_HOUSE_RENT, PRST_CONVENYANCE, PRST_UTILITIES, PRST_MEDICAL, PRST_ENTERTAINMENT, "
        //                    + "PRST_PS_SUBSIDY, PRST_HOUSE_MTS, PRST_WAGES, PRST_DIRECTOR_FEES, PRST_LUMSUM, PRS_PROVIDENT_FUND, "
        //                    + "PRST_ID, PS.GRD_ID, TAX_SLAV_ID FROM PR_STRUCTURE PS, PR_DESIGNATION D "
        //                    + "WHERE PS.GRD_ID=D.GRD_ID AND D.DSG_ID='" + strDesigId + "'";

        try
        {
            con = new SqlConnection(cn);
            con.Open();
            DataSet oDS = new DataSet();
            SqlDataAdapter odaData = new SqlDataAdapter(new SqlCommand(strQuery, con));
            odaData.Fill(oDS, "PR_STRUCTURE");
            return oDS;
        }
        catch (Exception ex)
        {
            return null;
        }
        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }
    }

    public DataSet GetSalaryStructureDynamic(string bId, string strdId, string strgId, string strGender, string strEmpT, string allbranch)
    {
        string strQuery = "SELECT PR_ITEM_TITLE, PI.PR_ITEM_ID, PR_ITEM_IS_TAXABLE, PR_ITNATURE_CODE, ACC_INT_ID, "
                            + "PI.CMP_BRANCH_ID, PRST_ID, DSG_ID, PRST_AMT, PS.TYP_CODE "
                            + "FROM PR_PAYROLL_ITEM  PI, PR_STRUCTURE PS "
                            + "WHERE PI.PR_ITEM_ID=PS.PR_ITEM_ID   AND (PI.CMP_BRANCH_ID=PS.CMP_BRANCH_ID or PI.CMP_BRANCH_ID='" + allbranch + "') "
                            + "AND (PI.CMP_BRANCH_ID = '" + bId + "' or PI.CMP_BRANCH_ID='" + allbranch + "') AND DPT_ID = '" + strdId + "' AND DSG_ID = '" + strgId + "' AND GENDER = '" + strGender + "' AND PS.TYP_CODE = '" + strEmpT + "' "
                            + "ORDER BY PR_ITEM_ORDER ASC";

        //string strQuery = "SELECT DISTINCT PRST_BASIC, PRST_HOUSE_RENT, PRST_CONVENYANCE, PRST_UTILITIES, PRST_MEDICAL, PRST_ENTERTAINMENT, "
        //                    + "PRST_PS_SUBSIDY, PRST_HOUSE_MTS, PRST_WAGES, PRST_DIRECTOR_FEES, PRST_LUMSUM, PRS_PROVIDENT_FUND, "
        //                    + "PRST_ID, PS.GRD_ID, TAX_SLAV_ID FROM PR_STRUCTURE PS, PR_DESIGNATION D "
        //                    + "WHERE PS.GRD_ID=D.GRD_ID AND D.DSG_ID='" + strDesigId + "'";
        try
        {
            con = new SqlConnection(cn);
            con.Open();
            DataSet oDS = new DataSet();
            SqlDataAdapter odaData = new SqlDataAdapter(new SqlCommand(strQuery, con));
            odaData.Fill(oDS, "PR_STRUCTURE");
            return oDS;
        }
        catch (Exception ex)
        {
            return null;
        }
        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }

    }

    public string SaveTaxCalculation(
                                    string bId,
                                    string gId,
                                    string strGender,
                                    string totIncome,
                                    string totTax,
                                    string taxCreateDate,
                                    string totTaxPeriod,
                                    string taxPerMonth,
                                    string strEmpTyp) // Tax Calculation
    {
        string strSql;

        //strSql = "SELECT EMP_ID, TOT_INCOME, TOT_TAX, TAX_CREATE_DATE, TOT_TAX_PERIOD, TAX_PER_MONTH FROM PR_TAX_CALCULATION";
        strSql = "SELECT CMP_BRANCH_ID, DSG_ID, EMP_GENDER, TOT_INCOME, TOT_TAX, TAX_CREATE_DATE, TOT_TAX_PERIOD, TAX_PER_MONTH, TYP_CODE FROM PR_TAX_CALCULATION";

        try
        {
            DataRow oOrderRow;

            con = new SqlConnection(cn);
            con.Open();
            dbTransaction = con.BeginTransaction();
            DataSet oDS = new DataSet();
            SqlDataAdapter oOrdersDataAdapter = new SqlDataAdapter(new SqlCommand(strSql, con, dbTransaction));
            SqlCommandBuilder oOrdersCmdBuilder = new SqlCommandBuilder(oOrdersDataAdapter);
            //oOrdersDataAdapter.MissingSchemaAction = MissingSchemaAction.AddWithKey;
            //oOrdersDataAdapter.Fill(oDS, "PR_TAX_CALCULATION");

            oOrdersDataAdapter.FillSchema(oDS, SchemaType.Source, "PR_TAX_CALCULATION");
            oOrderRow = oDS.Tables["PR_TAX_CALCULATION"].NewRow();

            //oOrderRow["EMP_ID"] = empId;
            oOrderRow["CMP_BRANCH_ID"] = bId;
            oOrderRow["DSG_ID"] = gId;
            oOrderRow["EMP_GENDER"] = strGender;
            oOrderRow["TOT_INCOME"] = totIncome;
            oOrderRow["TOT_TAX"] = totTax;
            oOrderRow["TAX_CREATE_DATE"] = taxCreateDate;
            oOrderRow["TOT_TAX_PERIOD"] = totTaxPeriod;
            oOrderRow["TAX_PER_MONTH"] = taxPerMonth;
            oOrderRow["TYP_CODE"] = strEmpTyp;

            oDS.Tables["PR_TAX_CALCULATION"].Rows.Add(oOrderRow);
            oOrdersDataAdapter.Update(oDS, "PR_TAX_CALCULATION");
            dbTransaction.Commit();
            //con.Close();
            return "Saved successfully";
        }
        catch (Exception ex)
        {
            return ex.Message.ToString();
        }
        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }

    }

    public string SaveTaxCalculation(
                                        string bId,
                                        string dId,
                                        string gId,
                                        string strGender,
                                        string totIncome,
                                        string totTax,
                                        string taxCreateDate,
                                        string totTaxPeriod,
                                        string taxPerMonth,
                                        string strEmpTyp) // Tax Calculation
    {
        string strSql;

        //strSql = "SELECT EMP_ID, TOT_INCOME, TOT_TAX, TAX_CREATE_DATE, TOT_TAX_PERIOD, TAX_PER_MONTH FROM PR_TAX_CALCULATION";
        strSql = "SELECT CMP_BRANCH_ID, DPT_ID, DSG_ID, EMP_GENDER, TOT_INCOME, TOT_TAX, TAX_CREATE_DATE, TOT_TAX_PERIOD, TAX_PER_MONTH, TYP_CODE FROM PR_TAX_CALCULATION";

        try
        {
            DataRow oOrderRow;

            con = new SqlConnection(cn);
            con.Open();
            dbTransaction = con.BeginTransaction();
            DataSet oDS = new DataSet();
            SqlDataAdapter oOrdersDataAdapter = new SqlDataAdapter(new SqlCommand(strSql, con, dbTransaction));
            SqlCommandBuilder oOrdersCmdBuilder = new SqlCommandBuilder(oOrdersDataAdapter);
            //oOrdersDataAdapter.MissingSchemaAction = MissingSchemaAction.AddWithKey;
            //oOrdersDataAdapter.Fill(oDS, "PR_TAX_CALCULATION");

            oOrdersDataAdapter.FillSchema(oDS, SchemaType.Source, "PR_TAX_CALCULATION");
            oOrderRow = oDS.Tables["PR_TAX_CALCULATION"].NewRow();

            oOrderRow["CMP_BRANCH_ID"] = bId;
            oOrderRow["DPT_ID"] = dId;
            oOrderRow["DSG_ID"] = gId;
            oOrderRow["EMP_GENDER"] = strGender;
            oOrderRow["TOT_INCOME"] = totIncome;
            oOrderRow["TOT_TAX"] = totTax;
            oOrderRow["TAX_CREATE_DATE"] = taxCreateDate;
            oOrderRow["TOT_TAX_PERIOD"] = totTaxPeriod;
            oOrderRow["TAX_PER_MONTH"] = taxPerMonth;
            oOrderRow["TYP_CODE"] = strEmpTyp;

            oDS.Tables["PR_TAX_CALCULATION"].Rows.Add(oOrderRow);
            oOrdersDataAdapter.Update(oDS, "PR_TAX_CALCULATION");
            dbTransaction.Commit();
            // con.Close();
            return "Saved successfully";
        }
        catch (Exception ex)
        {
            return ex.Message.ToString();
        }
        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }

    }

    public bool CheckEmpTaxRecord(string strbId, string strgId, string strGender, string strEmTyp)
    {
        string strSql;

        //strSql = "select PRT_CALC_ID from PR_TAX_CALCULATION WHERE EMP_ID = '" + strEmpId + "'";
        strSql = "select PRT_CALC_ID from PR_TAX_CALCULATION WHERE CMP_BRANCH_ID = '" + strbId + "' AND DSG_ID = '" + strgId + "' AND EMP_GENDER = '" + strGender + "' AND TYP_CODE = '" + strEmTyp + "'";

        try
        {
            //DataRow oOrderRow;
            con = new SqlConnection(cn);
            con.Open();
            DataSet oDS = new DataSet();
            SqlDataAdapter oOrdersDataAdapter = new SqlDataAdapter(new SqlCommand(strSql, con));
            SqlCommandBuilder oOrdersCmdBuilder = new SqlCommandBuilder(oOrdersDataAdapter);
            oOrdersDataAdapter.MissingSchemaAction = MissingSchemaAction.AddWithKey;
            oOrdersDataAdapter.Fill(oDS, "PR_TAX_CALCULATION");

            DataTable tbl_AD = oDS.Tables["PR_TAX_CALCULATION"];

            // already exists
            if (tbl_AD.Rows.Count > 0) { return true; }
            else { return false; }
        }
        catch (Exception ex)
        {
            return false;
        }
        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }
    }

    public bool CheckEmpTaxRecord(string strbId, string strdId, string strgId, string strGender, string strEmTyp)
    {
        string strSql;

        //strSql = "select PRT_CALC_ID from PR_TAX_CALCULATION WHERE EMP_ID = '" + strEmpId + "'";
        strSql = "select PRT_CALC_ID from PR_TAX_CALCULATION WHERE CMP_BRANCH_ID = '" + strbId + "' AND DPT_ID = '" + strdId + "' AND DSG_ID = '" + strgId + "' AND EMP_GENDER = '" + strGender + "' AND TYP_CODE = '" + strEmTyp + "'";

        try
        {
            //DataRow oOrderRow;
            con = new SqlConnection(cn);
            con.Open();
            DataSet oDS = new DataSet();
            SqlDataAdapter oOrdersDataAdapter = new SqlDataAdapter(new SqlCommand(strSql, con));
            SqlCommandBuilder oOrdersCmdBuilder = new SqlCommandBuilder(oOrdersDataAdapter);
            oOrdersDataAdapter.MissingSchemaAction = MissingSchemaAction.AddWithKey;
            oOrdersDataAdapter.Fill(oDS, "PR_TAX_CALCULATION");

            DataTable tbl_AD = oDS.Tables["PR_TAX_CALCULATION"];

            // already exists
            if (tbl_AD.Rows.Count > 0) { return true; }
            else { return false; }
        }
        catch (Exception ex)
        {
            return false;
        }
        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }
    }

    #endregion

    public DataSet GetEmpGender(string eId)
    {
        string strQuery = "SELECT EMP_GENDER FROM PR_EMPLOYEE_LIST WHERE EMP_ID='" + eId + "'";


        try
        {
            con = new SqlConnection(cn);
            con.Open();
            DataSet oDS = new DataSet();
            SqlDataAdapter odaData = new SqlDataAdapter(new SqlCommand(strQuery, con));
            odaData.Fill(oDS, "PR_EMPLOYEE_LIST");
            return oDS;
        }
        catch (Exception ex)
        {
            return null;
        }
        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }
    }

    public DataSet GetAssignedEmployee(string strTaskId)
    {
        string strQuery = "SELECT (CAST(ISNULL(EL.EMP_TITLE, '') AS VARCHAR) + ' ' + CAST(EL.EMP_NAME AS VARCHAR)) as ENAME "
                            + "FROM PR_EMPLOYEE_LIST EL, PR_ASSIGNMENT_DETAIL AD, PR_ASSIGNMENT AG "
                            + "WHERE EL.EMP_ID=AD.EMP_ID AND AD.ASGN_ID=AG.ASGN_ID AND AG.ASGN_ID='" + strTaskId + "'";

        try
        {
            con = new SqlConnection(cn);
            con.Open();
            DataSet oDS = new DataSet();
            SqlDataAdapter odaData = new SqlDataAdapter(new SqlCommand(strQuery, con));
            odaData.Fill(oDS, "PR_ASSIGNMENT");
            return oDS;
        }
        catch (Exception ex)
        {
            return null;
        }
        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }

    }

    public DataSet GetEmpTaxRecord(string eId)
    {
        string strQuery = "SELECT TOT_INCOME, TAX_PER_MONTH FROM PR_TAX_CALCULATION WHERE EMP_ID='" + eId + "'";

        try
        {
            con = new SqlConnection(cn);
            con.Open();
            DataSet oDS = new DataSet();
            SqlDataAdapter odaData = new SqlDataAdapter(new SqlCommand(strQuery, con));
            odaData.Fill(oDS, "PR_TAX_CALCULATION");
            return oDS;
        }
        catch (Exception ex)
        {
            return null;
        }
        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }
    }

    //public DataSet GetBonusAmt(string bonusId)
    //{
    //    string strQuery = "SELECT PRB_AMOUNT FROM PR_BONUS WHERE PRB_ID='"+bonusId+"'";
    //    try
    //    {
    //        SqlConnection
    //        DataSet oDS = new DataSet();
    //        SqlDataAdapter odaData = new SqlDataAdapter(new SqlCommand(strQuery, con));
    //        odaData.Fill(oDS, "PR_BONUS");
    //        return oDS;
    //    }
    //    catch (Exception ex)
    //    {
    //        return null;
    //    }
    //}

    #region ATTENDANCE

    public DataSet GetEmpAttdStatus(string bId, string eId)
    {
        #region old
        //string strQuery = "SELECT TO_CHAR(ATT_DATE_TIME, 'DD-MM-YYYY HH:MI:SS') AS DATE_TIME, ATT_TYPE "
        //                    + "FROM PR_EMP_ATTENDENCE "
        //                    + "WHERE EMP_ID='" + eId + "' AND CMP_BRANCH_ID='" + bId + "' "
        //    //+ "AND TO_CHAR(ATT_DATE_TIME, 'DD-MM-YYYY')=TO_CHAR(CURRENT_DATE, 'DD-MM-YYYY')"
        //                    + "AND TO_CHAR(ATT_DATE_TIME, 'DD-MM-YYYY')=TO_CHAR(SYSDATE, 'DD-MM-YYYY')"
        //                    + "AND ATT_DATE_TIME= "
        //                    + "(SELECT MAX(ATT_DATE_TIME) FROM PR_EMP_ATTENDENCE "
        //                    + "WHERE EMP_ID='" + eId + "' AND CMP_BRANCH_ID='" + bId + "' AND "
        //                    + "TO_CHAR(ATT_DATE_TIME, 'DD-MM-YYYY')=TO_CHAR(CURRENT_DATE, 'DD-MM-YYYY'))";
        #endregion
        //sayd
        string strQuery = "SELECT CONVERT(VARCHAR(23), ATT_DATE_TIME, 105) AS DATE_TIME,ATT_TYPE"
                           + " FROM PR_EMP_ATTENDENCE "
                           + "WHERE EMP_ID='" + eId + "' AND CMP_BRANCH_ID='" + bId + "' "
            //+ "AND TO_CHAR(ATT_DATE_TIME, 'DD-MM-YYYY')=TO_CHAR(CURRENT_DATE, 'DD-MM-YYYY')"
                           + " AND	CONVERT(VARCHAR(23), ATT_DATE_TIME, 105)  = CONVERT(VARCHAR(23), GETDATE(), 105)"
                           + "AND ATT_DATE_TIME= "
                           + "(SELECT MAX(ATT_DATE_TIME) FROM PR_EMP_ATTENDENCE "
                           + "WHERE EMP_ID='" + eId + "' AND CMP_BRANCH_ID='" + bId + "' AND "
                           + "CONVERT(VARCHAR(23), ATT_DATE_TIME, 105)  = CONVERT(VARCHAR(23), GETDATE(), 105))";

        try
        {
            con = new SqlConnection(cn);
            con.Open();
            DataSet oDS = new DataSet();
            SqlDataAdapter odaData = new SqlDataAdapter(new SqlCommand(strQuery, con));
            odaData.Fill(oDS, "PR_EMP_ATTENDENCE");
            return oDS;
        }
        catch (Exception ex)
        {
            return null;
        }
        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }
    }

    public DataSet GetUserEmpId(string uId, string bId) // get employee id from user id & branch id
    {
        string strQuery = "SELECT EMP_ID,EMP_CODE,LTRIM(CAST(ISNULL(EL.EMP_TITLE, '') AS VARCHAR(100)) + ' ' + CAST(EL.EMP_NAME AS VARCHAR(500)))  AS EMP_NAME, DSG_ID, DPT_ID,(SELECT PD.DPT_NAME FROM PR_DEPARTMENT PD WHERE PD.DPT_ID=EL.DPT_ID)DPT_NAME,MANAGEMENT_EMPLOYEE FROM PR_EMPLOYEE_LIST EL "
                            + "WHERE SYS_USR_ID = '" + uId + "' AND CMP_BRANCH_ID='" + bId + "'";



        try
        {
            con = new SqlConnection(cn);
            con.Open();
            DataSet oDS = new DataSet();
            SqlDataAdapter odaData = new SqlDataAdapter(new SqlCommand(strQuery, con));
            odaData.Fill(oDS, "PR_EMPLOYEE_LIST");
            return oDS;
        }
        catch (Exception ex)
        {
            return null;
        }
        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }
    }

    public DataSet GetUserEmpIdExceptDptHead(string uId, string bId) // get employee id from user id & branch id
    {
        string strQuery = "SELECT EMP_ID,EMP_CODE,LTRIM(CAST(ISNULL(EL.EMP_TITLE, '') AS VARCHAR(100)) + ' ' + CAST(EL.EMP_NAME AS VARCHAR(500)))  AS EMP_NAME, DSG_ID, DPT_ID,(SELECT PD.DPT_NAME FROM PR_DEPARTMENT PD WHERE PD.DPT_ID=EL.DPT_ID)DPT_NAME,MANAGEMENT_EMPLOYEE FROM PR_EMPLOYEE_LIST EL "
                            + "WHERE SYS_USR_ID = '" + uId + "' AND CMP_BRANCH_ID='" + bId + "' and EL.HEAD_OF_DEPT='N'";



        try
        {
            con = new SqlConnection(cn);
            con.Open();
            DataSet oDS = new DataSet();
            SqlDataAdapter odaData = new SqlDataAdapter(new SqlCommand(strQuery, con));
            odaData.Fill(oDS, "PR_EMPLOYEE_LIST");
            return oDS;
        }
        catch (Exception ex)
        {
            return null;
        }
        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }
    }
    public DataSet GetDeptIdasEmpId(string empId) // get employee id from user id & branch id
    {
        string strQuery = " select el.DPT_ID from PR_EMPLOYEE_LIST el where el.EMP_ID='" + empId + "' ";

        try
        {
            con = new SqlConnection(cn);
            con.Open();
            DataSet oDS = new DataSet();
            SqlDataAdapter odaData = new SqlDataAdapter(new SqlCommand(strQuery, con));
            odaData.Fill(oDS, "deptId");
            return oDS;
        }
        catch (Exception ex)
        {
            return null;
        }
        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }
    }
    ///<summary>
    /// Author:Tanjil Alam
    /// Date: 10/04/2014
    /// 
    /// </summary>
    /// 
    /// <returns></returns>
    /// 




    #endregion
    public DataSet GetEmployeeId(string dateFrom, string dateTo, string bId)
    {
        //string strQuery = "SELECT DISTINCT EMP_ID FROM PR_EMP_ATTENDENCE "
        //                    + "WHERE TO_DATE(TO_CHAR(ATT_DATE_TIME,'MM/DD/YYYY'),'MM/DD/YYYY') BETWEEN TO_DATE('" + dateFrom + "', 'MM/DD/YYYY') "
        //                    + "AND TO_DATE('" + dateTo + "', 'MM/DD/YYYY') "
        //                    + "AND CMP_BRANCH_ID='" + bId + "'";


        string strQuery = "SELECT DISTINCT EMP_ID FROM  PR_EMP_ATTENDENCE  "
                        + "WHERE CONVERT(DATETIME, CONVERT(VARCHAR(23), ATT_DATE_TIME, 101), 101)  BETWEEN CONVERT(DATETIME, '" + dateFrom + "', 101)  AND  CONVERT(DATETIME, '" + dateTo + "', 101)  AND	CMP_BRANCH_ID  = '" + bId + "' ";

        try
        {
            con = new SqlConnection(cn);
            con.Open();
            DataSet oDS = new DataSet();
            SqlDataAdapter odaData = new SqlDataAdapter(new SqlCommand(strQuery, con));
            odaData.Fill(oDS, "PR_EMP_ATTENDENCE");
            return oDS;
        }
        catch (Exception ex)
        {
            return null;
        }
        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }
    }

    public DataSet GetEmpInfo(string bId, string eId)
    {
        string strQuery = "SELECT EL.EMP_NAME, EL.DPT_ID, EL.DSG_ID, PG.SET_CODE, PG.SET_LEVEL,EL.MANAGEMENT_EMPLOYEE "
                            + "FROM PR_EMPLOYEE_LIST EL, PR_DESIGNATION PG "
                            + "WHERE EL.DSG_ID=PG.DSG_ID AND " //EL.CMP_BRANCH_ID=PG.CMP_BRANCH_ID AND EL.DPT_ID=PG.DPT_ID AND 
                            + "EL.EMP_ID = '" + eId + "' AND EL.CMP_BRANCH_ID='" + bId + "'";


        try
        {
            con = new SqlConnection(cn);
            con.Open();
            DataSet oDS = new DataSet();
            SqlDataAdapter odaData = new SqlDataAdapter(new SqlCommand(strQuery, con));
            odaData.Fill(oDS, "EMPLOYEE_INFO");
            return oDS;
        }
        catch (Exception ex)
        {
            return null;
        }
        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }
    }

    public DataSet GetPrItemAmtDetail(string bId, string eId, string gId, string itemId, string prMonth)
    {
        string strQuery = "SELECT PRST_AMT FROM PR_PAYROLL_DETAIL "
                            + "WHERE CMP_BRANCH_ID='" + bId + "' AND DSG_ID='" + gId + "' AND EMP_ID='" + eId + "' "
                            + "AND PR_ITEM_ID='" + itemId + "' AND PRM_ID='" + prMonth + "'";


        try
        {
            con = new SqlConnection(cn);
            con.Open();
            DataSet oDS = new DataSet();
            SqlDataAdapter odaData = new SqlDataAdapter(new SqlCommand(strQuery, con));
            odaData.Fill(oDS, "PR_PAYROLL_DETAIL");
            return oDS;
        }
        catch (Exception ex)
        {
            return null;
        }
        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }
    }

    //public DataSet GetPrItemAmtDetail(string bId, string eId, string itemId, string prMonth)
    //{
    //    string strQuery = "SELECT PRST_AMT FROM PR_PAYROLL_DETAIL "
    //                        + "WHERE CMP_BRANCH_ID='" + bId + "' AND EMP_ID='" + eId + "' "
    //                        + "AND PR_ITEM_ID='" + itemId + "' AND PRM_ID='" + prMonth + "'";

    //    try
    //    {
    //        SqlConnection
    //        DataSet oDS = new DataSet();
    //        SqlDataAdapter odaData = new SqlDataAdapter(new SqlCommand(strQuery, con));
    //        odaData.Fill(oDS, "PR_PAYROLL_DETAIL");
    //        return oDS;
    //    }
    //    catch (Exception ex)
    //    {
    //        return null;
    //    }
    //}

    #region Payroll General

    public string SaveEmpPayrollInfo(
                                string bId,
                                string empId,
                                string gId,
                                string totTax,
                                string taxPerMonth,
                                string totIncome)
    {
        string strSql;

        strSql = "SELECT TOT_TAX, TAX_PER_MONTH, TOT_INCOME, EMP_ID, DSG_ID, CMP_BRANCH_ID "
                + "FROM PR_EMPLOYEE_LIST WHERE EMP_ID = '" + empId + "' AND DSG_ID = '" + gId + "' AND CMP_BRANCH_ID = '" + bId + "'";


        try
        {
            DataRow oOrderRow;

            con = new SqlConnection(cn);
            con.Open();
            dbTransaction = con.BeginTransaction();
            DataSet oDs = new DataSet();
            SqlDataAdapter oDbAdapter = new SqlDataAdapter(new SqlCommand(strSql, con, dbTransaction));
            SqlCommandBuilder oDbCmdBuilder = new SqlCommandBuilder(oDbAdapter);
            oDbAdapter.MissingSchemaAction = MissingSchemaAction.AddWithKey;
            oDbAdapter.Fill(oDs, "EMP_PR_INFO");

            // UPDATE Data
            oOrderRow = oDs.Tables["EMP_PR_INFO"].Rows.Find(empId);

            oOrderRow["TOT_TAX"] = totTax;
            oOrderRow["TAX_PER_MONTH"] = taxPerMonth;
            oOrderRow["TOT_INCOME"] = totIncome;

            oDbAdapter.Update(oDs, "EMP_PR_INFO");
            dbTransaction.Commit();
            //con.Close();

            return "Saved successfully";

        }
        catch (Exception ex)
        {
            return ex.Message.ToString();
        }
        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }

        //return "Saved successfully";
    }


    public DataSet GetPrItemGeneral(string bId, string eId, string gId, string itemId) // get individual employee payroll item amount
    {
        string strQuery = "SELECT PRST_AMT FROM PR_PAYROLL_GENERUL "
                            + "WHERE CMP_BRANCH_ID='" + bId + "' AND DSG_ID='" + gId + "' AND PR_ITEM_ID='" + itemId + "' AND EMP_ID='" + eId + "'";

        try
        {
            con = new SqlConnection(cn);
            con.Open();
            DataSet oDS = new DataSet();
            SqlDataAdapter odaData = new SqlDataAdapter(new SqlCommand(strQuery, con));
            odaData.Fill(oDS, "EMP_PAYROLL_ITEM");
            return oDS;
        }
        catch (Exception ex)
        {
            return null;
        }
        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }
    }

    public DataSet GetPrItem(string bId, string AllBid)
    {
        string strSQL = "";
        strSQL = " (CMP_BRANCH_ID='" + bId + "' OR CMP_BRANCH_ID = '" + AllBid + "')";
        string strQuery = "SELECT * FROM PR_PAYROLL_ITEM WHERE "
                            + strSQL
                            + " ORDER BY PR_ITEM_ORDER ASC";

        try
        {
            con = new SqlConnection(cn);
            con.Open();
            DataSet oDS = new DataSet();
            SqlDataAdapter odaData = new SqlDataAdapter(new SqlCommand(strQuery, con));
            odaData.Fill(oDS, "PR_PAYROLL_ITEM");
            return oDS;
        }
        catch (Exception ex)
        {
            return null;
        }
        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }
    }

    public DataSet GetPrItem(string bId, string gId, string itemId, string strGender)
    {
        string strQuery = "SELECT PRST_AMT FROM PR_STRUCTURE "
                            + "WHERE CMP_BRANCH_ID='" + bId + "' AND DSG_ID='" + gId + "' AND PR_ITEM_ID='" + itemId + "' AND GENDER='" + strGender + "'";

        try
        {
            con = new SqlConnection(cn);
            con.Open();
            DataSet oDS = new DataSet();
            SqlDataAdapter odaData = new SqlDataAdapter(new SqlCommand(strQuery, con));
            odaData.Fill(oDS, "PR_STRUCTURE");
            return oDS;
        }
        catch (Exception ex)
        {
            return null;
        }
        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }
    }

    public DataSet GetPrItemAmt(string bId, string gId, string itemId, string strGender, string strEtype, string allbranch)
    {
        string strSQL = "";

        //if (bId != "100310001") // when not all branch
        //{
        strSQL = " (CMP_BRANCH_ID='" + bId + "' OR CMP_BRANCH_ID = '" + allbranch + "') ";
        //}
        //else
        //{
        //    strSQL = " CMP_BRANCH_ID='" + bId + "' ";
        //}

        string strQuery = "SELECT PRST_AMT FROM PR_STRUCTURE "
                            + "WHERE "
                            + strSQL + " "
            //CMP_BRANCH_ID='" + bId + "'
                            + " AND DSG_ID='" + gId + "' AND PR_ITEM_ID='" + itemId + "' AND GENDER='" + strGender + "' AND TYP_CODE='" + strEtype + "'";

        try
        {
            con = new SqlConnection(cn);
            con.Open();
            DataSet oDS = new DataSet();
            SqlDataAdapter odaData = new SqlDataAdapter(new SqlCommand(strQuery, con));
            odaData.Fill(oDS, "PR_STRUCTURE");
            return oDS;
        }
        catch (Exception ex)
        {
            return null;
        }
        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }
    }

    public DataSet GetPrItemAmt(string bId, string gId, string dId, string itemId, string strGender, string strEtype, string allbranch)
    {
        string strSQL = "";

        //if (bId != "100310001") // when not all branch
        //{
        strSQL = " (CMP_BRANCH_ID='" + bId + "' OR CMP_BRANCH_ID = '" + allbranch + "') ";
        //}
        //else
        //{
        //    strSQL = " CMP_BRANCH_ID='" + bId + "' ";
        //}

        string strQuery = "SELECT PRST_AMT FROM PR_STRUCTURE "
                            + "WHERE "
                            + strSQL + " "
            //CMP_BRANCH_ID='" + bId + "'
                            + " AND DSG_ID='" + gId + "' AND DPT_ID = '" + dId + "' AND PR_ITEM_ID='" + itemId + "' "
                            + " AND GENDER='" + strGender + "' AND TYP_CODE='" + strEtype + "'";

        try
        {
            con = new SqlConnection(cn);
            con.Open();
            DataSet oDS = new DataSet();
            SqlDataAdapter odaData = new SqlDataAdapter(new SqlCommand(strQuery, con));
            odaData.Fill(oDS, "PR_STRUCTURE");
            return oDS;
        }
        catch (Exception ex)
        {
            return null;
        }
        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }
    }

    public string TotIncomeGen(string bId, string eId, string gId)
    {
        string strQuery = "SELECT SUM(PRST_AMT) TOT_INCOME "
                            + "FROM PR_PAYROLL_GENERUL PG, PR_PAYROLL_ITEM PI "
                            + "WHERE PG.CMP_BRANCH_ID='" + bId + "' AND DSG_ID='" + gId + "' AND EMP_ID='" + eId + "' "
                            + "AND PI.PR_ITEM_ID=PG.PR_ITEM_ID "
                            + "AND PI.PR_ITNATURE_CODE <>'B' AND PI.PR_ITNATURE_CODE <>'C' AND PI.PR_ITNATURE_CODE <>'G'";

        try
        {
            con = new SqlConnection(cn);
            con.Open();
            DataSet oDS = new DataSet();
            SqlDataAdapter odaData = new SqlDataAdapter(new SqlCommand(strQuery, con));
            odaData.Fill(oDS, "EMP_TOTAL_INCOME");

            DataRow dRow = oDS.Tables["EMP_TOTAL_INCOME"].Rows[0];
            return (dRow["TOT_INCOME"].ToString().Trim() == "" ? "0" : dRow["TOT_INCOME"].ToString());
        }
        catch (Exception ex)
        {
            return ex.Message.ToString();
        }
        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }

    }

    public DataSet GetPrItemAmtGeneral(string bId, string eId, string gId, string itemId)
    {
        string strQuery = "SELECT PRST_AMT FROM PR_PAYROLL_GENERUL "
                            + "WHERE CMP_BRANCH_ID='" + bId + "' AND DSG_ID='" + gId + "' AND EMP_ID='" + eId + "' "
                            + "AND PR_ITEM_ID='" + itemId + "'";


        try
        {
            con = new SqlConnection(cn);
            con.Open();
            DataSet oDS = new DataSet();
            SqlDataAdapter odaData = new SqlDataAdapter(new SqlCommand(strQuery, con));
            odaData.Fill(oDS, "PR_PAYROLL_GENERAL");
            return oDS;
        }
        catch (Exception ex)
        {
            return null;
        }
        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }
    }

    //public DataSet GetPrItem(string bId, string gId, string itemId, string strGender, string strEtype)
    //{
    //    string strQuery = "SELECT PRST_AMT FROM PR_STRUCTURE "
    //                        + "WHERE CMP_BRANCH_ID='" + bId + "' AND DSG_ID='" + gId + "' AND PR_ITEM_ID='" + itemId + "' AND GENDER='" + strGender + "' AND TYP_CODE='" + strEtype + "'";

    //    try
    //    {
    //        SqlConnection
    //        DataSet oDS = new DataSet();
    //        SqlDataAdapter odaData = new SqlDataAdapter(new SqlCommand(strQuery, con));
    //        odaData.Fill(oDS, "PR_STRUCTURE");
    //        return oDS;
    //    }
    //    catch (Exception ex)
    //    {
    //        return null;
    //    }
    //}

    public DataSet ChkEmpGeneralPayroll(string bId, string eId, string gId)
    {
        string strQuery = "SELECT * FROM PR_PAYROLL_GENERUL "
                            + "WHERE CMP_BRANCH_ID='" + bId + "' AND  EMP_ID='" + eId + "' AND "
                            + "DSG_ID='" + gId + "'";

        try
        {
            con = new SqlConnection(cn);
            con.Open();
            DataSet oDS = new DataSet();
            SqlDataAdapter odaData = new SqlDataAdapter(new SqlCommand(strQuery, con));
            odaData.Fill(oDS, "EMP_GENERAL_PAYROLL");
            return oDS;
        }
        catch (Exception ex)
        {
            return null;
        }
        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }
    }

    public string RemovePayrollGeneral(string bId, string eId, string gId)
    {
        string strSql;
        //string strSql1;

        strSql = "DELETE from PR_PAYROLL_GENERUL "
                + "WHERE CMP_BRANCH_ID='" + bId + "' AND  EMP_ID='" + eId + "' AND "
                            + "DSG_ID='" + gId + "'";

        //strSql1 = "DELETE from PR_SHEET "
        //        + "WHERE CMP_BRANCH_ID='" + bId + "' AND  EMP_ID='" + eId + "' AND "
        //                    + "DSG_ID='" + gId + "' AND PRM_ID='" + prMonth + "'"; ;

        try
        {

            con = new SqlConnection(cn);
            con.Open();
            dbTransaction = con.BeginTransaction();
            SqlCommand olcmd = new SqlCommand(strSql, con, dbTransaction);
            olcmd.Connection = con;
            olcmd.ExecuteNonQuery();

            //SqlCommand olcmd1 = new SqlCommand(strSql1);
            //olcmd1.Connection = con;
            //olcmd1.ExecuteNonQuery();
            dbTransaction.Commit();
            //con.Close();

            return "Saved successfully";
        }
        catch (Exception ex)
        {
            return ex.Message.ToString();
        }
        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }
    }

    public string SaveEmployeePayroll(string bId, string empId, string gId, string itmId, string itmAmt) //  Employee Payroll
    {
        string strSql;


        strSql = "SELECT CMP_BRANCH_ID, EMP_ID, DSG_ID, PR_ITEM_ID, PRST_AMT FROM PR_PAYROLL_GENERUL";


        try
        {
            // Payroll Detail
            DataRow oOrderRow;

            con = new SqlConnection(cn);
            con.Open();
            dbTransaction = con.BeginTransaction();
            DataSet oDS = new DataSet();
            SqlDataAdapter oOrdersDataAdapter = new SqlDataAdapter(new SqlCommand(strSql, con, dbTransaction));
            SqlCommandBuilder oOrdersCmdBuilder = new SqlCommandBuilder(oOrdersDataAdapter);
            //oOrdersDataAdapter.MissingSchemaAction = MissingSchemaAction.AddWithKey;
            //oOrdersDataAdapter.Fill(oDS, "PR_PAYROLL_GENERUL");

            oOrdersDataAdapter.FillSchema(oDS, SchemaType.Source, "PR_PAYROLL_GENERUL");
            oOrderRow = oDS.Tables["PR_PAYROLL_GENERUL"].NewRow();

            // 5 fields
            oOrderRow["CMP_BRANCH_ID"] = bId;
            oOrderRow["EMP_ID"] = empId;
            oOrderRow["DSG_ID"] = gId;
            oOrderRow["PR_ITEM_ID"] = itmId;

            if (itmAmt != "")
                oOrderRow["PRST_AMT"] = itmAmt;
            else
                oOrderRow["PRST_AMT"] = 0;
            //oOrderRow["TOT_TAX"] = totTax;
            //oOrderRow["TOT_INCOME"] = totIncome;

            oDS.Tables["PR_PAYROLL_GENERUL"].Rows.Add(oOrderRow);
            oOrdersDataAdapter.Update(oDS, "PR_PAYROLL_GENERUL");

            dbTransaction.Commit();
            //con.Close();
            return "Saved successfully";

        }
        catch (Exception ex)
        {
            return ex.Message.ToString();
        }
        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }
    }


    #endregion
    public DataSet GetAllEmpInfo(string bId)
    {
        //string strQuery = "SELECT SET_LEVEL, EMP_ID,(EMP_TITLE || ' ' || EMP_NAME) as ENAME, EL.DSG_ID, "
        //                    + "DP.DPT_ID, DP.DPT_NAME, SET_CODE || ' - ' || DSG_TITLE AS DNAME, ET.TYP_TYPE, "
        //                    + "EMP_GENDER, EMP_STATUS, TOT_INCOME, TAX_PER_MONTH, TOT_TAX "
        //                    + "FROM PR_EMPLOYEE_LIST EL, PR_DESIGNATION DG, HR_EMP_TYPE ET, PR_DEPARTMENT DP "
        //                    + "WHERE EL.DSG_ID=DG.DSG_ID AND EL.CMP_BRANCH_ID='" + bId + "' AND DP.DPT_ID = EL.DPT_ID "
        //                    + "AND EL.EMP_STATUS=ET.TYP_CODE AND ET.PAYROLL_ALLOWED='Y' "
        //                    //+ "AND EMP_ID='120310010010000001' "
        //                    + "ORDER BY to_number(SET_LEVEL), NLSSORT(TRIM(ENAME), 'NLS_SORT=generic_m')";
        string strQuery = "SELECT "
             + "SET_LEVEL,"
             + "EMP_ID,"
             + "(CAST(ISNULL(EL.EMP_TITLE, '') AS VARCHAR) + ' ' + CAST(EL.EMP_NAME AS VARCHAR)) as ENAME,"
             + "EL.DSG_ID,"
             + "DP.DPT_ID,"
             + "DP.DPT_NAME,"
             + "CAST(SET_CODE AS VARCHAR) + ' - ' + CAST(DSG_TITLE AS VARCHAR) AS DNAME,"
             + "ET.TYP_TYPE,"
             + "EMP_GENDER,"
             + "EMP_STATUS,"
             + "TOT_INCOME,"
             + "TAX_PER_MONTH,"
             + "TOT_TAX"
                + " FROM  PR_EMPLOYEE_LIST EL,"
                + "PR_DESIGNATION DG,"
                + "HR_EMP_TYPE ET,"
                + "PR_DEPARTMENT DP "
                + "WHERE EL.DSG_ID=DG.DSG_ID "
                + " AND DP.DPT_ID = EL.DPT_ID "
                + " AND EL.CMP_BRANCH_ID='" + bId + "' "
                + " AND EL.EMP_STATUS = ET.TYP_CODE"
                + " AND ET.PAYROLL_ALLOWED ='Y'"
                + "ORDER BY CONVERT(NUMERIC(8, 2), SET_LEVEL), "
                + "ENAME";


        try
        {
            con = new SqlConnection(cn);
            con.Open();
            DataSet oDS = new DataSet();
            SqlDataAdapter odaData = new SqlDataAdapter(new SqlCommand(strQuery, con));
            odaData.Fill(oDS, "PR_EMPLOYEE_LIST");
            return oDS;
        }
        catch (Exception ex)
        {
            return null;
        }
        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }
    }
    //public DataSet GetAllEmpInfo(string bId)
    //{
    //    string strQuery = "SELECT SET_LEVEL, EMP_ID,(EMP_TITLE || ' ' || EMP_NAME) as ENAME, EL.DSG_ID, "
    //                        + "DP.DPT_ID, DP.DPT_NAME, SET_CODE || ' - ' || DSG_TITLE AS DNAME, ET.TYP_TYPE, "
    //                        + "EMP_GENDER, EMP_STATUS, TOT_INCOME, TAX_PER_MONTH, TOT_TAX "
    //                        + "FROM PR_EMPLOYEE_LIST EL, PR_DESIGNATION DG, HR_EMP_TYPE ET, PR_DEPARTMENT DP "
    //                        + "WHERE EL.DSG_ID=DG.DSG_ID AND EL.CMP_BRANCH_ID='" + bId + "' AND DP.DPT_ID = EL.DPT_ID "
    //                        + "AND EL.EMP_STATUS=ET.TYP_CODE AND ET.PAYROLL_ALLOWED='Y' "
    //                        //+ "AND EMP_ID='120310010010000001' "
    //                        + "ORDER BY to_number(SET_LEVEL), NLSSORT(TRIM(ENAME), 'NLS_SORT=generic_m')";


    //    try
    //    {
    //        con = new SqlConnection(cn);
    //        con.Open();
    //        DataSet oDS = new DataSet();
    //        SqlDataAdapter odaData = new SqlDataAdapter(new SqlCommand(strQuery, con));
    //        odaData.Fill(oDS, "PR_EMPLOYEE_LIST");
    //        return oDS;
    //    }
    //    catch (Exception ex)
    //    {
    //        return null;
    //    }
    //    finally
    //    {
    //        con.Close();
    //        con = null;
    //    }
    //}

    public DataSet GetAllEmpInfo(string bId, string prMonth) // Employee List from Payroll Sheet
    {
        string strQuery = "SELECT SET_LEVEL,  (CAST(ISNULL(EL.EMP_TITLE, '') AS VARCHAR) + ' ' + CAST(EL.EMP_NAME AS VARCHAR)) as ENAME, ST.DSG_ID, "
                            + "DP.DPT_ID, DP.DPT_NAME, CAST(SET_CODE AS VARCHAR) + ' - ' + CAST(DSG_TITLE AS VARCHAR) AS DNAME, "
                            + "EMP_GENDER, EMP_STATUS, ST.NET_AMOUNT TOT_INCOME, ST.TAX_PER_MONTH, ST.TOT_TAX "
                            + "FROM PR_EMPLOYEE_LIST EL, PR_DESIGNATION DG, PR_SHEET ST, PR_DEPARTMENT DP "
                            + "WHERE EL.DSG_ID=DG.DSG_ID AND EL.CMP_BRANCH_ID='" + bId + "' AND DP.DPT_ID = EL.DPT_ID "
                            + "AND ST.EMP_ID=EL.EMP_ID AND ST.PRM_ID='" + prMonth + "' "
                            + "ORDER BY CONVERT(NUMERIC(8,2),SET_LEVEL), RTRIM(LTRIM(ENAME))";


        try
        {
            con = new SqlConnection(cn);
            con.Open();
            DataSet oDS = new DataSet();
            SqlDataAdapter odaData = new SqlDataAdapter(new SqlCommand(strQuery, con));
            odaData.Fill(oDS, "PR_EMPLOYEE_LIST");
            return oDS;
        }
        catch (Exception ex)
        {
            return null;
        }
        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }
    }
    public DataSet GetAllEmpIn(string bId)
    {
        string strQuery = " SELECT EMP_ID, (CAST(ISNULL(EL.EMP_TITLE, '') AS VARCHAR) + ' ' + CAST(EL.EMP_NAME AS VARCHAR)) as ENAME, DSG_ID, EMP_GENDER FROM PR_EMPLOYEE_LIST "
                            + " WHERE CMP_BRANCH_ID='" + bId + "' "
                            + " ORDER BY LTRIM(RTRIM(ENAME))  ";

        try
        {
            con = new SqlConnection(cn);
            con.Open();
            DataSet oDS = new DataSet();
            SqlDataAdapter odaData = new SqlDataAdapter(new SqlCommand(strQuery, con));
            odaData.Fill(oDS, "PR_EMPLOYEE_LIST");
            return oDS;
        }
        catch (Exception ex)
        {
            return null;
        }
        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }
    }

    public DataSet GetAllEmpInfoAsDepartment(string DeptId)
    {
        string strQuery = " SELECT EL.EMP_ID, LTRIM(CAST(ISNULL(EL.EMP_TITLE, '') AS VARCHAR(100)) + ' ' + CAST(EL.EMP_NAME AS VARCHAR(500))) AS ENAME  "
                          + " FROM PR_EMPLOYEE_LIST AS EL INNER JOIN HR_EMP_TYPE AS ET ON EL.EMP_STATUS = ET.TYP_CODE "
                          + " WHERE  (EL.DPT_ID = '" + DeptId + "') ORDER BY ENAME ";

        try
        {
            con = new SqlConnection(cn);
            con.Open();
            DataSet oDS = new DataSet();
            SqlDataAdapter odaData = new SqlDataAdapter(new SqlCommand(strQuery, con));
            odaData.Fill(oDS, "PR_EMPLOYEE_LIST");
            return oDS;
        }
        catch (Exception ex)
        {
            return null;
        }
        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }
    }

    public DataSet GetAllEmployeeAsDepartmentPalo(string strFilter, string strFilteremp, string empId)
    {
        string strQuery = @" SELECT EL.EMP_CODE, EL.EMP_ID, el.CMP_BRANCH_ID, LTRIM(CAST(ISNULL(EL.EMP_TITLE, '') AS VARCHAR(100)) + ' ' + CAST(EL.EMP_NAME AS VARCHAR(500))) AS ENAME, LTRIM(RTRIM(EMP_LATE_SHOW)) EMP_LATE_SHOW 
                        FROM PR_EMPLOYEE_LIST AS EL INNER JOIN HR_EMP_TYPE AS ET ON EL.EMP_STATUS = ET.TYP_CODE 
                         left join PR_DEPARTMENT as DPT on DPT.DPT_ID=EL.DPT_ID"
                         + " WHERE  " + strFilter + " " + strFilteremp + " " + empId + " order by convert(int, isnull( DPT.ORDER_BY,'1000000')) , convert(int, isnull(EL.ORDER_BY,'10000000'))    ";

        try
        {
            con = new SqlConnection(cn);
            con.Open();
            DataSet oDS = new DataSet();
            SqlDataAdapter odaData = new SqlDataAdapter(new SqlCommand(strQuery, con));
            odaData.Fill(oDS, "PR_EMPLOYEE_LIST");
            return oDS;
        }
        catch (Exception ex)
        {
            return null;
        }
        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }
    }

    public DataSet GetAllEmployeeAsDepartment(string strFilter, string strFilteremp)
    {
        string strQuery = @" SELECT EL.EMP_CODE, EL.EMP_ID, el.CMP_BRANCH_ID, LTRIM(CAST(ISNULL(EL.EMP_TITLE, '') AS VARCHAR(100)) + ' ' + CAST(EL.EMP_NAME AS VARCHAR(500))) AS ENAME, LTRIM(RTRIM(EMP_LATE_SHOW)) EMP_LATE_SHOW 
                        FROM PR_EMPLOYEE_LIST AS EL INNER JOIN HR_EMP_TYPE AS ET ON EL.EMP_STATUS = ET.TYP_CODE 
                         left join PR_DEPARTMENT as DPT on DPT.DPT_ID=EL.DPT_ID"
                         + " WHERE  " + strFilter + " " + strFilteremp + " and EL.EMP_STATUS not in (9,11)  order by convert(int, isnull( DPT.ORDER_BY,'1000000')) , convert(int, isnull(EL.ORDER_BY,'10000000'))    ";

        try
        {
            con = new SqlConnection(cn);
            con.Open();
            DataSet oDS = new DataSet();
            SqlDataAdapter odaData = new SqlDataAdapter(new SqlCommand(strQuery, con));
            odaData.Fill(oDS, "PR_EMPLOYEE_LIST");
            return oDS;
        }
        catch (Exception ex)
        {
            return null;
        }
        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }
    }

    public DataSet GetAllEmployeeAsDepartment(string strFilter, string strFilteremp, string strFilterEmpStatus)
    {
        string strQuery = " SELECT EL.EMP_ID, LTRIM(CAST(ISNULL(EL.EMP_TITLE, '') AS VARCHAR(100)) + ' ' + CAST(EL.EMP_NAME AS VARCHAR(500))) AS ENAME, LTRIM(RTRIM(EMP_LATE_SHOW)) EMP_LATE_SHOW  "
                         + " FROM PR_EMPLOYEE_LIST AS EL INNER JOIN HR_EMP_TYPE AS ET ON EL.EMP_STATUS = ET.TYP_CODE "
                         + " WHERE  " + strFilter + " " + strFilteremp + " " + strFilterEmpStatus + " ORDER BY EL.DPT_ID ";

        try
        {
            con = new SqlConnection(cn);
            con.Open();
            DataSet oDS = new DataSet();
            SqlDataAdapter odaData = new SqlDataAdapter(new SqlCommand(strQuery, con));
            odaData.Fill(oDS, "PR_EMPLOYEE_LIST");
            return oDS;
        }
        catch (Exception ex)
        {
            return null;
        }
        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }
    }


    public DataSet GetAllEmployeeAsDepartmentForReport(string strFilter, string strFilteremp, string strFilterEmpStatus, string emptype)
    {
        string strQuery = " SELECT EL.EMP_ID, LTRIM(CAST(ISNULL(EL.EMP_TITLE, '') AS VARCHAR(100)) + ' ' + CAST(EL.EMP_NAME AS VARCHAR(500))) AS ENAME, LTRIM(RTRIM(EMP_LATE_SHOW)) EMP_LATE_SHOW  "
                         + " FROM PR_EMPLOYEE_LIST AS EL INNER JOIN HR_EMP_TYPE AS ET ON EL.EMP_STATUS = ET.TYP_CODE "
                         + " WHERE  " + strFilter + " " + strFilteremp + " " + strFilterEmpStatus + " and EL.MANAGEMENT_EMPLOYEE='" + emptype + "' ORDER BY EL.DPT_ID ";

        try
        {
            con = new SqlConnection(cn);
            con.Open();
            DataSet oDS = new DataSet();
            SqlDataAdapter odaData = new SqlDataAdapter(new SqlCommand(strQuery, con));
            odaData.Fill(oDS, "PR_EMPLOYEE_LIST");
            return oDS;
        }
        catch (Exception ex)
        {
            return null;
        }
        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }
    }


    public DataSet GetWorkflowType( string SetCode)
    {
        string strQuery = " Select * from  CM_WORKFLOW_MANAGE_TYPE where [WM_ITEM_PARENT_CODE]='04' "+ SetCode +"  order by [WM_ITEM_ID] desc ";

        try
        {
            con = new SqlConnection(cn);
            con.Open();
            DataSet oDS = new DataSet();
            SqlDataAdapter odaData = new SqlDataAdapter(new SqlCommand(strQuery, con));
            odaData.Fill(oDS, "CM_WORKFLOW_MANAGE_TYPE");
            return oDS;
        }
        catch (Exception ex)
        {
            return null;
        }
        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }
    }

    public DataSet GetAllEmployeeAsDepartmentDynamically(string strFilter, string status)
    {

        string strQuery = " SELECT EL.*, LTRIM(CAST(ISNULL(EL.EMP_TITLE, '') AS VARCHAR(100)) + ' ' + CAST(EL.EMP_NAME AS VARCHAR(500))) AS ENAME  "
                         + " FROM PR_EMPLOYEE_LIST AS EL INNER JOIN HR_EMP_TYPE AS ET ON EL.EMP_STATUS = ET.TYP_CODE "
                         + " WHERE  " + strFilter + "  " + status + "  ORDER BY EL.DPT_ID ";

        try
        {
            con = new SqlConnection(cn);
            con.Open();
            DataSet oDS = new DataSet();
            SqlDataAdapter odaData = new SqlDataAdapter(new SqlCommand(strQuery, con));
            odaData.Fill(oDS, "PR_EMPLOYEE_LIST");
            return oDS;
        }
        catch (Exception ex)
        {
            return null;
        }
        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }
    }

    public DataSet GetAllEmployeeAsDepartment(string strFilter)
    {
        string strQuery = " SELECT EL.EMP_ID, LTRIM(CAST(ISNULL(EL.EMP_TITLE, '') AS VARCHAR(100)) + ' ' + CAST(EL.EMP_NAME AS VARCHAR(500))) AS ENAME  "
                         + " FROM PR_EMPLOYEE_LIST AS EL INNER JOIN HR_EMP_TYPE AS ET ON EL.EMP_STATUS = ET.TYP_CODE "
                         + " WHERE  " + strFilter + "  ORDER BY EL.DPT_ID ";

        try
        {
            con = new SqlConnection(cn);
            con.Open();
            DataSet oDS = new DataSet();
            SqlDataAdapter odaData = new SqlDataAdapter(new SqlCommand(strQuery, con));
            odaData.Fill(oDS, "PR_EMPLOYEE_LIST");
            return oDS;
        }
        catch (Exception ex)
        {
            return null;
        }
        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }
    }
    public DataSet GetEmployeeInfoDeptSpecialLvRpt(string bId, string strFilter)
    {
        string strQuery = "SELECT EL.DPT_ID,EL.EMP_CODE,(SELECT DPT_NAME FROM PR_DEPARTMENT WHERE DPT_ID=EL.DPT_ID)DPT_NAME, EL.EMP_ID,EL.DSG_ID,EMP_NAME,EMP_STATUS,EL.CMP_BRANCH_ID,EMP_FINAL_CONFIR_DATE,DSG_TITLE,TYP_TYPE "
                       + "from PR_EMPLOYEE_LIST EL, PR_DESIGNATION PD, HR_EMP_TYPE ET,PR_LEAVE_POLICY LP "
                       + "WHERE EL.EMP_ID=LP.EMP_ID AND EL.DSG_ID_MAIN=PD.DSG_ID AND EL.EMP_STATUS=ET.TYP_CODE AND EL.CMP_BRANCH_ID='" + bId + "' AND EMP_STATUS='2' "
                       + "AND " + strFilter + "  ORDER BY EL.DPT_ID  ";

        var con = new SqlConnection(cn);
        try
        {

            DataSet oDS = new DataSet();
            SqlDataAdapter odaData = new SqlDataAdapter(new SqlCommand(strQuery, con));
            odaData.Fill(oDS, "PR_EMPLOYEE_INFO");
            return oDS;
        }
        catch (Exception ex)
        {
            return null;
        }
        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }
    }
    public DataSet GetRetiredEmployeeList(string bId, string DeptstrFilter, string yearOfRetired, string MonthOfRetired, string reportType)
    {
        string FilterMonth = "";
        string Filter2 = "";
        string Query = "";
        if (MonthOfRetired.Trim() != "")
        {
            if (reportType == "R")
            {
                FilterMonth = "and Month(convert(date, CONVERT(DATETIME, CONVERT(VARCHAR(23), L.EMP_RETIRE_DATE))))= '" + MonthOfRetired + "' ";
            }
            else
            {
                FilterMonth = "and Month(convert(date, CONVERT(DATETIME, CONVERT(VARCHAR(23), L.EMP_SERVICE_END_DATE))))= '" + MonthOfRetired + "' ";
            }
        }
        //##################################################################################################################################
        if (reportType == "R")
        {
            Filter2 = " AND L.EMP_STATUS = '9' and Year(convert(date, CONVERT(DATETIME, CONVERT(VARCHAR(23), L.EMP_RETIRE_DATE))))='" + yearOfRetired + "' AND L.EMP_RETIRE_DATE !='' ";
            Query = " convert(date, CONVERT(DATETIME, CONVERT(VARCHAR(23), L.EMP_RETIRE_DATE)))EMP_RETIRE_DATE,dbo.CALCULATE_SERVICETIME(convert(date, CONVERT(DATETIME, CONVERT(VARCHAR(23), L.EMP_JOINING_DATE))),  convert(date, CONVERT(DATETIME, CONVERT(VARCHAR(23), L.EMP_RETIRE_DATE))))ServicePeriod ";
        }
        else
        {
            Filter2 = " AND L.EMP_STATUS = '11' and Year(convert(date, CONVERT(DATETIME, CONVERT(VARCHAR(23), L.EMP_SERVICE_END_DATE))))='" + yearOfRetired + "' AND L.EMP_SERVICE_END_DATE !='' ";
            Query = " convert(date, CONVERT(DATETIME, CONVERT(VARCHAR(23), L.EMP_SERVICE_END_DATE)))EMP_RETIRE_DATE,dbo.CALCULATE_SERVICETIME(convert(date, CONVERT(DATETIME, CONVERT(VARCHAR(23), L.EMP_JOINING_DATE))),  convert(date, CONVERT(DATETIME, CONVERT(VARCHAR(23), L.EMP_SERVICE_END_DATE))))ServicePeriod ";
        }

        string strQuery = "SELECT L.EMP_ID, "
                        + "L.EMP_CODE,LTRIM(CAST(ISNULL(L.EMP_TITLE, '') AS VARCHAR(100)) + ' ' + CAST(L.EMP_NAME AS VARCHAR(500)))   AS ENAME, "
                        + "(SELECT DG.DSG_TITLE FROM PR_DESIGNATION DG WHERE DG.DSG_ID=L.DSG_ID_MAIN AND DG.SET_TYPE='D' )DSG_TITLE,  "
                        + "L.CMP_BRANCH_ID,(SELECT pd.DPT_NAME FROM PR_DEPARTMENT pd WHERE pd.DPT_ID= L.DPT_ID)DPT_NAME, "
                        + "convert(date, CONVERT(DATETIME, CONVERT(VARCHAR(23), L.EMP_FINAL_CONFIR_DATE)))EMP_FINAL_CONFIR_DATE, "
                        + "convert(date, CONVERT(DATETIME, CONVERT(VARCHAR(23), L.EMP_JOINING_DATE)))EMP_JOINING_DATE,  " + Query + " "
                        + "FROM PR_EMPLOYEE_LIST L   "
                        + "WHERE   L.CMP_BRANCH_ID='" + bId + "'  " + Filter2 + "  "
                        + " " + FilterMonth + " "
                        + " " + DeptstrFilter + " "
                        + "  ORDER BY L.DPT_ID  ";
        var con = new SqlConnection(cn);
        try
        {
            DataSet oDS = new DataSet();
            SqlDataAdapter odaData = new SqlDataAdapter(new SqlCommand(strQuery, con));
            odaData.Fill(oDS, "PR_EMPLOYEE_INFO");
            return oDS;
        }
        catch (Exception ex)
        {
            return null;
        }
        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }
    }

    public DataSet GetMediaStarEmployeeList(string bId, string DeptstrFilter, string yearOfJoining, string MonthOfJoining, string empStatus)
    {
        string FilterMonth = "";
        if (MonthOfJoining.Trim() != "")
        {
            FilterMonth = "and Month(convert(date, CONVERT(DATETIME, CONVERT(VARCHAR(23), L.EMP_JOINING_DATE))))= '" + MonthOfJoining + "' ";
        }
        string YrFilter = "";
        if (yearOfJoining != "")
        {
            YrFilter = "and Year(convert(date, CONVERT(DATETIME, CONVERT(VARCHAR(23), L.EMP_JOINING_DATE))))='" + yearOfJoining + "'  ";
        }
        string strQuery = "SELECT L.EMP_ID, "
                        + "L.EMP_CODE,LTRIM(CAST(ISNULL(L.EMP_TITLE, '') AS VARCHAR(100)) + ' ' + CAST(L.EMP_NAME AS VARCHAR(500)))   AS ENAME, "
                        + "(SELECT DG.DSG_TITLE FROM PR_DESIGNATION DG WHERE DG.DSG_ID=L.DSG_ID_MAIN AND DG.SET_TYPE='D' )DSG_TITLE,  "
                        + "L.CMP_BRANCH_ID,(SELECT pd.DPT_NAME FROM PR_DEPARTMENT pd WHERE pd.DPT_ID= L.DPT_ID)DPT_NAME, "
                        + "convert(date, CONVERT(DATETIME, CONVERT(VARCHAR(23), L.EMP_FINAL_CONFIR_DATE)))EMP_FINAL_CONFIR_DATE, "
                        + "convert(date, CONVERT(DATETIME, CONVERT(VARCHAR(23), L.EMP_CONFIR_DATE)))EMP_CONFIR_DATE, "
                        + "convert(date, CONVERT(DATETIME, CONVERT(VARCHAR(23), L.EMP_JOINING_DATE)))EMP_JOINING_DATE, "
                        + "convert(date, CONVERT(DATETIME, CONVERT(VARCHAR(23), L.EMP_RETIRE_DATE)))EMP_RETIRE_DATE, "
                        + "dbo.CALCULATE_SERVICETIME(convert(date, CONVERT(DATETIME, CONVERT(VARCHAR(23), L.EMP_JOINING_DATE))),  convert(date, CONVERT(DATETIME, CONVERT(VARCHAR(23), L.EMP_RETIRE_DATE))))ServicePeriod "
                        + "FROM PR_EMPLOYEE_LIST L   "
                        + " WHERE   L.CMP_BRANCH_ID='" + bId + "' AND L.EMP_STATUS = '" + empStatus + "'  "
                        + " " + YrFilter + " "
                        + " " + FilterMonth + " "
                        + " " + DeptstrFilter + " "
                        + " AND L.EMP_JOINING_DATE !='' ORDER BY L.DPT_ID  ";
        var con = new SqlConnection(cn);
        try
        {
            DataSet oDS = new DataSet();
            SqlDataAdapter odaData = new SqlDataAdapter(new SqlCommand(strQuery, con));
            odaData.Fill(oDS, "PR_EMPLOYEE_INFO");
            return oDS;
        }
        catch (Exception ex)
        {
            return null;
        }
        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }
    }

    public DataSet GetJoiningEmployeeList(string bId, string DeptstrFilter, string yearOfRetired, string MonthOfRetired)
    {
        string FilterMonth = "";
        if (MonthOfRetired.Trim() != "")
        {
            FilterMonth = "and Month(convert(date, CONVERT(DATETIME, CONVERT(VARCHAR(23), L.EMP_JOINING_DATE))))= '" + MonthOfRetired + "' ";
        }
        string strQuery = "SELECT L.EMP_ID, Year(Convert(varchar,L.EMP_RETIRE_DATE,103))Year_retired,Month(Convert(varchar,L.EMP_RETIRE_DATE,103))Month_retired1, "
                        + "L.EMP_CODE,LTRIM(CAST(ISNULL(L.EMP_TITLE, '') AS VARCHAR(100)) + ' ' + CAST(L.EMP_NAME AS VARCHAR(500)))   AS ENAME, "
                        + "(SELECT DG.DSG_TITLE FROM PR_DESIGNATION DG WHERE DG.DSG_ID=L.DSG_ID_MAIN AND DG.SET_TYPE='D' )DSG_TITLE,  "
                        + "L.CMP_BRANCH_ID,(SELECT pd.DPT_NAME FROM PR_DEPARTMENT pd WHERE pd.DPT_ID= L.DPT_ID)DPT_NAME, "
                        + "convert(date, CONVERT(DATETIME, CONVERT(VARCHAR(23), L.EMP_FINAL_CONFIR_DATE)))EMP_FINAL_CONFIR_DATE, "
                        + "convert(date, CONVERT(DATETIME, CONVERT(VARCHAR(23), L.EMP_JOINING_DATE)))EMP_JOINING_DATE, "
                        + "convert(date, CONVERT(DATETIME, CONVERT(VARCHAR(23), L.EMP_RETIRE_DATE)))EMP_RETIRE_DATE, "
                        + "dbo.CALCULATE_SERVICETIME(convert(date, CONVERT(DATETIME, CONVERT(VARCHAR(23), L.EMP_JOINING_DATE))),  convert(date, CONVERT(DATETIME, CONVERT(VARCHAR(23), '" + DateTime.Now.ToString("dd-MMM-yyyy") + "'))))ServicePeriod "
                        + "FROM PR_EMPLOYEE_LIST L   "
                        + "WHERE   L.CMP_BRANCH_ID='" + bId + "' and Year(convert(date, CONVERT(DATETIME, CONVERT(VARCHAR(23), L.EMP_JOINING_DATE))))='" + yearOfRetired + "'  "
                        + " " + FilterMonth + " "
                        + " " + DeptstrFilter + " "
                        + " AND L.EMP_JOINING_DATE !='' ORDER BY L.DPT_ID  ";
        var con = new SqlConnection(cn);
        try
        {
            DataSet oDS = new DataSet();
            SqlDataAdapter odaData = new SqlDataAdapter(new SqlCommand(strQuery, con));
            odaData.Fill(oDS, "PR_EMPLOYEE_INFO");
            return oDS;
        }
        catch (Exception ex)
        {
            return null;
        }
        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }
    }

    public DataSet GetGratuityEmployeeList(string bId, string DeptstrFilter, string EmpFilter) //, string yearOfRetired, string MonthOfRetired /* Md. Asaduzzaman Dated:09-Apr-2015  */
    {
        string dtNow = DateTime.Now.ToString("dd-MMM-yyyy");
        string RtrdDate = "LTRIM(RTRIM(ISNULL(L.EMP_RETIRE_DATE,'" + dtNow + "'))) ";
        //string FilterMonth = "";
        //if (MonthOfRetired.Trim() != "")
        //{
        //    FilterMonth = "and Month(convert(date, CONVERT(DATETIME, CONVERT(VARCHAR(23), L.EMP_RETIRE_DATE))))= '" + MonthOfRetired + "' ";
        //}
        string strQuery = "SELECT L.EMP_STATUS,L.EMP_ID,  "
                        + "L.EMP_CODE,LTRIM(CAST(ISNULL(L.EMP_TITLE, '') AS VARCHAR(100)) + ' ' + CAST(L.EMP_NAME AS VARCHAR(500)))   AS ENAME, "
                        + "(SELECT DG.DSG_TITLE FROM PR_DESIGNATION DG WHERE DG.DSG_ID=L.DSG_ID_MAIN AND DG.SET_TYPE='D' )DSG_TITLE,  "
                        + "(SELECT DG.DSG_TITLE FROM PR_DESIGNATION DG WHERE DG.DSG_ID=L.DSG_ID AND DG.SET_TYPE='G')GRADE_TITLE,  "
                        + "L.CMP_BRANCH_ID,(SELECT pd.DPT_NAME FROM PR_DEPARTMENT pd WHERE pd.DPT_ID= L.DPT_ID)DPT_NAME, "
            //+ "convert(date, CONVERT(DATETIME, CONVERT(VARCHAR(23), L.EMP_FINAL_CONFIR_DATE)))EMP_FINAL_CONFIR_DATE, "
                        + "convert(date, CONVERT(DATETIME, CONVERT(VARCHAR(23), L.EMP_JOINING_DATE)))EMP_JOINING_DATE, "
                        + "convert(date, CONVERT(DATETIME, CONVERT(VARCHAR(23), L.EMP_RETIRE_DATE)))EMP_RETIRE_DATE, "
                        + "dbo.CALCULATE_SERVICETIME(convert(date, CONVERT(DATETIME, CONVERT(VARCHAR(23), L.EMP_JOINING_DATE))),  convert(date, CONVERT(DATETIME, CONVERT(VARCHAR(23), " + RtrdDate + "))))ServicePeriod, "
                        + "dbo.CALCULATE_Yr(convert(date, CONVERT(DATETIME, CONVERT(VARCHAR(23), L.EMP_JOINING_DATE))),  convert(date, CONVERT(DATETIME, CONVERT(VARCHAR(23),    " + RtrdDate + "))))ServiceYear, "
                        + "dbo.CALCULATE_Month(convert(date, CONVERT(DATETIME, CONVERT(VARCHAR(23), L.EMP_JOINING_DATE))),  convert(date, CONVERT(DATETIME, CONVERT(VARCHAR(23), " + RtrdDate + "))))ServiceMonth, "
                        + "dbo.CALCULATE_Days(convert(date, CONVERT(DATETIME, CONVERT(VARCHAR(23), L.EMP_JOINING_DATE))),  convert(date, CONVERT(DATETIME, CONVERT(VARCHAR(23),  " + RtrdDate + "))))ServiceDays "
                        + "FROM PR_EMPLOYEE_LIST L   "
                        + "WHERE L.CMP_BRANCH_ID='" + bId + "' " //and Year(convert(date, CONVERT(DATETIME, CONVERT(VARCHAR(23), L.EMP_RETIRE_DATE))))='" + yearOfRetired + "'  "
            //+ " " + FilterMonth + " "
                        + " " + DeptstrFilter + " " + EmpFilter + "  AND EMP_STATUS NOT IN (9,11) "
                        + " AND  L.EMP_JOINING_DATE !='' ORDER BY L.DPT_ID  "; //L.EMP_RETIRE_DATE !='' and
        var con = new SqlConnection(cn);
        try
        {
            DataSet oDS = new DataSet();
            SqlDataAdapter odaData = new SqlDataAdapter(new SqlCommand(strQuery, con));
            odaData.Fill(oDS, "PR_EMPLOYEE_INFO");
            return oDS;
        }
        catch (Exception ex)
        {
            return null;
        }
        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }
    }

    public DataSet GetGratuitySetup(string empStatus)
    {
        string strQuery = "Select * from HR_EMP_GRATUITY_SETUP where TYP_CODE='" + empStatus + "'  ";
        var con = new SqlConnection(cn);
        try
        {
            DataSet oDS = new DataSet();
            SqlDataAdapter odaData = new SqlDataAdapter(new SqlCommand(strQuery, con));
            odaData.Fill(oDS, "Gratuity");
            return oDS;
        }
        catch (Exception ex)
        {
            return null;
        }
        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }
    }




    public DataSet GetEmployeeListDept(string bId, string DeptstrFilter)
    {
        string strQuery = "SELECT L.EMP_ID, "
                        + "L.EMP_CODE,LTRIM(CAST(ISNULL(L.EMP_TITLE, '') AS VARCHAR(100)) + ' ' + CAST(L.EMP_NAME AS VARCHAR(500)))   AS ENAME, "
                        + "(SELECT DG.DSG_TITLE FROM PR_DESIGNATION DG WHERE DG.DSG_ID=L.DSG_ID_MAIN AND DG.SET_TYPE='D' )DSG_TITLE,  "
                        + "L.CMP_BRANCH_ID,(SELECT pd.DPT_NAME FROM PR_DEPARTMENT pd WHERE pd.DPT_ID= L.DPT_ID)DPT_NAME, "
                        + "convert(date, CONVERT(DATETIME, CONVERT(VARCHAR(23), L.EMP_FINAL_CONFIR_DATE)))EMP_FINAL_CONFIR_DATE, "
                        + "convert(date, CONVERT(DATETIME, CONVERT(VARCHAR(23), L.EMP_JOINING_DATE)))EMP_JOINING_DATE, "
                        + "convert(date, CONVERT(DATETIME, CONVERT(VARCHAR(23), L.EMP_RETIRE_DATE)))EMP_RETIRE_DATE, "
                        + "dbo.CALCULATE_SERVICETIME(convert(date, CONVERT(DATETIME, CONVERT(VARCHAR(23), L.EMP_JOINING_DATE))),  convert(date, CONVERT(DATETIME, CONVERT(VARCHAR(23), L.EMP_RETIRE_DATE))))ServicePeriod "
                        + "FROM PR_EMPLOYEE_LIST L   "
                        + "WHERE   L.CMP_BRANCH_ID='" + bId + "' "
                        + " " + DeptstrFilter + "  AND L.EMP_STATUS NOT IN (9,11)  ORDER BY L.DPT_ID  ";
        var con = new SqlConnection(cn);
        try
        {
            DataSet oDS = new DataSet();
            SqlDataAdapter odaData = new SqlDataAdapter(new SqlCommand(strQuery, con));
            odaData.Fill(oDS, "PR_EMPLOYEE_INFO");
            return oDS;
        }
        catch (Exception ex)
        {
            return null;
        }
        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }
    }
    public string GetEmpName(string empId)
    {
        string strQuery = " SELECT LTRIM(CAST(ISNULL(EL.EMP_TITLE, '') AS VARCHAR(100)) + ' ' + CAST(EL.EMP_NAME AS VARCHAR(500))) AS ENAME  "
                        + " FROM PR_EMPLOYEE_LIST EL   WHERE  (EL.EMP_ID = '" + empId + "') ";
        string empName = "";
        try
        {
            con = new SqlConnection(cn);
            con.Open();
            DataSet oDS = new DataSet();
            SqlDataAdapter odaData = new SqlDataAdapter(new SqlCommand(strQuery, con));
            odaData.Fill(oDS, "PR_EMPLOYEE_LIST");
            empName = oDS.Tables[0].Rows[0]["ENAME"].ToString();
        }
        catch (Exception ex)
        {
            return null;
        }
        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }
        return empName;
    }

    public bool chkGratuity(string Typecode) /* Md.Asaduzzaman Dated:07-Apr-2015  */
    {
        string strQuery = "Select * from HR_EMP_GRATUITY_SETUP where TYP_CODE='" + Typecode + "' ";
        bool chkExist = false;
        try
        {
            con = new SqlConnection(cn);
            con.Open();
            DataSet oDS = new DataSet();
            SqlDataAdapter odaData = new SqlDataAdapter(new SqlCommand(strQuery, con));
            odaData.Fill(oDS, "check");
            if (oDS.Tables[0].Rows.Count > 0)
            {
                chkExist = true;
            }
        }
        catch (Exception)
        {
            return chkExist;
        }
        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }
        return chkExist;
    }

    //public DataSet GetAllBranchInfo()
    //{
    //    string strQuery = "SELECT CMP_BRANCH_ID, CMP_BRANCH_NAME FROM CM_CMP_BRANCH "
    //                        + "WHERE CMP_BRANCH_NAME !='All Branch' "
    //                        + "ORDER BY NLSSORT(TRIM(CMP_BRANCH_NAME), 'NLS_SORT=generic_m')";


    //    try
    //    {
    //        con = new SqlConnection(cn);
    //        con.Open(); 
    //        DataSet oDS = new DataSet();
    //        SqlDataAdapter odaData = new SqlDataAdapter(new SqlCommand(strQuery, con));
    //        odaData.Fill(oDS, "CM_CMP_BRANCH");
    //        return oDS;
    //    }
    //    catch (Exception ex)
    //    {
    //        return null;
    //    }

    //    finally
    //    {
    //        con.Close();
    //        con = null;
    //    }
    //}

    //sydur 24-02-2014

    public DataSet GetAllBranchInfo()
    {
        string strQuery = "SELECT CMP_BRANCH_ID, CMP_BRANCH_NAME FROM CM_CMP_BRANCH "
                            + "WHERE CMP_BRANCH_NAME !='All Branch' "
                            + "ORDER BY LTRIM(RTRIM(CMP_BRANCH_NAME))";


        try
        {
            con = new SqlConnection(cn);
            con.Open();
            DataSet oDS = new DataSet();
            SqlDataAdapter odaData = new SqlDataAdapter(new SqlCommand(strQuery, con));
            odaData.Fill(oDS, "CM_CMP_BRANCH");
            return oDS;
        }
        catch (Exception ex)
        {
            return null;
        }
        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }
    }
    //sydur 23-02-2014
    public DataSet GetAllBranchInfoLv()
    {
        //string strQuery = "SELECT CMP_BRANCH_ID, CMP_BRANCH_NAME FROM CM_CMP_BRANCH "
        //                    + "ORDER BY NLSSORT(TRIM(CMP_BRANCH_NAME), 'NLS_SORT=generic_m')";
        string strQuery = "SELECT CMP_BRANCH_ID,CMP_BRANCH_NAME FROM  CM_CMP_BRANCH ORDER BY (LTRIM(RTRIM(CMP_BRANCH_NAME))) ";

        try
        {
            con = new SqlConnection(cn);
            con.Open();
            DataSet oDS = new DataSet();
            SqlDataAdapter odaData = new SqlDataAdapter(new SqlCommand(strQuery, con));
            odaData.Fill(oDS, "CM_CMP_BRANCH");
            return oDS;
        }
        catch (Exception ex)
        {
            return null;
        }
        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }
    }

    //public DataSet GetAllBranchInfoLv()
    //{
    //    string strQuery = "SELECT CMP_BRANCH_ID, CMP_BRANCH_NAME FROM CM_CMP_BRANCH "
    //                        + "ORDER BY NLSSORT(TRIM(CMP_BRANCH_NAME), 'NLS_SORT=generic_m')";


    //    try
    //    {
    //        con = new SqlConnection(cn);
    //        con.Open();
    //        DataSet oDS = new DataSet();
    //        SqlDataAdapter odaData = new SqlDataAdapter(new SqlCommand(strQuery, con));
    //        odaData.Fill(oDS, "CM_CMP_BRANCH");
    //        return oDS;
    //    }
    //    catch (Exception ex)
    //    {
    //        return null;
    //    }

    //    finally
    //    {
    //        con.Close();
    //        con = null;
    //    }
    //}

    //public DataSet GetAllEmpInfo()
    //{
    //    string strQuery = "SELECT EMP_ID,(EMP_TITLE || ' ' || EMP_NAME) as ENAME, DSG_ID, EMP_GENDER, CMP_BRANCH_ID FROM PR_EMPLOYEE_LIST "
    //                        //+ "WHERE CMP_BRANCH_ID='" + bId + "' "
    //                        + "ORDER BY NLSSORT(TRIM(ENAME), 'NLS_SORT=generic_m')";
    //    try
    //    {
    //        SqlConnection
    //        DataSet oDS = new DataSet();
    //        SqlDataAdapter odaData = new SqlDataAdapter(new SqlCommand(strQuery, con));
    //        odaData.Fill(oDS, "PR_EMPLOYEE_LIST");
    //        return oDS;
    //    }
    //    catch (Exception ex)
    //    {
    //        return null;
    //    }
    //}

    public DataSet GetWEEKEND(string bId, string yr, string allbranch)
    {
        string strQuery = "select YR_YEAR, YR_ID, YR_WEEKEND_START, YR_WEEKEND_END "
                            + " from HR_YEAR  "
                            + " where YR_YEAR='" + yr + "' AND (CMP_BRANCH_ID='" + bId + "' OR CMP_BRANCH_ID = '" + allbranch + "') "
                            + " AND YR_STATUS='R' ";

        try
        {
            con = new SqlConnection(cn);
            con.Open();
            DataSet oDS = new DataSet();
            SqlDataAdapter odaData = new SqlDataAdapter(new SqlCommand(strQuery, con));
            odaData.Fill(oDS, "HR_YEAR");
            return oDS;
        }
        catch (Exception ex)
        {
            return null;
        }
        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }
    }
    public DataSet GetWEEKEND(string bId, string yr)
    {
        string strQuery = "select YR_YEAR, YR_ID, YR_WEEKEND_START, YR_WEEKEND_END "
                            + "from HR_YEAR  "
                            + "where YR_YEAR='" + yr + "' AND (CMP_BRANCH_ID='" + bId + "' OR CMP_BRANCH_ID = '100310000') "
                            + "AND YR_STATUS in ('R','TC')";
        var con = new SqlConnection(cn);
        try
        {

            DataSet oDS = new DataSet();
            SqlDataAdapter odaData = new SqlDataAdapter(new SqlCommand(strQuery, con));
            odaData.Fill(oDS, "HR_YEAR");
            return oDS;
        }
        catch (Exception ex)
        {
            return null;
        }
        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }
    }


    public DataSet GetWEEKENDForReport(string bId, string yr)
    {
        string strQuery = "select YR_YEAR, YR_ID, YR_WEEKEND_START, YR_WEEKEND_END "
                            + "from HR_YEAR  "
                            + "where YR_YEAR='" + yr + "' AND (CMP_BRANCH_ID='" + bId + "' OR CMP_BRANCH_ID = '100310000') ";
        var con = new SqlConnection(cn);
        try
        {

            DataSet oDS = new DataSet();
            SqlDataAdapter odaData = new SqlDataAdapter(new SqlCommand(strQuery, con));
            odaData.Fill(oDS, "HR_YEAR");
            return oDS;
        }
        catch (Exception ex)
        {
            return null;
        }
        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }
    }
    public DataSet GetWEEKENDRpt(string bId, string yr, string allbranch) // for report (any hr year)
    {
        //string strQuery = "select YR_YEAR, YR_WEEKEND_START, YR_WEEKEND_END "
        //                    + "from HR_YEAR  "
        //                    + "where YR_YEAR='" + yr + "' AND (CMP_BRANCH_ID='" + bId + "' OR CMP_BRANCH_ID = '100310000') ";

        string strQuery = "select YR_YEAR,YR_ID, YR_WEEKEND_START, YR_WEEKEND_END "
                         + "from HR_YEAR  "
                         + "where YR_YEAR='" + yr + "' AND (CMP_BRANCH_ID='" + bId + "' OR CMP_BRANCH_ID = '" + allbranch + "') ";



        try
        {
            con = new SqlConnection(cn);
            con.Open();
            DataSet oDS = new DataSet();
            SqlDataAdapter odaData = new SqlDataAdapter(new SqlCommand(strQuery, con));
            odaData.Fill(oDS, "HR_YEAR");
            return oDS;
        }
        catch (Exception ex)
        {
            return null;
        }
        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }

    }
    public DataSet FindEmpLeaveType(string bId, string eId, string year, string tId, string dept)
    {
        string strQuery = "SELECT LP.PRLP_CFORWARD, LT.PRLT_ID, PRLT_TITLE, PRLP_DAYS  LEAVE_ALLOWED, PRLT_IS_PAYABLE, LT.PRTL_CODE, LP.TYP_CODE, EL.EMP_CONFIR_DATE, EL.EMP_FINAL_CONFIR_DATE, PRLP_MON_DAYS "
                            + "FROM PR_LEAVE_TYPE LT, PR_LEAVE_POLICY LP, PR_EMPLOYEE_LIST EL, CM_CMP_BRANCH C, HR_YEAR Y "
                            + "WHERE LP.CMP_BRANCH_ID=C.CMP_BRANCH_ID AND LP.YR_ID=Y.YR_ID AND "
                            + "EL.CMP_BRANCH_ID=C.CMP_BRANCH_ID AND LT.PRLT_ID=LP.PRLT_ID AND EL.DSG_ID=LP.DSG_ID AND LP.TYP_CODE=EL.EMP_STATUS "
                            + "AND C.CMP_BRANCH_ID ='" + bId + "' AND EL.EMP_ID ='" + eId + "' AND Y.YR_YEAR ='" + year + "' AND LT.PRLT_ID='" + tId + "' "
                            + "AND LP.DPT_ID='" + dept + "' ORDER BY LT.PRTL_CODE ASC ";
        var con = new SqlConnection(cn);
        try
        {

            DataSet oDS = new DataSet();
            SqlDataAdapter odaData = new SqlDataAdapter(new SqlCommand(strQuery, con));
            odaData.Fill(oDS, "EMP_LEAVE_TYPE");
            return oDS;
        }
        catch (Exception ex)
        {
            return null;
        }
        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }

    }
    public DataSet FindEmpLeaveType(string bId, string eId, string year, string tId)
    {
        //LT.CMP_BRANCH_ID = C.CMP_BRANCH_ID AND 
        //string strQuery = "SELECT LT.PRLT_ID, PRLT_TITLE, PRLP_DAYS  LEAVE_ALLOWED, PRLT_IS_PAYABLE, LT.PRTL_CODE, LP.TYP_CODE, EL.EMP_CONFIR_DATE, EL.EMP_FINAL_CONFIR_DATE, PRLP_MON_DAYS "
        //                    + "FROM PR_LEAVE_TYPE LT, PR_LEAVE_POLICY LP, PR_EMPLOYEE_LIST EL, CM_CMP_BRANCH C, HR_YEAR Y "
        //                    + "WHERE LP.CMP_BRANCH_ID=C.CMP_BRANCH_ID AND LP.YR_ID=Y.YR_ID AND "
        //                    + "EL.CMP_BRANCH_ID=C.CMP_BRANCH_ID AND LT.PRLT_ID=LP.PRLT_ID AND EL.DSG_ID=LP.DSG_ID AND LP.TYP_CODE=EL.EMP_STATUS "
        //                    + "AND C.CMP_BRANCH_ID ='" + bId + "' AND EL.EMP_ID ='" + eId + "' AND Y.YR_YEAR ='" + year + "' AND LT.PRLT_ID='" + tId + "' ORDER BY LT.PRTL_CODE ASC ";

        string strQuery = "SELECT "
                            + "LT.PRLT_ID,"
                            + "PRLT_TITLE,"
                            + "PRLP_DAYS LEAVE_ALLOWED,"
                            + "PRLT_IS_PAYABLE,"
                            + "LT.PRTL_CODE,"
                            + "LP.TYP_CODE,"
                            + "EL.EMP_CONFIR_DATE,"
                            + "EL.EMP_FINAL_CONFIR_DATE,"
                            + "PRLP_MON_DAYS "
                            + "FROM  PR_LEAVE_TYPE LT,"
                            + "PR_LEAVE_POLICY LP,"
                            + "PR_EMPLOYEE_LIST EL,"
                            + "CM_CMP_BRANCH C,"
                            + "HR_YEAR Y "
                            + "WHERE LP.CMP_BRANCH_ID=C.CMP_BRANCH_ID"
                            + " AND LP.YR_ID  = Y.YR_ID"
                            + " AND EL.CMP_BRANCH_ID  = C.CMP_BRANCH_ID"
                            + " AND LT.PRLT_ID  = LP.PRLT_ID"
                            + " AND EL.DSG_ID  = LP.DSG_ID"
                            + " AND LP.TYP_CODE  = EL.EMP_STATUS"
                            + " AND C.CMP_BRANCH_ID  = '" + bId + "'"
                            + " AND EL.EMP_ID  = '" + eId + "'"
                            + " AND Y.YR_YEAR  = '" + year + "'"
                            + " AND LT.PRLT_ID  = '" + tId + "'"
                            + " ORDER BY LT.PRTL_CODE ASC ";
        try
        {
            con = new SqlConnection(cn);
            con.Open();
            DataSet oDS = new DataSet();
            SqlDataAdapter odaData = new SqlDataAdapter(new SqlCommand(strQuery, con));
            odaData.Fill(oDS, "EMP_LEAVE_TYPE");
            return oDS;
        }
        catch (Exception ex)
        {
            return null;
        }
        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }

    }
    //public DataSet FindEmpLeaveType(string bId, string eId, string year, string tId)
    //{
    //    //LT.CMP_BRANCH_ID = C.CMP_BRANCH_ID AND 
    //    string strQuery = "SELECT LT.PRLT_ID, PRLT_TITLE, PRLP_DAYS  LEAVE_ALLOWED, PRLT_IS_PAYABLE, LT.PRTL_CODE, LP.TYP_CODE, EL.EMP_CONFIR_DATE, EL.EMP_FINAL_CONFIR_DATE, PRLP_MON_DAYS "
    //                        + "FROM PR_LEAVE_TYPE LT, PR_LEAVE_POLICY LP, PR_EMPLOYEE_LIST EL, CM_CMP_BRANCH C, HR_YEAR Y "
    //                        + "WHERE LP.CMP_BRANCH_ID=C.CMP_BRANCH_ID AND LP.YR_ID=Y.YR_ID AND "
    //                        + "EL.CMP_BRANCH_ID=C.CMP_BRANCH_ID AND LT.PRLT_ID=LP.PRLT_ID AND EL.DSG_ID=LP.DSG_ID AND LP.TYP_CODE=EL.EMP_STATUS "
    //                        + "AND C.CMP_BRANCH_ID ='" + bId + "' AND EL.EMP_ID ='" + eId + "' AND Y.YR_YEAR ='" + year + "' AND LT.PRLT_ID='" + tId + "' ORDER BY LT.PRTL_CODE ASC ";

    //    try
    //    {
    //        con = new SqlConnection(cn);
    //        con.Open();
    //        DataSet oDS = new DataSet();
    //        SqlDataAdapter odaData = new SqlDataAdapter(new SqlCommand(strQuery, con));
    //        odaData.Fill(oDS, "EMP_LEAVE_TYPE");
    //        return oDS;
    //    }
    //    catch (Exception ex)
    //    {
    //        return null;
    //    }

    //    finally
    //    {
    //        con.Close();
    //        con = null;
    //    }

    //}

    //public DataSet FindLeaveType(string bId, string ltypeId)
    //{
    //    string strQuery = "SELECT PRLT_ID TID, PRLT_TITLE TITLE "
    //                        + "FROM  PR_LEAVE_TYPE WHERE PRLT_ID='" + ltypeId + "' AND CMP_BRANCH_ID ='" + bId + "' " //, LT.PRLT_IS_PAYABLE
    //                        + "from PR_LEAVE PL, PR_LEAVE_TYPE LT, PR_LEAVE_POLICY LP, PR_EMPLOYEE_LIST EL, CM_CMP_BRANCH C "
    //                        + "WHERE PL.EMP_ID=EL.EMP_ID AND PL.CMP_BRANCH_ID=C.CMP_BRANCH_ID AND  EL.CMP_BRANCH_ID=C.CMP_BRANCH_ID "
    //                        + "AND LT.PRLT_ID=LP.PRLT_ID AND EL.DSG_ID=LP.DSG_ID AND LT.PRLT_ID=PL.PRLT_ID "
    //                        + "AND C.CMP_BRANCH_ID ='" + bId + "' AND EL.EMP_ID ='" + eId + "' "
    //                        + "AND LT.PRLT_ID='" + ltypeId + "' AND LVE_STATUS='R' ";
    //    //+ "GROUP BY LT.PRLT_IS_PAYABLE";

    //    try
    //    {
    //        SqlConnection
    //        DataSet oDS = new DataSet();
    //        SqlDataAdapter odaData = new SqlDataAdapter(new SqlCommand(strQuery, con));
    //        odaData.Fill(oDS, "LeaveType");
    //        return oDS;
    //    }
    //    catch (Exception ex)
    //    {
    //        return null;
    //    }
    //}

    //public DataSet TotApprovedLeave(string bId, string eId, string ltypeId, string hrYR)
    //{
    //    string strQuery = "select nvl(sum(LVE_DURATION),0) LEAVE_APPROVED " //, LT.PRLT_IS_PAYABLE
    //                        + "from PR_LEAVE PL, PR_LEAVE_TYPE LT, PR_LEAVE_POLICY LP, PR_EMPLOYEE_LIST EL, CM_CMP_BRANCH C "
    //                        + "WHERE PL.EMP_ID=EL.EMP_ID AND PL.CMP_BRANCH_ID=C.CMP_BRANCH_ID AND  EL.CMP_BRANCH_ID=C.CMP_BRANCH_ID "
    //                        + "AND LT.PRLT_ID=LP.PRLT_ID AND EL.DSG_ID=LP.DSG_ID AND LT.PRLT_ID=PL.PRLT_ID AND EL.EMP_STATUS=LP.TYP_CODE "
    //                        + "AND C.CMP_BRANCH_ID ='" + bId + "' AND EL.EMP_ID ='" + eId + "' "
    //                        + "AND LT.PRLT_ID='" + ltypeId + "' AND LVE_STATUS='R' "
    //                        + "AND TO_DATE(TO_CHAR(LVE_APPROVED_DATE,'YYYY'), 'YYYY') >= TO_DATE('" + hrYR + "', 'YYYY') "
    //                        + "AND TO_DATE(TO_CHAR(LVE_FROM_DATE,'YYYY'), 'YYYY') = TO_DATE('" + hrYR + "', 'YYYY')";
    //    //+ "GROUP BY LT.PRLT_IS_PAYABLE";

    //    try
    //    {
    //        SqlConnection
    //        DataSet oDS = new DataSet();
    //        SqlDataAdapter odaData = new SqlDataAdapter(new SqlCommand(strQuery, con));
    //        odaData.Fill(oDS, "TOT_APPROVED_LEAVE");
    //        return oDS;
    //    }
    //    catch (Exception ex)
    //    {
    //        return null;
    //    }
    //}

    public DataSet TotApprovedLeave(string bId, string eId, string ltypeId, string hrYR)
    {

        //asad

        string strQuery = "SELECT ISNULL(SUM(CONVERT(FLOAT, LVE_APPROVED_DAY)), 0) LEAVE_APPROVED "
                            + "FROM  PR_LEAVE PL  "
                            + "WHERE PL.CMP_BRANCH_ID ='" + bId + "' AND PL.EMP_ID ='" + eId + "' "
                            + "AND PL.PRLT_ID='" + ltypeId + "' AND LVE_STATUS in ('R','TC') "
                            + "AND	DATEPART(YEAR, LVE_APPROVED_DATE)  >=  '" + hrYR + "' "
                            + "AND	DATEPART(YEAR, LVE_FROM_DATE)  =  '" + hrYR + "' ";

        try
        {
            con = new SqlConnection(cn);
            con.Open();
            DataSet oDS = new DataSet();
            SqlDataAdapter odaData = new SqlDataAdapter(new SqlCommand(strQuery, con));
            odaData.Fill(oDS, "TOT_APPROVED_LEAVE");
            return oDS;
        }
        catch (Exception ex)
        {
            return null;

        }
        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }
    }
    public DataSet ChkPayrollPolicy(string bId, string itemId)
    {
        string strQuery = "select SHOW_TAX_PG "
                            + "from PR_PAYROLL_POLICY "
                            + "WHERE CMP_BRANCH_ID ='" + bId + "' "
                            + "AND (PR_POLICY_LATE_DEDUCT='" + itemId + "' OR PR_POLICY_LEAVE_DEDUCT='" + itemId + "')";

        try
        {
            con = new SqlConnection(cn);
            con.Open();
            DataSet oDS = new DataSet();
            SqlDataAdapter odaData = new SqlDataAdapter(new SqlCommand(strQuery, con));
            odaData.Fill(oDS, "CHK_PAYROLL_POLICY");
            return oDS;
        }
        catch (Exception ex)
        {
            return null;
        }
        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }
    }

    public DataSet GetDepartment(string bId)
    {
        string strQuery = "select DPT_ID, DPT_NAME "
                            + "from PR_DEPARTMENT  "
                            + "where CMP_BRANCH_ID='" + bId + "'  ORDER BY LTRIM(RTRIM(DPT_NAME)) )";

        try
        {
            con = new SqlConnection(cn);
            con.Open();
            DataSet oDS = new DataSet();
            SqlDataAdapter odaData = new SqlDataAdapter(new SqlCommand(strQuery, con));
            odaData.Fill(oDS, "GET_DEPARTMENT");
            return oDS;
        }
        catch (Exception ex)
        {
            return null;
        }
        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }
    }
    public DataSet GetDepartment(string bId, string deptId)
    {
        string strQuery = "select DPT_ID, DPT_NAME "
                         + "from PR_DEPARTMENT  "
                         + "where CMP_BRANCH_ID='" + bId + "' and DPT_ID = '" + deptId + "'  ORDER BY LTRIM(RTRIM(DPT_NAME)) ";

        try
        {
            con = new SqlConnection(cn);
            con.Open();
            DataSet oDS = new DataSet();
            SqlDataAdapter odaData = new SqlDataAdapter(new SqlCommand(strQuery, con));
            odaData.Fill(oDS, "GET_DEPARTMENT");
            return oDS;
        }
        catch (Exception ex)
        {
            return null;
        }
        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }
    }

    public string UpdateChildItem(string bId, string gId, string gender, string ownId, double ownAmt)
    {
        string strSql;

        strSql = "SELECT PRST_ID, PRST_AMT,PAMT_PERCENTAGE FROM PR_STRUCTURE "
                + "WHERE CMP_BRANCH_ID='" + bId + "' AND DSG_ID='" + gId + "' AND GENDER='" + gender + "' "
                + "AND PARENT_ITEM_ID='" + ownId + "'";

        try
        {
            DataRow oOrderRow;

            con = new SqlConnection(cn);
            con.Open();
            dbTransaction = con.BeginTransaction();
            DataSet oDS = new DataSet();
            SqlDataAdapter oOrdersDataAdapter = new SqlDataAdapter(new SqlCommand(strSql, con, dbTransaction));
            SqlCommandBuilder oOrdersCmdBuilder = new SqlCommandBuilder(oOrdersDataAdapter);
            oOrdersDataAdapter.MissingSchemaAction = MissingSchemaAction.AddWithKey;
            oOrdersDataAdapter.Fill(oDS, "UPDATE_CHILD_ITEMS");

            double cItemPer = 0;
            double cNewAmt = 0;

            if (oDS.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow prow in oDS.Tables["UPDATE_CHILD_ITEMS"].Rows)
                {
                    prow.BeginEdit();

                    cItemPer = Convert.ToDouble(prow["PAMT_PERCENTAGE"].ToString());
                    cNewAmt = (ownAmt * cItemPer) / 100;
                    prow["PRST_AMT"] = cNewAmt.ToString();

                    cItemPer = 0;
                    cNewAmt = 0;

                    prow.EndEdit();
                }
                oOrdersDataAdapter.Update(oDS, "UPDATE_CHILD_ITEMS");
            }
            dbTransaction.Commit();
            //con.Close();
            return "";

        }
        catch (Exception ex)
        {
            return ex.Message.ToString();
        }
        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }
    }

    public DataSet EmpInvAchievement(string bId, string laNo)
    {
        //string strQuery = "SELECT CI.CI_CLIENT_ID, CI.CI_CLIENT_NAME, D.DISBURS_AMT, D.DISBURSE_DATE "
        //                    + "FROM CR_CLIENT_INFO CI, CM_CMP_BRANCH C, CA_FINANCIAL_CONTRACT FC, CA_DISBURSEMENT D, "
        //                    + "CR_APPLICATION AP, PR_EMPLOYEE_LIST EL, HR_EMP_ACHIEVMENT EA  "
        //                    + "WHERE CI.CMP_BRANCH_ID = C.CMP_BRANCH_ID AND AP.CMP_BRANCH_ID=C.CMP_BRANCH_ID AND EL.CMP_BRANCH_ID=C.CMP_BRANCH_ID "
        //                    + "AND FC.CI_CLIENT_ID=CI.CI_CLIENT_ID AND EA.CMP_BRANCH_ID=C.CMP_BRANCH_ID AND EA.CLIENT_ID=CI.CI_CLIENT_ID "
        //                    + "AND EA.ACH_DATE <> D.DISBURSE_DATE AND EA.ACH_AMMOUNT <> D.DISBURS_AMT AND FC.LA_NO=D.LA_NO AND AP.CI_CLIENT_ID=CI.CI_CLIENT_ID "
        //                    + "AND AP.APP_REFERENCE=EL.EMP_ID AND rownum < 2 "
        //                    + "AND C.CMP_BRANCH_ID ='" + bId + "' AND EL.EMP_ID ='" + eId + "' AND CI.CI_CLIENT_ID = '" + cId + "' "
        //                    + "ORDER BY D.DISBURSE_DATE ASC";

        string strQuery = "SELECT CI.CI_CLIENT_ID, AG.LA_NO, CI.CI_CLIENT_NAME, AG.CONTRACT_AMOUNT, AG.CONTRACT_DATE "
                        + "FROM CA_FINANCIAL_CONTRACT AG, CR_CLIENT_INFO CI, CM_CMP_BRANCH C  "
                        + "WHERE AG.CI_CLIENT_ID=CI.CI_CLIENT_ID AND CI.CMP_BRANCH_ID = C.CMP_BRANCH_ID "
                        + "AND C.CMP_BRANCH_ID='" + bId + "' AND AG.LA_NO='" + laNo + "'";

        try
        {
            con = new SqlConnection(cn);
            con.Open();
            DataSet oDS = new DataSet();
            SqlDataAdapter odaData = new SqlDataAdapter(new SqlCommand(strQuery, con));
            odaData.Fill(oDS, "INV_ACHIEVEMENT");
            return oDS;
        }
        catch (Exception ex)
        {
            return null;
        }
        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }
    }

    public bool ChkEmpInvAchievement(string bId, string eId, string laNo)
    {
        string strSql;

        strSql = "select ACH_ID from HR_EMP_ACHIEVMENT "
                + "WHERE CMP_BRANCH_ID = '" + bId + "' AND EMP_ID = '" + eId + "' AND LA_NO='" + laNo + "' AND TER_TYPE='INV' ";

        try
        {
            //DataRow oOrderRow;
            con = new SqlConnection(cn);
            con.Open();
            DataSet oDS = new DataSet();
            SqlDataAdapter oOrdersDataAdapter = new SqlDataAdapter(new SqlCommand(strSql, con));
            SqlCommandBuilder oOrdersCmdBuilder = new SqlCommandBuilder(oOrdersDataAdapter);
            oOrdersDataAdapter.MissingSchemaAction = MissingSchemaAction.AddWithKey;
            oOrdersDataAdapter.Fill(oDS, "CHK_INV_ACHIEVEMENT");

            DataTable tbl_AD = oDS.Tables["CHK_INV_ACHIEVEMENT"];

            // already exists
            if (tbl_AD.Rows.Count > 0) { return true; }
            else { return false; }
        }
        catch (Exception ex)
        {
            return false;
        }
        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }
    }

    public DataSet EmpDipoAchievement(string bId, string depoId)
    {
        string strQuery = "SELECT FN.FN_SOURCE_ID, FD.FN_DEPO_ACC_ID, FN.SRC_NAME, FD.DEPO_AMT, FD.ACTIVE_DATE "
                        + "FROM FN_DEPOSIT FD,FN_SOURCE FN, CM_CMP_BRANCH C "
                        + "WHERE FD.FN_SOURCE_ID = FN.FN_SOURCE_ID AND FN.CMP_BRANCH_ID = C.CMP_BRANCH_ID "
                        + "AND C.CMP_BRANCH_ID='" + bId + "' AND FD.FN_DEPO_ACC_ID='" + depoId + "'";

        try
        {
            con = new SqlConnection(cn);
            con.Open();
            DataSet oDS = new DataSet();
            SqlDataAdapter odaData = new SqlDataAdapter(new SqlCommand(strQuery, con));
            odaData.Fill(oDS, "DEPO_ACHIEVEMENT");
            return oDS;
        }
        catch (Exception ex)
        {
            return null;
        }
        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }
    }

    public bool ChkEmpDepoAchievement(string bId, string eId, string depoId)
    {
        string strSql;

        strSql = "select ACH_ID from HR_EMP_ACHIEVMENT "
                + "WHERE CMP_BRANCH_ID = '" + bId + "' AND EMP_ID = '" + eId + "' AND FN_DEPO_ACC_ID='" + depoId + "' AND TER_TYPE='DP' ";

        try
        {
            //DataRow oOrderRow;
            con = new SqlConnection(cn);
            con.Open();
            DataSet oDS = new DataSet();
            SqlDataAdapter oOrdersDataAdapter = new SqlDataAdapter(new SqlCommand(strSql, con));
            SqlCommandBuilder oOrdersCmdBuilder = new SqlCommandBuilder(oOrdersDataAdapter);
            oOrdersDataAdapter.MissingSchemaAction = MissingSchemaAction.AddWithKey;
            oOrdersDataAdapter.Fill(oDS, "CHK_DEPO_ACHIEVEMENT");

            DataTable tbl_AD = oDS.Tables["CHK_DEPO_ACHIEVEMENT"];

            // already exists
            if (tbl_AD.Rows.Count > 0) { return true; }
            else { return false; }
        }
        catch (Exception ex)
        {
            return false;
        }
        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }
    }

    #region Monthly Payroll

    public bool CheckEmpPayrollPosted(string bId, string prMonth)
    {
        string strSql;

        //strSql = "select PRD_ID from PR_PAYROLL_DETAIL WHERE CMP_BRANCH_ID = '" + bId + "' AND PRM_ID = '" + prMonth + "' AND IS_GL_POST='Y'";
        strSql = "select PRS_ID from PR_SHEET WHERE CMP_BRANCH_ID = '" + bId + "' AND PRM_ID = '" + prMonth + "' AND IS_GL_POST='Y' ";

        try
        {
            //DataRow oOrderRow;
            con = new SqlConnection(cn);
            con.Open();
            DataSet oDS = new DataSet();
            SqlDataAdapter oOrdersDataAdapter = new SqlDataAdapter(new SqlCommand(strSql, con));
            SqlCommandBuilder oOrdersCmdBuilder = new SqlCommandBuilder(oOrdersDataAdapter);
            oOrdersDataAdapter.MissingSchemaAction = MissingSchemaAction.AddWithKey;
            //oOrdersDataAdapter.Fill(oDS, "PR_PAYROLL_DETAIL");
            oOrdersDataAdapter.Fill(oDS, "PR_PAYROLL_SHEET");

            //DataTable tbl_AD = oDS.Tables["PR_PAYROLL_DETAIL"];
            DataTable tbl_AD = oDS.Tables["PR_PAYROLL_SHEET"];

            // already exists
            if (tbl_AD.Rows.Count > 0) { return true; }
            else { return false; }
        }
        catch (Exception ex)
        {
            return false;
        }
        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }
    }

    public bool CheckEmpPayrollExists(string bId, string prMonth)
    {
        string strSql;

        //strSql = "select PRD_ID from PR_PAYROLL_DETAIL WHERE CMP_BRANCH_ID = '" + bId + "' AND PRM_ID = '" + prMonth + "' AND IS_GL_POST='Y'";
        strSql = "select PRS_ID from PR_SHEET WHERE CMP_BRANCH_ID = '" + bId + "' AND PRM_ID = '" + prMonth + "' ";

        try
        {
            //DataRow oOrderRow;
            con = new SqlConnection(cn);
            con.Open();
            DataSet oDS = new DataSet();
            SqlDataAdapter oOrdersDataAdapter = new SqlDataAdapter(new SqlCommand(strSql, con));
            SqlCommandBuilder oOrdersCmdBuilder = new SqlCommandBuilder(oOrdersDataAdapter);
            oOrdersDataAdapter.MissingSchemaAction = MissingSchemaAction.AddWithKey;
            //oOrdersDataAdapter.Fill(oDS, "PR_PAYROLL_DETAIL");
            oOrdersDataAdapter.Fill(oDS, "PR_PAYROLL_SHEET");

            //DataTable tbl_AD = oDS.Tables["PR_PAYROLL_DETAIL"];
            DataTable tbl_AD = oDS.Tables["PR_PAYROLL_SHEET"];

            // already exists
            if (tbl_AD.Rows.Count > 0) { return true; }
            else { return false; }
        }
        catch (Exception ex)
        {
            return false;
        }
        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }
    }

    public bool CheckEmpPayrollRecord(string bId, string prMonth)
    {
        string strSql;

        //strSql = "select PRD_ID from PR_PAYROLL_DETAIL WHERE CMP_BRANCH_ID = '" + bId + "' AND PRM_ID = '" + prMonth + "' AND IS_GL_POST='Y'";
        strSql = "select PRS_ID from PR_SHEET WHERE CMP_BRANCH_ID = '" + bId + "' AND PRM_ID = '" + prMonth + "' AND IS_GL_POST='Y' ";

        try
        {
            //DataRow oOrderRow;
            con = new SqlConnection(cn);
            con.Open();
            DataSet oDS = new DataSet();
            SqlDataAdapter oOrdersDataAdapter = new SqlDataAdapter(new SqlCommand(strSql, con));
            SqlCommandBuilder oOrdersCmdBuilder = new SqlCommandBuilder(oOrdersDataAdapter);
            oOrdersDataAdapter.MissingSchemaAction = MissingSchemaAction.AddWithKey;
            //oOrdersDataAdapter.Fill(oDS, "PR_PAYROLL_DETAIL");
            oOrdersDataAdapter.Fill(oDS, "PR_PAYROLL_SHEET");

            //DataTable tbl_AD = oDS.Tables["PR_PAYROLL_DETAIL"];
            DataTable tbl_AD = oDS.Tables["PR_PAYROLL_SHEET"];

            // already exists
            if (tbl_AD.Rows.Count > 0) { return true; }
            else { return false; }
        }
        catch (Exception ex)
        {
            return false;
        }
        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }
    }


    public string SavePayroll(string bId, string empId, string gId, string itmId, string itmAmt, string prMonth) // Payroll Detail
    {
        string strSql;

        //SELECT PRM_ID, CMP_BRANCH_ID, EMP_ID, DSG_ID, PR_ITEM_ID, PRST_AMT, TOT_TAX, TOT_INCOME FROM PR_PAYROLL_DETAIL

        strSql = "SELECT PRM_ID, CMP_BRANCH_ID, EMP_ID, DSG_ID, PR_ITEM_ID, PRST_AMT FROM PR_PAYROLL_DETAIL";

        try
        {
            // Payroll Detail
            DataRow oOrderRow;

            con = new SqlConnection(cn);
            con.Open();
            dbTransaction = con.BeginTransaction();
            DataSet oDS = new DataSet();
            SqlDataAdapter oOrdersDataAdapter = new SqlDataAdapter(new SqlCommand(strSql, con, dbTransaction));
            SqlCommandBuilder oOrdersCmdBuilder = new SqlCommandBuilder(oOrdersDataAdapter);
            //oOrdersDataAdapter.MissingSchemaAction = MissingSchemaAction.AddWithKey;
            //oOrdersDataAdapter.Fill(oDS, "PR_PAYROLL_DETAIL");

            oOrdersDataAdapter.FillSchema(oDS, SchemaType.Source, "PR_PAYROLL_DETAIL");
            oOrderRow = oDS.Tables["PR_PAYROLL_DETAIL"].NewRow();

            // 6 fields
            oOrderRow["PRM_ID"] = prMonth;
            oOrderRow["CMP_BRANCH_ID"] = bId;
            oOrderRow["EMP_ID"] = empId;
            oOrderRow["DSG_ID"] = gId;
            oOrderRow["PR_ITEM_ID"] = itmId;

            if (itmAmt != "")
                oOrderRow["PRST_AMT"] = itmAmt;
            else
                oOrderRow["PRST_AMT"] = 0;

            //oOrderRow["PRST_AMT"] = itmAmt;
            //oOrderRow["TOT_TAX"] = totTax;
            //oOrderRow["TOT_INCOME"] = totIncome;

            oDS.Tables["PR_PAYROLL_DETAIL"].Rows.Add(oOrderRow);
            oOrdersDataAdapter.Update(oDS, "PR_PAYROLL_DETAIL");
            dbTransaction.Commit();
            con.Close();
            return "Saved successfully";
        }
        catch (Exception ex)
        {
            return ex.Message.ToString();
        }
        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }
    }

    public DataSet ChkIndPayrollSaved(string bId, string eId, string gId, string prMonth)
    {
        string strQuery = "SELECT * FROM PR_SHEET "
                            + "WHERE CMP_BRANCH_ID='" + bId + "' AND  EMP_ID='" + eId + "' AND "
                            + "DSG_ID='" + gId + "' AND PRM_ID='" + prMonth + "'";


        try
        {
            con = new SqlConnection(cn);
            con.Open();
            DataSet oDS = new DataSet();
            SqlDataAdapter odaData = new SqlDataAdapter(new SqlCommand(strQuery, con));
            odaData.Fill(oDS, "PR_SHEET");
            return oDS;
        }
        catch (Exception ex)
        {
            return null;
        }
        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }
    }

    public DataSet ChkIsGlPost(string bId, string eId, string gId, string prMonth)
    {
        string strQuery = "SELECT * FROM PR_SHEET "
                            + "WHERE CMP_BRANCH_ID='" + bId + "' AND  EMP_ID='" + eId + "' AND "
                            + "DSG_ID='" + gId + "' AND PRM_ID='" + prMonth + "'";


        try
        {
            con = new SqlConnection(cn);
            con.Open();
            DataSet oDS = new DataSet();
            SqlDataAdapter odaData = new SqlDataAdapter(new SqlCommand(strQuery, con));
            odaData.Fill(oDS, "PR_SHEET");
            return oDS;
        }
        catch (Exception ex)
        {
            return null;
        }
        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }
    }

    //public DataSet ChkIndPayrollSaved(string bId, string eId, string prMonth)
    //{
    //    string strQuery = "SELECT * FROM PR_SHEET "
    //                        + "WHERE CMP_BRANCH_ID='" + bId + "' AND  EMP_ID='" + eId + "' "
    //                        + "AND PRM_ID='" + prMonth + "'";
    //    try
    //    {
    //        SqlConnection
    //        DataSet oDS = new DataSet();
    //        SqlDataAdapter odaData = new SqlDataAdapter(new SqlCommand(strQuery, con));
    //        odaData.Fill(oDS, "PR_SHEET");
    //        return oDS;
    //    }
    //    catch (Exception ex)
    //    {
    //        return null;
    //    }
    //}

    public string RemovePayroll(string bId, string eId, string gId, string prMonth)
    {
        string strSql;
        string strSql1;

        strSql = "DELETE from PR_PAYROLL_DETAIL "
                + "WHERE CMP_BRANCH_ID='" + bId + "' AND  EMP_ID='" + eId + "' AND "
                            + "DSG_ID='" + gId + "' AND PRM_ID='" + prMonth + "'";

        strSql1 = "DELETE from PR_SHEET "
                + "WHERE CMP_BRANCH_ID='" + bId + "' AND  EMP_ID='" + eId + "' AND "
                            + "DSG_ID='" + gId + "' AND PRM_ID='" + prMonth + "'";

        try
        {

            con = new SqlConnection(cn);
            con.Open();
            dbTransaction = con.BeginTransaction();

            SqlCommand olcmd = new SqlCommand(strSql, con, dbTransaction);
            olcmd.Connection = con;
            olcmd.ExecuteNonQuery();
            dbTransaction.Commit();

            SqlTransaction dbTransaction1 = null;
            dbTransaction1 = con.BeginTransaction();
            SqlCommand olcmd1 = new SqlCommand(strSql1, con, dbTransaction1);
            olcmd1.Connection = con;
            olcmd1.ExecuteNonQuery();

            dbTransaction1.Commit();
            //con.Close();

            return "Deleted Successfully";
        }
        catch (Exception ex)
        {
            return ex.Message.ToString();
        }
        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }
    }

    public string SavePayrollSheet(
                                    string bId,
                                    string empId,
                                    string gId,
                                    string totTax,
                                    string taxPerMonth,
                                    string totIncome,
                                    string netIncome,
                                    bool isGlPost,
                                    bool isAddBonus,
                                    bool isAddClaim,
                                    string prMonth) // Payroll Sheet
    {
        string strSql;

        strSql = "SELECT TOT_TAX, TAX_PER_MONTH, TOT_INCOME, NET_AMOUNT, EMP_ID, DSG_ID, IS_GL_POST, IS_BONUS_ADD, IS_CLAIM_ADD, PRM_ID, CMP_BRANCH_ID FROM PR_SHEET";


        try
        {
            // Payroll Sheet
            DataRow oOrderRow;

            con = new SqlConnection(cn);
            con.Open();
            dbTransaction = con.BeginTransaction();
            DataSet oDS = new DataSet();
            SqlDataAdapter oOrdersDataAdapter = new SqlDataAdapter(new SqlCommand(strSql, con, dbTransaction));
            SqlCommandBuilder oOrdersCmdBuilder = new SqlCommandBuilder(oOrdersDataAdapter);
            //oOrdersDataAdapter.MissingSchemaAction = MissingSchemaAction.AddWithKey;
            //oOrdersDataAdapter.Fill(oDS, "PR_SHEET");
            oOrdersDataAdapter.FillSchema(oDS, SchemaType.Source, "PR_SHEET");
            oOrderRow = oDS.Tables["PR_SHEET"].NewRow();

            // 9 fields
            oOrderRow["PRM_ID"] = prMonth;
            oOrderRow["CMP_BRANCH_ID"] = bId;
            oOrderRow["EMP_ID"] = empId;
            oOrderRow["DSG_ID"] = gId;
            oOrderRow["TOT_TAX"] = totTax;
            oOrderRow["TAX_PER_MONTH"] = taxPerMonth;

            if (isGlPost)
            {
                oOrderRow["IS_GL_POST"] = "Y";
            }
            else
            {
                oOrderRow["IS_GL_POST"] = "N";
            }

            if (isAddBonus)
            {
                oOrderRow["IS_BONUS_ADD"] = "Y";
            }
            else
            {
                oOrderRow["IS_BONUS_ADD"] = "N";
            }

            if (isAddClaim)
            {
                oOrderRow["IS_CLAIM_ADD"] = "Y";
            }
            else
            {
                oOrderRow["IS_CLAIM_ADD"] = "N";
            }

            oOrderRow["TOT_INCOME"] = totIncome;
            oOrderRow["NET_AMOUNT"] = netIncome;

            oDS.Tables["PR_SHEET"].Rows.Add(oOrderRow);
            oOrdersDataAdapter.Update(oDS, "PR_SHEET");
            dbTransaction.Commit();
            //con.Close();
            return "Saved successfully";
        }
        catch (Exception ex)
        {
            return ex.Message.ToString();
        }
        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }

        //return "Saved successfully";
    }

    //public string TotClaimAmt(string bId, string eId, string prMonth)
    //{
    //    string strQuery = "select nvl(SUM(CL_AMOUNT),0) CLAIM_AMT "
    //                        + "FROM HR_CLAIM_LIST "
    //                        + "WHERE CMP_BRANCH_ID='" + bId + "' AND EMP_ID = '" + eId + "' "
    //                        + "AND PRM_ID = '" + prMonth + "' AND CLAIM_STATUS='S' AND CT_PAY_TYPE='G'";

    //    try
    //    {
    //        con = new SqlConnection(cn);
    //        con.Open();
    //        DataSet oDS = new DataSet();
    //        SqlDataAdapter odaData = new SqlDataAdapter(new SqlCommand(strQuery, con));
    //        odaData.Fill(oDS, "EMP_CLAIM_AMT");

    //        DataRow dRow = oDS.Tables["EMP_CLAIM_AMT"].Rows[0];
    //        return dRow["CLAIM_AMT"].ToString();
    //    }
    //    catch (Exception ex)
    //    {
    //        return ex.Message.ToString();
    //    }

    //    finally
    //    {
    //        con.Close();
    //        con = null;
    //    }
    //}

    //sydur 24-02-2014
    public string TotClaimAmt(string bId, string eId, string prMonth)
    {
        //string strQuery = "select nvl(SUM(CL_AMOUNT),0) CLAIM_AMT "
        //                    + "FROM HR_CLAIM_LIST "
        //                    + "WHERE CMP_BRANCH_ID='" + bId + "' AND EMP_ID = '" + eId + "' "
        //                    + "AND PRM_ID = '" + prMonth + "' AND CLAIM_STATUS='S' AND CT_PAY_TYPE='G'";

        string strQuery = "SELECT ISNULL(SUM(CONVERT(FLOAT, CL_AMOUNT)), 0) CLAIM_AMT FROM  HR_CLAIM_LIST "
                            + "WHERE CMP_BRANCH_ID='" + bId + "' AND EMP_ID = '" + eId + "' "
                            + "AND PRM_ID = '" + prMonth + "' AND CLAIM_STATUS='S' AND CT_PAY_TYPE='G'";

        try
        {
            con = new SqlConnection(cn);
            con.Open();
            DataSet oDS = new DataSet();
            SqlDataAdapter odaData = new SqlDataAdapter(new SqlCommand(strQuery, con));
            odaData.Fill(oDS, "EMP_CLAIM_AMT");

            DataRow dRow = oDS.Tables["EMP_CLAIM_AMT"].Rows[0];
            return dRow["CLAIM_AMT"].ToString();
        }
        catch (Exception ex)
        {
            return ex.Message.ToString();
        }
        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }
    }

    public string TotAdvanceAmt(string bId, string eId, string prMonth)
    {
        //string strQuery = "select nvl(SUM(CS.RETURN_AMOUNT),0) ADVANCE_AMT "
        //                   + " FROM HR_CLAIM_LIST CL, HR_CLAIM_SCHEDULE CS "
        //                   + " WHERE CL.CMP_BRANCH_ID='" + bId + "' AND CL.EMP_ID = '" + eId + "' "
        //                   + " AND CL.PRM_ID=CS.PRM_ID AND CL.EMP_ID=CS.EMP_ID AND CL.CL_ID=CS.CL_ID "
        //                   + " AND CL.PRM_ID = '" + prMonth + "' "
        //                   + " AND CS.STATUS='S' "
        //                   + " AND CL.CT_PAY_TYPE='A'";


        string strQuery = "select ISNULL(SUM(CONVERT(FLOAT, CS.RETURN_AMOUNT)), 0) ADVANCE_AMT "
                           + " FROM HR_CLAIM_LIST CL, HR_CLAIM_SCHEDULE CS "
                           + " WHERE CL.CMP_BRANCH_ID='" + bId + "' AND CL.EMP_ID = '" + eId + "' "
                           + " AND CL.PRM_ID=CS.PRM_ID AND CL.EMP_ID=CS.EMP_ID AND CL.CL_ID=CS.CL_ID "
                           + " AND CL.PRM_ID = '" + prMonth + "' "
                           + " AND CS.STATUS='S' "
                           + " AND CL.CT_PAY_TYPE='A'";
        try
        {
            con = new SqlConnection(cn);
            con.Open();
            DataSet oDS = new DataSet();
            SqlDataAdapter odaData = new SqlDataAdapter(new SqlCommand(strQuery, con));
            odaData.Fill(oDS, "EMP_ADVANCE_AMT");

            DataRow dRow = oDS.Tables["EMP_ADVANCE_AMT"].Rows[0];
            return dRow["ADVANCE_AMT"].ToString();
        }
        catch (Exception ex)
        {
            return ex.Message.ToString();
        }
        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }
    }

    //public string TotAdvanceAmt(string bId, string eId, string prMonth)
    //{
    //    string strQuery = "select nvl(SUM(CL_AMOUNT),0) ADVANCE_AMT "
    //                        + "FROM HR_CLAIM_LIST "
    //                        + "WHERE CMP_BRANCH_ID='" + bId + "' AND EMP_ID = '" + eId + "' "
    //                        + "AND PRM_ID = '" + prMonth + "' AND CLAIM_STATUS='S' AND CT_PAY_TYPE='A'";
    //   
    //    try
    //    {

    //        DataSet oDS = new DataSet();
    //        SqlDataAdapter odaData = new SqlDataAdapter(new SqlCommand(strQuery, con));
    //        odaData.Fill(oDS, "EMP_ADVANCE_AMT");

    //        DataRow dRow = oDS.Tables["EMP_ADVANCE_AMT"].Rows[0];
    //        return dRow["ADVANCE_AMT"].ToString();
    //    }
    //    catch (Exception ex)
    //    {
    //        return ex.Message.ToString();
    //    }

    //    finally
    //    {
    //        con.Close();
    //        con = null;
    //    }
    //}

    public string TotAdvanceAmtFromLoan(string eId, string prMonth)
    {
        //string strQuery = "select nvl(SUM(RETURN_AMOUNT),0) ADVANCE_AMT "
        //                    + "FROM HR_CLAIM_SCHEDULE "
        //                    + "WHERE EMP_ID = '" + eId + "' "
        //                    + "AND PRM_ID = '" + prMonth + "' AND STATUS='S'";

        string strQuery = "SELECT ISNULL(SUM(CONVERT(FLOAT, RETURN_AMOUNT)), 0) ADVANCE_AMT "
                             + "FROM HR_CLAIM_SCHEDULE "
                             + "WHERE EMP_ID = '" + eId + "' "
                             + "AND PRM_ID = '" + prMonth + "' AND STATUS='S'";
        try
        {
            con = new SqlConnection(cn);
            con.Open();
            DataSet oDS = new DataSet();
            SqlDataAdapter odaData = new SqlDataAdapter(new SqlCommand(strQuery, con));
            odaData.Fill(oDS, "EMP_ADVANCE_AMT");

            DataRow dRow = oDS.Tables["EMP_ADVANCE_AMT"].Rows[0];
            return dRow["ADVANCE_AMT"].ToString();
        }
        catch (Exception ex)
        {
            return ex.Message.ToString();
        }
        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }
    }

    public string UpdatePayrollSheet(string bId, string prMonth)
    {
        string updateString;
        //if (dtpReconcileDate.Equals(""))
        //{
        updateString = "UPDATE PR_SHEET SET IS_GL_POST = 'Y' WHERE CMP_BRANCH_ID = '" + bId + "' AND PRM_ID = '" + prMonth + "'";
        //}
        //else
        //{
        //    updateString = @"UPDATE CA_CHQ_REGISTER SET STATUS = '" + strStatus + "',RECONCILE_DATE='" + dtpReconcileDate + "' WHERE CHQ_REG_ID = '" + strChequeID + "'";
        //}


        string strReturn = "";
        try
        {
            con = new SqlConnection(cn);
            con.Open();
            dbTransaction = con.BeginTransaction();
            SqlCommand cmd = new SqlCommand(updateString, con, dbTransaction);
            cmd.Connection = con;
            cmd.ExecuteNonQuery();
            dbTransaction.Commit();
            //con.Close();
        }
        catch (Exception ex)
        {
            strReturn = ex.Message.ToString();
        }
        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }
        return strReturn;
    }

    public string UpdatePayrollMonth(string prMonth)
    {
        string updateString;
        //if (dtpReconcileDate.Equals(""))
        //{
        updateString = "UPDATE PR_PAYROLL_MASTER SET IS_GL_POST = 'Y' WHERE PRM_ID = '" + prMonth + "'";



        string strReturn = "";
        try
        {
            con = new SqlConnection(cn);
            con.Open();
            dbTransaction = con.BeginTransaction();
            SqlCommand cmd = new SqlCommand(updateString, con, dbTransaction);
            cmd.Connection = con;
            cmd.ExecuteNonQuery();
            dbTransaction.Commit();
            //con.Close();
        }
        catch (Exception ex)
        {
            strReturn = ex.Message.ToString();
        }
        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }
        return strReturn;
    }



    #endregion

    //public DataSet clintInfo(string fdate, string tdate, string yr, string Sql)
    #region Employee Performance

    #region old
    //public DataSet clintInfo(string bId, string yId, string asDate)
    //{
    //    //string strQuery = "select DISTINCT DP.EMP_ID, DP.EMP_CODE, DP.ENAME, DP.DSG_TITLE, DP.TER_YEAR_AMMOUNT DP_TER_YEAR_AMMOUNT, "
    //    //                 + "DP.TER_MONTH_AMMOUNT DP_TER_MONTH_AMMOUNT, DP.CMP_BRANCH_NAME, INV.TER_YEAR_AMMOUNT INV_TER_YEAR_AMMOUNT, "
    //    //                 + "INV.TER_MONTH_AMMOUNT INV_TER_MONTH_AMMOUNT, '" + fdate + "' FromDate, '" + tdate + "' ToDate, "
    //    //                 + "R1.TOT_DP_ACH_AMMOUNT, R2.TOT_DP_ACH_AMT_ON_DATE, R3.TOT_DP_CLINT, T1.TOT_INV_ACH_AMMOUNT, T2.TOT_INV_ACH_AMT_ON_DATE, "
    //    //                 + "T3.TOT_INV_CLINT, S1.TOT_SANCTION_AMT, S2.TOT_SANC_AMT_ON_DATE, A1.TOT_APPROVED_AMT, A2.TOT_APPR_AMT_ON_DATE, "
    //    //                 + "D1.TOT_DISBURSEMENT_AMT, D2.TOT_DISBMNT_AMT_ON_DATE "
    //    //                 + "FROM  ALL_DEPOSITE_TER DP, ALL_INVESTMENT_TER INV, "
    //    //                 + "(SELECT SUM( DP.ACH_AMMOUNT) TOT_DP_ACH_AMMOUNT,DP.EMP_ID E_ID FROM  ALL_DEPOSITE_TER DP WHERE DP.TER_DATE "
    //    //                 + "BETWEEN TO_DATE('" + fdate + "', 'DD/MM/YYYY') AND TO_DATE('" + tdate + "', 'DD/MM/YYYY') GROUP BY DP.EMP_ID) R1, "
    //    //                 + "(SELECT SUM( DP.ACH_AMMOUNT) TOT_DP_ACH_AMT_ON_DATE,DP.EMP_ID E_ID FROM  ALL_DEPOSITE_TER DP WHERE DP.TER_DATE "
    //    //                 + "BETWEEN TO_DATE('01-01-" + yr + "', 'DD/MM/YYYY') AND CURRENT_DATE GROUP BY DP.EMP_ID) R2, "
    //    //                 + "(SELECT count(CLIENT_ID) TOT_DP_CLINT, DP.EMP_ID E_ID FROM  ALL_DEPOSITE_TER DP WHERE DP.TER_DATE "
    //    //                 + "BETWEEN TO_DATE('" + fdate + "', 'DD/MM/YYYY') AND TO_DATE('" + tdate + "', 'DD/MM/YYYY') GROUP BY DP.EMP_ID) R3, "
    //    //                 + "(SELECT SUM( INV.ACH_AMMOUNT) TOT_INV_ACH_AMMOUNT,INV.EMP_ID E_ID FROM  ALL_INVESTMENT_TER INV WHERE INV.TER_DATE "
    //    //                 + "BETWEEN TO_DATE('" + fdate + "', 'DD/MM/YYYY') AND TO_DATE('" + tdate + "', 'DD/MM/YYYY') GROUP BY INV.EMP_ID) T1, "
    //    //                 + "(SELECT SUM( INV.ACH_AMMOUNT) TOT_INV_ACH_AMT_ON_DATE,INV.EMP_ID E_ID FROM  ALL_INVESTMENT_TER INV WHERE INV.TER_DATE "
    //    //                 + "BETWEEN TO_DATE('01-01-" + yr + "', 'DD/MM/YYYY') AND CURRENT_DATE GROUP BY INV.EMP_ID) T2, "
    //    //                 + "(SELECT count( INV.CLIENT_ID) TOT_INV_CLINT, INV.EMP_ID E_ID FROM  ALL_INVESTMENT_TER INV WHERE INV.TER_DATE "
    //    //                 + "BETWEEN TO_DATE('" + fdate + "', 'DD/MM/YYYY') AND TO_DATE('" + tdate + "', 'DD/MM/YYYY') GROUP BY INV.EMP_ID) T3, "
    //    //                 + "(select SUM(SANCTION_AMOUNT) TOT_SANCTION_AMT, INV.EMP_ID E_ID from CA_SANCTION SA, ALL_INVESTMENT_TER INV where "
    //    //                 + "SA.CI_CLIENT_ID= INV.CLIENT_ID and INV.ACH_DATE BETWEEN TO_DATE('" + fdate + "', 'DD/MM/YYYY') AND CURRENT_DATE GROUP BY INV.EMP_ID) S1, "
    //    //                 + "(select SUM(SANCTION_AMOUNT) TOT_SANC_AMT_ON_DATE, INV.EMP_ID E_ID from CA_SANCTION SA, ALL_INVESTMENT_TER INV where "
    //    //                 + "SA.CI_CLIENT_ID= INV.CLIENT_ID and INV.ACH_DATE BETWEEN TO_DATE('01-01-" + yr + "', 'DD/MM/YYYY') AND TO_DATE('" + tdate + "', 'DD/MM/YYYY') GROUP BY INV.EMP_ID) S2, "
    //    //                 + "(select SUM(APV_APPROVED_AMT) TOT_APPROVED_AMT, INV.EMP_ID E_ID from CR_APPROVED AP, ALL_INVESTMENT_TER INV where AP.CI_CLIENT_ID= INV.CLIENT_ID and "
    //    //                 + "INV.ACH_DATE BETWEEN TO_DATE('" + fdate + "', 'DD/MM/YYYY') AND TO_DATE('" + tdate + "', 'DD/MM/YYYY') GROUP BY INV.EMP_ID) A1, "
    //    //                 + "(select SUM(APV_APPROVED_AMT) TOT_APPR_AMT_ON_DATE, INV.EMP_ID E_ID from CR_APPROVED AP, ALL_INVESTMENT_TER INV where AP.CI_CLIENT_ID= INV.CLIENT_ID and INV.ACH_DATE "
    //    //                 + "BETWEEN TO_DATE('01-01-" + yr + "', 'DD/MM/YYYY') AND TO_DATE('" + tdate + "', 'DD/MM/YYYY') GROUP BY INV.EMP_ID) A2, "
    //    //                 + "(select SUM(DISBURS_AMT) TOT_DISBURSEMENT_AMT, INV.EMP_ID E_ID from CA_SANCTION SA, ALL_INVESTMENT_TER INV, CA_FINANCIAL_CONTRACT AG,CA_DISBURSEMENT DI "
    //    //                 + "where SA.CI_CLIENT_ID= INV.CLIENT_ID AND INV.CLIENT_ID=AG.CI_CLIENT_ID AND AG.LA_NO=DI.LA_NO and INV.ACH_DATE "
    //    //                 + "BETWEEN TO_DATE('" + fdate + "', 'DD/MM/YYYY') AND TO_DATE('" + tdate + "', 'DD/MM/YYYY') GROUP BY INV.EMP_ID) D1, "
    //    //                 + "(select SUM(DISBURS_AMT) TOT_DISBMNT_AMT_ON_DATE, INV.EMP_ID E_ID from CA_SANCTION SA, ALL_INVESTMENT_TER INV, CA_FINANCIAL_CONTRACT AG,CA_DISBURSEMENT DI "
    //    //                 + "where SA.CI_CLIENT_ID= INV.CLIENT_ID AND INV.CLIENT_ID=AG.CI_CLIENT_ID AND AG.LA_NO=DI.LA_NO and INV.ACH_DATE "
    //    //                 + "BETWEEN TO_DATE('01-01-" + yr + "', 'DD/MM/YYYY') AND TO_DATE('" + tdate + "', 'DD/MM/YYYY') GROUP BY INV.EMP_ID) D2 "
    //    //                 + "WHERE DP.EMP_ID=INV.EMP_ID AND DP.CMP_BRANCH_NAME = INV.CMP_BRANCH_NAME "
    //    //                 + "AND DP.TER_DATE BETWEEN TO_DATE('" + fdate + "', 'DD/MM/YYYY') AND "
    //    //                 + "TO_DATE('" + tdate + "', 'DD/MM/YYYY') AND INV.TER_DATE "
    //    //                 + "BETWEEN TO_DATE('" + fdate + "', 'DD/MM/YYYY') AND TO_DATE('" + tdate + "', 'DD/MM/YYYY') "
    //    //                 + "and DP.EMP_ID=R1.E_ID and DP.EMP_ID=R2.E_ID and DP.EMP_ID=R3.E_ID and INV.EMP_ID=T1.E_ID and INV.EMP_ID=T2.E_ID "
    //    //                 + "and INV.EMP_ID=T3.E_ID and INV.EMP_ID=S1.E_ID and INV.EMP_ID=S2.E_ID and INV.EMP_ID=A1.E_ID and INV.EMP_ID=A2.E_ID "
    //    //                 + "and INV.EMP_ID=D1.E_ID and INV.EMP_ID=D2.E_ID "
    //    //                 + " " + Sql + " ";

    //    string strQuery = "SELECT EP.EMP_ID, EP.ENAME, EP.EMP_CODE, EP.DESIGNATION, EP.BRANCH, EP.DP_YEAR_AMOUNT, EP.DP_MON_AMOUNT, "
    //                    + "INV_YEAR_AMOUNT, INV_MON_AMOUNT, DP_YEAR, INV_YEAR, EP.CMP_BRANCH_ID, '" + asDate + "' AsDate, "
    //                    + "nvl(TDP.TOT_DP_AMT,0) DP_YEAR_ACH_AMT, TDP.DP_NUM_CLIENT, nvl(TIP.TOT_INV_AMT,0) INV_YEAR_ACH_AMT, TIP.INV_NUM_CLIENT, "
    //                    + "nvl(DS.DISBURS_AMT,0) DISBURS_AMT, nvl(DA.DEPOSIT_AMT,0) DEPO_AMOUNT, nvl(DAA.DISBURS_AMT,0) DIS_ASON_AMOUNT, "
    //                    + "nvl(DCMA.DISBURS_AMT,0) DIS_CURR_MONTH_AMOUNT, nvl(AIA.APPROVED_AMT,0) APPROVED_AMOUNT, nvl(COLLECTED.COLLECTED_AMT,0)COLLECTED_AMT, nvl(COLLECTABLE.COLLECTABLE_AMT,0)COLLECTABLE_AMT, "
    //                    + "nvl(TDPM.TOT_DP_AMT,0) DP_MON_ACH_AMT, nvl(TIPM.TOT_INV_AMT,0) INV_MON_ACH_AMT "
    //                    + "FROM HRM_ALL_EMP_PERFOM EP, "

    //                    + "(SELECT EMP_ID, ENAME, SUM (DEPO_AMT) AS TOT_DP_AMT, count(EMP_ID) DP_NUM_CLIENT "
    //                    + "FROM HRM_ALL_YEARLY_DEPOSIT YD "
    //                    + "WHERE (DEPO_STATUS='A' OR (DEPO_STATUS='C' AND CLOSING_DATE BETWEEN  GL_ACC_YEAR_SDATE AND "
    //                    + "TO_DATE('" + asDate + "', 'DD/MM/YYYY') ) ) AND GL_ACC_YEAR_ID ='" + yId + "' "
    //                    + "GROUP BY EMP_ID, ENAME ) TDP, "

    //                    //+ "(SELECT EMP_ID, ENAME, GL_ACC_YEAR_ID, SUM (DEPO_AMT) AS TOT_DP_AMT, count(EMP_ID) DP_NUM_CLIENT FROM HRM_ALL_YEARLY_DEPOSIT YD "
    //        //+ "WHERE GL_ACC_YEAR_ID ='" + yId + "' AND ACTIVE_DATE BETWEEN  GL_ACC_YEAR_SDATE AND  TO_DATE('" + asDate + "', 'DD/MM/YYYY') "
    //        //+ "GROUP BY EMP_ID, ENAME, GL_ACC_YEAR_ID) TDP, "

    //                    + "(SELECT EMP_ID, ENAME, SUM (EXECUTE_AMT) AS TOT_INV_AMT, count(EMP_ID) INV_NUM_CLIENT "
    //                    + "FROM HRM_ALL_YEARLY_INVESTMENT YI "
    //                    + "GROUP BY EMP_ID, ENAME) TIP, "

    //                    //+ "(SELECT EMP_ID, ENAME, GL_ACC_YEAR_ID, SUM (CONTRACT_AMOUNT) AS TOT_INV_AMT, count(EMP_ID) INV_NUM_CLIENT FROM HRM_ALL_YEARLY_INVESTMENT YI "
    //        //+ "WHERE GL_ACC_YEAR_ID ='" + yId + "' AND CONTRACT_DATE BETWEEN  GL_ACC_YEAR_SDATE AND  TO_DATE('" + asDate + "', 'DD/MM/YYYY') "
    //        //+ "GROUP BY EMP_ID, ENAME, GL_ACC_YEAR_ID) TIP, "

    //                    + "(SELECT EMP_ID, ENAME, GL_ACC_YEAR_ID, SUM (DEPO_AMT) AS TOT_DP_AMT "
    //                    + "FROM HRM_ALL_YEARLY_DEPOSIT YD WHERE GL_ACC_YEAR_ID ='" + yId + "' "
    //                    + "AND TO_CHAR(ACTIVE_DATE, 'MM/YYYY')= TO_CHAR(TO_DATE('" + asDate + "', 'DD/MM/YYYY'), 'MM/YYYY') "
    //                    + "GROUP BY EMP_ID, ENAME, GL_ACC_YEAR_ID) TDPM, "

    //                    + "(SELECT EMP_ID, ENAME, GL_ACC_YEAR_ID, SUM (CONTRACT_AMOUNT) AS TOT_INV_AMT "
    //                    + "FROM HRM_ALL_YEARLY_INVESTMENT YI WHERE GL_ACC_YEAR_ID ='" + yId + "' "
    //                    + "AND TO_CHAR(CONTRACT_DATE, 'MM/YYYY')= TO_CHAR(TO_DATE('" + asDate + "', 'DD/MM/YYYY'), 'MM/YYYY') "
    //                    + "GROUP BY EMP_ID, ENAME, GL_ACC_YEAR_ID) TIPM, "

    //                    + "(SELECT sum(DI.DISBURS_AMT) DISBURS_AMT, INV.EMP_ID FROM HRM_ALL_INVESTMENT_TER INV, CR_APPLICATION AP, "
    //                    + "CA_DISBURSEMENT DI, CA_FINANCIAL_CONTRACT AG WHERE AP.REF1_EMP_ID=INV.EMP_ID AND AP.APP_APPLICATION_ID = AG.APPLICATION_ID "
    //                    + "AND AG.LA_NO=DI.LA_NO AND DI.DISBURSE_DATE BETWEEN INV.GL_ACC_YEAR_SDATE AND INV.GL_ACC_YEAR_EDATE AND "
    //                    + "GL_ACC_YEAR_ID='" + yId + "' group by INV.EMP_ID) DS,"

    //                    + "(SELECT EMP_ID, sum(DEPO_AMT) DEPOSIT_AMT FROM HRM_ALL_YEARLY_DEPOSIT "
    //                    + "WHERE GL_ACC_YEAR_ID='" + yId + "' group by EMP_ID) DA, "

    //                    + "(SELECT sum(DI.DISBURS_AMT) DISBURS_AMT, INV.EMP_ID FROM HRM_ALL_INVESTMENT_TER INV, CR_APPLICATION AP, CA_DISBURSEMENT DI, "
    //                    + "CA_FINANCIAL_CONTRACT AG WHERE AP.REF1_EMP_ID=INV.EMP_ID AND AP.APP_APPLICATION_ID = AG.APPLICATION_ID AND AG.LA_NO=DI.LA_NO "
    //                    + "AND TO_CHAR(DI.DISBURSE_DATE, 'MM/YYYY')= TO_CHAR(TO_DATE('" + asDate + "', 'DD/MM/YYYY'), 'MM/YYYY')"
    //                    + "AND GL_ACC_YEAR_ID='" + yId + "' group by INV.EMP_ID) DCMA, "

    //                    + "(SELECT sum(DI.DISBURS_AMT) DISBURS_AMT, INV.EMP_ID FROM HRM_ALL_INVESTMENT_TER INV, CR_APPLICATION AP, CA_DISBURSEMENT DI, "
    //                    + "CA_FINANCIAL_CONTRACT AG WHERE AP.REF1_EMP_ID=INV.EMP_ID AND AP.APP_APPLICATION_ID = AG.APPLICATION_ID AND "
    //                    + "AG.LA_NO=DI.LA_NO AND DI.DISBURSE_DATE BETWEEN INV.GL_ACC_YEAR_SDATE AND TO_DATE('" + asDate + "', 'DD/MM/YYYY') "
    //                    + "AND GL_ACC_YEAR_ID='" + yId + "' group by INV.EMP_ID) DAA, "

    //                    + "(SELECT EMP_ID, sum(APV_APPROVED_AMT) APPROVED_AMT FROM HRM_ALL_YEARLY_APPROVE_INV "
    //                    + "WHERE GL_ACC_YEAR_ID='" + yId + "' group by EMP_ID)AIA, "

    //                    + "(SELECT   DISTINCT EL.EMP_ID, (EMP_TITLE || ' ' || EMP_NAME) ENAME, SUM(INSTALLMENT_AMT) AS COLLECTED_AMT, "
    //                   + "AY.GL_ACC_YEAR_TITLE, AY.GL_ACC_YEAR_ID, AY.GL_ACC_YEAR_SDATE, AY.GL_ACC_YEAR_EDATE "
    //                   + "FROM PR_EMPLOYEE_LIST EL, CR_APPROVED APD, GL_ACCOUNTING_YEAR AY, CR_APPLICATION CA, "
    //                   + "CA_FINANCIAL_CONTRACT FC, CA_EXECUTION EX, CA_AMORTIZATION_SCHEDULE AMS "
    //                   + "WHERE EL.EMP_ID = CA.REF1_EMP_ID AND APD.APP_APPLICATION_ID = CA.APP_APPLICATION_ID AND FC.APPROVAL_ID = APD.APV_APPROVE_ID "
    //                   + "AND FC.LA_NO=EX.LA_NO AND FC.LA_NO=AMS.LA_NO AND EX.EXECUTION_ID=AMS.EXECUTION_ID AND AMS.STATUS='P' "
    //                   + "AND AMS.ADJUSTMENT_DATE BETWEEN AY.GL_ACC_YEAR_SDATE AND  AY.GL_ACC_YEAR_EDATE AND GL_ACC_YEAR_ID='" + yId + "' "
    //                   + "AND ADJUSTMENT_DATE BETWEEN AY.GL_ACC_YEAR_SDATE AND TO_DATE('" + asDate + "', 'DD/MM/YYYY')"
    //                   + "GROUP BY EL.EMP_ID, EMP_TITLE, EMP_NAME, "
    //                   + "AY.GL_ACC_YEAR_TITLE, AY.GL_ACC_YEAR_ID, AY.GL_ACC_YEAR_SDATE, AY.GL_ACC_YEAR_EDATE)COLLECTED, "

    //                   + "( SELECT   DISTINCT EL.EMP_ID, (EMP_TITLE || ' ' || EMP_NAME) ENAME, SUM(INSTALLMENT_AMT) COLLECTABLE_AMT, "
    //                   + "AY.GL_ACC_YEAR_TITLE, AY.GL_ACC_YEAR_ID, AY.GL_ACC_YEAR_SDATE, AY.GL_ACC_YEAR_EDATE "
    //                   + "FROM PR_EMPLOYEE_LIST EL, CR_APPROVED APD, GL_ACCOUNTING_YEAR AY, CR_APPLICATION CA, "
    //                   + "CA_FINANCIAL_CONTRACT FC, CA_EXECUTION EX, CA_AMORTIZATION_SCHEDULE AMS "
    //                   + "WHERE EL.EMP_ID = CA.REF1_EMP_ID AND APD.APP_APPLICATION_ID = CA.APP_APPLICATION_ID AND FC.APPROVAL_ID = APD.APV_APPROVE_ID "
    //                   + "AND FC.LA_NO=EX.LA_NO AND FC.LA_NO=AMS.LA_NO AND EX.EXECUTION_ID=AMS.EXECUTION_ID "
    //                   + "AND AMS.RENTAL_DATE BETWEEN AY.GL_ACC_YEAR_SDATE AND  AY.GL_ACC_YEAR_EDATE AND GL_ACC_YEAR_ID='" + yId + "' "
    //                   + "AND RENTAL_DATE BETWEEN AY.GL_ACC_YEAR_SDATE AND TO_DATE('" + asDate + "', 'DD/MM/YYYY')"
    //                   + "GROUP BY EL.EMP_ID, EMP_TITLE, EMP_NAME, "
    //                   + "AY.GL_ACC_YEAR_TITLE, AY.GL_ACC_YEAR_ID, AY.GL_ACC_YEAR_SDATE, AY.GL_ACC_YEAR_EDATE ) COLLECTABLE "

    //                    + "WHERE TDP.EMP_ID(+) = EP.EMP_ID AND TIP.EMP_ID(+) = EP.EMP_ID AND COLLECTED.EMP_ID(+)=EP.EMP_ID AND COLLECTABLE.EMP_ID(+)=EP.EMP_ID "
    //                    + "AND DS.EMP_ID(+)=EP.EMP_ID AND DA.EMP_ID(+)=EP.EMP_ID AND DCMA.EMP_ID(+)=EP.EMP_ID AND DAA.EMP_ID(+)=EP.EMP_ID AND AIA.EMP_ID(+)=EP.EMP_ID "
    //                    + "AND TDPM.EMP_ID(+) = EP.EMP_ID AND TIPM.EMP_ID(+) = EP.EMP_ID "
    //                    + "AND (DP_YEAR ='" + yId + "' or INV_YEAR = '" + yId + "') AND EP.CMP_BRANCH_ID='" + bId + "' ";

    //    try
    //    {
    //        SqlConnection
    //        DataSet oDS = new DataSet();
    //        SqlDataAdapter odaData = new SqlDataAdapter(new SqlCommand(strQuery, con));
    //        odaData.Fill(oDS, "EMP_PERFORMANCE");
    //        return oDS;
    //    }
    //    catch (Exception ex)
    //    {
    //        return null;
    //    }
    //}
    #endregion

    //not converted to sql
    public DataSet clintInfo(string bId, string yId, string pFromDate, string pToDate)
    {
        #region old
        //string strQuery = "SELECT EP.EMP_ID, EP.ENAME, EP.EMP_CODE, EP.DESIGNATION, EP.BRANCH, EP.DP_YEAR_AMOUNT, EP.DP_MON_AMOUNT, "
        //                + "INV_YEAR_AMOUNT, INV_MON_AMOUNT, DP_YEAR, INV_YEAR, EP.CMP_BRANCH_ID, '" + pToDate + "' pToDate, "
        //                + "nvl(TDP.TOT_DP_AMT,0) DP_YEAR_ACH_AMT, TDP.DP_NUM_CLIENT, nvl(TIP.TOT_INV_AMT,0) INV_YEAR_ACH_AMT, TIP.INV_NUM_CLIENT, "
        //                + "nvl(DS.DISBURS_AMT,0) DISBURS_AMT, nvl(DA.DEPOSIT_AMT,0) DEPO_AMOUNT, nvl(DAA.DISBURS_AMT,0) DIS_ASON_AMOUNT, "
        //                + "nvl(DCMA.DISBURS_AMT,0) DIS_CURR_MONTH_AMOUNT, nvl(AIA.APPROVED_AMT,0) APPROVED_AMOUNT, nvl(COLLECTED.COLLECTED_AMT,0)COLLECTED_AMT, nvl(COLLECTABLE.COLLECTABLE_AMT,0)COLLECTABLE_AMT, "
        //                + "nvl(TDPM.TOT_DP_AMT,0) DP_MON_ACH_AMT, nvl(TIPM.TOT_INV_AMT,0) INV_MON_ACH_AMT "
        //                + "FROM HRM_ALL_EMP_PERFOM EP, "

        //                + "(SELECT EMP_ID, ENAME, SUM (DEPO_AMT) AS TOT_DP_AMT, count(EMP_ID) DP_NUM_CLIENT "
        //                + "FROM HRM_ALL_YEARLY_DEPOSIT YD "
        //                + "WHERE (DEPO_STATUS='A' OR (DEPO_STATUS='C' AND CLOSING_DATE BETWEEN  GL_ACC_YEAR_SDATE AND "
        //                + "TO_DATE('" + pToDate + "', 'DD/MM/YYYY') ) ) AND GL_ACC_YEAR_ID ='" + yId + "' "
        //                + "GROUP BY EMP_ID, ENAME ) TDP, "

        //                //+ "(SELECT EMP_ID, ENAME, GL_ACC_YEAR_ID, SUM (DEPO_AMT) AS TOT_DP_AMT, count(EMP_ID) DP_NUM_CLIENT FROM HRM_ALL_YEARLY_DEPOSIT YD "
        //    //+ "WHERE GL_ACC_YEAR_ID ='" + yId + "' AND ACTIVE_DATE BETWEEN  GL_ACC_YEAR_SDATE AND  TO_DATE('" + pToDate + "', 'DD/MM/YYYY') "
        //    //+ "GROUP BY EMP_ID, ENAME, GL_ACC_YEAR_ID) TDP, "

        //                + "(SELECT EMP_ID, ENAME, SUM (EXECUTE_AMT) AS TOT_INV_AMT, count(EMP_ID) INV_NUM_CLIENT "
        //                + "FROM HRM_ALL_YEARLY_INVESTMENT YI "
        //                + "GROUP BY EMP_ID, ENAME) TIP, "

        //                //+ "(SELECT EMP_ID, ENAME, GL_ACC_YEAR_ID, SUM (CONTRACT_AMOUNT) AS TOT_INV_AMT, count(EMP_ID) INV_NUM_CLIENT FROM HRM_ALL_YEARLY_INVESTMENT YI "
        //    //+ "WHERE GL_ACC_YEAR_ID ='" + yId + "' AND CONTRACT_DATE BETWEEN  GL_ACC_YEAR_SDATE AND  TO_DATE('" + pToDate + "', 'DD/MM/YYYY') "
        //    //+ "GROUP BY EMP_ID, ENAME, GL_ACC_YEAR_ID) TIP, "

        //                + "(SELECT EMP_ID, ENAME, GL_ACC_YEAR_ID, SUM (DEPO_AMT) AS TOT_DP_AMT "
        //                + "FROM HRM_ALL_YEARLY_DEPOSIT YD WHERE GL_ACC_YEAR_ID ='" + yId + "' "
        //                + "AND TO_CHAR(ACTIVE_DATE, 'MM/YYYY')= TO_CHAR(TO_DATE('" + pToDate + "', 'DD/MM/YYYY'), 'MM/YYYY') "
        //                + "GROUP BY EMP_ID, ENAME, GL_ACC_YEAR_ID) TDPM, "

        //                + "(SELECT EMP_ID, ENAME, GL_ACC_YEAR_ID, SUM (CONTRACT_AMOUNT) AS TOT_INV_AMT "
        //                + "FROM HRM_ALL_YEARLY_INVESTMENT YI WHERE GL_ACC_YEAR_ID ='" + yId + "' "
        //                + "AND TO_CHAR(CONTRACT_DATE, 'MM/YYYY')= TO_CHAR(TO_DATE('" + pToDate + "', 'DD/MM/YYYY'), 'MM/YYYY') "
        //                + "GROUP BY EMP_ID, ENAME, GL_ACC_YEAR_ID) TIPM, "

        //                + "(SELECT sum(DI.DISBURS_AMT) DISBURS_AMT, INV.EMP_ID FROM HRM_ALL_INVESTMENT_TER INV, CR_APPLICATION AP, "
        //                + "CA_DISBURSEMENT DI, CA_FINANCIAL_CONTRACT AG WHERE AP.REF1_EMP_ID=INV.EMP_ID AND AP.APP_APPLICATION_ID = AG.APPLICATION_ID "
        //                + "AND AG.LA_NO=DI.LA_NO AND DI.DISBURSE_DATE BETWEEN INV.GL_ACC_YEAR_SDATE AND INV.GL_ACC_YEAR_EDATE AND "
        //                + "GL_ACC_YEAR_ID='" + yId + "' group by INV.EMP_ID) DS,"

        //                + "(SELECT EMP_ID, sum(DEPO_AMT) DEPOSIT_AMT FROM HRM_ALL_YEARLY_DEPOSIT "
        //                + "WHERE GL_ACC_YEAR_ID='" + yId + "' group by EMP_ID) DA, "

        //                + "(SELECT sum(DI.DISBURS_AMT) DISBURS_AMT, INV.EMP_ID FROM HRM_ALL_INVESTMENT_TER INV, CR_APPLICATION AP, CA_DISBURSEMENT DI, "
        //                + "CA_FINANCIAL_CONTRACT AG WHERE AP.REF1_EMP_ID=INV.EMP_ID AND AP.APP_APPLICATION_ID = AG.APPLICATION_ID AND AG.LA_NO=DI.LA_NO "
        //                + "AND TO_CHAR(DI.DISBURSE_DATE, 'MM/YYYY')= TO_CHAR(TO_DATE('" + pToDate + "', 'DD/MM/YYYY'), 'MM/YYYY')"
        //                + "AND GL_ACC_YEAR_ID='" + yId + "' group by INV.EMP_ID) DCMA, "

        //                + "(SELECT sum(DI.DISBURS_AMT) DISBURS_AMT, INV.EMP_ID FROM HRM_ALL_INVESTMENT_TER INV, CR_APPLICATION AP, CA_DISBURSEMENT DI, "
        //                + "CA_FINANCIAL_CONTRACT AG WHERE AP.REF1_EMP_ID=INV.EMP_ID AND AP.APP_APPLICATION_ID = AG.APPLICATION_ID AND "
        //                + "AG.LA_NO=DI.LA_NO AND DI.DISBURSE_DATE BETWEEN INV.GL_ACC_YEAR_SDATE AND TO_DATE('" + pToDate + "', 'DD/MM/YYYY') "
        //                + "AND GL_ACC_YEAR_ID='" + yId + "' group by INV.EMP_ID) DAA, "

        //                + "(SELECT EMP_ID, sum(APV_APPROVED_AMT) APPROVED_AMT FROM HRM_ALL_YEARLY_APPROVE_INV "
        //                + "WHERE GL_ACC_YEAR_ID='" + yId + "' group by EMP_ID)AIA, "

        //                + "(SELECT   DISTINCT EL.EMP_ID, (EMP_TITLE || ' ' || EMP_NAME) ENAME, SUM(INSTALLMENT_AMT) AS COLLECTED_AMT, "
        //               + "AY.GL_ACC_YEAR_TITLE, AY.GL_ACC_YEAR_ID, AY.GL_ACC_YEAR_SDATE, AY.GL_ACC_YEAR_EDATE "
        //               + "FROM PR_EMPLOYEE_LIST EL, CR_APPROVED APD, GL_ACCOUNTING_YEAR AY, CR_APPLICATION CA, "
        //               + "CA_FINANCIAL_CONTRACT FC, CA_EXECUTION EX, CA_AMORTIZATION_SCHEDULE AMS "
        //               + "WHERE EL.EMP_ID = CA.REF1_EMP_ID AND APD.APP_APPLICATION_ID = CA.APP_APPLICATION_ID AND FC.APPROVAL_ID = APD.APV_APPROVE_ID "
        //               + "AND FC.LA_NO=EX.LA_NO AND FC.LA_NO=AMS.LA_NO AND EX.EXECUTION_ID=AMS.EXECUTION_ID AND AMS.STATUS='P' "
        //               + "AND AMS.ADJUSTMENT_DATE BETWEEN AY.GL_ACC_YEAR_SDATE AND  AY.GL_ACC_YEAR_EDATE AND GL_ACC_YEAR_ID='" + yId + "' "
        //               + "AND ADJUSTMENT_DATE BETWEEN AY.GL_ACC_YEAR_SDATE AND TO_DATE('" + pToDate + "', 'DD/MM/YYYY')"
        //               + "GROUP BY EL.EMP_ID, EMP_TITLE, EMP_NAME, "
        //               + "AY.GL_ACC_YEAR_TITLE, AY.GL_ACC_YEAR_ID, AY.GL_ACC_YEAR_SDATE, AY.GL_ACC_YEAR_EDATE)COLLECTED, "

        //               + "( SELECT   DISTINCT EL.EMP_ID, (EMP_TITLE || ' ' || EMP_NAME) ENAME, SUM(INSTALLMENT_AMT) COLLECTABLE_AMT, "
        //               + "AY.GL_ACC_YEAR_TITLE, AY.GL_ACC_YEAR_ID, AY.GL_ACC_YEAR_SDATE, AY.GL_ACC_YEAR_EDATE "
        //               + "FROM PR_EMPLOYEE_LIST EL, CR_APPROVED APD, GL_ACCOUNTING_YEAR AY, CR_APPLICATION CA, "
        //               + "CA_FINANCIAL_CONTRACT FC, CA_EXECUTION EX, CA_AMORTIZATION_SCHEDULE AMS "
        //               + "WHERE EL.EMP_ID = CA.REF1_EMP_ID AND APD.APP_APPLICATION_ID = CA.APP_APPLICATION_ID AND FC.APPROVAL_ID = APD.APV_APPROVE_ID "
        //               + "AND FC.LA_NO=EX.LA_NO AND FC.LA_NO=AMS.LA_NO AND EX.EXECUTION_ID=AMS.EXECUTION_ID "
        //               + "AND AMS.RENTAL_DATE BETWEEN AY.GL_ACC_YEAR_SDATE AND  AY.GL_ACC_YEAR_EDATE AND GL_ACC_YEAR_ID='" + yId + "' "
        //               + "AND RENTAL_DATE BETWEEN AY.GL_ACC_YEAR_SDATE AND TO_DATE('" + pToDate + "', 'DD/MM/YYYY')"
        //               + "GROUP BY EL.EMP_ID, EMP_TITLE, EMP_NAME, "
        //               + "AY.GL_ACC_YEAR_TITLE, AY.GL_ACC_YEAR_ID, AY.GL_ACC_YEAR_SDATE, AY.GL_ACC_YEAR_EDATE ) COLLECTABLE "

        //                + "WHERE TDP.EMP_ID(+) = EP.EMP_ID AND TIP.EMP_ID(+) = EP.EMP_ID AND COLLECTED.EMP_ID(+)=EP.EMP_ID AND COLLECTABLE.EMP_ID(+)=EP.EMP_ID "
        //                + "AND DS.EMP_ID(+)=EP.EMP_ID AND DA.EMP_ID(+)=EP.EMP_ID AND DCMA.EMP_ID(+)=EP.EMP_ID AND DAA.EMP_ID(+)=EP.EMP_ID AND AIA.EMP_ID(+)=EP.EMP_ID "
        //                + "AND TDPM.EMP_ID(+) = EP.EMP_ID AND TIPM.EMP_ID(+) = EP.EMP_ID "
        //                + "AND (DP_YEAR ='" + yId + "' or INV_YEAR = '" + yId + "') AND EP.CMP_BRANCH_ID='" + bId + "' ";

        #endregion
        string strQuery = "SELECT EP.EMP_ID, EP.ENAME, EP.EMP_CODE, EP.DESIGNATION, EP.BRANCH, TOT_TER.*, '" + pToDate + "' pToDate, "
                        + "nvl(TDP.TOT_DP_AMT,0) DP_YEAR_ACH_AMT, TDP.DP_NUM_CLIENT, nvl(TIP.TOT_INV_AMT,0) INV_YEAR_ACH_AMT, TIP.INV_NUM_CLIENT, "
                        + "nvl(DS.DISBURS_AMT,0) DISBURS_AMT, nvl(DA.DEPOSIT_AMT,0) DEPO_AMOUNT, nvl(DAA.DISBURS_AMT,0) DIS_ASON_AMOUNT, "
                        + "nvl(DCMA.DISBURS_AMT,0) DIS_CURR_MONTH_AMOUNT, nvl(AIA.APPROVED_AMT,0) APPROVED_AMOUNT, nvl(COLLECTED.COLLECTED_AMT,0)COLLECTED_AMT, nvl(COLLECTABLE.COLLECTABLE_AMT,0)COLLECTABLE_AMT, "
                        + "nvl(TDPM.TOT_DP_AMT,0) DP_MON_ACH_AMT, nvl(TIPM.TOT_INV_AMT,0) INV_MON_ACH_AMT "
                        + "FROM HRM_NEW_EMP_PERFOM EP, "

                        + "(SELECT   DECODE (INV_YEAR_ID, NULL, DP_YEAR_ID, INV_YEAR_ID) YEAR_ID, nvl(INV.INV_YEAR_AMOUNT,0) INV_YEAR_AMOUNT, nvl(DP.DP_YEAR_AMOUNT,0) DP_YEAR_AMOUNT, "
                        + "DECODE (INV_BRANCH_ID, NULL, DP_BRANCH_ID, INV_BRANCH_ID) BRANCH_ID, DECODE (INV_EID, NULL, DP_EID, INV_EID) EID, "
                        + " nvl(INV.INV_MONTH_AMOUNT,0) INV_MON_AMOUNT, nvl(DP.DP_MONTH_AMOUNT,0) DP_MON_AMOUNT FROM (SELECT * FROM HRM_NEW_DP_TER DT WHERE DT.DP_YEAR_ID='" + yId + "' "
                        + "AND DT.DP_BRANCH_ID='" + bId + "') DP FULL JOIN (SELECT * FROM HRM_NEW_INV_TER IT WHERE IT.INV_YEAR_ID='" + yId + "' "
                        + "AND IT.INV_BRANCH_ID='" + bId + "')INV ON DP.DP_EID = INV.INV_EID) TOT_TER, "


                        + "(SELECT EMP_ID, ENAME, SUM (DEPO_AMT) AS TOT_DP_AMT, count(EMP_ID) DP_NUM_CLIENT "
                        + "FROM HRM_ALL_YEARLY_DEPOSIT YD "
                        + "WHERE (DEPO_STATUS='A' OR (DEPO_STATUS='C' AND CLOSING_DATE BETWEEN  TO_DATE('" + pFromDate + "', 'DD/MM/YYYY') AND "
                        + "TO_DATE('" + pToDate + "', 'DD/MM/YYYY') ) ) AND GL_ACC_YEAR_ID ='" + yId + "' "
                        + "GROUP BY EMP_ID, ENAME ) TDP, "

                        + "(SELECT EMP_ID, ENAME, SUM (EXECUTE_AMT) AS TOT_INV_AMT, count(EMP_ID) INV_NUM_CLIENT "
                        + "FROM HRM_ALL_YEARLY_INVESTMENT YI "
                        + "GROUP BY EMP_ID, ENAME) TIP, "

                        + "(SELECT EMP_ID, ENAME, GL_ACC_YEAR_ID, SUM (DEPO_AMT) AS TOT_DP_AMT "
                        + "FROM HRM_ALL_YEARLY_DEPOSIT YD WHERE GL_ACC_YEAR_ID ='" + yId + "' "
                        + "AND ACTIVE_DATE BETWEEN  TO_DATE('" + pFromDate + "', 'DD/MM/YYYY') AND TO_DATE('" + pToDate + "', 'DD/MM/YYYY') "
                        + "GROUP BY EMP_ID, ENAME, GL_ACC_YEAR_ID) TDPM, "

                        + "(SELECT EMP_ID, ENAME, GL_ACC_YEAR_ID, SUM (CONTRACT_AMOUNT) AS TOT_INV_AMT "
                        + "FROM HRM_ALL_YEARLY_INVESTMENT YI WHERE GL_ACC_YEAR_ID ='" + yId + "' "
                        + "AND CONTRACT_DATE BETWEEN  TO_DATE('" + pFromDate + "', 'DD/MM/YYYY') AND TO_DATE('" + pToDate + "', 'DD/MM/YYYY') "
                        + "GROUP BY EMP_ID, ENAME, GL_ACC_YEAR_ID) TIPM, "

                        + "(SELECT sum(DI.DISBURS_AMT) DISBURS_AMT, INV.EMP_ID FROM HRM_ALL_INVESTMENT_TER INV, CR_APPLICATION AP, "
                        + "CA_DISBURSEMENT DI, CA_FINANCIAL_CONTRACT AG WHERE AP.REF1_EMP_ID=INV.EMP_ID AND AP.APP_APPLICATION_ID = AG.APPLICATION_ID "
                        + "AND AG.LA_NO=DI.LA_NO AND DI.DISBURSE_DATE BETWEEN INV.GL_ACC_YEAR_SDATE AND INV.GL_ACC_YEAR_EDATE AND "
                        + "GL_ACC_YEAR_ID='" + yId + "' group by INV.EMP_ID) DS,"

                        + "(SELECT EMP_ID, sum(DEPO_AMT) DEPOSIT_AMT FROM HRM_ALL_YEARLY_DEPOSIT "
                        + "WHERE GL_ACC_YEAR_ID='" + yId + "' group by EMP_ID) DA, "

                        + "(SELECT sum(DI.DISBURS_AMT) DISBURS_AMT, INV.EMP_ID FROM HRM_ALL_INVESTMENT_TER INV, CR_APPLICATION AP, CA_DISBURSEMENT DI, "
                        + "CA_FINANCIAL_CONTRACT AG WHERE AP.REF1_EMP_ID=INV.EMP_ID AND AP.APP_APPLICATION_ID = AG.APPLICATION_ID AND AG.LA_NO=DI.LA_NO "
                        + "AND DI.DISBURSE_DATE BETWEEN  TO_DATE('" + pFromDate + "', 'DD/MM/YYYY') AND TO_DATE('" + pToDate + "', 'DD/MM/YYYY') "
                        + "AND GL_ACC_YEAR_ID='" + yId + "' group by INV.EMP_ID) DCMA, "

                        + "(SELECT sum(DI.DISBURS_AMT) DISBURS_AMT, INV.EMP_ID FROM HRM_ALL_INVESTMENT_TER INV, CR_APPLICATION AP, CA_DISBURSEMENT DI, "
                        + "CA_FINANCIAL_CONTRACT AG WHERE AP.REF1_EMP_ID=INV.EMP_ID AND AP.APP_APPLICATION_ID = AG.APPLICATION_ID AND "
                        + "AG.LA_NO=DI.LA_NO AND DI.DISBURSE_DATE BETWEEN  TO_DATE('" + pFromDate + "', 'DD/MM/YYYY') AND TO_DATE('" + pToDate + "', 'DD/MM/YYYY') "
                        + "AND GL_ACC_YEAR_ID='" + yId + "' group by INV.EMP_ID) DAA, "

                        + "(SELECT EMP_ID, sum(APV_APPROVED_AMT) APPROVED_AMT FROM HRM_ALL_YEARLY_APPROVE_INV "
                        + "WHERE GL_ACC_YEAR_ID='" + yId + "' group by EMP_ID)AIA, "

                        + "(SELECT   DISTINCT EL.EMP_ID, (EMP_TITLE || ' ' || EMP_NAME) ENAME, SUM(INSTALLMENT_AMT) AS COLLECTED_AMT, "
                       + "AY.GL_ACC_YEAR_TITLE, AY.GL_ACC_YEAR_ID, AY.GL_ACC_YEAR_SDATE, AY.GL_ACC_YEAR_EDATE "
                       + "FROM PR_EMPLOYEE_LIST EL, CR_APPROVED APD, GL_ACCOUNTING_YEAR AY, CR_APPLICATION CA, "
                       + "CA_FINANCIAL_CONTRACT FC, CA_EXECUTION EX, CA_AMORTIZATION_SCHEDULE AMS "
                       + "WHERE EL.EMP_ID = CA.REF1_EMP_ID AND APD.APP_APPLICATION_ID = CA.APP_APPLICATION_ID AND FC.APPROVAL_ID = APD.APV_APPROVE_ID "
                       + "AND FC.LA_NO=EX.LA_NO AND FC.LA_NO=AMS.LA_NO AND EX.EXECUTION_ID=AMS.EXECUTION_ID AND AMS.STATUS='P' "
                       + "AND AMS.ADJUSTMENT_DATE BETWEEN AY.GL_ACC_YEAR_SDATE AND  AY.GL_ACC_YEAR_EDATE AND GL_ACC_YEAR_ID='" + yId + "' "
                       + "AND ADJUSTMENT_DATE BETWEEN  TO_DATE('" + pFromDate + "', 'DD/MM/YYYY') AND TO_DATE('" + pToDate + "', 'DD/MM/YYYY')"
                       + "GROUP BY EL.EMP_ID, EMP_TITLE, EMP_NAME, "
                       + "AY.GL_ACC_YEAR_TITLE, AY.GL_ACC_YEAR_ID, AY.GL_ACC_YEAR_SDATE, AY.GL_ACC_YEAR_EDATE)COLLECTED, "

                       + "( SELECT   DISTINCT EL.EMP_ID, (EMP_TITLE || ' ' || EMP_NAME) ENAME, SUM(INSTALLMENT_AMT) COLLECTABLE_AMT, "
                       + "AY.GL_ACC_YEAR_TITLE, AY.GL_ACC_YEAR_ID, AY.GL_ACC_YEAR_SDATE, AY.GL_ACC_YEAR_EDATE "
                       + "FROM PR_EMPLOYEE_LIST EL, CR_APPROVED APD, GL_ACCOUNTING_YEAR AY, CR_APPLICATION CA, "
                       + "CA_FINANCIAL_CONTRACT FC, CA_EXECUTION EX, CA_AMORTIZATION_SCHEDULE AMS "
                       + "WHERE EL.EMP_ID = CA.REF1_EMP_ID AND APD.APP_APPLICATION_ID = CA.APP_APPLICATION_ID AND FC.APPROVAL_ID = APD.APV_APPROVE_ID "
                       + "AND FC.LA_NO=EX.LA_NO AND FC.LA_NO=AMS.LA_NO AND EX.EXECUTION_ID=AMS.EXECUTION_ID "
                       + "AND AMS.RENTAL_DATE BETWEEN AY.GL_ACC_YEAR_SDATE AND  AY.GL_ACC_YEAR_EDATE AND GL_ACC_YEAR_ID='" + yId + "' "
                       + "AND RENTAL_DATE BETWEEN TO_DATE('" + pFromDate + "', 'DD/MM/YYYY') AND TO_DATE('" + pToDate + "', 'DD/MM/YYYY') "
                       + "GROUP BY EL.EMP_ID, EMP_TITLE, EMP_NAME, "
                       + "AY.GL_ACC_YEAR_TITLE, AY.GL_ACC_YEAR_ID, AY.GL_ACC_YEAR_SDATE, AY.GL_ACC_YEAR_EDATE ) COLLECTABLE "

                       + "WHERE TDP.EMP_ID(+) = EP.EMP_ID AND TIP.EMP_ID(+) = EP.EMP_ID AND COLLECTED.EMP_ID(+)=EP.EMP_ID AND COLLECTABLE.EMP_ID(+)=EP.EMP_ID "
                       + "AND DS.EMP_ID(+)=EP.EMP_ID AND DA.EMP_ID(+)=EP.EMP_ID AND DCMA.EMP_ID(+)=EP.EMP_ID AND DAA.EMP_ID(+)=EP.EMP_ID AND AIA.EMP_ID(+)=EP.EMP_ID "
                       + "AND TDPM.EMP_ID(+) = EP.EMP_ID AND TIPM.EMP_ID(+) = EP.EMP_ID "
                       + "AND EP.CMP_BRANCH_ID=TOT_TER.BRANCH_ID AND EP.EMP_ID=TOT_TER.EID AND EP.CMP_BRANCH_ID='" + bId + "' ";


        try
        {
            con = new SqlConnection(cn);
            con.Open();
            DataSet oDS = new DataSet();
            SqlDataAdapter odaData = new SqlDataAdapter(new SqlCommand(strQuery, con));
            odaData.Fill(oDS, "EMP_PERFORMANCE");
            return oDS;
        }
        catch (Exception ex)
        {
            return null;
        }
        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }
    }

    public DataSet AccYear(string yId)
    {
        string strQuery = "SELECT GL_ACC_YEAR_ID, GL_ACC_YEAR_TITLE, GL_ACC_YEAR_SDATE FROM GL_ACCOUNTING_YEAR "
                        + "WHERE GL_ACC_YEAR_ID = '" + yId + "' ";


        try
        {
            con = new SqlConnection(cn);
            con.Open();
            DataSet oDS = new DataSet();
            SqlDataAdapter odaData = new SqlDataAdapter(new SqlCommand(strQuery, con));
            odaData.Fill(oDS, "GL_ACCOUNTING_YEAR");
            return oDS;
        }
        catch (Exception ex)
        {
            return null;
        }
        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }
    }
    #endregion

    public DataSet GetOfficeTime(string bId)  //CHANGED ASAD 24-07-14
    {
        string strQuery = " SELECT GRACE_TIME, CMP_BRANCH_ID, "
                        + " LEFT(DATEADD(mi,CONVERT(NUMERIC(8, 2), 0), (CONVERT(TIME(0),YR_OFFICE_HOUR))),5)  + ' ' + RIGHT(CONVERT(VARCHAR(30), CAST(DATEADD(MINUTE, "
                        + " CONVERT(NUMERIC(8, 2), 0), YR_OFFICE_HOUR) AS time(0)), 9), 2)YR_OFFICE_HOUR "
                        + " FROM HR_YEAR "
                        + " WHERE YR_YEAR  = DATEPART(YYYY, GETDATE()) AND CMP_BRANCH_ID='" + bId + "' AND YR_STATUS='R'";


        try
        {
            con = new SqlConnection(cn);
            con.Open();
            DataSet oDS = new DataSet();
            SqlDataAdapter odaData = new SqlDataAdapter(new SqlCommand(strQuery, con));
            odaData.Fill(oDS, "HR_YEAR");
            return oDS;
        }
        catch (Exception ex)
        {
            return null;
        }
        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }
    }

    #region Employee/Applicant PhotoSignature
    public string InsertPhotoSignature(string bId, string eId, string empPicName, string empSignatureName)
    {


        try
        {

            string strSql = "SELECT  EMP_ID, CMP_BRANCH_ID, EMP_PIC, EMP_SIGNATURE FROM PR_EMPLOYEE_LIST "
                            + "WHERE EMP_ID='" + eId + "' AND CMP_BRANCH_ID = '" + bId + "'";
            DataRow oOrderRow;

            con = new SqlConnection(cn);
            con.Open();
            dbTransaction = con.BeginTransaction();
            DataSet oDs = new DataSet();
            SqlDataAdapter oDbAdapter = new SqlDataAdapter(new SqlCommand(strSql, con, dbTransaction));
            SqlCommandBuilder oDbCmdBuilder = new SqlCommandBuilder(oDbAdapter);
            oDbAdapter.MissingSchemaAction = MissingSchemaAction.AddWithKey;
            oDbAdapter.Fill(oDs, "EMP_PHOTO_SIG");
            // UPDATE Data
            //oOrderRow = oDs.Tables["EMP_PHOTO_SIG"].NewRow();

            //oOrderRow["CMP_BRANCH_ID"] = bId;
            //oOrderRow["EMP_ID"] = eId;
            //string strPhotoID = oDs.Tables[0].Rows[0]["PHOTOSIG_ID"].ToString();
            oOrderRow = oDs.Tables["EMP_PHOTO_SIG"].Rows.Find(eId);

            oOrderRow["EMP_PIC"] = empPicName;
            oOrderRow["EMP_SIGNATURE"] = empSignatureName;


            //oDs.Tables["EMP_PHOTO_SIG"].Rows.Add(oOrderRow);
            oDbAdapter.Update(oDs, "EMP_PHOTO_SIG");
            dbTransaction.Commit();
            //con.Close();
        }
        catch (Exception ex)
        {
            return "Err:" + ex.Message.ToString();
            //return null;
        }
        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }

        return "Success";
    }

    public DataSet GetPhotoSignature(string bId, string eId)
    {


        try
        {
            string strSql = "SELECT EMP_ID, CMP_BRANCH_ID, EMP_PIC, EMP_SIGNATURE FROM PR_EMPLOYEE_LIST "
                            + "WHERE EMP_ID='" + eId + "' AND CMP_BRANCH_ID = '" + bId + "'";

            con = new SqlConnection(cn);
            con.Open();
            DataSet oDS = new DataSet();
            // Create the DataTable "Orders" in the Dataset and the OrdersDataAdapter
            SqlDataAdapter oOrdersDataAdapter = new SqlDataAdapter(new SqlCommand(strSql, con));
            SqlCommandBuilder oOrdersCmdBuilder = new SqlCommandBuilder(oOrdersDataAdapter);
            oOrdersDataAdapter.Fill(oDS, "EMP_PHOTO_SIG");
            return oDS;
        }
        catch (Exception ex)
        {
            return null;
        }

        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }
    }
    #endregion

    #region Address & Contact

    public string InsertAddressCon(string bId, string eId, string strPreAddress, string strPerAddress, string strConNumber, string strTinNumber, string strNationalID, string strPassportID, string strEmail, string facebookId, string linkdinId)
    {


        try
        {
            string strSql = "SELECT FACEBOOK_ID,LINKEDIN_ID, EMP_ID, EMP_PRE_ADDRES, EMP_PER_ADDRESS, EMP_CONTACT_NUM, EMP_TIN_NO, EMP_NATIONAL_ID, EMP_PASPORT_ID, EMP_EMAIL, CMP_BRANCH_ID FROM PR_EMPLOYEE_LIST "
                     + "WHERE EMP_ID='" + eId + "' AND CMP_BRANCH_ID = '" + bId + "'";
            DataRow oOrderRow;

            con = new SqlConnection(cn);
            con.Open();
            dbTransaction = con.BeginTransaction();
            DataSet oDs = new DataSet();
            SqlDataAdapter oDbAdapter = new SqlDataAdapter(new SqlCommand(strSql, con, dbTransaction));
            SqlCommandBuilder oDbCmdBuilder = new SqlCommandBuilder(oDbAdapter);
            oDbAdapter.MissingSchemaAction = MissingSchemaAction.AddWithKey;
            oDbAdapter.Fill(oDs, "PR_EMPLOYEE_LIST");
            // Insert the Data

            oOrderRow = oDs.Tables["PR_EMPLOYEE_LIST"].Rows.Find(eId);

            oOrderRow["EMP_PRE_ADDRES"] = strPreAddress;
            oOrderRow["EMP_PER_ADDRESS"] = strPerAddress;
            oOrderRow["EMP_CONTACT_NUM"] = strConNumber;
            oOrderRow["EMP_TIN_NO"] = strTinNumber;
            oOrderRow["EMP_NATIONAL_ID"] = strNationalID;
            oOrderRow["EMP_PASPORT_ID"] = strPassportID;
            oOrderRow["EMP_EMAIL"] = strEmail;
            oOrderRow["FACEBOOK_ID"] = facebookId;
            oOrderRow["LINKEDIN_ID"] = linkdinId;
            //oOrderRow["EMP_ID"] = eId;
            //oOrderRow["CMP_BRANCH_ID"] = bId;


            oDbAdapter.Update(oDs, "PR_EMPLOYEE_LIST");
            dbTransaction.Commit();
            con.Close();
        }
        catch (Exception ex)
        {

            return null;
        }

        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }

        return "Success";
    }

    #endregion

    #region academic qualification
    public string InsertAcademicQua(string bId, string eId, string strAcqLevelEducation, string strAcqDegreeTitle, string strAcqConcertration, string strAcqInstituteName, string strAcqResult, string strAcqCGPA, string strAcqScal, string strAcqYearPassing, string strAcqDuration, string strAcqAchievement)
    {


        try
        {
            string strSql = "SELECT EMP_ID, CMP_BRANCH_ID, ACQ_LEVEL_OF_EDUCATION, ACQ_DEGREE_TITLE, ACQ_CONCENTRATION, ACQ_INSTITUTE_NAME, ACQ_RESULT, ACQ_CGPA, ACQ_SCALE, ACQ_YEAR_PASSING, ACQ_DURATION, ACQ_ACHIEVEMENT FROM HR_EMP_JOB_ACADEMIC_QUA ";
            DataRow oOrderRow;

            con = new SqlConnection(cn);
            con.Open();
            dbTransaction = con.BeginTransaction();
            DataSet oDs = new DataSet();
            SqlDataAdapter oDbAdapter = new SqlDataAdapter(new SqlCommand(strSql, con, dbTransaction));
            SqlCommandBuilder oDbCmdBuilder = new SqlCommandBuilder(oDbAdapter);
            //oDbAdapter.MissingSchemaAction = MissingSchemaAction.AddWithKey;
            //oDbAdapter.Fill(oDs, "HR_EMP_JOB_ACADEMIC_QUA");
            oDbAdapter.FillSchema(oDs, SchemaType.Source, "HR_EMP_JOB_ACADEMIC_QUA");

            // Insert the Data
            oOrderRow = oDs.Tables["HR_EMP_JOB_ACADEMIC_QUA"].NewRow();

            // 12 fields
            oOrderRow["ACQ_LEVEL_OF_EDUCATION"] = strAcqLevelEducation;
            oOrderRow["ACQ_DEGREE_TITLE"] = strAcqDegreeTitle;
            oOrderRow["ACQ_CONCENTRATION"] = strAcqConcertration;
            oOrderRow["ACQ_INSTITUTE_NAME"] = strAcqInstituteName;
            oOrderRow["ACQ_RESULT"] = strAcqResult;
            oOrderRow["ACQ_CGPA"] = strAcqCGPA;
            oOrderRow["ACQ_SCALE"] = strAcqScal;
            oOrderRow["ACQ_YEAR_PASSING"] = strAcqYearPassing;
            oOrderRow["ACQ_DURATION"] = strAcqDuration;
            oOrderRow["ACQ_ACHIEVEMENT"] = strAcqAchievement;
            oOrderRow["EMP_ID"] = eId;
            oOrderRow["CMP_BRANCH_ID"] = bId;

            oDs.Tables["HR_EMP_JOB_ACADEMIC_QUA"].Rows.Add(oOrderRow);
            oDbAdapter.Update(oDs, "HR_EMP_JOB_ACADEMIC_QUA");
            dbTransaction.Commit();
            //con.Close();
        }
        catch (Exception ex)
        {

            return null;
        }
        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }

        return "Success";
    }

    public DataSet GetEmpAcademicQua(string bId, string eId)
    {
        string strQuery = "SELECT * FROM HR_EMP_JOB_ACADEMIC_QUA WHERE EMP_ID = '" + eId + "' AND CMP_BRANCH_ID='" + bId + "'";

        try
        {
            con = new SqlConnection(cn);
            con.Open();
            DataSet oDS = new DataSet();
            SqlDataAdapter odaData = new SqlDataAdapter(new SqlCommand(strQuery, con));
            odaData.Fill(oDS, "EMPLOYEE_ACADEMIC_QUA");
            return oDS;
        }
        catch (Exception ex)
        {
            return null;
        }

        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }
    }
    #endregion

    #region Training Summery

    //Traning Summary
    public string InsertTraningSummary(string bId, string eId, string strTitle, string strTopicsCovered, string strInstitute, string strCountry, string strLocation, string strYear, string strDuration)
    {


        try
        {
            string strSql = "SELECT EMP_ID, CMP_BRANCH_ID, TRA_TITLE, TRA_TOPICS_COVERED, TRA_INSTITUTE, TRA_COUNTRY, TRA_LOCATION, TRA_TRANING_YEAR, TRA_DURATION FROM HR_EMP_JOB_TRANING";
            DataRow oOrderRow;

            con = new SqlConnection(cn);
            con.Open();
            dbTransaction = con.BeginTransaction();
            DataSet oDs = new DataSet();
            SqlDataAdapter oDbAdapter = new SqlDataAdapter(new SqlCommand(strSql, con, dbTransaction));
            SqlCommandBuilder oDbCmdBuilder = new SqlCommandBuilder(oDbAdapter);
            //oDbAdapter.MissingSchemaAction = MissingSchemaAction.AddWithKey;
            //oDbAdapter.Fill(oDs, "HR_EMP_JOB_TRANING");
            oDbAdapter.FillSchema(oDs, SchemaType.Source, "HR_EMP_JOB_TRANING");

            // Insert the Data
            oOrderRow = oDs.Tables["HR_EMP_JOB_TRANING"].NewRow();

            //oOrderRow["SH_TRAN_ID"] = GetTransactionID();
            oOrderRow["TRA_TITLE"] = strTitle;
            oOrderRow["TRA_TOPICS_COVERED"] = strTopicsCovered;
            oOrderRow["TRA_INSTITUTE"] = strInstitute;
            oOrderRow["TRA_COUNTRY"] = strCountry;
            oOrderRow["TRA_LOCATION"] = strLocation;
            oOrderRow["TRA_TRANING_YEAR"] = strYear;
            oOrderRow["TRA_DURATION"] = strDuration;
            oOrderRow["EMP_ID"] = eId;
            oOrderRow["CMP_BRANCH_ID"] = bId;

            oDs.Tables["HR_EMP_JOB_TRANING"].Rows.Add(oOrderRow);
            oDbAdapter.Update(oDs, "HR_EMP_JOB_TRANING");
            dbTransaction.Commit();
            //con.Close();
        }
        catch (Exception ex)
        {

            return null;
        }

        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }
        return "Success";
    }

    public DataSet GetEmpTraningSummary(string bId, string eId)
    {
        string strQuery = "SELECT * FROM HR_EMP_JOB_TRANING WHERE EMP_ID = '" + eId + "' AND CMP_BRANCH_ID='" + bId + "'";

        try
        {
            con = new SqlConnection(cn);
            con.Open();
            DataSet oDS = new DataSet();
            SqlDataAdapter odaData = new SqlDataAdapter(new SqlCommand(strQuery, con));
            odaData.Fill(oDS, "EMPLOYEE_JOB_TRANING");
            return oDS;
        }
        catch (Exception ex)
        {
            return null;
        }
        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }
    }

    #endregion

    #region Proffessional Qualification

    //Professional Qualification

    public string InsertProfessonalQua(string bId, string eId, string strCertification, string strinstitute, string strLocation, string strFromDate, string strToDate)
    {


        try
        {
            string strSql = "SELECT EMP_ID, CMP_BRANCH_ID, PRQ_CERTIFICATION, PRQ_INSTITUTE, PRQ_LOCATION, PRQ_FROM_DATE, PRQ_TO_DATE FROM HR_EMP_JOB_PROFESSIONAL_QUA ";
            DataRow oOrderRow;

            con = new SqlConnection(cn);
            con.Open();
            dbTransaction = con.BeginTransaction();
            DataSet oDs = new DataSet();
            SqlDataAdapter oDbAdapter = new SqlDataAdapter(new SqlCommand(strSql, con, dbTransaction));
            SqlCommandBuilder oDbCmdBuilder = new SqlCommandBuilder(oDbAdapter);
            //oDbAdapter.MissingSchemaAction = MissingSchemaAction.AddWithKey;
            //oDbAdapter.Fill(oDs, "HR_EMP_JOB_PROFESSIONAL_QUA");
            oDbAdapter.FillSchema(oDs, SchemaType.Source, "HR_EMP_JOB_PROFESSIONAL_QUA");
            // Insert the Data
            oOrderRow = oDs.Tables["HR_EMP_JOB_PROFESSIONAL_QUA"].NewRow();
            //oOrderRow["SH_TRAN_ID"] = GetTransactionID();

            oOrderRow["PRQ_CERTIFICATION"] = strCertification;
            oOrderRow["PRQ_INSTITUTE"] = strinstitute;
            oOrderRow["PRQ_LOCATION"] = strLocation;
            oOrderRow["PRQ_FROM_DATE"] = Convert.ToDateTime(strFromDate);
            oOrderRow["PRQ_TO_DATE"] = Convert.ToDateTime(strToDate);
            oOrderRow["EMP_ID"] = eId;
            oOrderRow["CMP_BRANCH_ID"] = bId;

            oDs.Tables["HR_EMP_JOB_PROFESSIONAL_QUA"].Rows.Add(oOrderRow);
            oDbAdapter.Update(oDs, "HR_EMP_JOB_PROFESSIONAL_QUA");
            dbTransaction.Commit();
            //con.Close();
        }
        catch (Exception ex)
        {

            return null;
        }

        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }

        return "Success";
    }

    public DataSet GetEmpProfessonalQua(string bId, string eId)
    {
        string strQuery = "SELECT * FROM HR_EMP_JOB_PROFESSIONAL_QUA WHERE EMP_ID = '" + eId + "' AND CMP_BRANCH_ID='" + bId + "'";

        try
        {
            con = new SqlConnection(cn);
            con.Open();
            DataSet oDS = new DataSet();
            SqlDataAdapter odaData = new SqlDataAdapter(new SqlCommand(strQuery, con));
            odaData.Fill(oDS, "EMPLOYEE_JOB_PROFESSIONAL_QUA");
            return oDS;
        }
        catch (Exception ex)
        {
            return null;
        }
        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }

    }

    #endregion

    #region Employment History

    public string InsertEmpHistory(string bId, string eId, string strComName, string strComBusiness, string strComLocation, string strPositionHeld, string strDepartment, string strResponsibilities, string strFromDDate, string strTodate, string strAreaExperience, string HistoryAt)
    {


        try
        {
            string strSql = "SELECT EMP_ID, CMP_BRANCH_ID, EMH_COMPANY_NAME, EMH_COMPANY_BUSINESS, EMH_COMPANY_LOCATION, EMH_POSITION_HELD, EMP_DEPARTMENT, EMH_RESPONSIBILITIES, EMH_FROM_DATE, EMH_TO_DATE, EMH_AREA_EXPERIENCE,HistoryAt FROM HR_EMP_JOB_EMPLOYMENT_HISTORY";
            DataRow oOrderRow;

            con = new SqlConnection(cn);
            con.Open();
            dbTransaction = con.BeginTransaction();
            DataSet oDs = new DataSet();
            SqlDataAdapter oDbAdapter = new SqlDataAdapter(new SqlCommand(strSql, con, dbTransaction));
            SqlCommandBuilder oDbCmdBuilder = new SqlCommandBuilder(oDbAdapter);
            //oDbAdapter.MissingSchemaAction = MissingSchemaAction.AddWithKey;
            //oDbAdapter.Fill(oDs, "HR_EMP_JOB_EMPLOYMENT_HISTORY");
            oDbAdapter.FillSchema(oDs, SchemaType.Source, "HR_EMP_JOB_EMPLOYMENT_HISTORY");
            // Insert the Data
            oOrderRow = oDs.Tables["HR_EMP_JOB_EMPLOYMENT_HISTORY"].NewRow();
            //oOrderRow["SH_TRAN_ID"] = GetTransactionID();

            oOrderRow["EMH_COMPANY_NAME"] = strComName;
            oOrderRow["EMH_COMPANY_BUSINESS"] = strComBusiness;
            oOrderRow["EMH_COMPANY_LOCATION"] = strComLocation;
            oOrderRow["EMH_POSITION_HELD"] = strPositionHeld;
            oOrderRow["EMP_DEPARTMENT"] = strDepartment;
            oOrderRow["EMH_RESPONSIBILITIES"] = strResponsibilities;
            oOrderRow["EMH_FROM_DATE"] = Convert.ToDateTime(strFromDDate);
            oOrderRow["EMH_TO_DATE"] = Convert.ToDateTime(strTodate);
            oOrderRow["EMH_AREA_EXPERIENCE"] = strAreaExperience;
            oOrderRow["EMP_ID"] = eId;
            oOrderRow["CMP_BRANCH_ID"] = bId;
            oOrderRow["HistoryAt"] = HistoryAt;

            oDs.Tables["HR_EMP_JOB_EMPLOYMENT_HISTORY"].Rows.Add(oOrderRow);
            oDbAdapter.Update(oDs, "HR_EMP_JOB_EMPLOYMENT_HISTORY");
            dbTransaction.Commit();
            //con.Close();
        }
        catch (Exception ex)
        {

            return null;
        }

        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }

        return "Success";
    }

    public DataSet GetEmpEmpHistory(string bId, string eId)
    {
        string strQuery = "SELECT * FROM HR_EMP_JOB_EMPLOYMENT_HISTORY WHERE EMP_ID = '" + eId + "' AND CMP_BRANCH_ID='" + bId + "'";

        try
        {
            con = new SqlConnection(cn);
            con.Open();
            DataSet oDS = new DataSet();
            SqlDataAdapter odaData = new SqlDataAdapter(new SqlCommand(strQuery, con));
            odaData.Fill(oDS, "EMPLOYEE_EMPLOYMENT_HISTORY");
            return oDS;
        }
        catch (Exception ex)
        {
            return null;
        }
        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }
    }

    #endregion

    #region Certificates

    public string InsertCertificate(string bId, string eId, string cirtificateName, string cirtificateFilePath)
    {

        try
        {

            string strSql = "SELECT  EMP_ID, CMP_BRANCH_ID, CFT_NAME, CFT_FILE_LOCATION FROM HR_EMP_JOB_CERTIFICATE ";
            //+ "WHERE EMP_ID='" + eId + "' AND CMP_BRANCH_ID = '" + bId + "'";
            DataRow oOrderRow;

            con = new SqlConnection(cn);
            con.Open();
            dbTransaction = con.BeginTransaction();
            DataSet oDs = new DataSet();
            SqlDataAdapter oDbAdapter = new SqlDataAdapter(new SqlCommand(strSql, con, dbTransaction));
            SqlCommandBuilder oDbCmdBuilder = new SqlCommandBuilder(oDbAdapter);
            //oDbAdapter.MissingSchemaAction = MissingSchemaAction.AddWithKey;
            //oDbAdapter.Fill(oDs, "EMP_CERTIFICATE");
            oDbAdapter.FillSchema(oDs, SchemaType.Source, "EMP_CERTIFICATE");
            // Insert Data
            oOrderRow = oDs.Tables["EMP_CERTIFICATE"].NewRow();

            oOrderRow["CFT_NAME"] = cirtificateName;
            oOrderRow["CFT_FILE_LOCATION"] = cirtificateFilePath;
            oOrderRow["CMP_BRANCH_ID"] = bId;
            oOrderRow["EMP_ID"] = eId;

            oDs.Tables["EMP_CERTIFICATE"].Rows.Add(oOrderRow);
            oDbAdapter.Update(oDs, "EMP_CERTIFICATE");
            dbTransaction.Commit();
            //con.Close();
        }
        catch (Exception ex)
        {
            return "Err:" + ex.Message.ToString();
            //return null;
        }

        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }

        return "Success";
    }

    public DataSet GetEmpCertificate(string bId, string eId)
    {
        string strQuery = "SELECT * FROM HR_EMP_JOB_CERTIFICATE WHERE EMP_ID = '" + eId + "' AND CMP_BRANCH_ID='" + bId + "'";

        try
        {
            con = new SqlConnection(cn);
            con.Open();
            DataSet oDS = new DataSet();
            SqlDataAdapter odaData = new SqlDataAdapter(new SqlCommand(strQuery, con));
            odaData.Fill(oDS, "EMPLOYEE_JOB_CERTIFICATE");
            return oDS;
        }
        catch (Exception ex)
        {
            return null;
        }

        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }
    }

    #endregion

    #region References

    public string InserReference(string bId, string eId, string strName, string strOrganization, string strDesignation, string strAddress, string strPhoneOff, string strPhoneres, string strMobile, string strmail, string strRelation, string NomineeType, string Percentage, string AbsentInfo)
    {


        try
        {
            string strSql = "SELECT EMP_ID, CMP_BRANCH_ID, REF_NAME, REF_ORGANIZATION, REF_DESIGNATION, REF_ADDRESS, REF_PHONE_OFFICE, REF_PHONE_RES, REF_MOBILE, REF_EMAIL, REF_RELATION,REF_NOMINEE_TYPE,REF_PERCENTAGE,REF_NOMINEE_ABSENCE FROM HR_EMP_JOB_REFERENCE";
            DataRow oOrderRow;

            con = new SqlConnection(cn);
            con.Open();
            dbTransaction = con.BeginTransaction();
            DataSet oDs = new DataSet();
            SqlDataAdapter oDbAdapter = new SqlDataAdapter(new SqlCommand(strSql, con, dbTransaction));
            SqlCommandBuilder oDbCmdBuilder = new SqlCommandBuilder(oDbAdapter);

            oDbAdapter.FillSchema(oDs, SchemaType.Source, "HR_EMP_JOB_REFERENCE");
            // Insert the Data
            oOrderRow = oDs.Tables["HR_EMP_JOB_REFERENCE"].NewRow();


            oOrderRow["REF_NAME"] = strName;
            oOrderRow["REF_ORGANIZATION"] = strOrganization;
            oOrderRow["REF_DESIGNATION"] = strDesignation;
            oOrderRow["REF_ADDRESS"] = strAddress;
            oOrderRow["REF_PHONE_OFFICE"] = strPhoneOff;
            oOrderRow["REF_PHONE_RES"] = strPhoneres;
            oOrderRow["REF_MOBILE"] = strMobile;
            oOrderRow["REF_EMAIL"] = strmail;
            oOrderRow["REF_RELATION"] = strRelation;
            oOrderRow["REF_NOMINEE_TYPE"] = NomineeType;
            oOrderRow["REF_PERCENTAGE"] = Percentage;
            oOrderRow["EMP_ID"] = eId;
            oOrderRow["CMP_BRANCH_ID"] = bId;
            oOrderRow["REF_NOMINEE_ABSENCE"] = AbsentInfo;

            oDs.Tables["HR_EMP_JOB_REFERENCE"].Rows.Add(oOrderRow);
            oDbAdapter.Update(oDs, "HR_EMP_JOB_REFERENCE");
            dbTransaction.Commit();
            //con.Close();
        }
        catch (Exception ex)
        {

            return null;
        }

        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }

        return "Success";
    }
    public string InserReference(string bId, string eId, string strName, string strOrganization, string strDesignation, string strAddress, string strPhoneOff, string strPhoneres, string strMobile, string strmail, string strRelation, string NomineeType, string Percentage)
    {


        try
        {
            string strSql = "SELECT EMP_ID, CMP_BRANCH_ID, REF_NAME, REF_ORGANIZATION, REF_DESIGNATION, REF_ADDRESS, REF_PHONE_OFFICE, REF_PHONE_RES, REF_MOBILE, REF_EMAIL, REF_RELATION,REF_NOMINEE_TYPE,REF_PERCENTAGE FROM HR_EMP_JOB_REFERENCE";
            DataRow oOrderRow;

            con = new SqlConnection(cn);
            con.Open();
            dbTransaction = con.BeginTransaction();
            DataSet oDs = new DataSet();
            SqlDataAdapter oDbAdapter = new SqlDataAdapter(new SqlCommand(strSql, con, dbTransaction));
            SqlCommandBuilder oDbCmdBuilder = new SqlCommandBuilder(oDbAdapter);

            oDbAdapter.FillSchema(oDs, SchemaType.Source, "HR_EMP_JOB_REFERENCE");
            // Insert the Data
            oOrderRow = oDs.Tables["HR_EMP_JOB_REFERENCE"].NewRow();


            oOrderRow["REF_NAME"] = strName;
            oOrderRow["REF_ORGANIZATION"] = strOrganization;
            oOrderRow["REF_DESIGNATION"] = strDesignation;
            oOrderRow["REF_ADDRESS"] = strAddress;
            oOrderRow["REF_PHONE_OFFICE"] = strPhoneOff;
            oOrderRow["REF_PHONE_RES"] = strPhoneres;
            oOrderRow["REF_MOBILE"] = strMobile;
            oOrderRow["REF_EMAIL"] = strmail;
            oOrderRow["REF_RELATION"] = strRelation;
            oOrderRow["REF_NOMINEE_TYPE"] = NomineeType;
            oOrderRow["REF_PERCENTAGE"] = Percentage;
            oOrderRow["EMP_ID"] = eId;
            oOrderRow["CMP_BRANCH_ID"] = bId;

            oDs.Tables["HR_EMP_JOB_REFERENCE"].Rows.Add(oOrderRow);
            oDbAdapter.Update(oDs, "HR_EMP_JOB_REFERENCE");
            dbTransaction.Commit();
            //con.Close();
        }
        catch (Exception ex)
        {

            return null;
        }

        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }

        return "Success";
    }
    public DataSet GetEmpReference(string bId, string eId)
    {
        string strQuery = "SELECT * FROM HR_EMP_JOB_REFERENCE WHERE EMP_ID = '" + eId + "' AND CMP_BRANCH_ID='" + bId + "'";

        try
        {
            con = new SqlConnection(cn);
            con.Open();
            DataSet oDS = new DataSet();
            SqlDataAdapter odaData = new SqlDataAdapter(new SqlCommand(strQuery, con));
            odaData.Fill(oDS, "EMPLOYEE_REFERENCE");
            return oDS;
        }
        catch (Exception ex)
        {
            return null;
        }

        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }
    }
    public DataSet GetEmpReferenceAsNomineeType(string bId, string eId, string NomineeType) /* asad */
    {
        string strQuery = "SELECT * FROM HR_EMP_JOB_REFERENCE WHERE EMP_ID = '" + eId + "' AND CMP_BRANCH_ID='" + bId + "' AND REF_NOMINEE_TYPE='" + NomineeType + "' ";

        try
        {
            con = new SqlConnection(cn);
            con.Open();
            DataSet oDS = new DataSet();
            SqlDataAdapter odaData = new SqlDataAdapter(new SqlCommand(strQuery, con));
            odaData.Fill(oDS, "EMPLOYEE_REFERENCE");
            return oDS;
        }
        catch (Exception ex)
        {
            return null;
        }
        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }
    }
    #endregion

    #region EmployeeInfo

    public DataSet GetEmpAllInfo(string bId, string eId)
    {
        string strQuery = "SELECT * FROM PR_EMPLOYEE_LIST WHERE EMP_ID = '" + eId + "' AND CMP_BRANCH_ID='" + bId + "'";

        try
        {
            con = new SqlConnection(cn);
            con.Open();
            DataSet oDS = new DataSet();
            SqlDataAdapter odaData = new SqlDataAdapter(new SqlCommand(strQuery, con));
            odaData.Fill(oDS, "EMPLOYEE_ALL_INFO");
            return oDS;
        }
        catch (Exception ex)
        {
            return null;
        }

        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }
    }

    #endregion



    //public DataSet GetEmpLis(string d1, string d2, string d3)
    //{
    //    string strQuery = "SELECT HS.PRH_ID, HS.PRH_TYPE, YR.YR_YEAR, HS.FY_YEAR, HS.PRH_CAPTION, HS.PRH_DATE_DURATION, HS.PRH_DETAILS, "
    //                        + "HS.PRH_FROM_DATE, HS.PRH_TO_DATE, EL.EMP_NAME, CB.CMP_BRANCH_ID "
    //                        + "FROM PR_HOLIDAY_SETUP HS, CM_CMP_BRANCH CB, HR_YEAR YR, PR_EMPLOYEE_LIST EL  "
    //                        + "WHERE HS.CMP_BRANCH_ID=CB.CMP_BRANCH_ID AND HS.YR_ID=YR.YR_ID AND HS.EMP_ID=EL.EMP_ID "
    //                        + "where CMP_BRANCH_ID='" + bId + "' ORDER BY NLSSORT(TRIM(DPT_NAME), 'NLS_SORT=generic_m')";

    //    try
    //    {
    //        SqlConnection
    //        DataSet oDS = new DataSet();
    //        SqlDataAdapter odaData = new SqlDataAdapter(new SqlCommand(strQuery, con));
    //        odaData.Fill(oDS, "GET_DEPARTMENT");
    //        return oDS;
    //    }
    //    catch (Exception ex)
    //    {
    //        return null;
    //    }
    //}
    //public DataSet ClintList(string bId)
    //{
    //    string strQuery = "SELECT EL.EMP_ID, EL.EMP_CODE, (EMP_TITLE || ' ' || EMP_NAME) ENAME, EL.EMP_PRE_ADDRES, EL.EMP_PER_ADDRESS, "
    //                     + "EL.EMP_CONTACT_NUM, EL.EMP_JOINING_DATE, EL.EMP_RETIRE_DATE, EL.EMP_TIN_NO, EL.EMP_GENDER,EL.LVE_APPROVAL_PERM, "
    //                     + "EL.EMP_QUANTITY, EL.EMP_INDIVIDUAL, EL.EMP_FATHER_NAME, EL.EMP_MOTHER_NAME, EL.EMP_NATIONAL_ID, EL.EMP_PASPORT_ID, "
    //                     + "EL.EMP_BIRTHDAY, EL.EMP_MARITAL_STATAS, EL.EMP_NATIONALITY, EL.EMP_RELIGION, EL.EMP_CONFIR_DATE, CB.CMP_BRANCH_NAME, "
    //                     + "SET_CODE || ' - ' || DSG_TITLE AS DNAME, EH.PREH_ASSIGNING_DATE, EH.PREH_LAST_PROMOTION_DATE, DP.DPT_NAME, EL.EMP_STATUS, "
    //                     + "EMP_BANK_ACC_NO, EMP_PROVISION_PERIOD, EMP_FINAL_CONFIR_DATE "
    //                     + "FROM PR_EMPLOYEE_LIST EL, CM_CMP_BRANCH CB, PR_DEPARTMENT DP, PR_DESIGNATION DE, PR_EMPLOYEE_HISTORY EH  "
    //                     + "WHERE EL.CMP_BRANCH_ID=CB.CMP_BRANCH_ID AND EL.DPT_ID=DP.DPT_ID AND EL.DSG_ID=DE.DSG_ID "
    //                     + "AND EL.EMP_ID=EH.EMP_ID(+) AND CB.CMP_BRANCH_ID='" + bId + "' "
    //                     + "GROUP BY EL.EMP_ID, EMP_CODE, EMP_TITLE, EMP_NAME, EMP_PRE_ADDRES, EMP_PER_ADDRESS, EMP_CONTACT_NUM, EMP_JOINING_DATE, "
    //                     + "EMP_RETIRE_DATE, EMP_TIN_NO, EMP_GENDER, LVE_APPROVAL_PERM, EMP_QUANTITY, EMP_INDIVIDUAL, EMP_FATHER_NAME, SET_CODE, DSG_TITLE, "
    //                     + "EMP_MOTHER_NAME, EMP_NATIONAL_ID, EMP_PASPORT_ID, EMP_BIRTHDAY, EMP_MARITAL_STATAS, EMP_NATIONALITY, EMP_RELIGION, "
    //                     + "EMP_CONFIR_DATE, CMP_BRANCH_NAME, EH.PREH_ASSIGNING_DATE, EH.PREH_LAST_PROMOTION_DATE, DP.DPT_NAME, EL.EMP_STATUS, SET_LEVEL  "
    //                     + "ORDER BY to_number(SET_LEVEL), DE.SET_CODE ASC";

    //    try
    //    {
    //        SqlConnection
    //        DataSet oDS = new DataSet();
    //        SqlDataAdapter odaData = new SqlDataAdapter(new SqlCommand(strQuery, con));
    //        odaData.Fill(oDS, "PR_EMPLOYEE_LIST");
    //        return oDS;
    //    }
    //    catch (Exception ex)
    //    {
    //        return null;
    //    }
    //}

    //public DataSet ClintList(string bId)
    //{
    //    string strQuery = "SELECT EL.EMP_ID, EL.EMP_CODE, (EMP_TITLE || ' ' || EMP_NAME) ENAME, EL.EMP_PRE_ADDRES, EL.EMP_PER_ADDRESS, "
    //                    + "EL.EMP_CONTACT_NUM, EL.EMP_JOINING_DATE, EL.EMP_RETIRE_DATE, EL.EMP_TIN_NO, EL.EMP_GENDER,EL.LVE_APPROVAL_PERM, "
    //                    + "EL.EMP_QUANTITY, EL.EMP_INDIVIDUAL, EL.EMP_FATHER_NAME, EL.EMP_MOTHER_NAME, EL.EMP_NATIONAL_ID, EL.EMP_PASPORT_ID, "
    //                    + "EL.EMP_BIRTHDAY, EL.EMP_MARITAL_STATAS, EL.EMP_NATIONALITY, EL.EMP_RELIGION, EL.EMP_CONFIR_DATE, "
    //                    + "EL.EMP_BANK_ACC_NO, EL.EMP_PROVISION_PERIOD, EL.EMP_FINAL_CONFIR_DATE, EHIS.PREH_ASSIGNING_DATE, "
    //                    + "CB.CMP_BRANCH_NAME, DP.DPT_NAME, SET_CODE || ' - ' || DSG_TITLE AS DNAME, ET.TYP_TYPE, EHIS.PREH_ASSIGNING_DATE "
    //                    + "FROM PR_EMPLOYEE_LIST EL, CM_CMP_BRANCH CB, PR_DEPARTMENT DP, PR_DESIGNATION DE, HR_EMP_TYPE ET, "
    //                    + "(SELECT PREH_ASSIGNING_DATE, EMP_ID FROM (SELECT PREH_ASSIGNING_DATE, EI.EMP_ID "
    //                    + "FROM PR_EMPLOYEE_HISTORY EI, PR_EMPLOYEE_LIST EM WHERE EI.CMP_BRANCH_ID=EM.CMP_BRANCH_ID "
    //                    + "AND EM.EMP_ID=EI.EMP_ID AND  PREH_AMOUNT_TYPE='P' AND EI.CMP_BRANCH_ID='" + bId + "' "
    //                    + "ORDER BY  PREH_ASSIGNING_DATE DESC) where rownum<2 ) EHIS "
    //                    + "WHERE EL.CMP_BRANCH_ID=CB.CMP_BRANCH_ID AND EL.DPT_ID=DP.DPT_ID AND EL.DSG_ID=DE.DSG_ID AND EL.EMP_STATUS=ET.TYP_CODE "
    //                    + "AND EL.EMP_ID=EHIS.EMP_ID(+) AND CB.CMP_BRANCH_ID='" + bId + "' "
    //                    + "GROUP BY EL.EMP_ID, EMP_CODE, EMP_TITLE, EMP_NAME, EMP_PRE_ADDRES, EMP_PER_ADDRESS, EMP_CONTACT_NUM, "
    //                    + "EMP_JOINING_DATE, EMP_RETIRE_DATE, EMP_TIN_NO, EMP_GENDER, LVE_APPROVAL_PERM, EMP_QUANTITY, EMP_INDIVIDUAL, EMP_FATHER_NAME, SET_CODE, "
    //                    + "DSG_TITLE, EMP_MOTHER_NAME, EMP_NATIONAL_ID, EMP_PASPORT_ID, EMP_BIRTHDAY, EMP_MARITAL_STATAS, EMP_NATIONALITY, EMP_RELIGION, "
    //                    + "EMP_BANK_ACC_NO, EMP_PROVISION_PERIOD, EMP_FINAL_CONFIR_DATE, EMP_CONFIR_DATE, CMP_BRANCH_NAME, "
    //                    + "EHIS.PREH_ASSIGNING_DATE, DP.DPT_NAME, EL.EMP_STATUS, SET_LEVEL, ET.TYP_TYPE ORDER BY to_number(SET_LEVEL), DE.SET_CODE ASC ";

    //    try
    //    {
    //        con = new SqlConnection(cn);
    //        con.Open();
    //        DataSet oDS = new DataSet();
    //        SqlDataAdapter odaData = new SqlDataAdapter(new SqlCommand(strQuery, con));
    //        odaData.Fill(oDS, "PR_EMPLOYEE_LIST");
    //        return oDS;
    //    }
    //    catch (Exception ex)
    //    {
    //        return null;
    //    }

    //    finally
    //    {
    //        con.Close();
    //        con = null;
    //    }
    //}

    //ASAD


    public DataSet ClintList(string bId)
    {

        string strQuery = "SELECT EL.EMP_ID,EL.EMP_CODE,(CAST(EMP_TITLE AS VARCHAR) + ' ' + CAST(EMP_NAME AS VARCHAR)) ENAME,EL.EMP_PRE_ADDRES,EL.EMP_PER_ADDRESS,EL.EMP_CONTACT_NUM,EL.EMP_JOINING_DATE,EL.EMP_RETIRE_DATE,EL.EMP_TIN_NO,EL.EMP_GENDER,EL.LVE_APPROVAL_PERM,EL.EMP_QUANTITY,EL.EMP_INDIVIDUAL,EL.EMP_FATHER_NAME,EL.EMP_MOTHER_NAME,EL.EMP_NATIONAL_ID,EL.EMP_PASPORT_ID,EL.EMP_BIRTHDAY,EL.EMP_MARITAL_STATAS,EL.EMP_NATIONALITY,EL.EMP_RELIGION,EL.EMP_CONFIR_DATE,EL.EMP_BANK_ACC_NO,EL.EMP_PROVISION_PERIOD,EL.EMP_FINAL_CONFIR_DATE,EHIS.PREH_ASSIGNING_DATE,CB.CMP_BRANCH_NAME,DP.DPT_NAME,CAST(SET_CODE AS VARCHAR) + ' - ' + CAST(DSG_TITLE AS VARCHAR) AS DNAME,ET.TYP_TYPE,EHIS.PREH_ASSIGNING_DATE FROM  PR_EMPLOYEE_LIST EL  LEFT OUTER JOIN (SELECT PREH_ASSIGNING_DATE,EI.EMP_ID FROM  PR_EMPLOYEE_HISTORY EI,PR_EMPLOYEE_LIST EM WHERE EI.CMP_BRANCH_ID  = EM.CMP_BRANCH_ID AND	EM.EMP_ID  = EI.EMP_ID AND	PREH_AMOUNT_TYPE  = 'P' AND	EI.CMP_BRANCH_ID  = '" + bId + "') EHIS  ON  EL.EMP_ID  = EHIS.EMP_ID ,CM_CMP_BRANCH CB,PR_DEPARTMENT DP,PR_DESIGNATION DE,HR_EMP_TYPE ET WHERE	 EL.CMP_BRANCH_ID  = CB.CMP_BRANCH_ID AND	EL.DPT_ID  = DP.DPT_ID AND	EL.DSG_ID  = DE.DSG_ID AND	EL.EMP_STATUS  = ET.TYP_CODE AND	CB.CMP_BRANCH_ID  = '" + bId + "' GROUP BY   EL.EMP_ID,EMP_CODE,EMP_TITLE,EMP_NAME,EMP_PRE_ADDRES,EMP_PER_ADDRESS,EMP_CONTACT_NUM,EMP_JOINING_DATE,EMP_RETIRE_DATE,EMP_TIN_NO,EMP_GENDER,LVE_APPROVAL_PERM,EMP_QUANTITY,EMP_INDIVIDUAL,EMP_FATHER_NAME,SET_CODE,DSG_TITLE,EMP_MOTHER_NAME,EMP_NATIONAL_ID,EMP_PASPORT_ID,EMP_BIRTHDAY,EMP_MARITAL_STATAS,EMP_NATIONALITY,EMP_RELIGION,EMP_BANK_ACC_NO,EMP_PROVISION_PERIOD,EMP_FINAL_CONFIR_DATE,EMP_CONFIR_DATE,CMP_BRANCH_NAME,EHIS.PREH_ASSIGNING_DATE,DP.DPT_NAME,EL.EMP_STATUS,SET_LEVEL,ET.TYP_TYPE ORDER BY   CONVERT(NUMERIC(8, 2), SET_LEVEL), DE.SET_CODE ASC";
        try
        {
            con = new SqlConnection(cn);
            con.Open();
            DataSet oDS = new DataSet();
            SqlDataAdapter odaData = new SqlDataAdapter(new SqlCommand(strQuery, con));
            odaData.Fill(oDS, "PR_EMPLOYEE_LIST");
            return oDS;
        }
        catch (Exception ex)
        {
            return null;
        }
        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }
    }







    public string InsertPayrollStructure(
                                        string bId,
                                        string gId,
                                        string eTypeId,
                                        string parentId,
                                        string itemId,
                                        string percentage,
                                        string amount,
                                        string gender)
    {

        try
        {
            string strSql = "SELECT CMP_BRANCH_ID, DSG_ID, GENDER, PR_ITEM_ID, PRST_AMT, PARENT_ITEM_ID, PAMT_PERCENTAGE, TYP_CODE FROM PR_STRUCTURE";
            DataRow oOrderRow;
            //DataRow oOrderRow1;

            con = new SqlConnection(cn);
            con.Open();
            dbTransaction = con.BeginTransaction();
            DataSet oDs = new DataSet();
            SqlDataAdapter oDbAdapter = new SqlDataAdapter(new SqlCommand(strSql, con, dbTransaction));
            SqlCommandBuilder oDbCmdBuilder = new SqlCommandBuilder(oDbAdapter);
            //oDbAdapter.MissingSchemaAction = MissingSchemaAction.AddWithKey;
            //oDbAdapter.Fill(oDs, "PR_STRUCTURE");
            oDbAdapter.FillSchema(oDs, SchemaType.Source, "PR_STRUCTURE");

            // Insert the Data
            oOrderRow = oDs.Tables["PR_STRUCTURE"].NewRow();

            oOrderRow["CMP_BRANCH_ID"] = bId;
            oOrderRow["DSG_ID"] = gId;
            oOrderRow["GENDER"] = gender;
            oOrderRow["PR_ITEM_ID"] = itemId;
            oOrderRow["PRST_AMT"] = Convert.ToDouble(amount);
            oOrderRow["PARENT_ITEM_ID"] = parentId;
            oOrderRow["PAMT_PERCENTAGE"] = Convert.ToDouble(percentage);
            oOrderRow["TYP_CODE"] = eTypeId;

            //oOrderRow1 = oDs.Tables["PR_STRUCTURE"].NewRow();
            //oOrderRow1 = oOrderRow;
            oDs.Tables["PR_STRUCTURE"].Rows.Add(oOrderRow);

            //oOrderRow1["GENDER"] = "F";
            //oDs.Tables["PR_STRUCTURE"].Rows.Add(oOrderRow1);

            oDbAdapter.Update(oDs, "PR_STRUCTURE");
            dbTransaction.Commit();
            //con.Close();
        }
        catch (Exception ex)
        {

            return null;
        }

        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }

        return "Success";
    }


    public string InsertPayrollStructure(
                                            string bId,
                                            string gId,
                                            string dptId,
                                            string eTypeId,
                                            string parentId,
                                            string itemId,
                                            string percentage,
                                            string amount,
                                            string gender)
    {


        try
        {
            string strSql = "SELECT CMP_BRANCH_ID, DSG_ID, DPT_ID, GENDER, PR_ITEM_ID, PRST_AMT, PARENT_ITEM_ID, PAMT_PERCENTAGE, TYP_CODE FROM PR_STRUCTURE";
            DataRow oOrderRow;
            //DataRow oOrderRow1;

            con = new SqlConnection(cn);
            con.Open();
            dbTransaction = con.BeginTransaction();
            DataSet oDs = new DataSet();
            SqlDataAdapter oDbAdapter = new SqlDataAdapter(new SqlCommand(strSql, con, dbTransaction));
            SqlCommandBuilder oDbCmdBuilder = new SqlCommandBuilder(oDbAdapter);
            //oDbAdapter.MissingSchemaAction = MissingSchemaAction.AddWithKey;
            //oDbAdapter.Fill(oDs, "PR_STRUCTURE");
            oDbAdapter.FillSchema(oDs, SchemaType.Source, "PR_STRUCTURE");
            // Insert the Data
            oOrderRow = oDs.Tables["PR_STRUCTURE"].NewRow();

            oOrderRow["CMP_BRANCH_ID"] = bId;
            oOrderRow["DSG_ID"] = gId;
            oOrderRow["DPT_ID"] = dptId;
            oOrderRow["GENDER"] = gender;
            oOrderRow["PR_ITEM_ID"] = itemId;
            oOrderRow["PRST_AMT"] = Convert.ToDouble(amount);
            oOrderRow["PARENT_ITEM_ID"] = parentId;
            oOrderRow["PAMT_PERCENTAGE"] = Convert.ToDouble(percentage);
            oOrderRow["TYP_CODE"] = eTypeId;

            //oOrderRow1 = oDs.Tables["PR_STRUCTURE"].NewRow();
            //oOrderRow1 = oOrderRow;
            oDs.Tables["PR_STRUCTURE"].Rows.Add(oOrderRow);

            //oOrderRow1["GENDER"] = "F";
            //oDs.Tables["PR_STRUCTURE"].Rows.Add(oOrderRow1);

            oDbAdapter.Update(oDs, "PR_STRUCTURE");
            dbTransaction.Commit();
            //con.Close();
        }
        catch (Exception ex)
        {

            return null;
        }
        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }

        return "Success";
    }

    #region Payroll Voucher


    public DataSet GetPayrollVoucherInfo(string bId, string pmId) // get monthly payroll
    {
        string strQuery = "SELECT PM.PRM_DETAILS, PI.PR_ITEM_TITLE,COA.ACC_NAME,COA.ACC_ID, SUM(PD.PRST_AMT) TOTAL_AMOUNT, "
                            + "PD.PRM_ID, PD.CMP_BRANCH_ID "
                            + "FROM PR_PAYROLL_ITEM PI,GL_CHART_OF_ACC COA,PR_PAYROLL_DETAIL PD, "
                            + "PR_PAYROLL_MASTER PM WHERE PI.ACC_INT_ID=COA.ACC_INT_ID AND PI.PR_ITEM_ID=PD.PR_ITEM_ID "
                            + "AND PD.PRM_ID=PM.PRM_ID "
                            + "AND PD.PRM_ID='" + pmId + "' AND PD.CMP_BRANCH_ID='" + bId + "' "
                            + "GROUP BY PM.PRM_DETAILS, PI.PR_ITEM_TITLE,COA.ACC_NAME,COA.ACC_ID, "
                            + "PD.PRM_ID, PD.CMP_BRANCH_ID";

        try
        {
            con = new SqlConnection(cn);
            con.Open();
            DataSet oDS = new DataSet();
            SqlDataAdapter odaData = new SqlDataAdapter(new SqlCommand(strQuery, con));
            odaData.Fill(oDS, "MONTHLY_VOUCHER_INFO");
            return oDS;
        }
        catch (Exception ex)
        {
            return null;
        }
        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }
    }
    #endregion

    public string UpdateEmpDesignation(string eId, string gId, string empSt)
    {
        string updateString;

        updateString = "UPDATE PR_EMPLOYEE_LIST SET DSG_ID = '" + gId + "', EMP_STATUS = '" + empSt + "' WHERE EMP_ID = '" + eId + "'";



        string strReturn = "";
        try
        {
            con = new SqlConnection(cn);
            con.Open();
            dbTransaction = con.BeginTransaction();
            SqlCommand cmd = new SqlCommand(updateString, con, dbTransaction);
            cmd.Connection = con;
            cmd.ExecuteNonQuery();
            dbTransaction.Commit();
            con.Close();
        }
        catch (Exception ex)
        {
            strReturn = ex.Message.ToString();
        }
        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }
        return strReturn;
    }


    public string UpdateEmpInfo(string empid, string txtFth, string txtMth, string txtNation, string txtRelign, string txtEmail1,
                                string txtBirthdayActual, string txtBirthdayCer,
                                string BlooGrp, string MaritalStatus, string txtMarriage1, string txtSpouse1, string txtSpouseDB1
                                )
    {
        string strReturn = "";
        string updateString = "";

        if (txtBirthdayActual != "" && txtBirthdayCer != "" && txtSpouseDB1 != "" && txtMarriage1 != "")
        {
            updateString = "UPDATE PR_EMPLOYEE_LIST SET EMP_FATHER_NAME = '" + txtFth + "', EMP_MOTHER_NAME = '" + txtMth + "',"
                         + " EMP_NATIONALITY= '" + txtNation + "' ,EMP_RELIGION= '" + txtRelign + "',EMP_EMAIL_PERSONAL= '" + txtEmail1 + "',ACTUAL_BIRTHDAY= '" + txtBirthdayActual + "',"
                         + " EMP_BIRTHDAY= '" + txtBirthdayCer + "', EMP_BLOODGRP= '" + BlooGrp + "',EMP_MARITAL_STATAS= '" + MaritalStatus + "', MARRIAGE_DATE= '" + txtMarriage1 + "', "
                         + " EMP_SPOUSE_NAME= '" + txtSpouse1 + "',EMP_SPOUSE_DOB= '" + txtSpouseDB1 + "' "
                         + " WHERE EMP_ID = '" + empid + "' ";
            strReturn = ExecuteCommand(strReturn, updateString);
        }

        if (txtBirthdayActual != "")
        {
            updateString = "UPDATE PR_EMPLOYEE_LIST SET EMP_FATHER_NAME = '" + txtFth + "', EMP_MOTHER_NAME = '" + txtMth + "',"
                        + " EMP_NATIONALITY= '" + txtNation + "' ,EMP_RELIGION= '" + txtRelign + "',EMP_EMAIL_PERSONAL= '" + txtEmail1 + "',ACTUAL_BIRTHDAY= '" + txtBirthdayActual + "',"
                        + " EMP_BLOODGRP= '" + BlooGrp + "',EMP_MARITAL_STATAS= '" + MaritalStatus + "', "
                        + " EMP_SPOUSE_NAME= '" + txtSpouse1 + "' "
                        + " WHERE EMP_ID = '" + empid + "' ";
            strReturn = ExecuteCommand(strReturn, updateString);
        }

        if (txtBirthdayCer != "")
        {
            updateString = "UPDATE PR_EMPLOYEE_LIST SET EMP_FATHER_NAME = '" + txtFth + "', EMP_MOTHER_NAME = '" + txtMth + "',"
                        + " EMP_NATIONALITY= '" + txtNation + "' ,EMP_RELIGION= '" + txtRelign + "',EMP_EMAIL_PERSONAL= '" + txtEmail1 + "',"
                        + " EMP_BIRTHDAY= '" + txtBirthdayCer + "', EMP_BLOODGRP= '" + BlooGrp + "',EMP_MARITAL_STATAS= '" + MaritalStatus + "', "
                        + " EMP_SPOUSE_NAME= '" + txtSpouse1 + "' "
                        + " WHERE EMP_ID = '" + empid + "' ";
            strReturn = ExecuteCommand(strReturn, updateString);
        }

        if (txtSpouseDB1 != "")
        {
            updateString = "UPDATE PR_EMPLOYEE_LIST SET EMP_FATHER_NAME = '" + txtFth + "', EMP_MOTHER_NAME = '" + txtMth + "',"
                        + " EMP_NATIONALITY= '" + txtNation + "' ,EMP_RELIGION= '" + txtRelign + "',EMP_EMAIL_PERSONAL= '" + txtEmail1 + "',"
                        + " EMP_BLOODGRP= '" + BlooGrp + "',EMP_MARITAL_STATAS= '" + MaritalStatus + "',  "
                        + " EMP_SPOUSE_NAME= '" + txtSpouse1 + "',EMP_SPOUSE_DOB= '" + txtSpouseDB1 + "' "
                        + " WHERE EMP_ID = '" + empid + "' ";
            strReturn = ExecuteCommand(strReturn, updateString);
        }


        if (txtMarriage1 != "")
        {
            updateString = "UPDATE PR_EMPLOYEE_LIST SET EMP_FATHER_NAME = '" + txtFth + "', EMP_MOTHER_NAME = '" + txtMth + "',"
                       + " EMP_NATIONALITY= '" + txtNation + "' ,EMP_RELIGION= '" + txtRelign + "',EMP_EMAIL_PERSONAL= '" + txtEmail1 + "',"
                       + " EMP_BLOODGRP= '" + BlooGrp + "',EMP_MARITAL_STATAS= '" + MaritalStatus + "', MARRIAGE_DATE= '" + txtMarriage1 + "', "
                       + " EMP_SPOUSE_NAME= '" + txtSpouse1 + "' "
                       + " WHERE EMP_ID = '" + empid + "' ";
            strReturn = ExecuteCommand(strReturn, updateString);

        }
        strReturn = ExecuteCommand(strReturn, updateString);
        return strReturn;
    }

    private string ExecuteCommand(string strReturn, string updateString)
    {
        try
        {
            con = new SqlConnection(cn);
            con.Open();
            dbTransaction = con.BeginTransaction();
            SqlCommand cmd = new SqlCommand(updateString, con, dbTransaction);
            cmd.Connection = con;
            cmd.ExecuteNonQuery();
            dbTransaction.Commit();
            con.Close();
        }
        catch (Exception ex)
        {
            strReturn = ex.Message.ToString();
        }
        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }
        return strReturn;
    }


    #region OLD FUNCTIONS

    //public DataSet GetParentDesignationOLD(string strType, string strBranchID, string strDptID, string strLevel, string strParentCode)
    //{
    //    string strSql = "";
    //    string strSql1 = "";

    //    if (!strBranchID.Equals("-1"))
    //    {
    //        strSql = " AND C.CMP_BRANCH_ID ='" + strBranchID + "'";
    //    }

    //    if (!strDptID.Equals("-1"))
    //    {
    //        strSql1 = " AND D.DPT_ID ='" + strDptID + "'";
    //    }

    //    string strQuery = "SELECT DE.CMP_BRANCH_ID, DE.DPT_ID, DSG_ID, SET_TYPE, SET_CODE, PARENT_CODE, SET_CODE || ' - ' || DSG_TITLE AS DNAME "
    //                    + "FROM PR_DESIGNATION DE, CM_CMP_BRANCH C, PR_DEPARTMENT D "
    //                    + "WHERE DE.CMP_BRANCH_ID = C.CMP_BRANCH_ID AND DE.DPT_ID = D.DPT_ID AND DE.SET_TYPE='" + strType + "' AND DE.SET_LEVEL='" + strLevel + "' "
    //                    + "AND DE.PARENT_CODE='" + strParentCode + "' " + strSql + " " + strSql1 + " "
    //                    + "ORDER BY DE.CMP_BRANCH_ID, SET_CODE";

    //    try
    //    {
    //        SqlConnection
    //        DataSet oDS = new DataSet();
    //        SqlDataAdapter odaData = new SqlDataAdapter(new SqlCommand(strQuery, con));
    //        odaData.Fill(oDS, "PR_DESIGNATION");
    //        return oDS;
    //    }
    //    catch (Exception ex)
    //    {
    //        return null;
    //    }
    //}
    //public DataSet GetChildGradeOLD(string strType, string strBranchID, string strDptID, string strPID)
    //{
    //    string strQuery = "SELECT DE.CMP_BRANCH_ID, DE.DPT_ID, DSG_ID, SET_TYPE, SET_CODE, PARENT_CODE, SET_CODE || ' - ' || DSG_TITLE AS DNAME "
    //                    + "FROM PR_DESIGNATION DE, CM_CMP_BRANCH C, PR_DEPARTMENT D "
    //                    + "WHERE DE.CMP_BRANCH_ID = C.CMP_BRANCH_ID AND DE.SET_TYPE='" + strType + "' "
    //                    + "AND DE.PARENT_CODE='" + strPID + "' AND C.CMP_BRANCH_ID ='" + strBranchID + "' AND D.DPT_ID ='" + strDptID + "' "
    //                    + "ORDER BY DE.CMP_BRANCH_ID, SET_CODE";

    //    try
    //    {
    //        SqlConnection
    //        DataSet oDS = new DataSet();
    //        SqlDataAdapter odaData = new SqlDataAdapter(new SqlCommand(strQuery, con));
    //        odaData.Fill(oDS, "PR_CHILD_GRADE");
    //        return oDS;
    //    }
    //    catch (Exception ex)
    //    {
    //        return null;
    //    }
    //}
    //public DataSet GetTotalTaxIncomeOLD(string bId, string gId, string strGender)
    //{
    //    string strQuery = "SELECT TOT_TAX, TAX_PER_MONTH, TOT_INCOME FROM PR_TAX_CALCULATION "
    //                        + "WHERE CMP_BRANCH_ID='" + bId + "' AND DSG_ID='" + gId + "' AND EMP_GENDER='" + strGender + "'";
    //    try
    //    {
    //        SqlConnection
    //        DataSet oDS = new DataSet();
    //        SqlDataAdapter odaData = new SqlDataAdapter(new SqlCommand(strQuery, con));
    //        odaData.Fill(oDS, "PR_TAX_CALCULATION");
    //        return oDS;
    //    }
    //    catch (Exception ex)
    //    {
    //        return null;
    //    }
    //}
    //public string SavePayrollSheetOLD(string bId, string empId, string gId, string totTax, string taxPerMonth, string totIncome, bool isGlPost, string prMonth) // Payroll Sheet
    //{
    //    string strSql;

    //    strSql = "SELECT TOT_TAX, TAX_PER_MONTH, TOT_INCOME, NET_AMOUNT, EMP_ID, DSG_ID, IS_GL_POST, PRM_ID, CMP_BRANCH_ID FROM PR_SHEET";


    //    try
    //    {
    //        // Payroll Sheet
    //        DataRow oOrderRow;
    //        SqlConnection
    //        con.Open();
    //        dbTransaction = con.BeginTransaction();
    //        DataSet oDS = new DataSet();
    //        SqlDataAdapter oOrdersDataAdapter = new SqlDataAdapter(new SqlCommand(strSql, con,dbTransaction));
    //        SqlCommandBuilder oOrdersCmdBuilder = new SqlCommandBuilder(oOrdersDataAdapter);
    //        oOrdersDataAdapter.MissingSchemaAction = MissingSchemaAction.AddWithKey;
    //        oOrdersDataAdapter.Fill(oDS, "PR_SHEET");

    //        oOrderRow = oDS.Tables["PR_SHEET"].NewRow();

    //        // 9 fields
    //        oOrderRow["PRM_ID"] = prMonth;
    //        oOrderRow["CMP_BRANCH_ID"] = bId;
    //        oOrderRow["EMP_ID"] = empId;
    //        oOrderRow["DSG_ID"] = gId;
    //        oOrderRow["TOT_TAX"] = totTax;
    //        oOrderRow["TAX_PER_MONTH"] = taxPerMonth;

    //        if (isGlPost)
    //        {
    //            oOrderRow["IS_GL_POST"] = "Y";
    //        }
    //        else
    //        {
    //            oOrderRow["IS_GL_POST"] = "N";
    //        }

    //        oOrderRow["TOT_INCOME"] = totIncome;
    //        oOrderRow["NET_AMOUNT"] = "0";

    //        oDS.Tables["PR_SHEET"].Rows.Add(oOrderRow);
    //        oOrdersDataAdapter.Update(oDS, "PR_SHEET");
    //        dbTransaction.Commit();
    //        con.Close();
    //        return "Saved successfully";

    //    }
    //    catch (Exception ex)
    //    {
    //        return ex.Message.ToString();
    //    }

    //    //return "Saved successfully";
    //}

    #endregion

    #region Mail

    public DataSet GetSMTPInfoNew( string branchId)
    {
        var con = new SqlConnection(cn);
        try
        {

            con.Open();
            string strSql = "SELECT * FROM SYSTEM_SMTP where CMP_BRANCH_ID='" + branchId + "'";
            DataSet oDS = new DataSet();
            SqlDataAdapter oOrdersDataAdapter = new SqlDataAdapter(new SqlCommand(strSql, con));
            SqlCommandBuilder oOrdersCmdBuilder = new SqlCommandBuilder(oOrdersDataAdapter);
            oOrdersDataAdapter.Fill(oDS, "SMTP_INFO");
            return oDS;
        }
        catch (Exception ex)
        {
            return null;
        }

        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }
    }

    public DataSet GetSMTPInfo()
    {
        var con = new SqlConnection(cn);
        try
        {

            con.Open();
            string strSql = "SELECT * FROM SYSTEM_SMTP ";
            DataSet oDS = new DataSet();
            SqlDataAdapter oOrdersDataAdapter = new SqlDataAdapter(new SqlCommand(strSql, con));
            SqlCommandBuilder oOrdersCmdBuilder = new SqlCommandBuilder(oOrdersDataAdapter);
            oOrdersDataAdapter.Fill(oDS, "SMTP_INFO");
            return oDS;
        }
        catch (Exception ex)
        {
            return null;
        }

        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }
    }

    public DataSet GetEmpDetailInfo(string bId, string eId)
    {
        string strQuery = "SELECT EL.EMP_ID, (CAST(isnull(EMP_TITLE,'') AS VARCHAR(200))  + ' ' + CAST(EMP_NAME AS VARCHAR(500))) ENAME, CB.CMP_BRANCH_NAME, "
                            + "EL.EMP_CODE,EL.DSG_ID,  CAST(DSG_TITLE AS VARCHAR) AS DNAME, DP.DPT_NAME, EL.EMP_EMAIL, EL.EMP_REPORTING_ID "
                            + "FROM PR_EMPLOYEE_LIST EL, CM_CMP_BRANCH CB, PR_DESIGNATION DE, PR_DEPARTMENT DP "
                            + "WHERE EL.CMP_BRANCH_ID = CB.CMP_BRANCH_ID AND EL.DSG_ID_MAIN=DE.DSG_ID  AND DE.SET_TYPE='D' AND EL.DPT_ID=DP.DPT_ID "
                            + "AND CB.CMP_BRANCH_ID ='" + bId + "'  AND EL.EMP_ID ='" + eId + "' ";

        try
        {
            con = new SqlConnection(cn);
            con.Open();
            DataSet oDS = new DataSet();
            SqlDataAdapter odaData = new SqlDataAdapter(new SqlCommand(strQuery, con));
            odaData.Fill(oDS, "EMPLOYEE_DETAIL_INFO");
            return oDS;
        }
        catch (Exception ex)
        {
            return null;
        }
        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }
    }
    #endregion



    public DataSet GetLeaveType(string bId)
    {
        string strSQL = "";

        strSQL = " CMP_BRANCH_ID IN ('" + bId + "') AND NATURE_TYPE='NL' ";

        string strQuery = "SELECT * FROM PR_LEAVE_TYPE "
                            + "WHERE  "
                            + strSQL
                            + " ORDER BY PRTL_CODE ASC";

        try
        {
            con = new SqlConnection(cn);
            con.Open();
            DataSet oDS = new DataSet();
            SqlDataAdapter odaData = new SqlDataAdapter(new SqlCommand(strQuery, con));
            odaData.Fill(oDS, "PR_LEAVE_TYPE");
            return oDS;
        }
        catch (Exception ex)
        {
            return null;
        }
        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }
    }


    //public DataSet FindEmpLeaveTypeGenerel(string bId, string eId, string year)
    // {
    //     //LT.CMP_BRANCH_ID = C.CMP_BRANCH_ID AND
    //     string strQuery = "SELECT LT.PRLT_ID, PRLT_TITLE, LEAVEALL_ALLOWED, PRLT_IS_PAYABLE, LT.PRTL_CODE, EL.EMP_CONFIR_DATE, EL.EMP_FINAL_CONFIR_DATE "
    //                         + "FROM PR_LEAVE_TYPE LT, PR_LEAVE_ALLOWED LA, PR_EMPLOYEE_LIST EL, CM_CMP_BRANCH C, HR_YEAR Y "
    //                         + "WHERE LA.CMP_BRANCH_ID=C.CMP_BRANCH_ID AND LA.YR_ID=Y.YR_ID AND "
    //                         + "EL.CMP_BRANCH_ID=C.CMP_BRANCH_ID AND LT.PRLT_ID=LA.PRLT_ID AND EL.EMP_ID=LA.EMP_ID "
    //                         + "AND C.CMP_BRANCH_ID ='" + bId + "' AND EL.EMP_ID ='" + eId + "' AND Y.YR_ID ='" + year + "' ";

    //     try
    //     {
    //         con = new SqlConnection(cn);
    //         con.Open();
    //         DataSet oDS = new DataSet();
    //         SqlDataAdapter odaData = new SqlDataAdapter(new SqlCommand(strQuery, con));
    //         odaData.Fill(oDS, "PR_LEAVE_ALLOWED");
    //         return oDS;
    //     }
    //     catch (Exception ex)
    //     {
    //         return null;
    //     }

    //     finally
    //     {
    //         con.Close();
    //         con = null;
    //     }
    // }

    public DataSet FindEmpLeaveTypeGenerel(string bId, string eId, string year)
    {
        //LT.CMP_BRANCH_ID = C.CMP_BRANCH_ID AND
        //string strQuery = "SELECT LT.PRLT_ID, PRLT_TITLE, LEAVEALL_ALLOWED, PRLT_IS_PAYABLE, LT.PRTL_CODE, EL.EMP_CONFIR_DATE, EL.EMP_FINAL_CONFIR_DATE "
        //                    + "FROM PR_LEAVE_TYPE LT, PR_LEAVE_ALLOWED LA, PR_EMPLOYEE_LIST EL, CM_CMP_BRANCH C, HR_YEAR Y "
        //                    + "WHERE LA.CMP_BRANCH_ID=C.CMP_BRANCH_ID AND LA.YR_ID=Y.YR_ID AND "
        //                    + "EL.CMP_BRANCH_ID=C.CMP_BRANCH_ID AND LT.PRLT_ID=LA.PRLT_ID AND EL.EMP_ID=LA.EMP_ID "
        //                    + "AND C.CMP_BRANCH_ID ='" + bId + "' AND EL.EMP_ID ='" + eId + "' AND Y.YR_ID ='" + year + "' ";

        string strQuery = "SELECT "
                                + " LT.PRLT_ID,"
                                + "PRLT_TITLE,"
                                + "LEAVEALL_ALLOWED,"
                                + "PRLT_IS_PAYABLE,"
                                + "LT.PRTL_CODE,"
                                + "EL.EMP_CONFIR_DATE,"
                                + "EL.EMP_FINAL_CONFIR_DATE "
                                + "FROM  PR_LEAVE_TYPE LT,"
                                + "PR_LEAVE_ALLOWED LA,"
                                + "PR_EMPLOYEE_LIST EL,"
                                + "CM_CMP_BRANCH C,"
                                + "HR_YEAR Y "
                                + "WHERE LA.CMP_BRANCH_ID=C.CMP_BRANCH_ID "
                                + "AND LA.YR_ID=Y.YR_ID "
                                + "AND EL.CMP_BRANCH_ID=C.CMP_BRANCH_ID "
                                + "AND LT.PRLT_ID= LA.PRLT_ID "
                                + "AND EL.EMP_ID= LA.EMP_ID "
                                + "AND C.CMP_BRANCH_ID='" + bId + "' "
                                + "AND EL.EMP_ID='" + eId + "' "
                                + "AND Y.YR_ID ='" + year + "'";

        try
        {
            con = new SqlConnection(cn);
            con.Open();
            DataSet oDS = new DataSet();
            SqlDataAdapter odaData = new SqlDataAdapter(new SqlCommand(strQuery, con));
            odaData.Fill(oDS, "PR_LEAVE_ALLOWED");
            return oDS;
        }
        catch (Exception ex)
        {
            return null;
        }
        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }
    }
    public DataSet FindEmpLeaveTypeGenerel(string bId, string eId, string year, string dept)
    {
        string strQuery = "SELECT LT.PRLT_ID, PRLT_TITLE, LEAVEALL_ALLOWED, PRLT_IS_PAYABLE, LT.PRTL_CODE, EL.EMP_CONFIR_DATE, EL.EMP_FINAL_CONFIR_DATE "
                            + "FROM PR_LEAVE_TYPE LT, PR_LEAVE_ALLOWED LA, PR_EMPLOYEE_LIST EL, CM_CMP_BRANCH C, HR_YEAR Y "
                            + "WHERE LA.CMP_BRANCH_ID=C.CMP_BRANCH_ID AND LA.YR_ID=Y.YR_ID AND "
                            + "EL.CMP_BRANCH_ID=C.CMP_BRANCH_ID AND LT.PRLT_ID=LA.PRLT_ID AND EL.EMP_ID=LA.EMP_ID "
                            + "AND C.CMP_BRANCH_ID ='" + bId + "' AND EL.EMP_ID ='" + eId + "' AND Y.YR_ID ='" + year + "' "
                            + "AND LA.DPT_ID='" + dept + "' ";
        var con = new SqlConnection(cn);
        try
        {

            DataSet oDS = new DataSet();
            SqlDataAdapter odaData = new SqlDataAdapter(new SqlCommand(strQuery, con));
            odaData.Fill(oDS, "PR_LEAVE_ALLOWED");
            return oDS;
        }
        catch (Exception ex)
        {
            return null;
        }
        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }
    }
    public DataSet GetLeavedaysGeneral(string bId, string eId, string year, string tId)
    {
        //string strQuery = "SELECT LT.PRLT_ID, PRLT_TITLE, LEAVEALL_ALLOWED, PRLT_IS_PAYABLE, LT.PRTL_CODE, EL.EMP_CONFIR_DATE, EL.EMP_FINAL_CONFIR_DATE, LEAVE_ALLOWED_MON "
        //                    + "FROM PR_LEAVE_TYPE LT, PR_LEAVE_ALLOWED LA, PR_EMPLOYEE_LIST EL, CM_CMP_BRANCH C, HR_YEAR Y "
        //                    + "WHERE LT.CMP_BRANCH_ID = C.CMP_BRANCH_ID AND LA.CMP_BRANCH_ID=C.CMP_BRANCH_ID AND LA.YR_ID=Y.YR_ID AND "
        //                    + "EL.CMP_BRANCH_ID=C.CMP_BRANCH_ID AND LT.PRLT_ID=LA.PRLT_ID "
        //                    + "AND C.CMP_BRANCH_ID ='" + bId + "' AND LA.EMP_ID ='" + eId + "' AND LA.YR_ID ='" + year + "' AND LA.PRLT_ID='" + tId + "' ";


        string strQuery = "SELECT LA.ACTUAL_ALLOWED,LT.PRLT_ID, PRLT_TITLE, LEAVEALL_ALLOWED, PRLT_IS_PAYABLE, EL.EMP_CONFIR_DATE, EL.EMP_FINAL_CONFIR_DATE, LEAVE_ALLOWED_MON "
                            + "FROM PR_LEAVE_TYPE LT, PR_LEAVE_ALLOWED LA, PR_EMPLOYEE_LIST EL "
                            + "WHERE EL.EMP_ID=LA.EMP_ID AND EL.CMP_BRANCH_ID=LA.CMP_BRANCH_ID AND EL.CMP_BRANCH_ID ='" + bId + "' "
                            + "AND LT.PRLT_ID=LA.PRLT_ID AND EL.EMP_ID ='" + eId + "' "
                            + "AND YR_ID ='" + year + "' AND LT.PRLT_ID='" + tId + "'";


        try
        {
            con = new SqlConnection(cn);
            con.Open();
            DataSet oDS = new DataSet();
            SqlDataAdapter odaData = new SqlDataAdapter(new SqlCommand(strQuery, con));
            odaData.Fill(oDS, "PR_LEAVE_ALLOWED");
            return oDS;
        }
        catch (Exception ex)
        {
            return null;
        }
        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }
    }

    public DataSet GetLeavedaysGeneral(string bId, string eId, string year, string tId, string dept)
    {
        string strQuery = "SELECT LA.CARRY_FORWARD, LA.LEAVE_YEAR_BGN, LA.LEAVE_YEAR_END, LA.ACTUAL_ALLOWED, LT.PRLT_ID, PRLT_TITLE, LEAVEALL_ALLOWED, PRLT_IS_PAYABLE, EL.EMP_CONFIR_DATE, EL.EMP_FINAL_CONFIR_DATE, LEAVE_ALLOWED_MON "
                            + "FROM PR_LEAVE_TYPE LT, PR_LEAVE_ALLOWED LA, PR_EMPLOYEE_LIST EL "
                            + "WHERE EL.EMP_ID=LA.EMP_ID AND EL.CMP_BRANCH_ID=LA.CMP_BRANCH_ID AND EL.CMP_BRANCH_ID ='" + bId + "' "
                            + "AND LT.PRLT_ID=LA.PRLT_ID AND EL.EMP_ID ='" + eId + "' "
                            + "AND YR_ID ='" + year + "' AND LT.PRLT_ID='" + tId + "'"
                            + "AND LA.DPT_ID='" + dept + "'";

        try
        {
            con = new SqlConnection(cn);
            con.Open();
            DataSet oDS = new DataSet();
            SqlDataAdapter odaData = new SqlDataAdapter(new SqlCommand(strQuery, con));
            odaData.Fill(oDS, "PR_LEAVE_ALLOWED");
            return oDS;
        }
        catch (Exception ex)
        {
            return null;
        }
        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }
    }

    public DataSet ChkEmpGeneralLeave(string bId, string eId, string yId)
    {
        string strQuery = "SELECT * FROM PR_LEAVE_ALLOWED "
                            + "WHERE CMP_BRANCH_ID='" + bId + "' AND  EMP_ID='" + eId + "' AND "
                            + "YR_ID='" + yId + "'";


        try
        {
            con = new SqlConnection(cn);
            con.Open();
            DataSet oDS = new DataSet();
            SqlDataAdapter odaData = new SqlDataAdapter(new SqlCommand(strQuery, con));
            odaData.Fill(oDS, "PR_LEAVE_ALLOWED");
            return oDS;
        }
        catch (Exception ex)
        {
            return null;
        }
        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }
    }

    public string RemoveLeaveGeneral(string bId, string eId, string yId)
    {
        string strSql;

        strSql = "DELETE from PR_LEAVE_ALLOWED "
                + "WHERE CMP_BRANCH_ID='" + bId + "' AND  EMP_ID='" + eId + "' AND "
                            + "YR_ID='" + yId + "'";

        try
        {

            con = new SqlConnection(cn);
            con.Open();
            dbTransaction = con.BeginTransaction();
            SqlCommand olcmd = new SqlCommand(strSql, con, dbTransaction);
            olcmd.Connection = con;
            olcmd.ExecuteNonQuery();
            dbTransaction.Commit();
            //con.Close();

            return "Saved successfully";
        }
        catch (Exception ex)
        {
            return ex.Message.ToString();
        }
        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }
    }

    public string SaveEmployeeLeave(string bId, string empId, string yId, string typId, string allowed) //  Employee LEAVE
    {
        string strSql;


        strSql = "SELECT CMP_BRANCH_ID, EMP_ID, YR_ID, PRLT_ID, LEAVEALL_ALLOWED FROM PR_LEAVE_ALLOWED";


        try
        {
            // Payroll Detail
            DataRow oOrderRow;

            con = new SqlConnection(cn);
            con.Open();
            dbTransaction = con.BeginTransaction();
            DataSet oDS = new DataSet();
            SqlDataAdapter oOrdersDataAdapter = new SqlDataAdapter(new SqlCommand(strSql, con, dbTransaction));
            SqlCommandBuilder oOrdersCmdBuilder = new SqlCommandBuilder(oOrdersDataAdapter);
            //oOrdersDataAdapter.MissingSchemaAction = MissingSchemaAction.AddWithKey;
            //oOrdersDataAdapter.Fill(oDS, "PR_LEAVE_ALLOWED");
            oOrdersDataAdapter.FillSchema(oDS, SchemaType.Source, "PR_LEAVE_ALLOWED");
            oOrderRow = oDS.Tables["PR_LEAVE_ALLOWED"].NewRow();

            // 5 fields
            oOrderRow["CMP_BRANCH_ID"] = bId;
            oOrderRow["EMP_ID"] = empId;
            oOrderRow["YR_ID"] = yId;
            oOrderRow["PRLT_ID"] = typId;

            if (allowed != "")
                oOrderRow["LEAVEALL_ALLOWED"] = allowed;
            else
                oOrderRow["LEAVEALL_ALLOWED"] = 0;

            oDS.Tables["PR_LEAVE_ALLOWED"].Rows.Add(oOrderRow);
            oOrdersDataAdapter.Update(oDS, "PR_LEAVE_ALLOWED");
            dbTransaction.Commit();
            //con.Close();
            return "Saved successfully";

        }
        catch (Exception ex)
        {
            return ex.Message.ToString();
        }
        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }
    }
    public string SaveEmployeeLeave(string bId, string empId, string yId, string typId, string totYearlyallowed, string Yearlyallowed, string monAllowed) //  Employee LEAVE
    {
        string strSql;


        strSql = "SELECT ACTUAL_ALLOWED, CMP_BRANCH_ID, EMP_ID, YR_ID, PRLT_ID, LEAVEALL_ALLOWED, LEAVE_ALLOWED_MON FROM PR_LEAVE_ALLOWED";


        try
        {
            // Payroll Detail
            DataRow oOrderRow;

            con = new SqlConnection(cn);
            con.Open();
            dbTransaction = con.BeginTransaction();
            DataSet oDS = new DataSet();
            SqlDataAdapter oOrdersDataAdapter = new SqlDataAdapter(new SqlCommand(strSql, con, dbTransaction));
            SqlCommandBuilder oOrdersCmdBuilder = new SqlCommandBuilder(oOrdersDataAdapter);
            //oOrdersDataAdapter.MissingSchemaAction = MissingSchemaAction.AddWithKey;
            //oOrdersDataAdapter.Fill(oDS, "PR_LEAVE_ALLOWED");
            oOrdersDataAdapter.FillSchema(oDS, SchemaType.Source, "PR_LEAVE_ALLOWED");
            oOrderRow = oDS.Tables["PR_LEAVE_ALLOWED"].NewRow();

            // 5 fields
            oOrderRow["CMP_BRANCH_ID"] = bId;
            oOrderRow["EMP_ID"] = empId;
            oOrderRow["YR_ID"] = yId;
            oOrderRow["PRLT_ID"] = typId;

            if (totYearlyallowed != "")
            {
                oOrderRow["LEAVEALL_ALLOWED"] = totYearlyallowed;
            }
            else
            {
                oOrderRow["LEAVEALL_ALLOWED"] = 0;
            }

            if (Yearlyallowed != "")
            {
                oOrderRow["ACTUAL_ALLOWED"] = Yearlyallowed;
            }
            else
            {
                oOrderRow["ACTUAL_ALLOWED"] = 0;
            }
            if (monAllowed != "")
            {
                oOrderRow["LEAVE_ALLOWED_MON"] = monAllowed;
            }
            else
            {
                oOrderRow["LEAVE_ALLOWED_MON"] = 0;
            }

            oDS.Tables["PR_LEAVE_ALLOWED"].Rows.Add(oOrderRow);
            oOrdersDataAdapter.Update(oDS, "PR_LEAVE_ALLOWED");
            dbTransaction.Commit();
            //con.Close();
            return "Saved successfully";
        }
        catch (Exception ex)
        {
            return ex.Message.ToString();
        }
        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }
    }
    public string SaveEmployeeLeave(string bId, string empId, string yId, string typId, string totYearlyallowed, string Yearlyallowed, string monAllowed, string dept, string carryFrd) //  Employee LEAVE
    {
        string strSql;


        strSql = "SELECT CARRY_FORWARD, DPT_ID, ACTUAL_ALLOWED, CMP_BRANCH_ID, EMP_ID, YR_ID, PRLT_ID, LEAVEALL_ALLOWED, LEAVE_ALLOWED_MON FROM PR_LEAVE_ALLOWED";

        con = new SqlConnection(cn);
        try
        {
            // Payroll Detail
            DataRow oOrderRow;

            con.Open();
            dbTransaction = con.BeginTransaction();
            DataSet oDS = new DataSet();
            SqlDataAdapter oOrdersDataAdapter = new SqlDataAdapter(new SqlCommand(strSql, con, dbTransaction));
            SqlCommandBuilder oOrdersCmdBuilder = new SqlCommandBuilder(oOrdersDataAdapter);
            //oOrdersDataAdapter.MissingSchemaAction = MissingSchemaAction.AddWithKey;
            //oOrdersDataAdapter.Fill(oDS, "PR_LEAVE_ALLOWED");
            oOrdersDataAdapter.FillSchema(oDS, SchemaType.Source, "PR_LEAVE_ALLOWED");
            oOrderRow = oDS.Tables["PR_LEAVE_ALLOWED"].NewRow();

            // 5 fields
            oOrderRow["CARRY_FORWARD"] = carryFrd;
            oOrderRow["CMP_BRANCH_ID"] = bId;
            oOrderRow["EMP_ID"] = empId;
            oOrderRow["DPT_ID"] = dept;
            oOrderRow["YR_ID"] = yId;
            oOrderRow["PRLT_ID"] = typId;

            if (totYearlyallowed != "")
            {
                oOrderRow["LEAVEALL_ALLOWED"] = totYearlyallowed;
            }
            else
            {
                oOrderRow["LEAVEALL_ALLOWED"] = 0;
            }

            if (Yearlyallowed != "")
            {
                oOrderRow["ACTUAL_ALLOWED"] = Yearlyallowed;
            }
            else
            {
                oOrderRow["ACTUAL_ALLOWED"] = 0;
            }
            if (monAllowed != "")
            {
                oOrderRow["LEAVE_ALLOWED_MON"] = monAllowed;
            }
            else
            {
                oOrderRow["LEAVE_ALLOWED_MON"] = 0;
            }

            oDS.Tables["PR_LEAVE_ALLOWED"].Rows.Add(oOrderRow);
            oOrdersDataAdapter.Update(oDS, "PR_LEAVE_ALLOWED");
            dbTransaction.Commit();
            //con.Close();
            return "Saved successfully";
        }
        catch (Exception ex)
        {
            return ex.Message.ToString();
        }
        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }
    }

    public DataSet GetLeavedaysGeneral(string bId, string eId, string year)
    {
        string strQuery = "SELECT LT.PRLT_ID, PRLT_TITLE, LEAVEALL_ALLOWED, LT.PRTL_CODE, PRLT_IS_PAYABLE, EL.EMP_CONFIR_DATE, EL.EMP_FINAL_CONFIR_DATE, CARRY_FORWARD, LEAVE_ALLOWED_MON "
                            + "FROM PR_LEAVE_TYPE LT, PR_LEAVE_ALLOWED LA, PR_EMPLOYEE_LIST EL "
                            + "WHERE EL.EMP_ID=LA.EMP_ID AND EL.CMP_BRANCH_ID=LA.CMP_BRANCH_ID AND EL.CMP_BRANCH_ID ='" + bId + "' "
                            + "AND LT.PRLT_ID=LA.PRLT_ID AND EL.EMP_ID ='" + eId + "' AND LA.EMP_ID ='" + eId + "' "
                            + "AND YR_ID ='" + year + "' AND LA.LEAVEALL_ALLOWED > 0 ";

        //+ "AND LT.PRLT_ID=LA.PRLT_ID AND EL.EMP_ID ='" + eId + "' AND LA.EMP_ID ='" + eId + "' AND LA.DPT_ID = '" + DeptId + "' "
        //+"AND YR_ID ='" + year + "' AND LA.LEAVEALL_ALLOWED > 0 ";


        try
        {
            con = new SqlConnection(cn);
            con.Open();
            DataSet oDS = new DataSet();
            SqlDataAdapter odaData = new SqlDataAdapter(new SqlCommand(strQuery, con));
            odaData.Fill(oDS, "PR_LEAVE_ALLOWED");
            return oDS;
        }
        catch (Exception ex)
        {
            return null;
        }
        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }
    }

    public DataSet GetLeavedaysGeneralAsDept(string bId, string eId, string year, string DeptId)
    {
        string strQuery = "SELECT LT.PRLT_ID, PRLT_TITLE, LEAVEALL_ALLOWED, LT.PRTL_CODE, PRLT_IS_PAYABLE, EL.EMP_CONFIR_DATE, EL.EMP_FINAL_CONFIR_DATE, CARRY_FORWARD, LEAVE_ALLOWED_MON, LTRIM(CAST(ISNULL(EL.EMP_TITLE, '') AS VARCHAR(100)) + ' ' + CAST(EL.EMP_NAME AS VARCHAR(500))) AS ENAME,EL.EMP_CODE "
                            + "FROM PR_LEAVE_TYPE LT, PR_LEAVE_ALLOWED LA, PR_EMPLOYEE_LIST EL "
                            + "WHERE EL.EMP_ID=LA.EMP_ID AND EL.CMP_BRANCH_ID=LA.CMP_BRANCH_ID AND EL.CMP_BRANCH_ID ='" + bId + "' "
                            + "AND LT.PRLT_ID=LA.PRLT_ID AND EL.EMP_ID ='" + eId + "' AND LA.EMP_ID ='" + eId + "' AND LA.DPT_ID = '" + DeptId + "' "
                            + "AND YR_ID ='" + year + "' AND LA.LEAVEALL_ALLOWED > 0  order by EL.EMP_ID,EL.CMP_BRANCH_ID,LT.PRLT_ID";


        try
        {
            con = new SqlConnection(cn);
            con.Open();
            DataSet oDS = new DataSet();
            SqlDataAdapter odaData = new SqlDataAdapter(new SqlCommand(strQuery, con));
            odaData.Fill(oDS, "PR_LEAVE_ALLOWED");
            return oDS;
        }
        catch (Exception ex)
        {
            return null;
        }
        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }
    }
    public DataSet GetLeavedaysGeneralAsDept_LVtype(string bId, string eId, string year, string DeptId, string LvtypeId)
    {
        string strQuery = "SELECT LT.PRLT_ID, PRLT_TITLE, LEAVEALL_ALLOWED, LT.PRTL_CODE, PRLT_IS_PAYABLE, EL.EMP_CONFIR_DATE, EL.EMP_FINAL_CONFIR_DATE, CARRY_FORWARD, LEAVE_ALLOWED_MON, LTRIM(CAST(ISNULL(EL.EMP_TITLE, '') AS VARCHAR(100)) + ' ' + CAST(EL.EMP_NAME AS VARCHAR(500))) AS ENAME,EL.EMP_CODE "
                            + "FROM PR_LEAVE_TYPE LT, PR_LEAVE_ALLOWED LA, PR_EMPLOYEE_LIST EL "
                            + "WHERE EL.EMP_ID=LA.EMP_ID AND EL.CMP_BRANCH_ID=LA.CMP_BRANCH_ID AND EL.CMP_BRANCH_ID ='" + bId + "' "
                            + "AND LT.PRLT_ID=LA.PRLT_ID AND LT.PRLT_ID='" + LvtypeId + "' AND EL.EMP_ID ='" + eId + "' AND LA.EMP_ID ='" + eId + "' AND LA.DPT_ID = '" + DeptId + "' "
                            + "AND YR_ID ='" + year + "' AND LA.LEAVEALL_ALLOWED > 0 ";


        try
        {
            con = new SqlConnection(cn);
            con.Open();
            DataSet oDS = new DataSet();
            SqlDataAdapter odaData = new SqlDataAdapter(new SqlCommand(strQuery, con));
            odaData.Fill(oDS, "PR_LEAVE_ALLOWED");
            return oDS;
        }
        catch (Exception ex)
        {
            return null;
        }
        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }
    }
    public DataSet GetLeavedaysGeneralAsDeptTypeWise(string bId, string eId, string year, string DeptId, string typeLv)
    {
        string strQuery = "SELECT LT.PRLT_ID, PRLT_TITLE, LEAVEALL_ALLOWED, LT.PRTL_CODE, PRLT_IS_PAYABLE, EL.EMP_CONFIR_DATE, EL.EMP_FINAL_CONFIR_DATE, CARRY_FORWARD, LEAVE_ALLOWED_MON "
                            + "FROM PR_LEAVE_TYPE LT, PR_LEAVE_ALLOWED LA, PR_EMPLOYEE_LIST EL "
                            + "WHERE EL.EMP_ID=LA.EMP_ID AND EL.CMP_BRANCH_ID=LA.CMP_BRANCH_ID AND EL.CMP_BRANCH_ID ='" + bId + "' "
                            + "AND LT.PRTL_CODE ='" + typeLv + "' AND  LT.PRLT_ID=LA.PRLT_ID AND EL.EMP_ID ='" + eId + "'  AND LA.EMP_ID ='" + eId + "' AND LA.DPT_ID = '" + DeptId + "' "
                            + "AND YR_ID ='" + year + "' AND LA.LEAVEALL_ALLOWED > 0 ";


        try
        {
            con = new SqlConnection(cn);
            con.Open();
            DataSet oDS = new DataSet();
            SqlDataAdapter odaData = new SqlDataAdapter(new SqlCommand(strQuery, con));
            odaData.Fill(oDS, "PR_LEAVE_ALLOWED");
            return oDS;
        }
        catch (Exception ex)
        {
            return null;
        }
        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }
    }


    #region old

    public DataSet GetLeaveGeneralDay(string bId, string eId, string year)
    {
        string strQuery = "SELECT LT.PRLT_ID, PRLT_TITLE, LEAVEALL_ALLOWED, PRLT_IS_PAYABLE, LT.PRTL_CODE, EL.EMP_CONFIR_DATE, EL.EMP_FINAL_CONFIR_DATE, LEAVE_ALLOWED_MON "
                            + "FROM PR_LEAVE_TYPE LT, PR_LEAVE_ALLOWED LA, PR_EMPLOYEE_LIST EL, CM_CMP_BRANCH C, HR_YEAR Y "
                            + "WHERE LT.CMP_BRANCH_ID = C.CMP_BRANCH_ID AND LA.CMP_BRANCH_ID=C.CMP_BRANCH_ID AND LA.YR_ID=Y.YR_ID AND "
                            + "EL.CMP_BRANCH_ID=C.CMP_BRANCH_ID AND LT.PRLT_ID=LA.PRLT_ID AND EL.EMP_ID=LA.EMP_ID "
                            + "AND C.CMP_BRANCH_ID ='" + bId + "' AND LA.EMP_ID ='" + eId + "' AND LA.YR_ID ='" + year + "' ";



        try
        {
            con = new SqlConnection(cn);
            con.Open();
            DataSet oDS = new DataSet();
            SqlDataAdapter odaData = new SqlDataAdapter(new SqlCommand(strQuery, con));
            odaData.Fill(oDS, "PR_LEAVE_ALLOWED");
            return oDS;
        }
        catch (Exception ex)
        {
            return null;
        }
        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }
    }
    #endregion

    public DataSet GetYearStartDate(string bId)
    {
        string strSql = "select YR_STARAT_DATE from HR_YEAR where YR_STATUS in ('R','TC') AND CMP_BRANCH_ID='" + bId + "' order by YR_STARAT_DATE";
        DataSet oDS = new DataSet();

        try
        {

            con = new SqlConnection(cn);
            con.Open();
            SqlDataAdapter odaData = new SqlDataAdapter(new SqlCommand(strSql, con));
            odaData.Fill(oDS, "YEAR_START_DATE");

        }
        catch (Exception ex)
        {

        }
        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }
        return oDS;
    }
    public DataSet GetYearStartDate(string bId, string Allbranch)
    {
        string strSql = "select YR_STARAT_DATE from HR_YEAR where YR_STATUS='R'"; // AND (CMP_BRANCH_ID='" + bId + "' OR CMP_BRANCH_ID='" + Allbranch + "')
        DataSet oDS = new DataSet();

        try
        {

            con = new SqlConnection(cn);
            con.Open();
            SqlDataAdapter odaData = new SqlDataAdapter(new SqlCommand(strSql, con));
            odaData.Fill(oDS, "YEAR_START_DATE");

        }
        catch (Exception ex)
        {

        }
        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }
        return oDS;
    }
    public void UpdateDeskToDate(string empID, string DurToDate)
    {
        string updateString;
        updateString = "UPDATE HR_DESK_ASSIGNED SET [TO_DATE]= '" + DurToDate + "' WHERE [FROM_DATE]=(SELECT MAX([FROM_DATE]) FROM HR_DESK_ASSIGNED "
        + " WHERE EMP_ID = '" + empID + "')  AND EMP_ID =  '" + empID + "'";
        try
        {
            con = new SqlConnection(cn);
            con.Open();
            dbTransaction = con.BeginTransaction();
            SqlCommand cmd = new SqlCommand(updateString, con, dbTransaction);
            cmd.Connection = con;
            cmd.ExecuteNonQuery();
            dbTransaction.Commit();
        }
        catch (Exception ex)
        {
            ex.Message.ToString();
        }
        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }
    }
    public void InsertDeskAssign(string empID, string deskID, string fromDate, string branchID, string refNo)//string divisionID //Developed By: Md. Sydur rahman (12-Dec-13)
    {
        string tblAssignDesk = "HR_DESK_ASSIGNED";
        string fldSlNo = "SLNO";
        int slno = SlNo(tblAssignDesk, fldSlNo, empID);
        string strSql;
        strSql = "SELECT EMP_ID, DESK_ID, FROM_DATE, TO_DATE, CMP_BRANCH_ID, ENTRY_DATE, SLNO, REF_NO FROM HR_DESK_ASSIGNED";
        try
        {
            DataRow oOrderRow;
            con = new SqlConnection(cn);
            con.Open();
            dbTransaction = con.BeginTransaction();
            DataSet oDS = new DataSet();
            SqlDataAdapter oOrdersDataAdapter = new SqlDataAdapter(new SqlCommand(strSql, con, dbTransaction));
            SqlCommandBuilder oOrdersCmdBuilder = new SqlCommandBuilder(oOrdersDataAdapter);
            oOrdersDataAdapter.FillSchema(oDS, SchemaType.Source);

            DataTable dtAddSchedule = oDS.Tables["Table"];
            dtAddSchedule.TableName = "HR_DESK_ASSIGNED";

            oOrderRow = oDS.Tables["HR_DESK_ASSIGNED"].NewRow();

            oOrderRow["EMP_ID"] = empID;
            oOrderRow["DESK_ID"] = deskID;
            oOrderRow["FROM_DATE"] = fromDate;
            oOrderRow["CMP_BRANCH_ID"] = branchID;
            oOrderRow["ENTRY_DATE"] = DateTime.Today;
            oOrderRow["REF_NO"] = refNo;
            oOrderRow["SLNO"] = slno + 1;


            oDS.Tables["HR_DESK_ASSIGNED"].Rows.Add(oOrderRow);
            oOrdersDataAdapter.Update(oDS, "HR_DESK_ASSIGNED");
            dbTransaction.Commit();
        }
        catch (Exception ex)
        {
            ex.Message.ToString();
        }
        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }
    }
    public int SlNo(string tableName, string fieldName, string empID)
    {
        //string strQuery = " HR_EMP_TRANSFER '" + empID + "'";
        string strQuery = "SELECT isnull(MAX(" + fieldName + "),0)" + fieldName + " FROM " + tableName + " WHERE EMP_ID='" + empID + "'";
        DataSet oDS = new DataSet();
        int result = 0;
        try
        {
            con = new SqlConnection(cn);
            con.Open();

            SqlDataAdapter odaData = new SqlDataAdapter(new SqlCommand(strQuery, con));
            //odaData.Fill(oDS, "HR_EMP_TRANSFER");
            odaData.Fill(oDS, tableName);
            if (oDS.Tables[0].Rows.Count > 0)
            {
                //result = Convert.ToInt32(oDS.Tables[0].Rows[0]["SL_NO"].ToString());
                result = Convert.ToInt32(oDS.Tables[0].Rows[0][fieldName].ToString());
            }
        }
        catch (Exception ex)
        {
            return result;
        }
        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }
        return result;
    }
    public string GetPrMonthDesc(string bId, string mId)
    {
        string strSql = "SELECT PRM_DETAILS FROM PR_PAYROLL_MASTER WHERE CMP_BRANCH_ID = '" + bId + "' AND PRM_ID = '" + mId + "'";

        try
        {
            con = new SqlConnection(cn);
            con.Open();
            DataSet oDs = new DataSet();
            SqlDataAdapter oOrdersDataAdapter = new SqlDataAdapter(new SqlCommand(strSql, con));
            SqlCommandBuilder oOrdersCmdBuilder = new SqlCommandBuilder(oOrdersDataAdapter);
            oOrdersDataAdapter.Fill(oDs, "PAYROLL_MONTH");

            DataRow dRow = oDs.Tables["PAYROLL_MONTH"].Rows[0];
            return dRow["PRM_DETAILS"].ToString();
        }
        catch (Exception ex)
        {
            return ex.Message.ToString();

        }
        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }
    }

    public bool CheckEmpAllowedLeave(string bId, string hrYear)
    {
        string strSql;

        strSql = "select LEAVEALL_ID from PR_LEAVE_ALLOWED WHERE CMP_BRANCH_ID = '" + bId + "' AND YR_ID = '" + hrYear + "'";

        try
        {
            con = new SqlConnection(cn);
            con.Open();
            DataSet oDS = new DataSet();
            SqlDataAdapter oOrdersDataAdapter = new SqlDataAdapter(new SqlCommand(strSql, con));
            SqlCommandBuilder oOrdersCmdBuilder = new SqlCommandBuilder(oOrdersDataAdapter);
            oOrdersDataAdapter.MissingSchemaAction = MissingSchemaAction.AddWithKey;
            oOrdersDataAdapter.Fill(oDS, "LEAVE_ALLOWED");

            DataTable tbl_AD = oDS.Tables["LEAVE_ALLOWED"];

            // already exists
            if (tbl_AD.Rows.Count > 0) { return true; }
            else { return false; }
        }
        catch (Exception ex)
        {
            return false;
        }
        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }
    }

    public string RemoveAllowedLeave(string bId, string hrYear)
    {
        string strSql;

        strSql = "DELETE from PR_LEAVE_ALLOWED "
                + "WHERE CMP_BRANCH_ID='" + bId + "' AND YR_ID='" + hrYear + "'";

        try
        {

            con = new SqlConnection(cn);
            con.Open();
            dbTransaction = con.BeginTransaction();
            SqlCommand olcmd = new SqlCommand(strSql, con, dbTransaction);
            olcmd.Connection = con;
            olcmd.ExecuteNonQuery();
            dbTransaction.Commit();
            //con.Close();

            return "Deleted Successfully";
        }
        catch (Exception ex)
        {
            return ex.Message.ToString();
        }
        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }
    }

    #region Leave Policy
    public bool CheckLeavePolicyExists(string bId, string gId, string LvTypId, string yrId, string eTypeId, string dptID)
    {
        string strSql;

        strSql = "select PRLP_ID from PR_LEAVE_POLICY WHERE CMP_BRANCH_ID = '" + bId + "' AND DSG_ID = '" + gId + "' "
                 + "AND PRLT_ID = '" + LvTypId + "' AND YR_ID = '" + yrId + "' AND TYP_CODE = '" + eTypeId + "' AND DPT_ID='" + dptID + "' ";
        try
        {

            con = new SqlConnection(cn);
            con.Open();
            DataSet oDS = new DataSet();
            SqlDataAdapter oOrdersDataAdapter = new SqlDataAdapter(new SqlCommand(strSql, con));
            SqlCommandBuilder oOrdersCmdBuilder = new SqlCommandBuilder(oOrdersDataAdapter);
            oOrdersDataAdapter.MissingSchemaAction = MissingSchemaAction.AddWithKey;
            oOrdersDataAdapter.Fill(oDS, "HR_LEAVE_POLICY");

            DataTable tbl_AD = oDS.Tables["HR_LEAVE_POLICY"];

            // already exists
            if (tbl_AD.Rows.Count > 0) { return true; }
            else { return false; }
        }
        catch (Exception ex)
        {
            return false;
        }
        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }
    }

    public bool CheckLeavePolicyExistsEmp(string bId, string gId, string LvTypId, string yrId, string eTypeId, string dptID, string empId)
    {
        string strSql;

        strSql = "select PRLP_ID from PR_LEAVE_POLICY WHERE CMP_BRANCH_ID = '" + bId + "' AND DSG_ID = '" + gId + "' "
                 + "AND PRLT_ID = '" + LvTypId + "' AND YR_ID = '" + yrId + "' AND TYP_CODE = '" + eTypeId + "' AND DPT_ID='" + dptID + "' and EMP_ID='" + empId + "'  ";
        try
        {

            con = new SqlConnection(cn);
            con.Open();
            DataSet oDS = new DataSet();
            SqlDataAdapter oOrdersDataAdapter = new SqlDataAdapter(new SqlCommand(strSql, con));
            SqlCommandBuilder oOrdersCmdBuilder = new SqlCommandBuilder(oOrdersDataAdapter);
            oOrdersDataAdapter.MissingSchemaAction = MissingSchemaAction.AddWithKey;
            oOrdersDataAdapter.Fill(oDS, "HR_LEAVE_POLICY");

            DataTable tbl_AD = oDS.Tables["HR_LEAVE_POLICY"];

            // already exists
            if (tbl_AD.Rows.Count > 0) { return true; }
            else { return false; }
        }
        catch (Exception ex)
        {
            return false;
        }
        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }
    }

    public bool CheckSPLeavePolicyExistsEmp(string bId, string gId, string LvTypId, string yrId, string eTypeId, string dptID, string empId)
    {
        string strSql;

        strSql = "select PRLP_ID from PR_LEAVE_POLICY WHERE CMP_BRANCH_ID = '" + bId + "' AND DSG_ID = '" + gId + "' "
                 + "AND PRLT_ID = '" + LvTypId + "' AND TYP_CODE = '" + eTypeId + "' AND DPT_ID='" + dptID + "' and EMP_ID='" + empId + "'  ";
        try
        {

            con = new SqlConnection(cn);
            con.Open();
            DataSet oDS = new DataSet();
            SqlDataAdapter oOrdersDataAdapter = new SqlDataAdapter(new SqlCommand(strSql, con));
            SqlCommandBuilder oOrdersCmdBuilder = new SqlCommandBuilder(oOrdersDataAdapter);
            oOrdersDataAdapter.MissingSchemaAction = MissingSchemaAction.AddWithKey;
            oOrdersDataAdapter.Fill(oDS, "HR_LEAVE_POLICY");

            DataTable tbl_AD = oDS.Tables["HR_LEAVE_POLICY"];

            // already exists
            if (tbl_AD.Rows.Count > 0) { return true; }
            else { return false; }
        }
        catch (Exception ex)
        {
            return false;
        }
        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }
    }
    public string LeaveStatusAsEmpId(string empId)
    {
        string strQuery = " SELECT LVE_STATUS FROM PR_LEAVE WHERE LVE_ID IN (select MAX(LVE_ID) from PR_LEAVE WHERE EMP_ID='" + empId + "')   ";
        string lvStatus = "";
        try
        {
            con = new SqlConnection(cn);
            con.Open();
            DataSet oDS = new DataSet();
            SqlDataAdapter odaData = new SqlDataAdapter(new SqlCommand(strQuery, con));
            odaData.Fill(oDS, "PR_LEAVE");
            if (oDS.Tables[0].Rows.Count > 0)
            {
                lvStatus = oDS.Tables[0].Rows[0]["LVE_STATUS"].ToString();
            }
        }
        catch (Exception ex)
        {
            return "";
        }

        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }
        return lvStatus;
    }
    public bool CheckLeavePolicyExists(string bId, string gId, string LvTypId, string yrId, string eTypeId)
    {
        string strSql;

        strSql = "select PRLP_ID from PR_LEAVE_POLICY WHERE CMP_BRANCH_ID = '" + bId + "' AND DSG_ID = '" + gId + "' "
                 + "AND PRLT_ID = '" + LvTypId + "' AND YR_ID = '" + yrId + "' AND TYP_CODE = '" + eTypeId + "'";

        try
        {
            con = new SqlConnection(cn);
            con.Open();
            DataSet oDS = new DataSet();
            SqlDataAdapter oOrdersDataAdapter = new SqlDataAdapter(new SqlCommand(strSql, con));
            SqlCommandBuilder oOrdersCmdBuilder = new SqlCommandBuilder(oOrdersDataAdapter);
            oOrdersDataAdapter.MissingSchemaAction = MissingSchemaAction.AddWithKey;
            oOrdersDataAdapter.Fill(oDS, "HR_LEAVE_POLICY");

            DataTable tbl_AD = oDS.Tables["HR_LEAVE_POLICY"];

            // already exists
            if (tbl_AD.Rows.Count > 0) { return true; }
            else { return false; }
        }
        catch (Exception ex)
        {
            return false;
        }
        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }
    }

    public DataSet GetDesig(string empType)
    {
        string strQuery = "SELECT DISTINCT (CAST(DG.DSG_TITLE AS VARCHAR) + CAST(ET.TYP_TYPE AS VARCHAR)) DESIG_WITH_TYP,EL.DSG_ID,DG.DSG_TITLE,ET.TYP_TYPE,DG.SET_CODE,ET.TYP_ID,ET.TYP_CODE "
                          + " FROM PR_EMPLOYEE_LIST EL,PR_DESIGNATION DG,HR_EMP_TYPE ET  "
                          + " WHERE DG.DSG_ID=EL.DSG_ID AND ET.TYP_CODE=EL.EMP_STATUS AND ET.PAYROLL_ALLOWED='Y' AND ET.TYP_CODE='" + empType + "' "
                          + "ORDER BY DG.SET_CODE ";

        try
        {
            con = new SqlConnection(cn);
            con.Open();
            DataSet oDS = new DataSet();
            SqlDataAdapter odaData = new SqlDataAdapter(new SqlCommand(strQuery, con));
            odaData.Fill(oDS, "PR_DESIG");
            return oDS;
        }
        catch (Exception ex)
        {
            return null;
        }

        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }

    }
    public DataSet GetDesigAsEmpId(string empId)
    {
        string strQuery = "SELECT DISTINCT (CAST(DG.DSG_TITLE AS VARCHAR) + CAST(ET.TYP_TYPE AS VARCHAR)) DESIG_WITH_TYP,EL.DSG_ID,DG.DSG_TITLE,ET.TYP_TYPE,DG.SET_CODE,ET.TYP_ID,ET.TYP_CODE "
                          + " FROM PR_EMPLOYEE_LIST EL,PR_DESIGNATION DG,HR_EMP_TYPE ET  "
                          + " WHERE DG.DSG_ID=EL.DSG_ID AND ET.TYP_CODE=EL.EMP_STATUS AND  EL.EMP_ID='" + empId + "' ";


        try
        {
            con = new SqlConnection(cn);
            con.Open();
            DataSet oDS = new DataSet();
            SqlDataAdapter odaData = new SqlDataAdapter(new SqlCommand(strQuery, con));
            odaData.Fill(oDS, "PR_DESIG");
            return oDS;
        }
        catch (Exception ex)
        {
            return null;
        }

        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }

    }
    public string InsertLeavePolicyEmpId(string bId,
                            string gId,
                            string LvTypId,
                            double TotLvMon,
                            double TotLv,
                            double LvAllwnce,
                            string eligiblePeriod,
                            string yrId,
                            string eTypeId,
                            string IHC,
                            string ICA, string frequancyLeave, string validTill, string natureType, string dptID, string empId)
    {
        try
        {
            string strSql = "SELECT EMP_GENDER, DPT_ID, PRLT_ID, PRLP_MON_DAYS, PRLP_DAYS, PRLP_ALLOWANCE, PRLP_ELIGIBLE_PERIOD, CMP_BRANCH_ID,EMP_ID,  "
                          + "DSG_ID,YR_ID, TYP_CODE, PRLP_IHCOUNT, PRLP_CFORWARD,PRLP_FREQUANCY_LEAVE,PRLP_VALID_DAY,NATURE_TYPE FROM PR_LEAVE_POLICY";
            DataRow oOrderRow;
            //DataRow oOrderRow1;
            con = new SqlConnection(cn);
            con.Open();
            dbTransaction = con.BeginTransaction();
            DataSet oDs = new DataSet();
            SqlDataAdapter oDbAdapter = new SqlDataAdapter(new SqlCommand(strSql, con, dbTransaction));
            SqlCommandBuilder oDbCmdBuilder = new SqlCommandBuilder(oDbAdapter);

            oDbAdapter.FillSchema(oDs, SchemaType.Source, "PR_LEAVE_POLICY");

            // Insert the Data
            oOrderRow = oDs.Tables["PR_LEAVE_POLICY"].NewRow();
            oOrderRow["CMP_BRANCH_ID"] = bId;
            oOrderRow["DSG_ID"] = gId;
            oOrderRow["DPT_ID"] = dptID;
            oOrderRow["PRLT_ID"] = LvTypId;
            oOrderRow["PRLP_MON_DAYS"] = TotLvMon;
            oOrderRow["PRLP_DAYS"] = TotLv;
            oOrderRow["PRLP_ALLOWANCE"] = LvAllwnce;
            oOrderRow["PRLP_ELIGIBLE_PERIOD"] = eligiblePeriod;
            oOrderRow["YR_ID"] = yrId;
            oOrderRow["TYP_CODE"] = eTypeId;
            oOrderRow["PRLP_IHCOUNT"] = IHC;
            oOrderRow["PRLP_CFORWARD"] = ICA;
            oOrderRow["PRLP_FREQUANCY_LEAVE"] = frequancyLeave;
            oOrderRow["PRLP_VALID_DAY"] = validTill;
            oOrderRow["NATURE_TYPE"] = natureType;
            oOrderRow["EMP_GENDER"] = "";
            oOrderRow["EMP_ID"] = empId;
            oDs.Tables["PR_LEAVE_POLICY"].Rows.Add(oOrderRow);
            oDbAdapter.Update(oDs, "PR_LEAVE_POLICY");
            dbTransaction.Commit();
            con.Close();
        }
        catch (Exception ex)
        {

            return null;
        }
        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }
        return "Saved Successfully.";
    }

    public string InsertLeavePolicy(string bId,
                             string gId,
                             string LvTypId,
                             double TotLvMon,
                             double TotLv,
                             double LvAllwnce,
                             string eligiblePeriod,
                             string yrId,
                             string eTypeId,
                             string IHC,
                             string ICA, string frequancyLeave, string validTill, string natureType, string dptID)
    {
        try
        {
            string strSql = "SELECT EMP_GENDER, DPT_ID, PRLT_ID, PRLP_MON_DAYS, PRLP_DAYS, PRLP_ALLOWANCE, PRLP_ELIGIBLE_PERIOD, CMP_BRANCH_ID,  "
                          + "DSG_ID,YR_ID, TYP_CODE, PRLP_IHCOUNT, PRLP_CFORWARD,PRLP_FREQUANCY_LEAVE,PRLP_VALID_DAY,NATURE_TYPE FROM PR_LEAVE_POLICY";
            DataRow oOrderRow;
            //DataRow oOrderRow1;
            con = new SqlConnection(cn);
            con.Open();
            dbTransaction = con.BeginTransaction();
            DataSet oDs = new DataSet();
            SqlDataAdapter oDbAdapter = new SqlDataAdapter(new SqlCommand(strSql, con, dbTransaction));
            SqlCommandBuilder oDbCmdBuilder = new SqlCommandBuilder(oDbAdapter);
            //oDbAdapter.MissingSchemaAction = MissingSchemaAction.AddWithKey;
            //oDbAdapter.Fill(oDs, "PR_LEAVE_POLICY");
            oDbAdapter.FillSchema(oDs, SchemaType.Source, "PR_LEAVE_POLICY");

            // Insert the Data
            oOrderRow = oDs.Tables["PR_LEAVE_POLICY"].NewRow();

            oOrderRow["CMP_BRANCH_ID"] = bId;
            oOrderRow["DSG_ID"] = gId;
            oOrderRow["DPT_ID"] = dptID;
            oOrderRow["PRLT_ID"] = LvTypId;
            oOrderRow["PRLP_MON_DAYS"] = TotLvMon;
            oOrderRow["PRLP_DAYS"] = TotLv;
            oOrderRow["PRLP_ALLOWANCE"] = LvAllwnce;
            oOrderRow["PRLP_ELIGIBLE_PERIOD"] = eligiblePeriod;
            oOrderRow["YR_ID"] = yrId;
            oOrderRow["TYP_CODE"] = eTypeId;
            oOrderRow["PRLP_IHCOUNT"] = IHC;
            oOrderRow["PRLP_CFORWARD"] = ICA;
            oOrderRow["PRLP_FREQUANCY_LEAVE"] = frequancyLeave;
            oOrderRow["PRLP_VALID_DAY"] = validTill;
            oOrderRow["NATURE_TYPE"] = natureType;
            oOrderRow["EMP_GENDER"] = "";


            oDs.Tables["PR_LEAVE_POLICY"].Rows.Add(oOrderRow);



            oDbAdapter.Update(oDs, "PR_LEAVE_POLICY");
            dbTransaction.Commit();
            con.Close();
        }
        catch (Exception ex)
        {

            return null;
        }
        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }
        return "Saved Successfully.";
    }


    public string InsertLeavePolicy(string bId,
                                     string gId,
                                     string LvTypId,
                                     double TotLvMon,
                                     double TotLv,
                                     double LvAllwnce,
                                     string eligiblePeriod,
                                     string yrId,
                                     string eTypeId,
                                     string IHC,
                                     string ICA, string frequancyLeave, string validTill, string natureType)
    {
        try
        {
            string strSql = "SELECT EMP_GENDER, PRLT_ID, PRLP_MON_DAYS, PRLP_DAYS, PRLP_ALLOWANCE, PRLP_ELIGIBLE_PERIOD, CMP_BRANCH_ID,  "
                          + "DSG_ID,YR_ID, TYP_CODE, PRLP_IHCOUNT, PRLP_CFORWARD,PRLP_FREQUANCY_LEAVE,PRLP_VALID_DAY,NATURE_TYPE FROM PR_LEAVE_POLICY";
            DataRow oOrderRow;
            //DataRow oOrderRow1;
            con = new SqlConnection(cn);
            con.Open();
            dbTransaction = con.BeginTransaction();
            DataSet oDs = new DataSet();
            SqlDataAdapter oDbAdapter = new SqlDataAdapter(new SqlCommand(strSql, con, dbTransaction));
            SqlCommandBuilder oDbCmdBuilder = new SqlCommandBuilder(oDbAdapter);
            //oDbAdapter.MissingSchemaAction = MissingSchemaAction.AddWithKey;
            //oDbAdapter.Fill(oDs, "PR_LEAVE_POLICY");
            oDbAdapter.FillSchema(oDs, SchemaType.Source, "PR_LEAVE_POLICY");

            // Insert the Data
            oOrderRow = oDs.Tables["PR_LEAVE_POLICY"].NewRow();

            oOrderRow["CMP_BRANCH_ID"] = bId;
            oOrderRow["DSG_ID"] = gId;
            oOrderRow["PRLT_ID"] = LvTypId;
            oOrderRow["PRLP_MON_DAYS"] = TotLvMon;
            oOrderRow["PRLP_DAYS"] = TotLv;
            oOrderRow["PRLP_ALLOWANCE"] = LvAllwnce;
            oOrderRow["PRLP_ELIGIBLE_PERIOD"] = eligiblePeriod;
            oOrderRow["YR_ID"] = yrId;
            oOrderRow["TYP_CODE"] = eTypeId;
            oOrderRow["PRLP_IHCOUNT"] = IHC;
            oOrderRow["PRLP_CFORWARD"] = ICA;
            oOrderRow["PRLP_FREQUANCY_LEAVE"] = frequancyLeave;
            oOrderRow["PRLP_VALID_DAY"] = validTill;
            oOrderRow["NATURE_TYPE"] = natureType;
            oOrderRow["EMP_GENDER"] = "";

            oDs.Tables["PR_LEAVE_POLICY"].Rows.Add(oOrderRow);



            oDbAdapter.Update(oDs, "PR_LEAVE_POLICY");
            dbTransaction.Commit();
            con.Close();
        }
        catch (Exception ex)
        {

            return null;
        }
        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }
        return "Saved Successfully.";
    }

    #endregion

    #region Monthly Payroll Sheet
    public string UpdateLateDeductionField(string pId)
    {
        string updateString;




        updateString = " UPDATE  PR_EMP_ATTENDENCE SET	APPLY_LATE_DEDUCT = 'Y'  WHERE  ATT_ID  IN "
          + "( "
             + " SELECT PA.ATT_ID "
              + "FROM  PR_EMP_ATTENDENCE PA, "
              + " PR_PAYROLL_MASTER PM  "
              + " WHERE	 PA.CMP_BRANCH_ID  = PM.CMP_BRANCH_ID "
              + "AND	CONVERT(DATETIME, CONVERT(VARCHAR(23), PA.ATT_DATE_TIME, 103), 103)  BETWEEN CONVERT(DATETIME, CONVERT(VARCHAR(23), PM.PRM_MONTH, 103), 103)  AND  CONVERT(DATETIME, CONVERT(VARCHAR(23), PM.PRM_MONTH_TO, 103), 103) "
              + "AND	PM.PRM_ID  = '" + pId + "' "
              + "AND	PA.ATT_TYPE  = 'IN' "
             + " AND	PA.ATT_LATE_STATUS  = 'YES' "
        + " )";



        string strReturn = "";
        try
        {
            con = new SqlConnection(cn);
            con.Open();
            dbTransaction = con.BeginTransaction();
            SqlCommand cmd = new SqlCommand(updateString, con, dbTransaction);
            cmd.Connection = con;
            cmd.ExecuteNonQuery();
            dbTransaction.Commit();
            // con.Close();
        }
        catch (Exception ex)
        {
            strReturn = ex.Message.ToString();
        }

        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }
        return strReturn;
    }
    #endregion


    #region Annual Performance Report

    public DataSet AnnPerfReport(string bID, string eID)
    {
        #region old
        // string strQuery = " SELECT EMP_NAME,EMP_JOINING_DATE,EMP_STATUS,(EMP_TITLE || ' ' || EMP_NAME) ENAME,EL.EMP_ID,EL.DSG_ID, "
        //+ " SET_CODE || ' - ' || DSG_TITLE AS DNAME,EMP_FATHER_NAME,EMP_MOTHER_NAME,EMP_NATIONAL_ID, "
        //+ " EMP_PASPORT_ID,EMP_CODE,EDUCATION.ACADEMIC,PROFESSIONAL.PROFQUA,EMP_BIRTHDAY,EMP_NATIONALITY, "
        //+ " EMP_RELIGION,EMP_CONFIR_DATE,EMP_FINAL_CONFIR_DATE, DP.DPT_SORT_NAME "

        //+ " FROM PR_EMPLOYEE_LIST EL,PR_DESIGNATION DE,CM_CMP_BRANCH CB, PR_DEPARTMENT DP, "

        //+ " (SELECT EDU.ACQ_DEGREE_TITLE||','||ACQ_CONCENTRATION||','||ACQ_INSTITUTE_NAME AS ACADEMIC,EDU.CMP_BRANCH_ID,EDU.EMP_ID "
        //+ " FROM (SELECT * FROM HR_EMP_JOB_ACADEMIC_QUA WHERE EMP_ID='" + eID + "' ORDER BY ACQ_ID DESC) EDU WHERE ROWNUM=1) EDUCATION, "

        //+ " (SELECT PROF.PRQ_CERTIFICATION||','||PROF.PRQ_INSTITUTE PROFQUA,PROF.EMP_ID,PROF.CMP_BRANCH_ID FROM "
        //+ " (SELECT * FROM HR_EMP_JOB_PROFESSIONAL_QUA WHERE EMP_ID='" + eID + "' ORDER BY PRQ_ID DESC) PROF WHERE ROWNUM=1) PROFESSIONAL "

        //+ " WHERE EL.DSG_ID=DE.DSG_ID AND EL.CMP_BRANCH_ID = CB.CMP_BRANCH_ID AND EDUCATION.EMP_ID(+)=EL.EMP_ID "
        //+ " AND PROFESSIONAL.EMP_ID(+)=EL.EMP_ID AND DP.DPT_ID=EL.DPT_ID "

        //+ " AND CB.CMP_BRANCH_ID ='" + bID + "' "
        //+ " AND EL.EMP_ID='" + eID + "' ";
        #endregion
        //ASAD
        string strQuery = @"  SELECT top 1 EMP_NAME,EMP_JOINING_DATE,EMP_STATUS,(EMP_TITLE + ' ' + EMP_NAME) ENAME,EL.EMP_ID,EL.DSG_ID, SET_CODE + ' - '  + DSG_TITLE AS DNAME, EMP_FATHER_NAME,EMP_MOTHER_NAME, EMP_NATIONAL_ID, EMP_PASPORT_ID, EMP_CODE, 
  EMP_BIRTHDAY, EMP_NATIONALITY, EMP_RELIGION, EMP_CONFIR_DATE, EMP_FINAL_CONFIR_DATE, DP.DPT_SORT_NAME, DP.DPT_NAME,EDUCATION.ACADEMIC ,PROFESSIONAL.PROFQUA  
  FROM  PR_EMPLOYEE_LIST EL
  left join  (SELECT EDU.ACQ_DEGREE_TITLE + ',' + ACQ_CONCENTRATION + ',' + ACQ_INSTITUTE_NAME AS ACADEMIC,  EDU.CMP_BRANCH_ID, EDU.EMP_ID FROM (SELECT top 1 * FROM  HR_EMP_JOB_ACADEMIC_QUA 
  WHERE EMP_ID  = '"+eID+"' order by ACQ_ID desc ) EDU 	) EDUCATION on EDUCATION.EMP_ID=El.EMP_ID " +
 " left join (SELECT PROF.PRQ_CERTIFICATION + ',' + PROF.PRQ_INSTITUTE PROFQUA, PROF.EMP_ID, PROF.CMP_BRANCH_ID FROM (SELECT top 1 * FROM  HR_EMP_JOB_PROFESSIONAL_QUA WHERE EMP_ID  = '"+eID+"'  ORDER BY PRQ_ID DESC  ) PROF  ) PROFESSIONAL on PROFESSIONAL.EMP_ID=EL.EMP_ID " + 
  
 " , PR_DESIGNATION DE,CM_CMP_BRANCH CB,PR_DEPARTMENT DP" +
 " WHERE EL.DSG_ID  = DE.DSG_ID AND	EL.CMP_BRANCH_ID  = CB.CMP_BRANCH_ID AND	DP.DPT_ID  = EL.DPT_ID  AND	CB.CMP_BRANCH_ID  = '" + bID + "' AND	EL.EMP_ID  = '" + eID + "' ";
        try
        {
            con = new SqlConnection(cn);
            con.Open();
            DataSet oDS = new DataSet();
            SqlDataAdapter odaData = new SqlDataAdapter(new SqlCommand(strQuery, con));
            odaData.Fill(oDS, "HR_ANNUAL");
            return oDS;
        }
        catch (Exception ex)
        {
            return null;
        }
        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }
    }

    public DataSet StartingSalary(string eID)
    {
        string strSal = "SELECT EMP_NAME, PREH_AMOUNT FROM PR_EMPLOYEE_HISTORY EH, PR_EMPLOYEE_LIST EL "
                         + " WHERE EH.EMP_ID=EL.EMP_ID AND PREH_AMOUNT_TYPE='S' AND  EH.EMP_ID='" + eID + "' ";

        try
        {
            con = new SqlConnection(cn);
            con.Open();
            DataSet oDS = new DataSet();
            SqlDataAdapter odaData = new SqlDataAdapter(new SqlCommand(strSal, con));
            odaData.Fill(oDS, "HR_SAL");
            return oDS;
        }
        catch (Exception ex)
        {
            return null;
        }

        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }
    }

    public DataSet Increment(string eID)
    {
        //string strInc = "SELECT * FROM (SELECT PREH_ID, EH.EMP_ID, EMP_NAME,PREH_AMOUNT,PREH_ASSIGNING_DATE FROM PR_EMPLOYEE_HISTORY EH, "
        //                + " PR_EMPLOYEE_LIST EL WHERE EH.EMP_ID=EL.EMP_ID "
        //                + " AND PREH_AMOUNT_TYPE='I' AND  EH.EMP_ID='" + eID + "' ORDER BY PREH_ID DESC) WHERE rownum<=5 ";

        string strInc = "SELECT TOP 5   PREH_ID,EH.EMP_ID,EMP_NAME, PREH_AMOUNT,PREH_ASSIGNING_DATE "
                      + "FROM   PR_EMPLOYEE_HISTORY EH, PR_EMPLOYEE_LIST EL "
                      + "WHERE       EH.EMP_ID = EL.EMP_ID "
                      + "AND PREH_AMOUNT_TYPE = 'I' "
                      + "AND EH.EMP_ID = '" + eID + "' "
                      + "ORDER BY PREH_ID DESC ";


        try
        {
            con = new SqlConnection(cn);
            con.Open();
            DataSet oDS = new DataSet();
            SqlDataAdapter odaData = new SqlDataAdapter(new SqlCommand(strInc, con));
            odaData.Fill(oDS, "HR_INC");
            return oDS;
        }
        catch (Exception ex)
        {
            return null;
        }

        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }
    }

    public DataSet Promotion(string eID)
    {
        //string strProm = "SELECT * FROM (SELECT PREH_ID, EH.EMP_ID, EMP_NAME,PREH_AMOUNT,PREH_ASSIGNING_DATE, PD.DSG_TITLE FROM PR_EMPLOYEE_HISTORY EH, "
        //                 + " PR_EMPLOYEE_LIST EL, PR_DESIGNATION PD WHERE EH.EMP_ID=EL.EMP_ID AND PD.DSG_ID=EL.DSG_ID "
        //                 + " AND PREH_AMOUNT_TYPE='P' AND  EH.EMP_ID='" + eID + "' ORDER BY PREH_ID DESC) WHERE rownum<=5 ";


        string strProm = "SELECT TOP 5 PREH_ID, EH.EMP_ID, EMP_NAME,PREH_AMOUNT,PREH_ASSIGNING_DATE, PD.DSG_TITLE FROM PR_EMPLOYEE_HISTORY EH, "
                         + " PR_EMPLOYEE_LIST EL, PR_DESIGNATION PD WHERE EH.EMP_ID=EL.EMP_ID AND PD.DSG_ID=EL.DSG_ID "
                         + " AND PREH_AMOUNT_TYPE='P' AND  EH.EMP_ID='" + eID + "' ORDER BY PREH_ID DESC ";



        try
        {
            con = new SqlConnection(cn);
            con.Open();
            DataSet oDS = new DataSet();
            SqlDataAdapter odaData = new SqlDataAdapter(new SqlCommand(strProm, con));
            odaData.Fill(oDS, "HR_PROM");
            return oDS;
        }
        catch (Exception ex)
        {
            return null;
        }

        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }
    }

    public DataSet PrevPostings(string eID)
    {
        string strPrev = "SELECT CAST(EMH_COMPANY_NAME AS VARCHAR) + ',' + CAST(EMH_COMPANY_LOCATION AS VARCHAR) PLACE_OF_POSTING,EMH_POSITION_HELD,EMH_FROM_DATE,EMH_TO_DATE "
                         + " FROM HR_EMP_JOB_EMPLOYMENT_HISTORY WHERE EMP_ID= '" + eID + "' ORDER BY EMH_ID  ";

        try
        {
            con = new SqlConnection(cn);
            con.Open();
            DataSet oDS = new DataSet();
            SqlDataAdapter odaData = new SqlDataAdapter(new SqlCommand(strPrev, con));
            odaData.Fill(oDS, "HR_PREV_POSTS");
            return oDS;
        }
        catch (Exception ex)
        {
            return null;
        }
        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }
    }

    public DataSet Transfer(string eID)
    {
        string strPrev = "SELECT NEW_CMP_BRANCH_ID, POSITION_HELD, DURATION_FROM, DURATIO_TO, CMP_BRANCH_NAME "
                          + "FROM HR_EMP_TRANSFER ET, CM_CMP_BRANCH CM WHERE EMP_ID= '" + eID + "' "
                          + "AND ET.NEW_CMP_BRANCH_ID=CM.CMP_BRANCH_ID order by TRANSFER_DATE ";

        try
        {
            con = new SqlConnection(cn);
            con.Open();
            DataSet oDS = new DataSet();
            SqlDataAdapter odaData = new SqlDataAdapter(new SqlCommand(strPrev, con));
            odaData.Fill(oDS, "HR_EMP_TRANSFER");
            return oDS;
        }
        catch (Exception ex)
        {
            return null;
        }
        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }
    }

    public DataSet BasicSal(string eID, string bID)
    {
        //string strBS = "SELECT PRST_AMT FROM PR_PAYROLL_DETAIL PD, PR_PAYROLL_MASTER PM, PR_PAYROLL_ITEM PI "
        //               + " WHERE PD.PRM_ID=PM.PRM_ID AND PD.PR_ITEM_ID= PI.PR_ITEM_ID AND "
        //               + " EMP_ID='" + eID + "' AND PI.PR_ITEM_IS_TAXABLE='Y' AND PI.PR_ITEM_ORDER='2' " 
        //               + " AND PM.PRM_ID = (SELECT PRM_ID FROM (SELECT PRM_ID,PRM_MONTH,PRM_DETAILS FROM PR_PAYROLL_MASTER "
        //               + " WHERE CMP_BRANCH_ID='" + bID + "' ORDER BY PRM_ID DESC) where rownum=1)  ";


        //asad
        string strBS = "SELECT   PRST_AMT "
                       + "FROM   PR_PAYROLL_DETAIL PD, PR_PAYROLL_MASTER PM, PR_PAYROLL_ITEM PI "
                       + "WHERE       PD.PRM_ID = PM.PRM_ID "
                       + "AND PD.PR_ITEM_ID = PI.PR_ITEM_ID "
                       + "AND EMP_ID = '" + eID + "' "
                       + "AND PI.PR_ITEM_IS_TAXABLE = 'Y' "
                       + "AND PI.PR_ITEM_ORDER = '2' "
                       + "AND PM.PRM_ID = (SELECT top 1 PRM_ID "
                       + "FROM   PR_PAYROLL_MASTER "
                       + "WHERE   CMP_BRANCH_ID ='" + bID + "' "
                       + "ORDER BY   PRM_ID DESC) ";

        try
        {
            con = new SqlConnection(cn);
            con.Open();
            DataSet oDS = new DataSet();
            SqlDataAdapter odaData = new SqlDataAdapter(new SqlCommand(strBS, con));
            odaData.Fill(oDS, "HR_BASIC_SAL");
            return oDS;
        }
        catch (Exception ex)
        {
            return null;
        }
        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }
    }

    public DataSet GrosSal(string eID, string bID)
    {
        string strGS = "SELECT PRST_AMT FROM PR_PAYROLL_DETAIL PD, PR_PAYROLL_MASTER PM, PR_PAYROLL_ITEM PI "
                       + " WHERE PD.PRM_ID=PM.PRM_ID AND PD.PR_ITEM_ID= PI.PR_ITEM_ID AND "
                       + " EMP_ID='" + eID + "' AND PI.PR_ITEM_ORDER='1' "
                       + " AND PM.PRM_ID = (SELECT TOP 1 [PRM_ID] FROM PR_PAYROLL_MASTER "
                       + " WHERE CMP_BRANCH_ID='" + bID + "' ORDER BY PRM_ID DESC)  ";

        try
        {
            con = new SqlConnection(cn);
            con.Open();
            DataSet oDS = new DataSet();
            SqlDataAdapter odaData = new SqlDataAdapter(new SqlCommand(strGS, con));
            odaData.Fill(oDS, "HR_GROSS_SAL");
            return oDS;
        }
        catch (Exception ex)
        {
            return null;
        }
        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }
    }

    public DataSet Target_Achvmnt(string eID)
    {
        string strTA = "SELECT EMP_ID,ENAME,DESIGNATION,DP_YEAR_AMOUNT,DP_MON_AMOUNT,INV_YEAR_AMOUNT,DP_YEAR_AMOUNT+INV_YEAR_AMOUNT COLLECTION, "
                       + " INV_MON_AMOUNT,DP_YEAR,INV_YEAR FROM HRM_ALL_EMP_PERFOM WHERE EMP_ID='" + eID + "'  ";

        try
        {
            con = new SqlConnection(cn);
            con.Open();
            DataSet oDS = new DataSet();
            SqlDataAdapter odaData = new SqlDataAdapter(new SqlCommand(strTA, con));
            odaData.Fill(oDS, "HR_TARGET_ACHV");
            return oDS;
        }
        catch (Exception ex)
        {
            return null;
        }

        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }
    }

    public DataSet Yearly_Achvmnt_Depo(string eID)
    {
        string strTD = "SELECT ISNULL(SUM(CONVERT(FLOAT, DEPO_AMT)), 0) TOT_DP_AMT FROM  HRM_ALL_YEARLY_DEPOSIT WHERE EMP_ID= '" + eID + "' ";

        try
        {
            con = new SqlConnection(cn);
            con.Open();
            DataSet oDS = new DataSet();
            SqlDataAdapter odaData = new SqlDataAdapter(new SqlCommand(strTD, con));
            odaData.Fill(oDS, "HR_ACHV_DEPO");
            return oDS;
        }
        catch (Exception ex)
        {
            return null;
        }
        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }
    }

    public DataSet Yearly_Achvmnt_Inv(string eID)
    {
        string strTI = "SELECT ISNULL(SUM(CONVERT(FLOAT, CONTRACT_AMOUNT)), 0) TOT_INV_AMT FROM  HRM_ALL_YEARLY_INVESTMENT  WHERE EMP_ID= '" + eID + "' ";

        try
        {
            con = new SqlConnection(cn);
            con.Open();
            DataSet oDS = new DataSet();
            SqlDataAdapter odaData = new SqlDataAdapter(new SqlCommand(strTI, con));
            odaData.Fill(oDS, "HR_ACHV_INV");
            return oDS;
        }
        catch (Exception ex)
        {
            return null;
        }
        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }
    }

    #endregion

    public bool ChkLeaveDate(string eId, string fDate)
    {
        string strSql;
        //strSql = "SELECT LVE_FROM_DATE FROM PR_LEAVE WHERE EMP_ID= '" + eId + "' "
        //       + "AND TO_DATE(TO_CHAR(LVE_FROM_DATE,'DD/MM/YYYY'),'DD/MM/YYYY') = TO_DATE('" + fDate + "', 'DD/MM/YYYY') ";
        strSql = "SELECT LVE_FROM_DATE FROM  PR_LEAVE  "
                + " WHERE	 EMP_ID  = '" + eId + "'  AND CONVERT(DATETIME, CONVERT(VARCHAR(23), LVE_FROM_DATE, 103), 103)  = CONVERT(DATETIME, '" + fDate + "', 103) ";
        try
        {
            //DataRow oOrderRow;
            con = new SqlConnection(cn);
            con.Open();
            DataSet oDS = new DataSet();
            SqlDataAdapter oOrdersDataAdapter = new SqlDataAdapter(new SqlCommand(strSql, con));
            SqlCommandBuilder oOrdersCmdBuilder = new SqlCommandBuilder(oOrdersDataAdapter);
            oOrdersDataAdapter.MissingSchemaAction = MissingSchemaAction.AddWithKey;
            oOrdersDataAdapter.Fill(oDS, "PR_EMP_ATTENDENCE");

            DataTable tbl_AD = oDS.Tables["PR_EMP_ATTENDENCE"];

            // already exists
            if (tbl_AD.Rows.Count > 0) { return true; }
            else { return false; }
        }
        catch (Exception ex)
        {
            return false;
        }
        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }
    }

    public bool ChkAttendance(string eId, string fDate)
    {
        string strSql;

        //strSql = "SELECT ATT_TYPE FROM PR_EMP_ATTENDENCE WHERE ATT_TYPE = 'IN' AND EMP_ID= '" + eId + "' "
        //       + "AND TO_DATE(TO_CHAR(ATT_DATE_TIME,'DD/MM/YYYY'),'DD/MM/YYYY') = TO_DATE('" + fDate + "', 'DD/MM/YYYY') ";

        //ASAD
        strSql = "SELECT ATT_TYPE FROM PR_EMP_ATTENDENCE WHERE ATT_TYPE = 'IN' AND EMP_ID= '" + eId + "' "
              + "AND CONVERT(DATETIME, CONVERT(VARCHAR(23), ATT_DATE_TIME, 103), 103) = CONVERT(DATETIME, '" + fDate + "', 103) ";


        try
        {
            //DataRow oOrderRow;
            con = new SqlConnection(cn);
            con.Open();
            DataSet oDS = new DataSet();
            SqlDataAdapter oOrdersDataAdapter = new SqlDataAdapter(new SqlCommand(strSql, con));
            SqlCommandBuilder oOrdersCmdBuilder = new SqlCommandBuilder(oOrdersDataAdapter);
            oOrdersDataAdapter.MissingSchemaAction = MissingSchemaAction.AddWithKey;
            oOrdersDataAdapter.Fill(oDS, "PR_EMP_ATTENDENCE");

            DataTable tbl_AD = oDS.Tables["PR_EMP_ATTENDENCE"];

            // already exists
            if (tbl_AD.Rows.Count > 0) { return true; }
            else { return false; }
        }
        catch (Exception ex)
        {
            return false;
        }

        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }
    }

    #region Employee Individual Performance Report
    //For Investment
    public DataSet EmpIndivReportInv(string asDate, string yId, string eId)
    {
        #region old
        //string strQuery = " SELECT CA.CI_CLIENT_ID,CI.CI_CLIENT_NAME,CA.APP_APPLICATION_ID,CA.REF1_EMP_ID,CI.STATUS,CA.TERM, "
        //                   + " CS.REPAY_MODE,CS.SANCTION_AMOUNT,CS.RATE,CD.DISBURS_AMT,EP.INV_YEAR_AMOUNT COLLECTABLE_AMOUNT, "
        //                   + " YI.CONTRACT_AMOUNT COLLECTED_AMT,LI.NO_OF_OVERDUE,LI.TOT_OVERDUE_AMT, "

        //                   + " EX.EXECUTION_DATE,EX.EXPIRE_DATE,APV.APV_APPROVED_AMT,APV.REPAY_MONTH,RD.REN_DUE, "
        //                   + " (EP.INV_YEAR_AMOUNT-YI.CONTRACT_AMOUNT) OVERDUE_AMT "

        //                   + " FROM ALL_EMP_PERFOM EP,ALL_YEARLY_INVESTMENT YI,CR_APPLICATION CA,CR_CLIENT_INFO CI, "
        //                   + " CA_DISBURSEMENT CD,CA_SANCTION CS,CR_FINANCE_MODE FM,LG_LETTER_ISSUE LI,CA_EXECUTION EX, "
        //                   + " CR_APPROVED APV,CA_FINANCIAL_CONTRACT FC, "

        //                   + " (SELECT nvl(COUNT (*),0) AS REN_DUE, ASE.STATUS, ASE.LA_NO,ASE.EXECUTION_ID "
        //                   + " FROM CA_AMORTIZATION_SCHEDULE ASE,CA_FINANCIAL_CONTRACT AG WHERE AG.LA_NO=ASE.LA_NO "
        //                   + " AND ASE.STATUS='D'GROUP BY ASE.STATUS, ASE.LA_NO,ASE.EXECUTION_ID) RD "

        //                   + " WHERE EP.EMP_ID=CA.REF1_EMP_ID AND YI.EMP_ID=CA.REF1_EMP_ID AND EP.EMP_ID=YI.EMP_ID AND "
        //                   + " CI.CMP_BRANCH_ID=EP.CMP_BRANCH_ID AND CA.CMP_BRANCH_ID=EP.CMP_BRANCH_ID AND "
        //                   + " CI.CMP_BRANCH_ID=CA.CMP_BRANCH_ID AND CA.CI_CLIENT_ID=CS.CI_CLIENT_ID AND "
        //                   + " CA.APP_APPLICATION_ID=CS.APPLICATION_ID AND CI.CI_CLIENT_ID=CS.CI_CLIENT_ID "

        //                   + " AND FC.CI_CLIENT_ID=CA.CI_CLIENT_ID AND APV.CI_CLIENT_ID=CA.CI_CLIENT_ID "
        //                   + " AND LI.CMP_BRANCH_ID=CA.CMP_BRANCH_ID  AND LI.CRM_CLIENT_ID=CA.CI_CLIENT_ID "
        //                   + " AND CD.LA_NO=YI.LA_NO AND CD.LA_NO=FC.LA_NO AND EX.LA_NO=FC.LA_NO  AND LI.EXECUTION_ID=EX.EXECUTION_ID "
        //                   + " AND CA.APP_APPLICATION_ID=APV.APP_APPLICATION_ID AND APV.APV_APPROVE_ID=CS.APPROVAL_ID "

        //                   + " AND CA.FM_FIN_MODE=FM.FM_FINANCE_ID  AND FC.APPLICATION_ID=CS.APPLICATION_ID "
        //                   + " AND RD.EXECUTION_ID=EX.EXECUTION_ID AND FC.LA_NO=RD.LA_NO AND YI.LA_NO=RD.LA_NO "
        //                   + " AND CA.REF1_EMP_ID='" + eId + "' AND EP.INV_YEAR ='" + yId + "' "
        //                   + " AND YI.CONTRACT_DATE BETWEEN GL_ACC_YEAR_SDATE AND TO_DATE('" + asDate + "', 'DD/MM/YYYY') ";
        #endregion

        //ASAD

        string strQuery = " SELECT CA.CI_CLIENT_ID, "
                        + " CI.CI_CLIENT_NAME, "
                        + " CA.APP_APPLICATION_ID,"
                        + " CA.REF1_EMP_ID,"
                        + " CI.STATUS,"
                        + " CA.TERM,"
                        + " CS.REPAY_MODE,"
                        + " CS.SANCTION_AMOUNT,"
                        + " CS.RATE,"
                        + " CD.DISBURS_AMT,"
                        + " EP.INV_YEAR_AMOUNT COLLECTABLE_AMOUNT,"
                        + " YI.CONTRACT_AMOUNT COLLECTED_AMT,"
                        + " LI.NO_OF_OVERDUE,"
                        + " LI.TOT_OVERDUE_AMT,"
                        + " EX.EXECUTION_DATE,"
                        + " EX.EXPIRE_DATE,"
                        + " APV.APV_APPROVED_AMT,"
                        + " APV.REPAY_MONTH,"
                        + " RD.REN_DUE,"
                        + " (EP.INV_YEAR_AMOUNT - YI.CONTRACT_AMOUNT) OVERDUE_AMT "
                        + " FROM  ALL_EMP_PERFOM EP,"
                        + "  ALL_YEARLY_INVESTMENT YI,"
                        + " CR_APPLICATION CA,"
                        + " CR_CLIENT_INFO CI,"
                        + " CA_DISBURSEMENT CD,"
                        + " CA_SANCTION CS,"
                        + "  CR_FINANCE_MODE FM,"
                        + "  LG_LETTER_ISSUE LI,"
                        + " CA_EXECUTION EX,"
                        + " CR_APPROVED APV,"
                        + " CA_FINANCIAL_CONTRACT FC,"
                        + " (	SELECT "
                        + "  ISNULL(COUNT(*), 0) AS REN_DUE,"
                        + "  ASE.STATUS,"
                        + "  ASE.LA_NO,"
                        + "  ASE.EXECUTION_ID"
                        + " FROM  CA_AMORTIZATION_SCHEDULE ASE,"
                        + "  CA_FINANCIAL_CONTRACT AG "
                        + " WHERE	 AG.LA_NO  = ASE.LA_NO"
                        + "  AND	ASE.STATUS  = 'D'"
                        + " GROUP BY ASE.STATUS,"
                        + "  ASE.LA_NO,"
                        + "  ASE.EXECUTION_ID"
                        + " 	) RD "
                        + " WHERE	 EP.EMP_ID  = CA.REF1_EMP_ID"
                        + " AND	YI.EMP_ID  = CA.REF1_EMP_ID"
                        + " AND	EP.EMP_ID  = YI.EMP_ID"
                        + " AND	CI.CMP_BRANCH_ID  = EP.CMP_BRANCH_ID"
                        + " AND	CA.CMP_BRANCH_ID  = EP.CMP_BRANCH_ID"
                        + " AND	CI.CMP_BRANCH_ID  = CA.CMP_BRANCH_ID"
                        + " AND	CA.CI_CLIENT_ID  = CS.CI_CLIENT_ID"
                        + " AND	CA.APP_APPLICATION_ID  = CS.APPLICATION_ID"
                        + " AND	CI.CI_CLIENT_ID  = CS.CI_CLIENT_ID"
                        + " AND	FC.CI_CLIENT_ID  = CA.CI_CLIENT_ID"
                        + " AND	APV.CI_CLIENT_ID  = CA.CI_CLIENT_ID"
                        + " AND	LI.CMP_BRANCH_ID  = CA.CMP_BRANCH_ID"
                        + " AND	LI.CRM_CLIENT_ID  = CA.CI_CLIENT_ID"
                        + " AND	CD.LA_NO  = YI.LA_NO"
                        + " AND	CD.LA_NO  = FC.LA_NO"
                        + " AND	EX.LA_NO  = FC.LA_NO"
                        + " AND	LI.EXECUTION_ID  = EX.EXECUTION_ID"
                        + " AND	CA.APP_APPLICATION_ID  = APV.APP_APPLICATION_ID"
                        + " AND	APV.APV_APPROVE_ID  = CS.APPROVAL_ID"
                        + " AND	CA.FM_FIN_MODE  = FM.FM_FINANCE_ID"
                        + " AND	FC.APPLICATION_ID  = CS.APPLICATION_ID"
                        + " AND	RD.EXECUTION_ID  = EX.EXECUTION_ID"
                        + " AND	FC.LA_NO  = RD.LA_NO"
                        + " AND	YI.LA_NO  = RD.LA_NO"
                        + " AND	CA.REF1_EMP_ID  = '" + eId + "'"
                        + " AND	EP.INV_YEAR  = '" + yId + "'"
                        + " AND	YI.CONTRACT_DATE  BETWEEN GL_ACC_YEAR_SDATE  AND  CONVERT(DATETIME, '" + asDate + "', 103)";


        try
        {
            con = new SqlConnection(cn);
            con.Open();
            DataSet oDS = new DataSet();
            SqlDataAdapter odaData = new SqlDataAdapter(new SqlCommand(strQuery, con));
            odaData.Fill(oDS, "EMP_INDIVIDUAL_PERF_INV");
            return oDS;
        }
        catch (Exception ex)
        {
            return null;
        }
        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }
    }

    //For Deposit

    public DataSet EmpIndivReportDepo(string asDate, string yId, string eId)
    {
        string strQuery = " SELECT DISTINCT FS.SRC_NAME,FD.FAVOUR,EP.ENAME,EP.DP_MON_AMOUNT, EP.DP_YEAR_AMOUNT COLLECTABLE_AMOUNT, "
                           + " YD.DEPO_AMT COLLECTED_AMT,FD.DEPO_ACC_ID,FD.DEPO_AMT,FD.TERM,PT.PT_NAME,LI.NO_OF_OVERDUE, "
                           + " LI.TOT_OVERDUE_AMT,EX.EXECUTION_DATE,EX.EXPIRE_DATE "
                           + " FROM ALL_EMP_PERFOM EP, ALL_YEARLY_DEPOSIT YD, FN_DEPOSIT FD, FN_SOURCE FS,FN_PRODUCT_TYPE PT, "
                           + " LG_LETTER_ISSUE LI,CA_EXECUTION EX "

                           + " WHERE EP.EMP_ID=FD.FAVOUR AND FD.FAVOUR='" + eId + "' "
                           + " AND YD.EMP_ID=FD.FAVOUR AND YD.EMP_ID=EP.EMP_ID AND EP.CMP_BRANCH_ID=FS.CMP_BRANCH_ID "
                           + " AND YD.CMP_BRANCH_ID=EP.CMP_BRANCH_ID AND YD.CMP_BRANCH_ID=FS.CMP_BRANCH_ID "
                           + " AND LI.CMP_BRANCH_ID=FS.CMP_BRANCH_ID AND LI.CMP_BRANCH_ID=EP.CMP_BRANCH_ID "

                           + " AND YD.CMP_BRANCH_ID=LI.CMP_BRANCH_ID AND PT.PT_ID=FD.PT_ID AND FD.FMOD_ID=PT.FMOD_ID "
                           + " AND EX.EXECUTION_ID=LI.EXECUTION_ID AND FS.FN_SOURCE_ID=FD.FN_SOURCE_ID  "
                           + " AND FD.SRC_TYPE_ID=FS.SRC_TYPE_ID AND YD.DEPO_ACC_ID=FD.DEPO_ACC_ID AND EP.DP_YEAR ='" + yId + "' "
                           + " AND YD.ACTIVE_DATE BETWEEN GL_ACC_YEAR_SDATE AND CONVERT(DATETIME, '" + asDate + "', 103)";


        try
        {
            con = new SqlConnection(cn);
            con.Open();
            DataSet oDS = new DataSet();
            SqlDataAdapter odaData = new SqlDataAdapter(new SqlCommand(strQuery, con));
            odaData.Fill(oDS, "EMP_INDIVIDUAL_PERF_DEPO");
            return oDS;
        }
        catch (Exception ex)
        {
            return null;
        }
        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }
    }

    #endregion


    public bool ChktypeOrder(string order, string item)
    {
        string strSql;

        strSql = "SELECT PR_ITEM_ID, PR_ITEM_IS_TAXABLE, PR_ITNATURE_CODE, ACC_INT_ID, CMP_BRANCH_ID, PR_ITEM_TITLE, PR_ITEM_ORDER "
               + "FROM PR_PAYROLL_ITEM "
               + "WHERE PR_ITEM_ORDER='" + order + "' OR PR_ITEM_TITLE='" + item + "'";


        try
        {
            //DataRow oOrderRow;
            con = new SqlConnection(cn);
            con.Open();
            DataSet oDS = new DataSet();
            SqlDataAdapter oOrdersDataAdapter = new SqlDataAdapter(new SqlCommand(strSql, con));
            SqlCommandBuilder oOrdersCmdBuilder = new SqlCommandBuilder(oOrdersDataAdapter);
            oOrdersDataAdapter.MissingSchemaAction = MissingSchemaAction.AddWithKey;
            oOrdersDataAdapter.Fill(oDS, "PR_PAYROLL_ITEM");

            DataTable tbl_AD = oDS.Tables["PR_PAYROLL_ITEM"];

            // already exists
            if (tbl_AD.Rows.Count > 0) { return true; }
            else { return false; }
        }
        catch (Exception ex)
        {
            return false;
        }
        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }
    }

    public bool ChktypeCode(string code)
    {
        string strSql;

        strSql = "SELECT TYP_TYPE, TYP_CODE from HR_EMP_TYPE WHERE TYP_CODE='" + code + "' ";

        try
        {
            //DataRow oOrderRow;
            con = new SqlConnection(cn);
            con.Open();
            DataSet oDS = new DataSet();
            SqlDataAdapter oOrdersDataAdapter = new SqlDataAdapter(new SqlCommand(strSql, con));
            SqlCommandBuilder oOrdersCmdBuilder = new SqlCommandBuilder(oOrdersDataAdapter);
            oOrdersDataAdapter.MissingSchemaAction = MissingSchemaAction.AddWithKey;
            oOrdersDataAdapter.Fill(oDS, "HR_EMP_TYPE");

            DataTable tbl_AD = oDS.Tables["HR_EMP_TYPE"];

            // already exists
            if (tbl_AD.Rows.Count > 0) { return true; }
            else { return false; }
        }
        catch (Exception ex)
        {
            return false;
        }

        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }
    }

    public bool ChkHRYear(string year, string bId)
    {
        string strSql;

        strSql = "SELECT YR_ID, YR_YEAR, CMP_BRANCH_ID from HR_YEAR WHERE YR_YEAR='" + year + "' AND CMP_BRANCH_ID='" + bId + "' ";

        try
        {
            //DataRow oOrderRow;
            con = new SqlConnection(cn);
            con.Open();
            DataSet oDS = new DataSet();
            SqlDataAdapter oOrdersDataAdapter = new SqlDataAdapter(new SqlCommand(strSql, con));
            SqlCommandBuilder oOrdersCmdBuilder = new SqlCommandBuilder(oOrdersDataAdapter);
            oOrdersDataAdapter.MissingSchemaAction = MissingSchemaAction.AddWithKey;
            oOrdersDataAdapter.Fill(oDS, "YR_YEAR");

            DataTable tbl_AD = oDS.Tables["YR_YEAR"];

            // already exists
            if (tbl_AD.Rows.Count > 0) { return true; }
            else { return false; }
        }
        catch (Exception ex)
        {
            return false;
        }
        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }
    }

    public bool EmpTargetList(string eID, string yearId, string targetType)
    {
        string strQuery = "SELECT * FROM HR_EMP_TARGET  "
                           + " WHERE EMP_ID= '" + eID + "' AND TER_TYPE ='" + targetType + "' AND GL_ACC_YEAR_ID = '" + yearId + "'";



        try
        {
            con = new SqlConnection(cn);
            con.Open();
            DataSet oDS = new DataSet();
            SqlDataAdapter odaData = new SqlDataAdapter(new SqlCommand(strQuery, con));
            odaData.Fill(oDS, "HR_EMP_TAR");

            DataTable tbl_AD = oDS.Tables["HR_EMP_TAR"];

            // already exists
            if (tbl_AD.Rows.Count > 0) { return true; }
            else { return false; }
        }
        catch (Exception ex)
        {
            return false;
        }
        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }
    }

    #region Give Attendance
    /** added By : Rezoana, 13-02-2013 ***/

    //public DataSet GetRosterTime(string desigID, string eID, string bID) // Get Roster Timing DesignationWise
    //{
    //    string strQuery = "SELECT RS.PRR_TIME_FROM,RS.PRR_TYPE,EMP_ID "
    //                      + " FROM PR_ROSTER_SETUP RS, PR_EMPLOYEE_LIST EL "
    //                      + " WHERE RS.DSG_ID=EL.DSG_ID AND RS.DSG_ID='" + desigID + "' "
    //                      + "AND EL.EMP_ID='" + eID + "' AND EL.CMP_BRANCH_ID='" + bID + "' ";

    //    try
    //    {
    //        SqlConnection
    //        DataSet oDS = new DataSet();
    //        SqlDataAdapter odaData = new SqlDataAdapter(new SqlCommand(strQuery, con));
    //        odaData.Fill(oDS, "PR_ROSTER_SETUP");
    //        return oDS;
    //    }
    //    catch (Exception ex)
    //    {
    //        return null;
    //    }
    //}

    public DataSet GetRosterTime(string desigID, string eID, string bID, int dayValue) // Get Roster Timing DesignationWise
    {
        string strQuery = "SELECT RS.PRR_TIME_FROM,RS.PRR_TYPE,EMP_ID,GRACE_TIME "
                          + " FROM PR_ROSTER_SETUP RS, PR_EMPLOYEE_LIST EL "
                          + " WHERE RS.DSG_ID=EL.DSG_ID AND RS.DSG_ID='" + desigID + "' "
                          + "AND EL.EMP_ID='" + eID + "' AND EL.CMP_BRANCH_ID='" + bID + "' AND '" + dayValue + "' BETWEEN RS.FROM_DAY AND RS.TO_DAY ";

        try
        {
            con = new SqlConnection(cn);
            con.Open();
            DataSet oDS = new DataSet();
            SqlDataAdapter odaData = new SqlDataAdapter(new SqlCommand(strQuery, con));
            odaData.Fill(oDS, "PR_ROSTER_SETUP");
            return oDS;
        }
        catch (Exception ex)
        {
            return null;
        }
        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }
    }

    public DataSet GetRosterTimeNew(string date, string eID, string bID, int dayValue) // Get Roster Timing DesignationWise
    {
        //string strQuery = "SELECT DISTINCT RS.PRR_TIME_FROM, PRS_START_DATE, PRR_TYPE, TO_DAY, FROM_DAY, RS.GRACE_TIME "
        //                  + "FROM PR_ROSTER_SETUP RS, PR_ROSTER_SEHEDULE RSE, PR_EMP_ROSTER ER "
        //                  + "WHERE RSE.PRR_ID=RS.PRR_ID AND ER.PRR_ID=RS.PRR_ID AND RSE.PR_ROSTER_ID=RSE.PR_ROSTER_ID AND "
        //                  + "PRS_START_DATE<= to_date('" + date + "') AND PRS_END_DATE >=to_date('" + date + "') AND ER.EMP_ID='" + eID + "' "
        //                  + "AND(SUN='" + dayValue + "' OR MON='" + dayValue + "' OR TUE='" + dayValue + "' OR WED='" + dayValue + "' "
        //                  + " OR THU='" + dayValue + "' OR FRI='" + dayValue + "' OR SAT='" + dayValue + "') ";

        //sayd
        string strQuery = "SELECT DISTINCT RS.PRR_TIME_FROM, PRS_START_DATE, PRR_TYPE, TO_DAY, FROM_DAY, RS.GRACE_TIME "
                          + "FROM PR_ROSTER_SETUP RS, PR_ROSTER_SEHEDULE RSE, PR_EMP_ROSTER ER "
                          + "WHERE RSE.PRR_ID=RS.PRR_ID AND ER.PRR_ID=RS.PRR_ID AND RSE.PR_ROSTER_ID=RSE.PR_ROSTER_ID AND "
                          + "PRS_START_DATE<= CONVERT(DATETIME, '" + date + "') AND PRS_END_DATE >= CONVERT(DATETIME, '" + date + "') AND ER.EMP_ID='" + eID + "' "
                          + "AND(SUN='" + dayValue + "' OR MON='" + dayValue + "' OR TUE='" + dayValue + "' OR WED='" + dayValue + "' "
                          + " OR THU='" + dayValue + "' OR FRI='" + dayValue + "' OR SAT='" + dayValue + "') ";
        try
        {
            con = new SqlConnection(cn);
            con.Open();
            DataSet oDS = new DataSet();
            SqlDataAdapter odaData = new SqlDataAdapter(new SqlCommand(strQuery, con));
            odaData.Fill(oDS, "PR_ROSTER_SETUP");
            return oDS;
        }
        catch (Exception ex)
        {
            return null;
        }
        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }
    }

    public DataSet GetRosterTimeNew(string date, string eID, string bID) /*  //ASAD DATED:24-07-14  */
    {

        string strQuery = " SELECT DISTINCT RS.PRR_TIME_FROM, RS.PRR_TIME_TO,PRR_TYPE,RS.GRACE_TIME "
                         + " FROM PR_ROSTER_SETUP RS,PR_EMP_ROSTER ER  "
                         + " WHERE RS.PRR_ID=ER.PRR_ID and convert(date, CONVERT(DATETIME, CONVERT(VARCHAR(23), ER.ROSTER_DATE)))  =  convert(date, CONVERT(DATETIME, CONVERT(VARCHAR(23), '" + date + "'))) "
                         + " AND ER.CMP_BRANCH_ID = '" + bID + "' AND ER.EMP_ID='" + eID + "' ";
        try
        {
            con = new SqlConnection(cn);
            con.Open();
            DataSet oDS = new DataSet();
            SqlDataAdapter odaData = new SqlDataAdapter(new SqlCommand(strQuery, con));
            odaData.Fill(oDS, "PR_ROSTER_SETUP");
            return oDS;
        }
        catch (Exception ex)
        {
            return null;
        }
        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }
    }
    public DataSet GetRosterTimeN(string desigID, string eID, string bID, int dayValue) // Get Roster Timing DesignationWise
    {
        string strQuery = "SELECT RS.PRR_TIME_FROM,RS.PRR_TYPE,EMP_ID,GRACE_TIME "
                          + " FROM PR_ROSTER_SETUP RS, PR_EMPLOYEE_LIST EL "
                          + " WHERE RS.DSG_ID=EL.DSG_ID AND RS.DSG_ID='" + desigID + "' "
                          + "AND EL.EMP_ID='" + eID + "' AND EL.CMP_BRANCH_ID='" + bID + "' AND '" + dayValue + "' BETWEEN RS.FROM_DAY AND RS.TO_DAY ";

        try
        {
            con = new SqlConnection(cn);
            con.Open();
            DataSet oDS = new DataSet();
            SqlDataAdapter odaData = new SqlDataAdapter(new SqlCommand(strQuery, con));
            odaData.Fill(oDS, "PR_ROSTER_SETUP");
            return oDS;
        }
        catch (Exception ex)
        {
            return null;
        }
        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }
    }

    public bool PreviousAttendence(string eID)
    {
        //string strQuery = "SELECT * FROM "
        //                  + " (SELECT * FROM PR_EMP_ATTENDENCE "
        //                  + " WHERE EMP_ID='" + eID + "' AND ATT_TYPE='IN' "
        //                  + " ORDER BY ATT_DATE_TIME DESC) WHERE  rownum<=2 ";

        string strQuery = "SELECT TOP 2[ATT_ID] as id,* FROM  PR_EMP_ATTENDENCE pr WHERE pr.EMP_ID = '" + eID + "' AND pr.ATT_TYPE = 'IN' order by pr.ATT_ID desc";

        bool present = false;
        try
        {
            con = new SqlConnection(cn);
            con.Open();
            DataSet oDS = new DataSet();
            SqlDataAdapter odaData = new SqlDataAdapter(new SqlCommand(strQuery, con));
            odaData.Fill(oDS, "PR_EMP_ATTENDENCE");

            DataTable tbl_AD = oDS.Tables["PR_EMP_ATTENDENCE"];
            if (tbl_AD.Rows.Count > 0)
            {
                // already exists
                if ((tbl_AD.Rows[0][3].Equals("YES")) && (tbl_AD.Rows[1][3].Equals("YES")))
                {
                    present = true;
                    return present;
                }
                else
                {
                    return present;
                }
            }
            return present;
        }
        catch (Exception ex)
        {
            return false;
        }
        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }
    }

    #endregion

    #region Advance Return Plan
    public DataSet GetClaimAdvance(string bId, string eId, string cId)
    {
        string strQuery = "SELECT CL_AMOUNT, DURATION, DURATION_TYPE, EFFECT_DATE FROM HR_CLAIM_LIST WHERE EMP_ID = '" + eId + "' AND CMP_BRANCH_ID='" + bId + "' AND CL_ID='" + cId + "'";

        try
        {
            con = new SqlConnection(cn);
            con.Open();
            DataSet oDS = new DataSet();
            SqlDataAdapter odaData = new SqlDataAdapter(new SqlCommand(strQuery, con));
            odaData.Fill(oDS, "HR_CLAIM_LIST");
            return oDS;
        }
        catch (Exception ex)
        {
            return null;
        }
        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }
    }

    public DataSet SearClaimSchedule(string cId)
    {


        try
        {
            con = new SqlConnection(cn);
            con.Open();
            DataSet oDS = new DataSet();
            SqlDataAdapter oOrdersDataAdapter = new SqlDataAdapter(new SqlCommand(" select  * FROM HR_CLAIM_SCHEDULE WHERE CL_ID='" + cId + "'", con));
            SqlCommandBuilder oOrdersCmdBuilder = new SqlCommandBuilder(oOrdersDataAdapter);
            oOrdersDataAdapter.Fill(oDS, "SERCH_HR_CLAIM_SCHEDULE");
            return oDS;
        }
        catch (Exception ex)
        {

            return null;

        }
        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }

    }

    public string InsertClaimSchedule(string strEmployeeID, string strClaimID, DateTime dtpPayDate,
                                      string ClaimAmt, string BegPin_Amt, string Int_Amt, string Pri_Amt,
                                      string Rental_Amt, string BlnPin_Amt, string CumInm_Amt)
    {
        string strSql;
        strSql = "SELECT EMP_ID, PAYMENT_DATE, RETURN_AMOUNT, CL_ID, CLAIM_AMOUNT, BEGINING_PRINCIPAL, INTERERST_AMT, "
                 + " PRINCIPAL_AMT, BALANCE_PRINCIPAL, CUMULATIVE_INCOME FROM HR_CLAIM_SCHEDULE ";


        try
        {
            DataRow oOrderRow;

            con = new SqlConnection(cn);
            con.Open();
            dbTransaction = con.BeginTransaction();
            DataSet oDS = new DataSet();
            SqlDataAdapter oOrdersDataAdapter = new SqlDataAdapter(new SqlCommand(strSql, con, dbTransaction));
            SqlCommandBuilder oOrdersCmdBuilder = new SqlCommandBuilder(oOrdersDataAdapter);
            oOrdersDataAdapter.FillSchema(oDS, SchemaType.Source);

            DataTable dtSchedule = oDS.Tables["Table"];
            dtSchedule.TableName = "HR_CLAIM_SCHEDULE";

            oOrderRow = oDS.Tables["HR_CLAIM_SCHEDULE"].NewRow();

            oOrderRow["EMP_ID"] = strEmployeeID;
            oOrderRow["CL_ID"] = strClaimID;
            oOrderRow["PAYMENT_DATE"] = dtpPayDate;
            oOrderRow["CLAIM_AMOUNT"] = double.Parse(ClaimAmt);
            oOrderRow["BEGINING_PRINCIPAL"] = double.Parse(BegPin_Amt);
            oOrderRow["INTERERST_AMT"] = double.Parse(Int_Amt);
            oOrderRow["PRINCIPAL_AMT"] = double.Parse(Pri_Amt);
            oOrderRow["RETURN_AMOUNT"] = double.Parse(Rental_Amt);
            oOrderRow["BALANCE_PRINCIPAL"] = double.Parse(BlnPin_Amt);
            oOrderRow["CUMULATIVE_INCOME"] = double.Parse(CumInm_Amt);

            oDS.Tables["HR_CLAIM_SCHEDULE"].Rows.Add(oOrderRow);
            oOrdersDataAdapter.Update(oDS, "HR_CLAIM_SCHEDULE");
            dbTransaction.Commit();
            //con.Close();
            return "Saved Successfully.";
        }
        catch (Exception ex)
        {
            return ex.Message.ToString();
        }
        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }
    }

    public string InsertClaimAdvance(string strBranchID, string strEmployeeID, string strClaimID, string strAdvanceAmt,
                                     string dtpFirstDate, string strReturnAmt, string strScheduleType, string strInteresRate,
                                     string PaymentType, string paymentPeriod, string PaymentMode, string Duration, string DurationType)
    {
        string strSql;
        strSql = "SELECT EMP_ID, CMP_BRANCH_ID, ADV_AMOUNT, CL_ID, RETURN_AMOUNT, SCHEDULE_TYPE, "
                + "FIRST_PAY_DATE, INTEREST_RATE, PAYMENT_TYPE, PAYMENT_PERIOD, PAYMENT_MODE, "
                + "TOT_DURATION, DURATION_TYPE FROM HR_CLAIM_ADVANCE_LIST ";


        try
        {
            DataRow oOrderRow;
            con = new SqlConnection(cn);
            con.Open();
            dbTransaction = con.BeginTransaction();
            DataSet oDS = new DataSet();
            SqlDataAdapter oOrdersDataAdapter = new SqlDataAdapter(new SqlCommand(strSql, con, dbTransaction));
            SqlCommandBuilder oOrdersCmdBuilder = new SqlCommandBuilder(oOrdersDataAdapter);
            oOrdersDataAdapter.FillSchema(oDS, SchemaType.Source);

            DataTable dtSchedule = oDS.Tables["Table"];
            dtSchedule.TableName = "HR_CLAIM_ADVANCE_LIST";

            oOrderRow = oDS.Tables["HR_CLAIM_ADVANCE_LIST"].NewRow();

            oOrderRow["CMP_BRANCH_ID"] = strBranchID;
            oOrderRow["EMP_ID"] = strEmployeeID;
            oOrderRow["CL_ID"] = strClaimID;
            oOrderRow["ADV_AMOUNT"] = double.Parse(strAdvanceAmt);
            oOrderRow["FIRST_PAY_DATE"] = Convert.ToDateTime(dtpFirstDate);
            oOrderRow["RETURN_AMOUNT"] = double.Parse(strReturnAmt);
            oOrderRow["SCHEDULE_TYPE"] = strScheduleType;
            oOrderRow["INTEREST_RATE"] = strInteresRate;
            oOrderRow["PAYMENT_TYPE"] = PaymentType;
            oOrderRow["PAYMENT_PERIOD"] = paymentPeriod;
            oOrderRow["PAYMENT_MODE"] = PaymentMode;
            oOrderRow["TOT_DURATION"] = Duration;
            oOrderRow["DURATION_TYPE"] = DurationType;

            oDS.Tables["HR_CLAIM_ADVANCE_LIST"].Rows.Add(oOrderRow);
            oOrdersDataAdapter.Update(oDS, "HR_CLAIM_ADVANCE_LIST");
            dbTransaction.Commit();
            //con.Close();
            return "Saved Successfully.";
        }
        catch (Exception ex)
        {
            return ex.Message.ToString();
        }

        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }
    }

    #endregion


    #region Performance Analysis

    public string InsertPerformance(string BRANCH_ID, string EmployeeID, string DeptID, string joiningDate, string EVALUATION_DATE, string REVIEW_DATE, string AUTHORITY_COMMENTS, string AREA_OF_WEAK, string AREA_OF_STRENGTH, string evaAuthority)
    {


        try
        {
            string strSql = "SELECT CMP_BRANCH_ID,EMP_ID,DPT_ID,JOINING_DATE,EVALUATION_DATE,REVIEW_DATE,EVALUATION_AUTHORITY,AUTHORITY_COMMENTS,AREA_OF_WEAK,AREA_OF_STRENGTH FROM HR_EMPLOYEE_PERFORMANCE";
            DataRow oOrderRow;
            //DataRow oOrderRow1;

            con = new SqlConnection(cn);
            con.Open();
            dbTransaction = con.BeginTransaction();
            DataSet oDs = new DataSet();
            SqlDataAdapter oDbAdapter = new SqlDataAdapter(new SqlCommand(strSql, con, dbTransaction));
            SqlCommandBuilder oDbCmdBuilder = new SqlCommandBuilder(oDbAdapter);
            //oDbAdapter.MissingSchemaAction = MissingSchemaAction.AddWithKey;
            //oDbAdapter.Fill(oDs, "HR_EMPLOYEE_PERFORMANCE");
            oDbAdapter.FillSchema(oDs, SchemaType.Source, "HR_EMPLOYEE_PERFORMANCE");

            // Insert the Data
            oOrderRow = oDs.Tables["HR_EMPLOYEE_PERFORMANCE"].NewRow();

            oOrderRow["EMP_ID"] = EmployeeID;
            oOrderRow["DPT_ID"] = DeptID;
            oOrderRow["EVALUATION_AUTHORITY"] = evaAuthority;
            oOrderRow["AREA_OF_STRENGTH"] = AREA_OF_STRENGTH;
            oOrderRow["AUTHORITY_COMMENTS"] = AUTHORITY_COMMENTS;
            oOrderRow["AREA_OF_WEAK"] = AREA_OF_WEAK;
            oOrderRow["JOINING_DATE"] = Convert.ToDateTime(joiningDate);
            oOrderRow["EVALUATION_DATE"] = Convert.ToDateTime(EVALUATION_DATE);
            oOrderRow["REVIEW_DATE"] = Convert.ToDateTime(REVIEW_DATE);
            oOrderRow["CMP_BRANCH_ID"] = BRANCH_ID;

            //oOrderRow1 = oDs.Tables["PR_STRUCTURE"].NewRow();
            //oOrderRow1 = oOrderRow;
            oDs.Tables["HR_EMPLOYEE_PERFORMANCE"].Rows.Add(oOrderRow);

            //oOrderRow1["GENDER"] = "F";
            //oDs.Tables["PR_STRUCTURE"].Rows.Add(oOrderRow1);

            oDbAdapter.Update(oDs, "HR_EMPLOYEE_PERFORMANCE");
            dbTransaction.Commit();
            //con.Close();
        }
        catch (Exception ex)
        {

            return null;
        }

        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }

        return "Success";
    }

    public string InsertRatingScore(string performanceID, string RatingSLNo, string RatingScore)
    {


        try
        {
            string strSql = "SELECT  PERFORMANCE_ID,SLNO,SCORE FROM HR_RATING_SCORE";
            DataRow oOrderRow;

            con = new SqlConnection(cn);
            con.Open();
            dbTransaction = con.BeginTransaction();
            DataSet oDs = new DataSet();
            SqlDataAdapter oDbAdapter = new SqlDataAdapter(new SqlCommand(strSql, con, dbTransaction));
            SqlCommandBuilder oDbCmdBuilder = new SqlCommandBuilder(oDbAdapter);
            //oDbAdapter.MissingSchemaAction = MissingSchemaAction.AddWithKey;
            //oDbAdapter.Fill(oDs, "HR_RATING_SCORE");
            oDbAdapter.FillSchema(oDs, SchemaType.Source, "HR_RATING_SCORE");
            // Insert the Data
            oOrderRow = oDs.Tables["HR_RATING_SCORE"].NewRow();
            oOrderRow["PERFORMANCE_ID"] = performanceID;
            oOrderRow["SLNO"] = RatingSLNo;
            oOrderRow["SCORE"] = RatingScore;
            oDs.Tables["HR_RATING_SCORE"].Rows.Add(oOrderRow);
            oDbAdapter.Update(oDs, "HR_RATING_SCORE");
            dbTransaction.Commit();
            //con.Close();
        }
        catch (Exception ex)
        {

            return null;
        }

        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }
        return "Success";


    }

    // GetPerformanceID
    public string GetPerformancID()
    {
        string DivPerformanceID;
        string strSql = "SELECT LTRIM(RTRIM(dbo.GET_NEW_HRPerformance_ID(0))) PERFORMANCE_ID "; //"{call PKG_ERP_PRIMARY_KEY.GET_NEW_DIVMaster_ID({0})}"

        try
        {
            con = new SqlConnection(cn);
            con.Open();
            DataSet oDs = new DataSet();
            SqlDataAdapter oOrdersDataAdapter = new SqlDataAdapter(new SqlCommand(strSql, con));
            SqlCommandBuilder oOrdersCmdBuilder = new SqlCommandBuilder(oOrdersDataAdapter);
            oOrdersDataAdapter.Fill(oDs, "DUAL");
            DivPerformanceID = oDs.Tables[0].Rows[0]["PERFORMANCE_ID"].ToString();
            return DivPerformanceID;
        }
        catch (Exception ex)
        {

            return ex.Message.ToString();
        }
        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }
    }

    //getRating Info
    public DataSet GetRatingInfo()
    {


        SqlDataAdapter oOrdersDataAdapter = new SqlDataAdapter(new SqlCommand("SELECT RATING_SLNO FROM HR_PERFORMANCE_RATING ", con));

        try
        {
            con = new SqlConnection(cn);
            con.Open();
            DataSet oDS = new DataSet();
            oOrdersDataAdapter.Fill(oDS, "HR_PERFORMANCE_RATING");
            return oDS;
        }
        catch (Exception e)
        {
            return null;
        }
        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }


    }

    public DataSet GetEmpInfo(string eId)
    {
        string strQuery = "SELECT EMP_JOINING_DATE, EL.DPT_ID "
                        + "FROM PR_EMPLOYEE_LIST EL, PR_DEPARTMENT DT "
                        + "WHERE DT.DPT_ID=EL.DPT_ID AND EMP_ID='" + eId + "'";

        try
        {
            con = new SqlConnection(cn);
            con.Open();
            DataSet oDS = new DataSet();
            SqlDataAdapter odaData = new SqlDataAdapter(new SqlCommand(strQuery, con));
            odaData.Fill(oDS, "EMPLOYEE_INFO");
            return oDS;
        }
        catch (Exception ex)
        {
            return null;
        }
        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }
    }

    public DataSet GetEmpReviewDate(string eId)
    {
        string strQuery = "SELECT MAX(EVALUATION_DATE) EDATE "
                        + "FROM HR_EMPLOYEE_PERFORMANCE "
                        + "WHERE EMP_ID='" + eId + "'";



        try
        {
            con = new SqlConnection(cn);
            con.Open();
            DataSet oDS = new DataSet();
            SqlDataAdapter odaData = new SqlDataAdapter(new SqlCommand(strQuery, con));
            odaData.Fill(oDS, "EMPLOYEE_LAST_REVIEW");
            return oDS;
        }
        catch (Exception ex)
        {
            return null;
        }
        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }
    }

    #endregion

    public string InsertClaimReq(string BRANCH_ID, string EMPL_ID, string claimType, string claAmt,  //*** added by : Niaz Morshed, 29-Apr-2013, Decription : Insert Claim Request ***/
                                 string payMode, string claimPourpes, string EffDate, string Duration, string DurationType)
    {


        try
        {
            string strSql = "SELECT CMP_BRANCH_ID, EMP_ID, CT_ID, CL_AMOUNT, CT_PAY_TYPE, CL_PURPOSE, EFFECT_DATE, DURATION, DURATION_TYPE FROM HR_CLAIM_LIST ";
            DataRow oOrderRow;
            //DataRow oOrderRow1;

            con = new SqlConnection(cn);
            con.Open();
            dbTransaction = con.BeginTransaction();
            DataSet oDs = new DataSet();
            SqlDataAdapter oDbAdapter = new SqlDataAdapter(new SqlCommand(strSql, con, dbTransaction));
            SqlCommandBuilder oDbCmdBuilder = new SqlCommandBuilder(oDbAdapter);
            //oDbAdapter.MissingSchemaAction = MissingSchemaAction.AddWithKey;
            //oDbAdapter.Fill(oDs, "HCR_TREATMENT");
            oDbAdapter.FillSchema(oDs, SchemaType.Source, "HR_CLAIM_LIST");
            oOrderRow = oDs.Tables["HR_CLAIM_LIST"].NewRow();

            oOrderRow["CMP_BRANCH_ID"] = BRANCH_ID;
            oOrderRow["EMP_ID"] = EMPL_ID;
            oOrderRow["CT_ID"] = claimType;
            oOrderRow["CL_AMOUNT"] = Convert.ToDouble(claAmt);
            oOrderRow["CT_PAY_TYPE"] = payMode;
            oOrderRow["CL_PURPOSE"] = claimPourpes;
            oOrderRow["EFFECT_DATE"] = EffDate;
            oOrderRow["DURATION"] = Duration;
            oOrderRow["DURATION_TYPE"] = DurationType;

            oDs.Tables["HR_CLAIM_LIST"].Rows.Add(oOrderRow);
            oDbAdapter.Update(oDs, "HR_CLAIM_LIST");
            dbTransaction.Commit();
            //con.Close();
        }
        catch (Exception ex)
        {

            return null;
        }
        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }
        return "Success";
    }

    public DataSet GetClaimSch(string empId, string clmId)
    {
        string strSQL;

        strSQL = "SELECT TOP 1 [SCHEDULE_ID]AS ID,* FROM HR_CLAIM_SCHEDULE WHERE EMP_ID='" + empId + "' AND CL_ID='" + clmId + "' ORDER By PAYMENT_DATE DESC";

        try
        {
            con = new SqlConnection(cn);
            con.Open();
            DataSet oDS = new DataSet();
            // Create the DataTable "Orders" in the Dataset and the OrdersDataAdapter
            SqlDataAdapter oOrdersDataAdapter = new SqlDataAdapter(new SqlCommand(strSQL, con));
            SqlCommandBuilder oOrdersCmdBuilder = new SqlCommandBuilder(oOrdersDataAdapter);
            oOrdersDataAdapter.Fill(oDS, "GET_HR_CLAIM_SCHEDULE");
            return oDS;
        }
        catch (Exception ex)
        {

            return null;
        }
        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }
    }

    public string InsertClaimShColumn(string strEmployeeID, string strClaimID, string strReturnAmt,
                                    string dtpPayDate, string ClaimAmt)
    {
        string strSql;
        strSql = "SELECT EMP_ID, CL_ID, RETURN_AMOUNT, PAYMENT_DATE, CLAIM_AMOUNT FROM HR_CLAIM_SCHEDULE ";



        try
        {
            DataRow oOrderRow;

            con = new SqlConnection(cn);
            con.Open();
            dbTransaction = con.BeginTransaction();
            DataSet oDS = new DataSet();
            SqlDataAdapter oOrdersDataAdapter = new SqlDataAdapter(new SqlCommand(strSql, con, dbTransaction));
            SqlCommandBuilder oOrdersCmdBuilder = new SqlCommandBuilder(oOrdersDataAdapter);
            oOrdersDataAdapter.FillSchema(oDS, SchemaType.Source);

            DataTable dtAddSchedule = oDS.Tables["Table"];
            dtAddSchedule.TableName = "HR_CLAIM_SCHEDULE";

            oOrderRow = oDS.Tables["HR_CLAIM_SCHEDULE"].NewRow();

            oOrderRow["EMP_ID"] = strEmployeeID;
            oOrderRow["CL_ID"] = strClaimID;
            oOrderRow["RETURN_AMOUNT"] = double.Parse(strReturnAmt);
            oOrderRow["PAYMENT_DATE"] = Convert.ToDateTime(dtpPayDate);
            oOrderRow["CLAIM_AMOUNT"] = double.Parse(ClaimAmt);


            oDS.Tables["HR_CLAIM_SCHEDULE"].Rows.Add(oOrderRow);
            oOrdersDataAdapter.Update(oDS, "HR_CLAIM_SCHEDULE");
            dbTransaction.Commit();
            //con.Close();
            return "Saved Successfully.";
        }
        catch (Exception ex)
        {
            return ex.Message.ToString();
        }
        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }
    }

    public string InsertRosterT(string branch, string rosType, string rosterName,
                                   string fromTime, string toTime, string grace, string dayMaxHour, string dayMinHour, string monthMaxHour,
                                   string monthMinHour, string overtimeRate, string CalMode, string details, string sun, string mon, string tue, string wed, string thu, string fri, string sat)
    {
        string strSql;
        strSql = "SELECT CMP_BRANCH_ID, OVERTIME_REGULAR, PRR_TYPE, PRR_TIME_FROM, PRR_TIME_TO, GRACE_TIME, CALCULATION_MODE, "
                 + "DAY_MAX_HOUR, DAY_MIN_HOUR, MONTH_MAX_HOUR, MONTH_MIN_HOUR, OVERTIME_RATE, FROM_DAY, TO_DAY, PRR_DETAILS, SUN, MON, TUE, WED, THU, FRI, SAT FROM PR_ROSTER_SETUP ";
        try
        {
            DataRow oOrderRow;

            con = new SqlConnection(cn);
            con.Open();
            dbTransaction = con.BeginTransaction();
            DataSet oDS = new DataSet();
            SqlDataAdapter oOrdersDataAdapter = new SqlDataAdapter(new SqlCommand(strSql, con, dbTransaction));
            SqlCommandBuilder oOrdersCmdBuilder = new SqlCommandBuilder(oOrdersDataAdapter);
            oOrdersDataAdapter.FillSchema(oDS, SchemaType.Source);

            DataTable dtAddSchedule = oDS.Tables["Table"];
            dtAddSchedule.TableName = "PR_ROSTER_SETUP";

            oOrderRow = oDS.Tables["PR_ROSTER_SETUP"].NewRow();

            oOrderRow["CMP_BRANCH_ID"] = branch;
            oOrderRow["OVERTIME_REGULAR"] = rosType;
            oOrderRow["PRR_TYPE"] = rosterName;
            oOrderRow["PRR_TIME_FROM"] = fromTime;
            oOrderRow["PRR_TIME_TO"] = toTime;
            oOrderRow["GRACE_TIME"] = grace == "" ? "0" : grace;
            oOrderRow["DAY_MAX_HOUR"] = dayMaxHour;
            oOrderRow["DAY_MIN_HOUR"] = dayMinHour;
            oOrderRow["MONTH_MAX_HOUR"] = monthMaxHour;
            oOrderRow["MONTH_MIN_HOUR"] = monthMinHour;
            oOrderRow["OVERTIME_RATE"] = overtimeRate;
            oOrderRow["CALCULATION_MODE"] = CalMode;
            oOrderRow["PRR_DETAILS"] = details;
            oOrderRow["SUN"] = sun;
            oOrderRow["MON"] = mon;
            oOrderRow["TUE"] = tue;
            oOrderRow["WED"] = wed;
            oOrderRow["THU"] = thu;
            oOrderRow["FRI"] = fri;
            oOrderRow["SAT"] = sat;


            oDS.Tables["PR_ROSTER_SETUP"].Rows.Add(oOrderRow);
            oOrdersDataAdapter.Update(oDS, "PR_ROSTER_SETUP");
            dbTransaction.Commit();
            con.Close();
            return "Saved Successfully.";
        }
        catch (Exception ex)
        {
            return ex.Message.ToString();
        }
        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }
    }

    //public string InsertRosterOverTime(string branch, string grade, string rosType,
    //                              string rosterName, string dayMaxHour, string dayMinHour,
    //                              string monthMaxHour, string monthMinHour, string overtimeRate,
    //                              string details, string fromday, string todat)
    //{
    //    string strSql;
    //    strSql = "SELECT CMP_BRANCH_ID, DSG_ID, OVERTIME_REGULAR, PRR_TYPE, DAY_MAX_HOUR, DAY_MIN_HOUR, MONTH_MAX_HOUR, MONTH_MIN_HOUR, OVERTIME_RATE, PRR_DETAILS, TO_DAY, FROM_DAY FROM PR_ROSTER_SETUP ";
    //    try
    //    {
    //        DataRow oOrderRow;
    //        SqlConnection
    //        con.Open();
    //        dbTransaction = con.BeginTransaction();
    //        DataSet oDS = new DataSet();
    //        SqlDataAdapter oOrdersDataAdapter = new SqlDataAdapter(new SqlCommand(strSql, con, dbTransaction));
    //        SqlCommandBuilder oOrdersCmdBuilder = new SqlCommandBuilder(oOrdersDataAdapter);
    //        oOrdersDataAdapter.FillSchema(oDS, SchemaType.Source);

    //        DataTable dtAddSchedule = oDS.Tables["Table"];
    //        dtAddSchedule.TableName = "PR_ROSTER_SETUP";

    //        oOrderRow = oDS.Tables["PR_ROSTER_SETUP"].NewRow();

    //        oOrderRow["CMP_BRANCH_ID"] = branch;
    //        oOrderRow["DSG_ID"] = grade;
    //        oOrderRow["OVERTIME_REGULAR"] = rosType;
    //        oOrderRow["PRR_TYPE"] = rosterName;
    //        oOrderRow["DAY_MAX_HOUR"] = dayMaxHour;
    //        oOrderRow["DAY_MIN_HOUR"] = dayMinHour;
    //        oOrderRow["MONTH_MAX_HOUR"] = monthMaxHour;
    //        oOrderRow["MONTH_MIN_HOUR"] = monthMinHour;
    //        oOrderRow["OVERTIME_RATE"] = overtimeRate;
    //        oOrderRow["PRR_DETAILS"] = details;
    //        oOrderRow["FROM_DAY"] = overtimeRate;
    //        oOrderRow["TO_DAY"] = details;


    //        oDS.Tables["PR_ROSTER_SETUP"].Rows.Add(oOrderRow);
    //        oOrdersDataAdapter.Update(oDS, "PR_ROSTER_SETUP");
    //        dbTransaction.Commit();
    //        con.Close();
    //        return "Saved Successfully.";
    //    }
    //    catch (Exception ex)
    //    {
    //        return ex.Message.ToString();
    //    }
    //}

    public DataSet GetClaimAmt(string empId, string clmId)
    {
        string strSQL;

        strSQL = "SELECT CL_AMOUNT, sum(RETURN_AMOUNT) RENEW_AMT FROM HR_CLAIM_LIST CL, HR_CLAIM_SCHEDULE CS WHERE CL.CL_ID=CS.CL_ID AND "
               + "CLAIM_STATUS='R' AND CT_PAY_TYPE='A' AND CS.EMP_ID='" + empId + "' AND CS.CL_ID='" + clmId + "' GROUP BY CL_AMOUNT ";

        try
        {
            con = new SqlConnection(cn);
            con.Open();
            DataSet oDS = new DataSet();
            // Create the DataTable "Orders" in the Dataset and the OrdersDataAdapter
            SqlDataAdapter oOrdersDataAdapter = new SqlDataAdapter(new SqlCommand(strSQL, con));
            SqlCommandBuilder oOrdersCmdBuilder = new SqlCommandBuilder(oOrdersDataAdapter);
            oOrdersDataAdapter.Fill(oDS, "HR_CLAIM_LIST_AMT");
            return oDS;
        }
        catch (Exception ex)
        {

            return null;
        }
        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }
    }
    public string InsertAssetUser(string branch, string assetType, string assetName,
                                 string user, string details, string DptId)  /* niaz */
    {
        string strSql;
        strSql = "SELECT CMP_BRANCH_ID, ASSET_TYPE_ID, ASSET_ID, EMP_ID, AT_DETAILS, DPT_ID FROM HR_ASSET_USER ";



        try
        {
            DataRow oOrderRow;

            con = new SqlConnection(cn);
            con.Open();
            dbTransaction = con.BeginTransaction();
            DataSet oDS = new DataSet();
            SqlDataAdapter oOrdersDataAdapter = new SqlDataAdapter(new SqlCommand(strSql, con, dbTransaction));
            SqlCommandBuilder oOrdersCmdBuilder = new SqlCommandBuilder(oOrdersDataAdapter);
            oOrdersDataAdapter.FillSchema(oDS, SchemaType.Source);

            DataTable dtAddSchedule = oDS.Tables["Table"];
            dtAddSchedule.TableName = "HR_ASSET_USER";

            oOrderRow = oDS.Tables["HR_ASSET_USER"].NewRow();

            oOrderRow["CMP_BRANCH_ID"] = branch;
            oOrderRow["ASSET_TYPE_ID"] = assetType;
            oOrderRow["ASSET_ID"] = assetName;
            oOrderRow["EMP_ID"] = user;
            oOrderRow["AT_DETAILS"] = details;
            oOrderRow["DPT_ID"] = DptId;


            oDS.Tables["HR_ASSET_USER"].Rows.Add(oOrderRow);
            oOrdersDataAdapter.Update(oDS, "HR_ASSET_USER");
            dbTransaction.Commit();
            //con.Close();
            return "Saved Successfully.";
        }
        catch (Exception ex)
        {
            return ex.Message.ToString();
        }
        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }
    }



    public DataSet EmpMaster(string bId)
    {
        //string strQuery = "SELECT EL.EMP_ID, EL.EMP_CODE, (EMP_TITLE || ' ' || EMP_NAME) ENAME, EL.EMP_PRE_ADDRES, "
        //                + "EL.EMP_JOINING_DATE, DP.DPT_NAME, "
        //                + "EL.EMP_CONFIR_DATE, EL.EMP_FINAL_CONFIR_DATE, EHIS.PREH_ASSIGNING_DATE, "
        //                + "CB.CMP_BRANCH_NAME, DP.DPT_NAME, SET_CODE || ' - ' || DSG_TITLE AS DNAME, SET_CODE, ET.TYP_TYPE, EHIS.PREH_ASSIGNING_DATE "
        //                + "FROM PR_EMPLOYEE_LIST EL, CM_CMP_BRANCH CB, PR_DEPARTMENT DP, PR_DESIGNATION DE, HR_EMP_TYPE ET, "
        //                + "(SELECT PREH_ASSIGNING_DATE, EMP_ID FROM (SELECT PREH_ASSIGNING_DATE, EI.EMP_ID "
        //                + "FROM PR_EMPLOYEE_HISTORY EI, PR_EMPLOYEE_LIST EM WHERE EI.CMP_BRANCH_ID=EM.CMP_BRANCH_ID "
        //                + "AND EM.EMP_ID=EI.EMP_ID AND  PREH_AMOUNT_TYPE='P' AND EI.CMP_BRANCH_ID='" + bId + "' "
        //                + "ORDER BY  PREH_ASSIGNING_DATE DESC) where rownum<2 ) EHIS "
        //                + "WHERE EL.CMP_BRANCH_ID=CB.CMP_BRANCH_ID AND EL.DPT_ID=DP.DPT_ID AND EL.DSG_ID=DE.DSG_ID AND EL.EMP_STATUS=ET.TYP_CODE "
        //                + "AND EL.EMP_ID=EHIS.EMP_ID(+) AND CB.CMP_BRANCH_ID='" + bId + "' "
        //                + "GROUP BY EL.EMP_ID, EMP_CODE, EMP_TITLE, EMP_NAME, EMP_PRE_ADDRES, EMP_JOINING_DATE, SET_CODE, DSG_TITLE, "
        //                + "EMP_FINAL_CONFIR_DATE, EMP_CONFIR_DATE, CMP_BRANCH_NAME, SET_LEVEL, "
        //                + "EHIS.PREH_ASSIGNING_DATE, DP.DPT_NAME, EL.EMP_STATUS, ET.TYP_TYPE ORDER BY to_number(SET_LEVEL), DE.SET_CODE ASC ";

        //ASAD

        string strQuery = " SELECT"
                            + "EL.EMP_ID, EL.EMP_CODE, (CAST(EMP_TITLE AS VARCHAR) + ' ' + CAST(EMP_NAME AS VARCHAR)), EL.EMP_PRE_ADDRES, EL.EMP_JOINING_DATE, "
                            + "DP.DPT_NAME,EL.EMP_CONFIR_DATE, EL.EMP_FINAL_CONFIR_DATE, EHIS.PREH_ASSIGNING_DATE, CB.CMP_BRANCH_NAME, DP.DPT_NAME, CAST(SET_CODE AS VARCHAR) + ' - ' + CAST(DSG_TITLE AS VARCHAR) AS DNAME,SET_CODE,ET.TYP_TYPE, EHIS.PREH_ASSIGNING_DATE "
                            + "FROM  PR_EMPLOYEE_LIST EL  LEFT OUTER JOIN (	SELECT TOP 1 "
                            + "[PREH_ID] AS ID, "
                            + "PREH_ASSIGNING_DATE, "
                            + "EI.EMP_ID "
                            + "FROM  PR_EMPLOYEE_HISTORY EI, "
                            + " PR_EMPLOYEE_LIST EM  "
                            + "WHERE	 EI.CMP_BRANCH_ID  = EM.CMP_BRANCH_ID "
                            + " AND	EM.EMP_ID  = EI.EMP_ID "
                            + "AND	PREH_AMOUNT_TYPE  = 'P' "
                            + "AND	EI.CMP_BRANCH_ID  = '" + bId + " ' "
                            + "ORDER BY PREH_ASSIGNING_DATE DESC  "
                            + ") EHIS  ON  EL.EMP_ID  = EHIS.EMP_ID , "
                            + "CM_CMP_BRANCH CB, "
                            + " PR_DEPARTMENT DP, "
                            + "PR_DESIGNATION DE, "
                            + "HR_EMP_TYPE ET  "
                            + "WHERE	 EL.CMP_BRANCH_ID  = CB.CMP_BRANCH_ID "
                            + "AND	EL.DPT_ID  = DP.DPT_ID "
                            + "AND	EL.DSG_ID  = DE.DSG_ID "
                            + "AND	EL.EMP_STATUS  = ET.TYP_CODE "
                            + " AND	CB.CMP_BRANCH_ID  = '" + bId + " ' "
                            + "GROUP BY EL.EMP_ID, EMP_CODE,EMP_TITLE, EMP_NAME, EMP_PRE_ADDRES,EMP_JOINING_DATE,SET_CODE,DSG_TITLE, "
                            + "EMP_FINAL_CONFIR_DATE, EMP_CONFIR_DATE, CMP_BRANCH_NAME,SET_LEVEL,EHIS.PREH_ASSIGNING_DATE, DP.DPT_NAME, EL.EMP_STATUS,ET.TYP_TYPE "
                            + "ORDER BY CONVERT(NUMERIC(8, 2), SET_LEVEL), "
                            + " DE.SET_CODE ASC  ";

        try
        {
            con = new SqlConnection(cn);
            con.Open();
            DataSet oDS = new DataSet();
            SqlDataAdapter odaData = new SqlDataAdapter(new SqlCommand(strQuery, con));
            odaData.Fill(oDS, "PR_EMPLOYEE_LIST");
            return oDS;
        }
        catch (Exception ex)
        {
            return null;
        }
        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }
    }

    public DataSet GetPayrollItemList()
    {
        string strQuery = "SELECT PR_ITEM_TITLE, PR_ITEM_ID FROM PR_PAYROLL_ITEM ";


        try
        {
            con = new SqlConnection(cn);
            con.Open();
            DataSet oDS = new DataSet();
            SqlDataAdapter odaData = new SqlDataAdapter(new SqlCommand(strQuery, con));
            odaData.Fill(oDS, "PR_PAYROLL_ITEM");
            return oDS;
        }
        catch (Exception ex)
        {
            return null;
        }
        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }
    }

    public DataSet GetPayrollAmt(string EmpId, string pMonth, string pItem)
    {
        string strQuery = "SELECT ISNULL(PRST_AMT, 0) PA_AMOUNT FROM  PR_PAYROLL_DETAIL PD,  PR_PAYROLL_ITEM PI  "
                          + "WHERE PI.PR_ITEM_ID=PD.PR_ITEM_ID AND PRM_ID='" + pMonth + "' "
                          + "AND PI.PR_ITEM_ID ='" + pItem + "' AND PD.EMP_ID='" + EmpId + "' ";


        try
        {
            con = new SqlConnection(cn);
            con.Open();
            DataSet oDS = new DataSet();
            SqlDataAdapter odaData = new SqlDataAdapter(new SqlCommand(strQuery, con));
            odaData.Fill(oDS, "PR_PAYROLL_AMT");
            return oDS;
        }
        catch (Exception ex)
        {
            return null;
        }

        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }
    }

    #region Employee Assessment
    public DataSet GetEmp(string empID, string dId, string assCode, string bID)
    {
        string strQuery = "SELECT   DESTINATION_EMP_ID, DESTINATION_DSG_ID "
                        + "FROM   CM_WORKFLOW_SETUP WS "
                        + "WHERE  (SOURCE_EMP_ID = '" + empID + "' OR SOURCE_DSG_ID='" + dId + "') "
                        + "AND ITEM_SET_CODE='" + assCode + "' AND MAIN_TYPE_ID='04' AND CMP_BRANCH_ID='" + bID + "' ";
        var con = new SqlConnection(cn);
        try
        {
            DataSet oDS = new DataSet();
            SqlDataAdapter odaData = new SqlDataAdapter(new SqlCommand(strQuery, con));
            odaData.Fill(oDS, "EMPLOYEE_RECOMMEND_INFO");
            return oDS;
        }
        catch (Exception ex)
        {
            return null;
        }
        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }
    }

    public DataSet GetEmpForLineManager(string empID, string dId, string assCode, string bID)
    {
        string strQuery = "SELECT distinct   DESTINATION_EMP_ID, DESTINATION_DSG_ID "
                        + "FROM   CM_WORKFLOW_SETUP WS,HR_ASSESSMENT_EMPLOYEE_MASTER as EM "
                        + "WHERE em.EMP_ID=ws.DESTINATION_EMP_ID and em.STATUS='LM' and  (SOURCE_EMP_ID = '" + empID + "' OR SOURCE_DSG_ID='" + dId + "') "
                        + "AND ITEM_SET_CODE='" + assCode + "' AND MAIN_TYPE_ID='04' AND ws.CMP_BRANCH_ID='" + bID + "' ";
        var con = new SqlConnection(cn);
        try
        {
            DataSet oDS = new DataSet();
            SqlDataAdapter odaData = new SqlDataAdapter(new SqlCommand(strQuery, con));
            odaData.Fill(oDS, "EMPLOYEE_RECOMMEND_INFO");
            return oDS;
        }
        catch (Exception ex)
        {
            return null;
        }
        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }
    }

    public DataSet GetEmpForDptHead(string empID, string dId, string assCode, string bID)
    {
        string strQuery = "SELECT distinct  DESTINATION_EMP_ID, DESTINATION_DSG_ID "
                        + "FROM   CM_WORKFLOW_SETUP WS,HR_ASSESSMENT_EMPLOYEE_MASTER as EM "
                        + "WHERE em.EMP_ID=ws.DESTINATION_EMP_ID and em.STATUS='DH' and  (SOURCE_EMP_ID = '" + empID + "' OR SOURCE_DSG_ID='" + dId + "') "
                        + "AND ITEM_SET_CODE='" + assCode + "' AND MAIN_TYPE_ID='04' AND ws.CMP_BRANCH_ID='" + bID + "' ";
        var con = new SqlConnection(cn);
        try
        {
            DataSet oDS = new DataSet();
            SqlDataAdapter odaData = new SqlDataAdapter(new SqlCommand(strQuery, con));
            odaData.Fill(oDS, "EMPLOYEE_RECOMMEND_INFO");
            return oDS;
        }
        catch (Exception ex)
        {
            return null;
        }
        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }
    }
    public string InsertAssessmentType(string BRANCH_ID, string TypeCode, string TypeName, string Nature, string Details)
    {
        string strSql;
        strSql = "SELECT ASSIS_TYPE_NAME, ASSIS_TYPE_CODE, ASSIS_NATURE, CMP_BRANCH_ID, ASSIS_DETAILS FROM HR_ASSESSMENT_TYPE ";


        try
        {
            DataRow oOrderRow;

            con = new SqlConnection(cn);
            con.Open();
            dbTransaction = con.BeginTransaction();
            DataSet oDS = new DataSet();
            SqlDataAdapter oOrdersDataAdapter = new SqlDataAdapter(new SqlCommand(strSql, con, dbTransaction));
            SqlCommandBuilder oOrdersCmdBuilder = new SqlCommandBuilder(oOrdersDataAdapter);
            oOrdersDataAdapter.FillSchema(oDS, SchemaType.Source);

            DataTable dtSchedule = oDS.Tables["Table"];
            dtSchedule.TableName = "HR_ASSESSMENT_TYPE";

            oOrderRow = oDS.Tables["HR_ASSESSMENT_TYPE"].NewRow();

            oOrderRow["CMP_BRANCH_ID"] = BRANCH_ID;
            oOrderRow["ASSIS_TYPE_NAME"] = TypeName;
            oOrderRow["ASSIS_TYPE_CODE"] = TypeCode;
            oOrderRow["ASSIS_NATURE"] = Nature;
            oOrderRow["ASSIS_DETAILS"] = Details;



            oDS.Tables["HR_ASSESSMENT_TYPE"].Rows.Add(oOrderRow);
            oOrdersDataAdapter.Update(oDS, "HR_ASSESSMENT_TYPE");
            dbTransaction.Commit();
            //con.Close();
            return "Saved Successfully.";
        }
        catch (Exception ex)
        {
            return ex.Message.ToString();
        }
        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }
    }
    public string InsertAssessmentType(string BRANCH_ID, string TypeCode, string TypeName, string Nature, string EmployeNature, string comStatus, string Details)
    {
        string strSql;
        strSql = "SELECT ASSIS_TYPE_NAME, ASSIS_TYPE_CODE, ASSIS_NATURE, CMP_BRANCH_ID, ASSIS_DETAILS, COMMENT_STATUS, ASSIS_EMP_NATURE FROM HR_ASSESSMENT_TYPE ";

        var con = new SqlConnection(cn);
        try
        {
            DataRow oOrderRow;

            con.Open();
            dbTransaction = con.BeginTransaction();
            DataSet oDS = new DataSet();
            SqlDataAdapter oOrdersDataAdapter = new SqlDataAdapter(new SqlCommand(strSql, con, dbTransaction));
            SqlCommandBuilder oOrdersCmdBuilder = new SqlCommandBuilder(oOrdersDataAdapter);
            oOrdersDataAdapter.FillSchema(oDS, SchemaType.Source);

            DataTable dtSchedule = oDS.Tables["Table"];
            dtSchedule.TableName = "HR_ASSESSMENT_TYPE";

            oOrderRow = oDS.Tables["HR_ASSESSMENT_TYPE"].NewRow();

            oOrderRow["CMP_BRANCH_ID"] = BRANCH_ID;
            oOrderRow["ASSIS_TYPE_NAME"] = TypeName;
            oOrderRow["ASSIS_TYPE_CODE"] = TypeCode;
            oOrderRow["ASSIS_NATURE"] = Nature;
            oOrderRow["ASSIS_EMP_NATURE"] = EmployeNature;
            oOrderRow["COMMENT_STATUS"] = comStatus;
            oOrderRow["ASSIS_DETAILS"] = Details;



            oDS.Tables["HR_ASSESSMENT_TYPE"].Rows.Add(oOrderRow);
            oOrdersDataAdapter.Update(oDS, "HR_ASSESSMENT_TYPE");
            dbTransaction.Commit();
            //con.Close();
            return "Saved Successfully.";
        }
        catch (Exception ex)
        {
            return ex.Message.ToString();
        }
        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }
    }
    public string InsertAssPerson(string BRANCH_ID, string AssNo, string AssPerson, string DegId, string EmpId, string Details)
    {
        string strSql;
        strSql = "SELECT CMP_BRANCH_ID, ASSIS_PERSON_NUMBER, PERSON, DSG_ID, EMP_ID, ASSIS_PERSON_DETAILS FROM HR_ASSESSMENT_PERSON ";


        try
        {
            DataRow oOrderRow;

            con = new SqlConnection(cn);
            con.Open();
            dbTransaction = con.BeginTransaction();
            DataSet oDS = new DataSet();
            SqlDataAdapter oOrdersDataAdapter = new SqlDataAdapter(new SqlCommand(strSql, con, dbTransaction));
            SqlCommandBuilder oOrdersCmdBuilder = new SqlCommandBuilder(oOrdersDataAdapter);
            oOrdersDataAdapter.FillSchema(oDS, SchemaType.Source);

            DataTable dtSchedule = oDS.Tables["Table"];
            dtSchedule.TableName = "HR_ASSESSMENT_PERSON";

            oOrderRow = oDS.Tables["HR_ASSESSMENT_PERSON"].NewRow();

            oOrderRow["CMP_BRANCH_ID"] = BRANCH_ID;
            oOrderRow["ASSIS_PERSON_NUMBER"] = AssNo;
            oOrderRow["PERSON"] = AssPerson;
            oOrderRow["DSG_ID"] = DegId;
            oOrderRow["EMP_ID"] = EmpId;
            oOrderRow["ASSIS_PERSON_DETAILS"] = Details;



            oDS.Tables["HR_ASSESSMENT_PERSON"].Rows.Add(oOrderRow);
            oOrdersDataAdapter.Update(oDS, "HR_ASSESSMENT_PERSON");
            dbTransaction.Commit();
            //con.Close();
            return "Saved Successfully.";
        }
        catch (Exception ex)
        {
            return ex.Message.ToString();
        }
        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }
    }

    public string InsertProductCategory(string ItemCode,
                                           string ItemName,
                                           string ItemType,
                                           string ItemCategory,
                                           string ItemDesc,
                                           string cmpBranch,
                                           string AssessType)
    {


        try
        {
            string strSql = "SELECT ASS_ITEM_NAME, ASS_ITEM_PARENT_CODE, ASS_ITEM_SET_CODE, "
                                + "ASS_ITEM_DESCRIPTION, ASS_ITEM_TYPE, CMP_BRANCH_ID, ASSIS_TYPE_ID FROM HR_ASSESSMENT_ITEM";

            DataRow oOrderRow;

            con = new SqlConnection(cn);
            con.Open();
            dbTransaction = con.BeginTransaction();
            DataSet oDs = new DataSet();
            SqlDataAdapter oDbAdapter = new SqlDataAdapter(new SqlCommand(strSql, con, dbTransaction));
            SqlCommandBuilder oDbCmdBuilder = new SqlCommandBuilder(oDbAdapter);
            //oDbAdapter.MissingSchemaAction = MissingSchemaAction.AddWithKey;
            oDbAdapter.FillSchema(oDs, SchemaType.Source, "HR_ASSESSMENT_ITEM");

            #region Insert Data
            oOrderRow = oDs.Tables["HR_ASSESSMENT_ITEM"].NewRow();

            //7 fields
            oOrderRow["ASS_ITEM_SET_CODE"] = ItemCode;
            oOrderRow["ASS_ITEM_NAME"] = ItemName;
            oOrderRow["ASS_ITEM_TYPE"] = ItemType;
            oOrderRow["ASS_ITEM_PARENT_CODE"] = ItemCategory;

            oOrderRow["ASS_ITEM_DESCRIPTION"] = ItemDesc;
            oOrderRow["CMP_BRANCH_ID"] = cmpBranch;
            oOrderRow["ASSIS_TYPE_ID"] = AssessType;

            oDs.Tables["HR_ASSESSMENT_ITEM"].Rows.Add(oOrderRow);
            oDbAdapter.Update(oDs, "HR_ASSESSMENT_ITEM");
            dbTransaction.Commit();
            //con.Close();
            #endregion
        }
        catch (Exception ex)
        {

            return "Err:" + ex.Message.ToString();
        }
        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }

        return "Success";
    }

    public DataSet GetCategoryItem(string strType,
                                          string strBranchID,
                                          string AssTypeId,
                                          string strParentCode)
    {
        //string strQuery = "SELECT CMP_BRANCH_ID, ASSIS_TYPE_ID, ASS_ITEM_PARENT_CODE, ASS_ITEM_TYPE, ASS_ITEM_ID, ASS_ITEM_SET_CODE, "
        //                + "DECODE(ASS_ITEM_SET_CODE,'', ASS_ITEM_NAME, '  ' || ASS_ITEM_SET_CODE || ' - ' || ASS_ITEM_NAME) PNAME "
        //                + "FROM HR_ASSESSMENT_ITEM "
        //                + "WHERE ASS_ITEM_TYPE='" + strType + "' AND ASS_ITEM_PARENT_CODE='" + strParentCode + "' "
        //                + "AND CMP_BRANCH_ID = '" + strBranchID + "' AND ASSIS_TYPE_ID = '" + AssTypeId + "' "
        //                + "ORDER BY CMP_BRANCH_ID, ASS_ITEM_SET_CODE";


        //asad
        string strQuery = "SELECT CMP_BRANCH_ID, ASSIS_TYPE_ID, ASS_ITEM_PARENT_CODE, ASS_ITEM_TYPE, ASS_ITEM_ID, ASS_ITEM_SET_CODE, "
                            + "CASE  "
                                + "WHEN ASS_ITEM_SET_CODE IS NULL THEN ASS_ITEM_NAME  "
                                + "ELSE '  ' + CAST(ASS_ITEM_SET_CODE AS VARCHAR) + ' - ' + CAST(ASS_ITEM_NAME AS VARCHAR) "
                            + "END PNAME "
                            + "FROM  HR_ASSESSMENT_ITEM "
                            + "WHERE ASS_ITEM_TYPE='" + strType + "' AND ASS_ITEM_PARENT_CODE='" + strParentCode + "' "
                            + "AND CMP_BRANCH_ID = '" + strBranchID + "' AND ASSIS_TYPE_ID = '" + AssTypeId + "' "
                            + "ORDER BY CMP_BRANCH_ID, ASS_ITEM_SET_CODE";

        try
        {
            con = new SqlConnection(cn);
            con.Open();
            DataSet oDS = new DataSet();
            SqlDataAdapter odaData = new SqlDataAdapter(new SqlCommand(strQuery, con));
            odaData.Fill(oDS, "PARENT_LIST");
            return oDS;
        }
        catch (Exception ex)
        {
            return null;
        }
        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }
    }

    public DataSet GetChildCategoryItem(string strType,
                                          string strBranchID,
                                          string AssTypeId,
                                          string strParentCode)
    {
        //string strQuery = "SELECT CMP_BRANCH_ID, ASSIS_TYPE_ID, ASS_ITEM_PARENT_CODE, ASS_ITEM_TYPE, ASS_ITEM_ID, ASS_ITEM_SET_CODE, "
        //                + "DECODE(ASS_ITEM_SET_CODE,'', ASS_ITEM_NAME, '  ' || ASS_ITEM_SET_CODE || ' - ' || ASS_ITEM_NAME) PNAME "
        //                + "FROM HR_ASSESSMENT_ITEM "
        //                + "WHERE ASS_ITEM_TYPE='" + strType + "' AND ASS_ITEM_PARENT_CODE='" + strParentCode + "' "
        //                + "AND CMP_BRANCH_ID = '" + strBranchID + "' AND ASSIS_TYPE_ID = '" + AssTypeId + "' "
        //                + "ORDER BY CMP_BRANCH_ID, ASS_ITEM_SET_CODE";


        //asad
        string strQuery = " SELECT CMP_BRANCH_ID,ASSIS_TYPE_ID, "
        + "ASS_ITEM_PARENT_CODE, "
        + "ASS_ITEM_TYPE, "
        + "ASS_ITEM_ID, "
        + "ASS_ITEM_SET_CODE, "
        + "CASE  "
            + "WHEN ASS_ITEM_SET_CODE IS NULL THEN ASS_ITEM_NAME "
            + "ELSE '  ' + CAST(ASS_ITEM_SET_CODE AS VARCHAR) + ' - ' + CAST(ASS_ITEM_NAME AS VARCHAR)  "
        + "END PNAME "
        + "FROM  HR_ASSESSMENT_ITEM  "
        + "WHERE ASS_ITEM_TYPE='" + strType + "' AND ASS_ITEM_PARENT_CODE='" + strParentCode + "' "
        + "AND CMP_BRANCH_ID = '" + strBranchID + "' AND ASSIS_TYPE_ID = '" + AssTypeId + "' "
        + "ORDER BY CMP_BRANCH_ID, ASS_ITEM_SET_CODE";



        try
        {
            con = new SqlConnection(cn);
            con.Open();
            DataSet oDS = new DataSet();
            SqlDataAdapter odaData = new SqlDataAdapter(new SqlCommand(strQuery, con));
            odaData.Fill(oDS, "CHILD_LIST");
            return oDS;
        }
        catch (Exception ex)
        {
            return null;
        }
        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }
    }
    public string InsertAssTypeValue(string TypeId, string Value, string Details)//
    {
        string strSql;
        strSql = "SELECT ASSIS_TYPE_ID, ASSIS_ITEM_VALUE, ASSIS_DETAILS FROM HR_ASSESSMENT_ITEM_VALUE ";


        try
        {
            DataRow oOrderRow;

            con = new SqlConnection(cn);
            con.Open();
            dbTransaction = con.BeginTransaction();
            DataSet oDS = new DataSet();
            SqlDataAdapter oOrdersDataAdapter = new SqlDataAdapter(new SqlCommand(strSql, con, dbTransaction));
            SqlCommandBuilder oOrdersCmdBuilder = new SqlCommandBuilder(oOrdersDataAdapter);
            oOrdersDataAdapter.FillSchema(oDS, SchemaType.Source);

            DataTable dtSchedule = oDS.Tables["Table"];
            dtSchedule.TableName = "HR_ASSESSMENT_ITEM_VALUE";

            oOrderRow = oDS.Tables["HR_ASSESSMENT_ITEM_VALUE"].NewRow();

            oOrderRow["ASSIS_TYPE_ID"] = TypeId;
            oOrderRow["ASSIS_ITEM_VALUE"] = Value;
            oOrderRow["ASSIS_DETAILS"] = Details;



            oDS.Tables["HR_ASSESSMENT_ITEM_VALUE"].Rows.Add(oOrderRow);
            oOrdersDataAdapter.Update(oDS, "HR_ASSESSMENT_ITEM_VALUE");
            dbTransaction.Commit();
            //con.Close();
            return "Saved Successfully.";
        }
        catch (Exception ex)
        {
            return ex.Message.ToString();
        }
        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }
    }
    public DataSet GetAssessmentGroup(string branchId, string AllBranch, string strtypeID)      /*** added by : Niaz Morshed, 08-Oct-2013, Decription : Select Assessment Group ***/
    {


        //string strSql = "SELECT AI.ASS_ITEM_SET_CODE ||' - ' || AI.ASS_ITEM_NAME ITEM_NAME, AI.ASS_ITEM_ID, ASS_ITEM_NAME, ATP.ASSIS_NATURE, AI.ASS_ITEM_SET_CODE FROM HR_ASSESSMENT_ITEM AI, HR_ASSESSMENT_TYPE ATP "
        //                + "WHERE AI.ASSIS_TYPE_ID=ATP.ASSIS_TYPE_ID AND ATP.ASSIS_TYPE_ID='" + strtypeID + "' AND ASS_ITEM_TYPE='G' "
        //                + " AND (AI.CMP_BRANCH_ID='" + branchId + "' OR AI.CMP_BRANCH_ID='" + AllBranch + "') ORDER by ASS_ITEM_SET_CODE ";


        //asad

        string strSql = "SELECT  CAST(AI.ASS_ITEM_SET_CODE AS VARCHAR(1000)) + ' - ' + CAST(AI.ASS_ITEM_NAME AS VARCHAR(2000)) ITEM_NAME, AI.ASS_ITEM_ID, "
        + "ASS_ITEM_NAME,ATP.ASSIS_NATURE, AI.ASS_ITEM_SET_CODE, ATP.COMMENT_STATUS,WM_ITEM_SET_CODE "
        + "FROM  HR_ASSESSMENT_ITEM AI, HR_ASSESSMENT_TYPE ATP "
        + "WHERE AI.ASSIS_TYPE_ID=ATP.ASSIS_TYPE_ID AND ATP.ASSIS_TYPE_ID='" + strtypeID + "' AND ASS_ITEM_TYPE='G' "
        + " AND (AI.CMP_BRANCH_ID='" + branchId + "' OR AI.CMP_BRANCH_ID='" + AllBranch + "') ORDER by ASS_ITEM_SET_CODE ";





        try
        {
            con = new SqlConnection(cn);
            con.Open();
            DataSet oDS = new DataSet();
            // Create the DataTable "Orders" in the Dataset and the OrdersDataAdapter
            SqlDataAdapter oOrdersDataAdapter = new SqlDataAdapter(new SqlCommand(strSql, con));
            SqlCommandBuilder oOrdersCmdBuilder = new SqlCommandBuilder(oOrdersDataAdapter);
            oOrdersDataAdapter.Fill(oDS, "ASSESSMENT_GROUP");
            return oDS;
        }
        catch (Exception ex)
        {

            return null;
        }
        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }
    }

    public DataSet GetAssessmentGroupForLineManager(string branchId, string AllBranch, string strtypeID,string yearId,string empId)      /*** polash ***/
    {

        string strSql = @"SELECT  EM.EMP_ID,EM.CMP_BRANCH_ID,EM.YR_ID, CAST(AI.ASS_ITEM_SET_CODE AS VARCHAR(1000)) + ' - ' + CAST(AI.ASS_ITEM_NAME AS VARCHAR(2000)) ITEM_NAME, AI.ASS_ITEM_ID, ASS_ITEM_NAME,ATP.ASSIS_NATURE, AI.ASS_ITEM_SET_CODE, ATP.COMMENT_STATUS,WM_ITEM_SET_CODE,ATP.ASSIS_TYPE_ID,AE.ASS_EMP_ASSISMENT FROM 

 HR_ASSESSMENT_ITEM AI left join  HR_ASSESSMENT_EMPLOYEE AE on AI.ASS_ITEM_ID  = AE.ASS_ITEM_ID and AE.EMP_ID='" + empId + "', HR_ASSESSMENT_TYPE ATP,HR_ASSESSMENT_EMPLOYEE_MASTER as EM WHERE AI.ASSIS_TYPE_ID=ATP.ASSIS_TYPE_ID  and EM.ASSIS_TYPE_ID=ATP.ASSIS_TYPE_ID AND ATP.ASSIS_TYPE_ID='" + strtypeID + "' AND ASS_ITEM_TYPE='G' "
        + " AND (AI.CMP_BRANCH_ID='" + branchId + "' OR AI.CMP_BRANCH_ID='" + AllBranch + "') and  EM.EMP_ID='" + empId + "' and EM.YR_ID='" + yearId + "' and STATUS='LM' ORDER by ASS_ITEM_SET_CODE ";





        try
        {
            con = new SqlConnection(cn);
            con.Open();
            DataSet oDS = new DataSet();
            // Create the DataTable "Orders" in the Dataset and the OrdersDataAdapter
            SqlDataAdapter oOrdersDataAdapter = new SqlDataAdapter(new SqlCommand(strSql, con));
            SqlCommandBuilder oOrdersCmdBuilder = new SqlCommandBuilder(oOrdersDataAdapter);
            oOrdersDataAdapter.Fill(oDS, "ASSESSMENT_GROUP");
            return oDS;
        }
        catch (Exception ex)
        {

            return null;
        }
        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }
    }


    public DataSet GetAssessmentGroupForLineManagerForDraft(string branchId, string AllBranch, string strtypeID, string yearId, string empId,string draftStatus)      /*** polash ***/
    {

        string strSql = @"SELECT  EM.EMP_ID,EM.CMP_BRANCH_ID,EM.YR_ID, CAST(AI.ASS_ITEM_SET_CODE AS VARCHAR(1000)) + ' - ' + CAST(AI.ASS_ITEM_NAME AS VARCHAR(2000)) ITEM_NAME, AI.ASS_ITEM_ID, ASS_ITEM_NAME,ATP.ASSIS_NATURE, AI.ASS_ITEM_SET_CODE, ATP.COMMENT_STATUS,WM_ITEM_SET_CODE,ATP.ASSIS_TYPE_ID,AE.ASS_EMP_ASSISMENT FROM 

 HR_ASSESSMENT_ITEM AI left join  HR_ASSESSMENT_EMPLOYEE AE on AI.ASS_ITEM_ID  = AE.ASS_ITEM_ID and AE.EMP_ID='" + empId + "' and AE.DRAFT_STATUS='" + draftStatus + "', HR_ASSESSMENT_TYPE ATP,HR_ASSESSMENT_EMPLOYEE_MASTER as EM WHERE AI.ASSIS_TYPE_ID=ATP.ASSIS_TYPE_ID  and EM.ASSIS_TYPE_ID=ATP.ASSIS_TYPE_ID AND ATP.ASSIS_TYPE_ID='" + strtypeID + "' AND ASS_ITEM_TYPE='G' "
        + " AND (AI.CMP_BRANCH_ID='" + branchId + "' OR AI.CMP_BRANCH_ID='" + AllBranch + "') and  EM.EMP_ID='" + empId + "' and EM.YR_ID='" + yearId + "' and EM.DRAFT_STATUS='" + draftStatus + "' ORDER by ASS_ITEM_SET_CODE ";





        try
        {
            con = new SqlConnection(cn);
            con.Open();
            DataSet oDS = new DataSet();
            // Create the DataTable "Orders" in the Dataset and the OrdersDataAdapter
            SqlDataAdapter oOrdersDataAdapter = new SqlDataAdapter(new SqlCommand(strSql, con));
            SqlCommandBuilder oOrdersCmdBuilder = new SqlCommandBuilder(oOrdersDataAdapter);
            oOrdersDataAdapter.Fill(oDS, "ASSESSMENT_GROUP");
            return oDS;
        }
        catch (Exception ex)
        {

            return null;
        }
        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }
    }

    public DataSet GetAssessmentGroupForDepartmentHead(string branchId, string AllBranch, string strtypeID, string yearId, string empId)      /*** polash ***/
    {

        string strSql = @"SELECT  EM.EMP_ID,EM.CMP_BRANCH_ID,EM.YR_ID, CAST(AI.ASS_ITEM_SET_CODE AS VARCHAR(1000)) + ' - ' + CAST(AI.ASS_ITEM_NAME AS VARCHAR(2000)) ITEM_NAME, AI.ASS_ITEM_ID, ASS_ITEM_NAME,ATP.ASSIS_NATURE, AI.ASS_ITEM_SET_CODE, ATP.COMMENT_STATUS,WM_ITEM_SET_CODE,ATP.ASSIS_TYPE_ID,AE.ASS_EMP_ASSISMENT FROM 

 HR_ASSESSMENT_ITEM AI left join  HR_ASSESSMENT_EMPLOYEE AE on AI.ASS_ITEM_ID  = AE.ASS_ITEM_ID and AE.ASSE_STATUS='DH' and AE.EMP_ID='" + empId + "', HR_ASSESSMENT_TYPE ATP,HR_ASSESSMENT_EMPLOYEE_MASTER as EM WHERE  AI.ASSIS_TYPE_ID=ATP.ASSIS_TYPE_ID  and EM.ASSIS_TYPE_ID=ATP.ASSIS_TYPE_ID AND ATP.ASSIS_TYPE_ID='" + strtypeID + "' AND ASS_ITEM_TYPE='G' "
        + " AND (AI.CMP_BRANCH_ID='" + branchId + "' OR AI.CMP_BRANCH_ID='" + AllBranch + "') and  EM.EMP_ID='" + empId + "' and EM.YR_ID='" + yearId + "' and STATUS='DH'  ORDER by ASS_ITEM_SET_CODE ";





        try
        {
            con = new SqlConnection(cn);
            con.Open();
            DataSet oDS = new DataSet();
            // Create the DataTable "Orders" in the Dataset and the OrdersDataAdapter
            SqlDataAdapter oOrdersDataAdapter = new SqlDataAdapter(new SqlCommand(strSql, con));
            SqlCommandBuilder oOrdersCmdBuilder = new SqlCommandBuilder(oOrdersDataAdapter);
            oOrdersDataAdapter.Fill(oDS, "ASSESSMENT_GROUP");
            return oDS;
        }
        catch (Exception ex)
        {

            return null;
        }
        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }
    }

    public DataSet GetAssessmentInfoByEmpId(string empId, string loginId, string personTypeId)      /*** added by : Polash 12-06-2018***/
    {


       
        string strSql = @"select * from [dbo].[CM_WORKFLOW_MANAGE_TYPE] as CT
inner join [dbo].[CM_WORKFLOW_SETUP] as ST on ST.ITEM_SET_CODE=CT.WM_ITEM_SET_CODE
where SOURCE_EMP_ID='" + loginId + "' and DESTINATION_EMP_ID='" + empId + "' and WM_ITEM_SET_CODE='" + personTypeId + "'";
        try
        {
            con = new SqlConnection(cn);
            con.Open();
            DataSet oDS = new DataSet();
            // Create the DataTable "Orders" in the Dataset and the OrdersDataAdapter
            SqlDataAdapter oOrdersDataAdapter = new SqlDataAdapter(new SqlCommand(strSql, con));
            SqlCommandBuilder oOrdersCmdBuilder = new SqlCommandBuilder(oOrdersDataAdapter);
            oOrdersDataAdapter.Fill(oDS, "CM_WORKFLOW_MANAGE_TYPE");
            return oDS;
        }
        catch (Exception ex)
        {

            return null;
        }
        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }
    }

    public DataSet GetAssessmentChild(string branchId, string branAll, string strtypeID, string code)      /*** added by : Niaz Morshed, 08-Oct-2013, Decription : Select Assessment Child ***/
    {


        //string strSql = "SELECT ASS_ITEM_SET_CODE ||' - ' || ASS_ITEM_NAME ITEM_NAME, ASS_ITEM_ID, ASS_ITEM_TYPE, ASS_ITEM_NAME, ASS_ITEM_SET_CODE FROM HR_ASSESSMENT_ITEM "
        //                + "WHERE ASS_ITEM_SET_CODE!='" + code + "' AND ASS_ITEM_SET_CODE LIKE '" + code + "%' AND ASS_ITEM_TYPE='P' "
        //                + "AND ASSIS_TYPE_ID='" + strtypeID + "' AND (CMP_BRANCH_ID='" + branchId + "' OR CMP_BRANCH_ID='" + branAll + "') order by ASS_ITEM_SET_CODE ";

        //asad

        string strSql = "SELECT CAST(ASS_ITEM_SET_CODE AS VARCHAR(1000)) + ' - ' + CAST(ASS_ITEM_NAME AS VARCHAR(2000)) ITEM_NAME, ASS_ITEM_ID, "
                        + " ASS_ITEM_TYPE,ASS_ITEM_NAME,ASS_ITEM_SET_CODE "
                        + "FROM  HR_ASSESSMENT_ITEM  "
                        + "WHERE ASS_ITEM_SET_CODE!='" + code + "' AND ASS_ITEM_SET_CODE LIKE '" + code + "%' AND ASS_ITEM_TYPE='P' "
                        + "AND ASSIS_TYPE_ID='" + strtypeID + "' AND (CMP_BRANCH_ID='" + branchId + "' OR CMP_BRANCH_ID='" + branAll + "') order by ASS_ITEM_SET_CODE ";



        try
        {
            con = new SqlConnection(cn);
            con.Open();
            DataSet oDS = new DataSet();
            // Create the DataTable "Orders" in the Dataset and the OrdersDataAdapter
            SqlDataAdapter oOrdersDataAdapter = new SqlDataAdapter(new SqlCommand(strSql, con));
            SqlCommandBuilder oOrdersCmdBuilder = new SqlCommandBuilder(oOrdersDataAdapter);
            oOrdersDataAdapter.Fill(oDS, "ASSESSMENT_CHILD");
            return oDS;
        }
        catch (Exception ex)
        {

            return null;
        }
        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }
    }
    public string InsertAssessment(string BranchId, string yearId, string emplyeeId,   /*** added by : Niaz Morshed, 28-Nov-2012, Decription : Insert Node Property value ***/
                                string typeId, string itemId, string value, string Assper)
    {


        try
        {
            string strSql = "SELECT ASSIS_TYPE_ID, ASS_ITEM_ID, ASS_EMP_ASSISMENT, EMP_ID, YR_ID, CMP_BRANCH_ID, ASSIS_PERSON_TYPE FROM HR_ASSESSMENT_EMPLOYEE ";
            DataRow oOrderRow;
            con = new SqlConnection(cn);
            con.Open();
            DataSet oDs = new DataSet();
            SqlDataAdapter oDbAdapter = new SqlDataAdapter(new SqlCommand(strSql, con));
            SqlCommandBuilder oDbCmdBuilder = new SqlCommandBuilder(oDbAdapter);
            oDbAdapter.MissingSchemaAction = MissingSchemaAction.AddWithKey;
            oDbAdapter.Fill(oDs, "HR_ASSESSMENT_EMPLOYEE");
            // Insert Data
            oOrderRow = oDs.Tables["HR_ASSESSMENT_EMPLOYEE"].NewRow();

            //6 fields
            oOrderRow["CMP_BRANCH_ID"] = BranchId;
            oOrderRow["YR_ID"] = yearId;
            oOrderRow["EMP_ID"] = emplyeeId;
            oOrderRow["ASSIS_TYPE_ID"] = typeId;
            oOrderRow["ASS_ITEM_ID"] = itemId;
            oOrderRow["ASS_EMP_ASSISMENT"] = value;
            oOrderRow["ASSIS_PERSON_TYPE"] = Assper;

            oDs.Tables["HR_ASSESSMENT_EMPLOYEE"].Rows.Add(oOrderRow);
            oDbAdapter.Update(oDs, "HR_ASSESSMENT_EMPLOYEE");
        }
        catch (Exception ex)
        {
            return "Err:" + ex.Message.ToString();
            //return null;
        }

        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }
        return "Success";
    }

    public string InsertAssessment(string BranchId, string yearId, string emplyeeId,   /*** added by : Niaz Morshed, 28-Nov-2012, Decription : Insert Node Property value ***/
                                string typeId, string itemId, string value, string Assper, string comments,string status,string draftStatus)
    {
        var con = new SqlConnection(cn);

        try
        {
            string strSql = "SELECT DRAFT_STATUS,ASSIS_TYPE_ID, ASS_ITEM_ID, ASS_EMP_ASSISMENT, EMP_ID, YR_ID, CMP_BRANCH_ID, ASSIS_PERSON_TYPE, COMMENTS,ASSE_STATUS FROM HR_ASSESSMENT_EMPLOYEE ";
            DataRow oOrderRow;

            DataSet oDs = new DataSet();
            SqlDataAdapter oDbAdapter = new SqlDataAdapter(new SqlCommand(strSql, con));
            SqlCommandBuilder oDbCmdBuilder = new SqlCommandBuilder(oDbAdapter);
            oDbAdapter.MissingSchemaAction = MissingSchemaAction.AddWithKey;
            oDbAdapter.Fill(oDs, "HR_ASSESSMENT_EMPLOYEE");
            // Insert Data
            oOrderRow = oDs.Tables["HR_ASSESSMENT_EMPLOYEE"].NewRow();

            //6 fields
            oOrderRow["CMP_BRANCH_ID"] = BranchId;
            oOrderRow["YR_ID"] = yearId;
            oOrderRow["EMP_ID"] = emplyeeId;
            oOrderRow["ASSIS_TYPE_ID"] = typeId;
            oOrderRow["ASS_ITEM_ID"] = itemId;
            oOrderRow["ASS_EMP_ASSISMENT"] = value;
            oOrderRow["ASSIS_PERSON_TYPE"] = Assper;
            oOrderRow["COMMENTS"] = comments;
            oOrderRow["ASSE_STATUS"] = status;
            oOrderRow["DRAFT_STATUS"] = draftStatus;


            oDs.Tables["HR_ASSESSMENT_EMPLOYEE"].Rows.Add(oOrderRow);
            oDbAdapter.Update(oDs, "HR_ASSESSMENT_EMPLOYEE");
        }
        catch (Exception ex)
        {
            return "Err:" + ex.Message.ToString();
            //return null;
        }
        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }
        return "Success";
    }


    public string InsertAssessmentMaster(string BranchId, string yearId, string emplyeeId,   /*** Polash 12-06-2018 ***/
                               string typeId, string Assper, string status,string draftStatus)
    {
        var con = new SqlConnection(cn);

        try
        {
            string strSql = "SELECT DRAFT_STATUS, ASSIS_TYPE_ID, EMP_ID, YR_ID, CMP_BRANCH_ID, ASSIS_PERSON_TYPE, STATUS FROM HR_ASSESSMENT_EMPLOYEE_MASTER ";
            DataRow oOrderRow;

            DataSet oDs = new DataSet();
            SqlDataAdapter oDbAdapter = new SqlDataAdapter(new SqlCommand(strSql, con));
            SqlCommandBuilder oDbCmdBuilder = new SqlCommandBuilder(oDbAdapter);
            oDbAdapter.MissingSchemaAction = MissingSchemaAction.AddWithKey;
            oDbAdapter.Fill(oDs, "HR_ASSESSMENT_EMPLOYEE_MASTER");
            // Insert Data
            oOrderRow = oDs.Tables["HR_ASSESSMENT_EMPLOYEE_MASTER"].NewRow();

            //6 fields
            oOrderRow["CMP_BRANCH_ID"] = BranchId;
            oOrderRow["YR_ID"] = yearId;
            oOrderRow["EMP_ID"] = emplyeeId;
            oOrderRow["ASSIS_TYPE_ID"] = typeId;           
            oOrderRow["ASSIS_PERSON_TYPE"] = Assper;
            oOrderRow["STATUS"] = status;
            oOrderRow["DRAFT_STATUS"] = draftStatus;

            oDs.Tables["HR_ASSESSMENT_EMPLOYEE_MASTER"].Rows.Add(oOrderRow);
            oDbAdapter.Update(oDs, "HR_ASSESSMENT_EMPLOYEE_MASTER");
        }
        catch (Exception ex)
        {
            return "Err:" + ex.Message.ToString();
            //return null;
        }
        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }
        return "Success";
    }


    public string UpdateAssessmentMaster(string BranchId, string yearId, string emplyeeId,   /*** Polash 12-06-2018 ***/
                             string typeId, string Assper, string status,string draftStatus)
    {
        var con = new SqlConnection(cn);
               
            string strSql = "SELECT ASSIS_TYPE_ID, EMP_ID, YR_ID, CMP_BRANCH_ID, ASSIS_PERSON_TYPE, STATUS FROM HR_ASSESSMENT_EMPLOYEE_MASTER ";

            string updateString;

            updateString = "UPDATE HR_ASSESSMENT_EMPLOYEE_MASTER SET ASSIS_PERSON_TYPE = '" + Assper + "',STATUS='" + status + "',DRAFT_STATUS='" + draftStatus + "' "
                         + "WHERE CMP_BRANCH_ID='" + BranchId + "' and EMP_ID='" + emplyeeId + "' AND YR_ID='" + yearId + "' "
                         + "AND ASSIS_TYPE_ID='" + typeId + "'  and  STATUS='LM' ";



            string strReturn = "";
            try
            {
                con = new SqlConnection(cn);
                con.Open();
                dbTransaction = con.BeginTransaction();
                SqlCommand cmd = new SqlCommand(updateString, con, dbTransaction);
                cmd.Connection = con;
                cmd.ExecuteNonQuery();
                dbTransaction.Commit();
                strReturn = "Success";
            }
            catch (Exception ex)
            {
                strReturn = ex.Message.ToString();
            }
            finally
            {
                con.Close();
                con.Dispose();
                SqlConnection.ClearPool(con);
                con = null;
            }
            return strReturn;


    }

    public string UpdateAssessmentMasterForSelf(string BranchId, string yearId, string emplyeeId,   /*** Polash 12-06-2018 ***/
                            string typeId, string Assper, string status, string draftStatus,string paramitter)
    {
        var con = new SqlConnection(cn);

        string strSql = "SELECT ASSIS_TYPE_ID, EMP_ID, YR_ID, CMP_BRANCH_ID, ASSIS_PERSON_TYPE, STATUS FROM HR_ASSESSMENT_EMPLOYEE_MASTER ";

        string updateString;

        updateString = "UPDATE HR_ASSESSMENT_EMPLOYEE_MASTER SET ASSIS_PERSON_TYPE = '" + Assper + "',STATUS='" + status + "',DRAFT_STATUS='" + draftStatus + "' "
                     + "WHERE CMP_BRANCH_ID='" + BranchId + "' and EMP_ID='" + emplyeeId + "' AND YR_ID='" + yearId + "' "
                     + "AND ASSIS_TYPE_ID='" + typeId + "'  and  DRAFT_STATUS='" + paramitter + "' ";



        string strReturn = "";
        try
        {
            con = new SqlConnection(cn);
            con.Open();
            dbTransaction = con.BeginTransaction();
            SqlCommand cmd = new SqlCommand(updateString, con, dbTransaction);
            cmd.Connection = con;
            cmd.ExecuteNonQuery();
            dbTransaction.Commit();
            strReturn = "Success";
        }
        catch (Exception ex)
        {
            strReturn = ex.Message.ToString();
        }
        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }
        return strReturn;


    }


    public string UpdateAssessmentMasterDptHead(string BranchId, string yearId, string emplyeeId,   /*** Polash 12-06-2018 ***/
                         string typeId, string Assper, string dropdownListvalue, string status)
    {
        var con = new SqlConnection(cn);

        string strSql = "SELECT ASSIS_TYPE_ID, EMP_ID, YR_ID, CMP_BRANCH_ID, ASSIS_PERSON_TYPE, STATUS,PROMOTION_CATEGORY_ID FROM HR_ASSESSMENT_EMPLOYEE_MASTER ";

        string updateString;

        updateString = "UPDATE HR_ASSESSMENT_EMPLOYEE_MASTER SET ASSIS_PERSON_TYPE = '" + Assper + "',STATUS='" + status + "',PROMOTION_CATEGORY_ID='" + dropdownListvalue + "',DRAFT_STATUS='' "
                     + "WHERE CMP_BRANCH_ID='" + BranchId + "' and EMP_ID='" + emplyeeId + "' AND YR_ID='" + yearId + "' "
                     + "AND ASSIS_TYPE_ID='" + typeId + "'  and  STATUS='DH' ";



        string strReturn = "";
        try
        {
            con = new SqlConnection(cn);
            con.Open();
            dbTransaction = con.BeginTransaction();
            SqlCommand cmd = new SqlCommand(updateString, con, dbTransaction);
            cmd.Connection = con;
            cmd.ExecuteNonQuery();
            dbTransaction.Commit();
            strReturn = "Success";
        }
        catch (Exception ex)
        {
            strReturn = ex.Message.ToString();
        }
        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }
        return strReturn;


    }

    public DataSet GetAssValue(string branchId, string EmpId, string YearId, string ItemId, string AssPer)      /*** added by : Niaz Morshed, 08-Oct-2013, Decription : Select Assessment Value ***/
    {


        string strSql = "SELECT COMMENTS, ASS_EMP_ASSISMENT FROM HR_ASSESSMENT_EMPLOYEE WHERE EMP_ID='" + EmpId + "' AND "
                        + "YR_ID='" + YearId + "' AND CMP_BRANCH_ID='" + branchId + "' AND ASS_ITEM_ID='" + ItemId + "' AND ASSIS_PERSON_TYPE=" + AssPer + " ";

        try
        {
            con = new SqlConnection(cn);
            con.Open();
            DataSet oDS = new DataSet();
            // Create the DataTable "Orders" in the Dataset and the OrdersDataAdapter
            SqlDataAdapter oOrdersDataAdapter = new SqlDataAdapter(new SqlCommand(strSql, con));
            SqlCommandBuilder oOrdersCmdBuilder = new SqlCommandBuilder(oOrdersDataAdapter);
            oOrdersDataAdapter.Fill(oDS, "ASSESSMENT_VALUE");
            return oDS;
        }
        catch (Exception ex)
        {

            return null;
        }
        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }


    }

    public DataSet GetTotAssValue01(string branchId, string EmpId, string YearId, string TypeId)      /*** added by : Niaz Morshed, 08-Oct-2013, Decription : Select Assessment Value ***/
    {


        string strSql = "SELECT FUNC_ASSESSMENT('01','" + EmpId + "','" + YearId + "','" + branchId + "','" + TypeId + "')TYPE1, "
                      + "FUNC_ASSESSMENT('02','" + EmpId + "','" + YearId + "','" + branchId + "','" + TypeId + "')TYPE2, "
                      + "FUNC_ASSESSMENT('03','" + EmpId + "','" + YearId + "','" + branchId + "','" + TypeId + "')TYPE3 FROM DUAL ";


        try
        {
            con = new SqlConnection(cn);
            con.Open();
            DataSet oDS = new DataSet();
            // Create the DataTable "Orders" in the Dataset and the OrdersDataAdapter
            SqlDataAdapter oOrdersDataAdapter = new SqlDataAdapter(new SqlCommand(strSql, con));
            SqlCommandBuilder oOrdersCmdBuilder = new SqlCommandBuilder(oOrdersDataAdapter);
            oOrdersDataAdapter.Fill(oDS, "ASSESSMENT_TOT");
            return oDS;
        }
        catch (Exception ex)
        {

            return null;
        }
        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }
    }

    public DataSet GetTotPerson(string branchId, string EmpId, string YearId, string TypeId)      /*** added by : Niaz Morshed, 08-Oct-2013, Decription : Select Assessment Value ***/
    {


        string strSql = "SELECT count (DISTINCT ASSIS_PERSON_TYPE)  TOT_PER FROM HR_ASSESSMENT_EMPLOYEE "
                        + "WHERE EMP_ID='" + EmpId + "' AND YR_ID='" + YearId + "' "
                        + "AND CMP_BRANCH_ID='" + branchId + "' AND ASSIS_TYPE_ID='" + TypeId + "' AND ASSIS_PERSON_TYPE !=4 ";


        try
        {
            con = new SqlConnection(cn);
            con.Open();
            DataSet oDS = new DataSet();
            // Create the DataTable "Orders" in the Dataset and the OrdersDataAdapter
            SqlDataAdapter oOrdersDataAdapter = new SqlDataAdapter(new SqlCommand(strSql, con));
            SqlCommandBuilder oOrdersCmdBuilder = new SqlCommandBuilder(oOrdersDataAdapter);
            oOrdersDataAdapter.Fill(oDS, "ASSESSMENT_TOT_PER");
            return oDS;
        }
        catch (Exception ex)
        {
            return null;
        }
        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }
    }

    #endregion

    public string InsertMultipleProduct(string BranchID, string EmpStatus, string empTitle, string empName, string empGender, string empFName, string empMName, string empBirthD,
                                        string empMaritalStatus, string empNationality, string empReligion, string empBankAcc,
                                        string empID, string empCode, string empReportingID, string DepartmentID, string DesignationID,
                                        string empJoiningDate, string empIndividual, string empQuantity, string empProvision,
                                        string empFinalConDate, string empComments)
    {
        string strSql;
        strSql = "SELECT CMP_BRANCH_ID, EMP_STATUS, EMP_TITLE, EMP_NAME, EMP_GENDER, EMP_FATHER_NAME, EMP_MOTHER_NAME, EMP_BIRTHDAY, EMP_MARITAL_STATAS, EMP_NATIONALITY, " +
            "EMP_RELIGION, EMP_BANK_ACC_NO, EMP_ID, EMP_CODE, EMP_REPORTING_ID, DPT_ID, DSG_ID, EMP_JOINING_DATE, EMP_INDIVIDUAL, " +
            "EMP_QUANTITY, EMP_PROVISION_PERIOD, EMP_FINAL_CONFIR_DATE, EMP_COMMENTS FROM PR_EMPLOYEE_LIST ";


        try
        {
            con = new SqlConnection(cn);
            con.Open();
            #region MyRegion

            DataRow oOrderRow;

            con.Open();
            dbTransaction = con.BeginTransaction();
            DataSet oDS = new DataSet();
            SqlDataAdapter oOrdersDataAdapter = new SqlDataAdapter(new SqlCommand(strSql, con, dbTransaction));
            SqlCommandBuilder oOrdersCmdBuilder = new SqlCommandBuilder(oOrdersDataAdapter);
            oOrdersDataAdapter.FillSchema(oDS, SchemaType.Source);

            DataTable dtSchedule = oDS.Tables["Table"];
            dtSchedule.TableName = "PR_EMPLOYEE_LIST";

            oOrderRow = oDS.Tables["PR_EMPLOYEE_LIST"].NewRow();
            #endregion
            #region MyRegion

            oOrderRow["CMP_BRANCH_ID"] = BranchID;
            oOrderRow["EMP_STATUS"] = EmpStatus;
            oOrderRow["EMP_TITLE"] = empTitle;
            oOrderRow["EMP_NAME"] = empName;
            oOrderRow["EMP_GENDER"] = empGender;
            oOrderRow["EMP_FATHER_NAME"] = empFName;
            oOrderRow["EMP_MOTHER_NAME"] = empMName;
            oOrderRow["EMP_BIRTHDAY"] = empBirthD;
            oOrderRow["EMP_MARITAL_STATAS"] = empMaritalStatus;
            oOrderRow["EMP_NATIONALITY"] = empNationality;
            oOrderRow["EMP_RELIGION"] = empReligion;
            oOrderRow["EMP_BANK_ACC_NO"] = empBankAcc;
            oOrderRow["EMP_ID"] = empID;
            oOrderRow["EMP_CODE"] = empCode;
            oOrderRow["EMP_REPORTING_ID"] = empReportingID;
            oOrderRow["DPT_ID"] = DepartmentID;
            oOrderRow["DSG_ID"] = DesignationID;
            oOrderRow["EMP_JOINING_DATE"] = empJoiningDate;
            oOrderRow["EMP_INDIVIDUAL"] = empIndividual;
            oOrderRow["EMP_QUANTITY"] = empQuantity;
            oOrderRow["EMP_PROVISION_PERIOD"] = empProvision;
            oOrderRow["EMP_FINAL_CONFIR_DATE"] = empFinalConDate;
            oOrderRow["EMP_COMMENTS"] = empComments;
            #endregion

            oDS.Tables["PR_EMPLOYEE_LIST"].Rows.Add(oOrderRow);
            oOrdersDataAdapter.Update(oDS, "PR_EMPLOYEE_LIST");
            dbTransaction.Commit();
            //con.Close();
            return "Saved Successfully.";
        }
        catch (Exception ex)
        {
            return ex.Message.ToString();
        }
        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }
    }

    public DataSet GetAllEmpInfo(string bId, string empDsgID, string empID)
    {
        //string strQuery = "SELECT SET_LEVEL, EMP_ID,(EMP_TITLE || ' ' || EMP_NAME) as ENAME, EL.DSG_ID, "
        //                    + "DP.DPT_ID, DP.DPT_NAME, SET_CODE || ' - ' || DSG_TITLE AS DNAME, ET.TYP_TYPE, "
        //                    + "EMP_GENDER, EMP_STATUS, TOT_INCOME, TAX_PER_MONTH, TOT_TAX "
        //                    + "FROM PR_EMPLOYEE_LIST EL, PR_DESIGNATION DG, HR_EMP_TYPE ET, PR_DEPARTMENT DP "
        //                    + "WHERE EL.DSG_ID=DG.DSG_ID AND EL.CMP_BRANCH_ID='" + bId + "' ";
        //strQuery += empDsgID.Equals("") ? "" : " AND EL.DSG_ID='" + empDsgID + "' ";
        //strQuery += empID.Equals("") ? "" : " AND EL.EMP_ID='" + empID + "' ";
        //strQuery += " AND DP.DPT_ID = EL.DPT_ID "
        //        + "AND EL.EMP_STATUS=ET.TYP_CODE AND ET.PAYROLL_ALLOWED='Y' "
        //        + "ORDER BY to_number(SET_LEVEL), NLSSORT(TRIM(ENAME), 'NLS_SORT=generic_m')";


        //asad


        string strQuery = " SELECT SET_LEVEL, EMP_ID, (CAST(ISNULL(EL.EMP_TITLE, '') AS VARCHAR) + ' ' + CAST(EL.EMP_NAME AS VARCHAR)) as ENAME, EL.DSG_ID, DP.DPT_ID, DP.DPT_NAME, "
                            + "CAST(SET_CODE AS VARCHAR) + ' - ' + CAST(DSG_TITLE AS VARCHAR) AS DNAME, TAX_PER_MONTH, "
                            + "TOT_TAX, ET.TYP_TYPE,EMP_GENDER, EMP_STATUS, TOT_INCOME "
                            + "FROM  PR_EMPLOYEE_LIST EL, PR_DESIGNATION DG, HR_EMP_TYPE ET,PR_DEPARTMENT DP  "
                            + "WHERE EL.DSG_ID=DG.DSG_ID AND EL.CMP_BRANCH_ID='" + bId + "' ";
        strQuery += empDsgID.Equals("") ? "" : " AND EL.DSG_ID='" + empDsgID + "' ";
        strQuery += empID.Equals("") ? "" : " AND EL.EMP_ID='" + empID + "' ";
        strQuery += " AND DP.DPT_ID = EL.DPT_ID "
        + "AND EL.EMP_STATUS=ET.TYP_CODE AND ET.PAYROLL_ALLOWED='Y' "
        + "ORDER BY CONVERT(NUMERIC(8, 2), SET_LEVEL),LTRIM(RTRIM(EMP_NAME)) ";


        try
        {
            con = new SqlConnection(cn);
            con.Open();
            DataSet oDS = new DataSet();
            SqlDataAdapter odaData = new SqlDataAdapter(new SqlCommand(strQuery, con));
            odaData.Fill(oDS, "PR_EMPLOYEE_LIST");
            return oDS;
        }
        catch (Exception ex)
        {
            return null;
        }

        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }
    }
    public DataSet GetAllEmpInfo(string bId, DropDownList empDsgID)
    {
        string strQuery = "SELECT SET_LEVEL, EMP_ID,(CAST(ISNULL(EL.EMP_TITLE, '') AS VARCHAR) + ' ' + CAST(EL.EMP_NAME AS VARCHAR)) as ENAME, EL.DSG_ID, "
                            + "DP.DPT_ID, DP.DPT_NAME, CAST(SET_CODE AS VARCHAR) + ' - ' + CAST(DSG_TITLE AS VARCHAR) AS DNAME, ET.TYP_TYPE, "
                            + "EMP_GENDER, EMP_STATUS, TOT_INCOME, TAX_PER_MONTH, TOT_TAX "
                            + "FROM PR_EMPLOYEE_LIST EL, PR_DESIGNATION DG, HR_EMP_TYPE ET, PR_DEPARTMENT DP "
                            + "WHERE EL.DSG_ID=DG.DSG_ID AND EL.CMP_BRANCH_ID='" + bId + "' ";
        strQuery += empDsgID.Equals("") ? "" : " AND EL.DSG_ID='" + empDsgID.SelectedValue.ToString() + "' ";
        strQuery += " AND DP.DPT_ID = EL.DPT_ID "
                + "AND EL.EMP_STATUS=ET.TYP_CODE AND ET.PAYROLL_ALLOWED='Y' "
                + "ORDER BY CONVERT(NUMERIC(8, 2), SET_LEVEL),LTRIM(RTRIM(ENAME)) ";



        try
        {
            con = new SqlConnection(cn);
            con.Open();
            DataSet oDS = new DataSet();
            SqlDataAdapter odaData = new SqlDataAdapter(new SqlCommand(strQuery, con));
            odaData.Fill(oDS, "PR_EMPLOYEE_LIST");
            return oDS;
        }
        catch (Exception ex)
        {
            return null;
        }
        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }
    }

    public DataSet GEmpDetails(string branchId, string EmpId)      /*** added by : Niaz Morshed, 23-Oct-2013, Decription : Select EmployeeDetails ***/
    {
        string strSql = " SELECT CAST(SET_CODE AS VARCHAR) + ' -' + CAST(DSG_TITLE AS VARCHAR) DGE, DPT_NAME, EL.EMP_CODE, EL.EMP_JOINING_DATE ,EL.DPT_ID "
                        + " FROM PR_EMPLOYEE_LIST EL, PR_DESIGNATION DE, PR_DEPARTMENT DP "
                        + " WHERE EL.EMP_ID='" + EmpId + "' AND EL.DSG_ID=DE.DSG_ID AND DP.DPT_ID=EL.DPT_ID AND EL.CMP_BRANCH_ID='" + branchId + "'  ";

        DataSet oDS = new DataSet();
        try
        {
            con = new SqlConnection(cn);
            con.Open();
            // Create the DataTable "Orders" in the Dataset and the OrdersDataAdapter
            SqlDataAdapter oOrdersDataAdapter = new SqlDataAdapter(new SqlCommand(strSql, con));
            SqlCommandBuilder oOrdersCmdBuilder = new SqlCommandBuilder(oOrdersDataAdapter);
            oOrdersDataAdapter.Fill(oDS, "EMP_DETAILS");
            return oDS;
        }
        catch (Exception ex)
        {

            return null;
        }
        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }
    }
    // Not converted to sql
    public DataSet EmployeePerformanceForLoanNLease(string employee, string Todate, string FrDate)
    {
        string strQuery = "SELECT CI_CLIENT_TITLE || '' || CI_CLIENT_NAME CNAME, APP.APP_APPLICATION_DATE, APP.APP_APPLICATION_AMT, SA.SANCTION_DATE, SA.SANCTION_AMOUNT, "
                       + "FM.FM_FINANCE_NAME, CA.APV_RATE,  CC.DISBURS_AMT,CollectableAmt(EX.LA_NO,TO_DATE('" + FrDate + "', 'DD/MM/YYYY'),TO_DATE('" + Todate + "', 'DD/MM/YYYY'))COLLECTABLE_AMT, CollectedAmt(EX.LA_NO,TO_DATE('" + FrDate + "', 'DD/MM/YYYY'),TO_DATE('" + Todate + "', 'DD/MM/YYYY'))COLLECTED_AMT, nvl(BB.TOT_OVERDUE,0)as TOT_OVERDUE, APP.REF1_EMP_ID, APP.REF2_EMP_ID, APP.REF3_EMP_ID "
                       + "FROM CR_CLIENT_INFO CI, CR_APPLICATION APP, CR_APPROVED CA, CA_SANCTION SA, CA_FINANCIAL_CONTRACT FC, CA_EXECUTION EX, CA_AMORTIZATION_SCHEDULE AM, CR_FINANCE_MODE FM, "

                       + "(SELECT sum(CD.DISBURS_AMT) DISBURS_AMT, CD.LA_NO  FROM CR_CLIENT_INFO CI, CR_APPLICATION APP, CA_SANCTION SA, "
                       + "CA_FINANCIAL_CONTRACT FC, CA_DISBURSEMENT CD "
                       + "WHERE CI.CI_CLIENT_ID=APP.CI_CLIENT_ID AND SA.APPLICATION_ID=APP.APP_APPLICATION_ID AND "
                       + "FC.SANCTION_NO=SA.SANCTION_NO AND FC.LA_NO=CD.LA_NO AND (APP.REF1_EMP_ID='" + employee + "' OR APP.REF2_EMP_ID='" + employee + "' "
                       + "OR APP.REF3_EMP_ID='" + employee + "') "
                       + "AND TO_DATE(TO_CHAR(CD.DISBURSE_DATE, 'DD/MM/YYYY'), 'DD/MM/YYYY') "
                       + "BETWEEN TO_DATE('" + FrDate + "', 'DD/MM/YYYY') AND TO_DATE('" + Todate + "', 'DD/MM/YYYY') GROUP BY CD.LA_NO) CC, "

                       + "(SELECT SUM(INSTALLMENT_AMT) COLLECTED_AMT, EX.EXECUTION_ID  "
                       + "FROM CR_CLIENT_INFO CI, CR_APPLICATION APP, CA_SANCTION SA, CA_FINANCIAL_CONTRACT FC, CA_EXECUTION EX, CA_AMORTIZATION_SCHEDULE AM "
                       + "WHERE CI.CI_CLIENT_ID=APP.CI_CLIENT_ID AND SA.APPLICATION_ID=APP.APP_APPLICATION_ID AND "
                       + " FC.SANCTION_NO=SA.SANCTION_NO AND FC.LA_NO=EX.LA_NO AND AM.EXECUTION_ID=EX.EXECUTION_ID AND AM.STATUS='P' AND "
                       + "(APP.REF1_EMP_ID='" + employee + "' OR APP.REF2_EMP_ID='" + employee + "' OR APP.REF3_EMP_ID='" + employee + "') "
                       + "GROUP BY EX.EXECUTION_ID) AA, "

                       + "(SELECT COUNT(RENTAL_DATE) TOT_OVERDUE, EX.EXECUTION_ID  "
                       + "FROM CR_CLIENT_INFO CI, CR_APPLICATION APP, CA_SANCTION SA, CA_FINANCIAL_CONTRACT FC, CA_EXECUTION EX, CA_AMORTIZATION_SCHEDULE AM "
                       + "WHERE CI.CI_CLIENT_ID=APP.CI_CLIENT_ID AND SA.APPLICATION_ID=APP.APP_APPLICATION_ID AND "
                       + " FC.SANCTION_NO=SA.SANCTION_NO AND FC.LA_NO=EX.LA_NO AND AM.EXECUTION_ID=EX.EXECUTION_ID AND AM.STATUS='D' AND "
                       + "(APP.REF1_EMP_ID='" + employee + "' OR APP.REF2_EMP_ID='" + employee + "' OR APP.REF3_EMP_ID='" + employee + "') "
                       + "AND AM.RENTAL_DATE < '" + Todate + "' "
                       + "GROUP BY EX.EXECUTION_ID, RENTAL_DATE) BB "

                       + "WHERE CI.CI_CLIENT_ID=APP.CI_CLIENT_ID AND SA.APPLICATION_ID=APP.APP_APPLICATION_ID AND CA.APP_APPLICATION_ID=APP.APP_APPLICATION_ID AND "
                       + " FC.SANCTION_NO=SA.SANCTION_NO AND FC.LA_NO=EX.LA_NO AND AM.EXECUTION_ID=EX.EXECUTION_ID AND FM.FM_FINANCE_ID=APP.FM_FIN_MODE AND AM.EXECUTION_ID=AA.EXECUTION_ID AND "
                       + " AM.EXECUTION_ID=BB.EXECUTION_ID(+) AND EX.STATUS='N' AND "

                       + "(APP.REF1_EMP_ID='" + employee + "' OR APP.REF2_EMP_ID='" + employee + "' OR APP.REF3_EMP_ID='" + employee + "') "
                       + "AND TO_DATE(TO_CHAR(EX.EXECUTION_DATE, 'DD/MM/YYYY'), 'DD/MM/YYYY') BETWEEN "
                       + "TO_DATE('" + FrDate + "', 'DD/MM/YYYY') AND TO_DATE('" + Todate + "', 'DD/MM/YYYY') "
                       + " AND CC.LA_NO=FC.LA_NO "
                       + "GROUP BY EX.LA_NO, CI_CLIENT_TITLE, CI_CLIENT_NAME, APP.APP_APPLICATION_DATE, APP.APP_APPLICATION_AMT, SA.SANCTION_DATE, SA.SANCTION_AMOUNT, "
                       + "AA.COLLECTED_AMT, BB.TOT_OVERDUE, APP.REF1_EMP_ID, APP.REF2_EMP_ID, APP.REF3_EMP_ID,FM.FM_FINANCE_NAME, CA.APV_RATE,CC.DISBURS_AMT ";



        try
        {
            con = new SqlConnection(cn);
            con.Open();
            DataSet oDS = new DataSet();
            SqlDataAdapter odaData = new SqlDataAdapter(new SqlCommand(strQuery, con));
            odaData.Fill(oDS, "PR_EMPLOYEE_PERFORMANCE");
            return oDS;
        }
        catch (Exception ex)
        {
            return null;
        }
        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }
    }

    /// Author: Tanjil Alam
    /// Date:04/11/2013
    /// Task:Employee Performance for deposit
    /// </summary>
    /// <param name="employee"></param>
    /// <param name="date"></param>
    /// <returns></returns>
    public DataSet EmployeePerformanceForDeposit(string employee, string Todate, string FDate)
    {
        //string strQuery = "SELECT FS.SRC_NAME, (CAST(GA.LINK_ID AS VARCHAR) + '->' + CAST(ST.SRC_TYPE_NAME AS VARCHAR)) SCHEME, FD.APPLICATION_DATE, FD.DEPO_AMT, FD.CLOSING_DATE, GA.FN_GL_AC_ID "
        //             + "FROM FN_SOURCE FS,FN_DEPOSIT FD, FN_GL_AC GA,FN_SOURCE_TYPE ST "
        //             + "WHERE FS.FN_SOURCE_ID=FD.FN_SOURCE_ID AND GA.SRC_TYPE_ID=ST.FN_SOURCE_TYPE_ID "
        //             + "AND GA.LOAN_TYPE_CODE='TDR' AND FD.SCHEM_ID=GA.FN_GL_AC_ID AND FD.DEPO_STATUS='A' "
        //             + "AND (FD.FAVOUR='" + employee + "' or FD.FAVOUR2='" + employee + "') "
        //             + "AND TO_DATE(TO_CHAR(FD.ACTIVE_DATE, 'DD/MM/YYYY'), 'DD/MM/YYYY') BETWEEN "
        //             + "TO_DATE('" + FDate + "', 'DD/MM/YYYY') AND TO_DATE('" + Todate + "', 'DD/MM/YYYY') ";



        string strQuery = "SELECT FS.SRC_NAME, (CAST(GA.LINK_ID AS VARCHAR) + '->' + CAST(ST.SRC_TYPE_NAME AS VARCHAR)) SCHEME, FD.APPLICATION_DATE, FD.DEPO_AMT, FD.CLOSING_DATE, GA.FN_GL_AC_ID "
                    + "FROM FN_SOURCE FS,FN_DEPOSIT FD, FN_GL_AC GA,FN_SOURCE_TYPE ST "
                    + "WHERE FS.FN_SOURCE_ID=FD.FN_SOURCE_ID AND GA.SRC_TYPE_ID=ST.FN_SOURCE_TYPE_ID "
                    + "AND GA.LOAN_TYPE_CODE='TDR' AND FD.SCHEM_ID=GA.FN_GL_AC_ID AND FD.DEPO_STATUS='A' "
                    + "AND (FD.FAVOUR='" + employee + "' or FD.FAVOUR2='" + employee + "') "
                    + "AND CONVERT(DATETIME, CONVERT(VARCHAR(23), FD.ACTIVE_DATE, 103), 103)  BETWEEN CONVERT(DATETIME, '" + FDate + "', 103)  AND  CONVERT(DATETIME, '" + Todate + "', 103) ";
        try
        {
            con = new SqlConnection(cn);
            con.Open();
            DataSet oDS = new DataSet();
            SqlDataAdapter odaData = new SqlDataAdapter(new SqlCommand(strQuery, con));
            odaData.Fill(oDS, "PR_EMPLOYEE_PERFORMANCE");
            return oDS;
        }
        catch (Exception ex)
        {
            return null;
        }
        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }
    }
    /// <summary>
    /// author:Tanjil Alam
    /// date:05/11/2013
    /// </summary>
    /// <param name="employee"></param>
    /// <returns></returns>
    public DataSet MonthlyTerget(string employee, string bId, string yId, string type)
    {
        string strQuery = "select TER_MONTH_AMMOUNT from HR_EMP_TARGET "
                        + "where EMP_ID='" + employee + "' AND TER_TYPE='" + type + "' AND YR_ID='" + yId + "' AND CMP_BRANCH_ID='" + bId + "' ";



        try
        {
            con = new SqlConnection(cn);
            con.Open();
            DataSet oDS = new DataSet();
            SqlDataAdapter odaData = new SqlDataAdapter(new SqlCommand(strQuery, con));
            odaData.Fill(oDS, "MONTHLY_TERGET");
            return oDS;
        }
        catch (Exception ex)
        {
            return null;
        }
        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }
    }
    public DataSet GetAssEmpId(string uId, string bId) // get employee id from user id & branch id
    {
        string strQuery = "SELECT EMP_NAME, ASSIS_PERSON_NUMBER FROM HR_ASSESSMENT_PERSON AP, PR_EMPLOYEE_LIST EL, CM_SYSTEM_USERS SU "
                            + "WHERE EL.EMP_ID=AP.EMP_ID AND SU.SYS_USR_ID=EL.SYS_USR_ID AND SU.SYS_USR_ID = '" + uId + "' AND EL.CMP_BRANCH_ID='" + bId + "'";



        try
        {
            con = new SqlConnection(cn);
            con.Open();
            DataSet oDS = new DataSet();
            SqlDataAdapter odaData = new SqlDataAdapter(new SqlCommand(strQuery, con));
            odaData.Fill(oDS, "ASSESSMENT_EMP");
            return oDS;
        }
        catch (Exception ex)
        {
            return null;


        }
        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }
    }

    public DataSet GetAssDegId(string dId, string bId) // get employee id from user id & branch id
    {
        string strQuery = "SELECT EMP_NAME FROM PR_EMPLOYEE_LIST EL, PR_DESIGNATION DE, HR_ASSESSMENT_PERSON AP "
                            + "WHERE DE.DSG_ID=EL.DSG_ID AND DE.DSG_ID=AP.DSG_ID AND EL.CMP_BRANCH_ID = '" + bId + "' AND DE.DSG_ID ='" + dId + "'";



        try
        {
            con = new SqlConnection(cn);
            con.Open();
            DataSet oDS = new DataSet();
            SqlDataAdapter odaData = new SqlDataAdapter(new SqlCommand(strQuery, con));
            odaData.Fill(oDS, "ASSESSMENT_DEG");
            return oDS;
        }
        catch (Exception ex)
        {
            return null;
        }
        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }
    }

    public bool ChkAssEmp(string branchId, string YearId, string AssTypeId,  /*** added by : Niaz Morshed, 30-Oct-2013, Decription : Check Assessment Employee  ***/
                             string empId, string PertypeId)
    {
        string strSql;

        strSql = "SELECT ASS_EMP_ASSISMENT FROM HR_ASSESSMENT_EMPLOYEE "
                 + "WHERE ASSIS_TYPE_ID='" + AssTypeId + "' AND EMP_ID='" + empId + "' AND YR_ID='" + YearId + "' "
                 + "AND CMP_BRANCH_ID='" + branchId + "' AND ASSIS_PERSON_TYPE='" + PertypeId + "' ";



        try
        {
            //DataRow oOrderRow;
            con = new SqlConnection(cn);
            con.Open();
            DataSet oDS = new DataSet();
            SqlDataAdapter oOrdersDataAdapter = new SqlDataAdapter(new SqlCommand(strSql, con));
            SqlCommandBuilder oOrdersCmdBuilder = new SqlCommandBuilder(oOrdersDataAdapter);
            oOrdersDataAdapter.MissingSchemaAction = MissingSchemaAction.AddWithKey;
            oOrdersDataAdapter.Fill(oDS, "CHK_ASS_EMP");

            DataTable tbl_AD = oDS.Tables["CHK_ASS_EMP"];

            // already exists
            if (tbl_AD.Rows.Count > 0) { return true; }
            else { return false; }
        }
        catch (Exception ex)
        {
            return false;
        }
        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }
    }


    public bool ChkAssEmpDraft(string branchId, string YearId, string AssTypeId,  /*** added by : polash, 12-09-2018, Decription : Check Assessment Employee Draft self  ***/
                            string empId, string PertypeId,string draftStatus)
    {
        string strSql;

        strSql = "SELECT ASS_EMP_ASSISMENT FROM HR_ASSESSMENT_EMPLOYEE "
                 + "WHERE ASSIS_TYPE_ID='" + AssTypeId + "' AND EMP_ID='" + empId + "' AND YR_ID='" + YearId + "' "
                 + "AND CMP_BRANCH_ID='" + branchId + "' AND ASSIS_PERSON_TYPE='" + PertypeId + "' and DRAFT_STATUS='" + draftStatus + "'";



        try
        {
            //DataRow oOrderRow;
            con = new SqlConnection(cn);
            con.Open();
            DataSet oDS = new DataSet();
            SqlDataAdapter oOrdersDataAdapter = new SqlDataAdapter(new SqlCommand(strSql, con));
            SqlCommandBuilder oOrdersCmdBuilder = new SqlCommandBuilder(oOrdersDataAdapter);
            oOrdersDataAdapter.MissingSchemaAction = MissingSchemaAction.AddWithKey;
            oOrdersDataAdapter.Fill(oDS, "CHK_ASS_EMP");

            DataTable tbl_AD = oDS.Tables["CHK_ASS_EMP"];

            // already exists
            if (tbl_AD.Rows.Count > 0) { return true; }
            else { return false; }
        }
        catch (Exception ex)
        {
            return false;
        }
        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }
    }


    public bool ChkAssEmpforLineManager(string branchId, string YearId, string AssTypeId,  /*** added by : polash for check line manager 12-06-2018  ***/
                            string empId, string PertypeId,string Status)
    {
        string strSql;

        strSql = "SELECT ASS_EMP_ASSISMENT FROM HR_ASSESSMENT_EMPLOYEE  ,HR_ASSESSMENT_EMPLOYEE_MASTER as EM  "
                 + " WHERE   EM.ASSIS_TYPE_ID=HR_ASSESSMENT_EMPLOYEE.ASSIS_TYPE_ID and em.ASSIS_TYPE_ID='" + AssTypeId + "' AND EM.EMP_ID='" + empId + "' AND EM.YR_ID='" + YearId + "' "
                 + "AND EM.CMP_BRANCH_ID='" + branchId + "'  and EM.STATUS='" + Status + "' ";



        try
        {
            //DataRow oOrderRow;
            con = new SqlConnection(cn);
            con.Open();
            DataSet oDS = new DataSet();
            SqlDataAdapter oOrdersDataAdapter = new SqlDataAdapter(new SqlCommand(strSql, con));
            SqlCommandBuilder oOrdersCmdBuilder = new SqlCommandBuilder(oOrdersDataAdapter);
            oOrdersDataAdapter.MissingSchemaAction = MissingSchemaAction.AddWithKey;
            oOrdersDataAdapter.Fill(oDS, "CHK_ASS_EMP");

            DataTable tbl_AD = oDS.Tables["CHK_ASS_EMP"];

            // already exists
            if (tbl_AD.Rows.Count > 0) { return true; }
            else { return false; }
        }
        catch (Exception ex)
        {
            return false;
        }
        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }
    }

    public bool ChkAssEmpforDepartmentHead(string branchId, string YearId, string AssTypeId,  /*** added by : polash for check line manager 12-06-2018  ***/
                           string empId, string PertypeId)
    {
        string strSql;

        strSql = "SELECT ASS_EMP_ASSISMENT FROM HR_ASSESSMENT_EMPLOYEE  ,HR_ASSESSMENT_EMPLOYEE_MASTER as EM  "
                 + " WHERE   EM.ASSIS_TYPE_ID=HR_ASSESSMENT_EMPLOYEE.ASSIS_TYPE_ID and em.ASSIS_TYPE_ID='" + AssTypeId + "' AND EM.EMP_ID='" + empId + "' AND EM.YR_ID='" + YearId + "' "
                 + "AND EM.CMP_BRANCH_ID='" + branchId + "'  and EM.STATUS='DH' ";



        try
        {
            //DataRow oOrderRow;
            con = new SqlConnection(cn);
            con.Open();
            DataSet oDS = new DataSet();
            SqlDataAdapter oOrdersDataAdapter = new SqlDataAdapter(new SqlCommand(strSql, con));
            SqlCommandBuilder oOrdersCmdBuilder = new SqlCommandBuilder(oOrdersDataAdapter);
            oOrdersDataAdapter.MissingSchemaAction = MissingSchemaAction.AddWithKey;
            oOrdersDataAdapter.Fill(oDS, "CHK_ASS_EMP");

            DataTable tbl_AD = oDS.Tables["CHK_ASS_EMP"];

            // already exists
            if (tbl_AD.Rows.Count > 0) { return true; }
            else { return false; }
        }
        catch (Exception ex)
        {
            return false;
        }
        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }
    }
    public DataSet GetAsseGroupValue(string branchId, string AllBranch, string strtypeID,     /*** added by : Niaz Morshed, 08-Oct-2013, Decription : Select Assessment Group ***/
                                      string YearId, string EmpId, string perId)
    {


        //string strSql = "SELECT AI.ASS_ITEM_SET_CODE ||' - ' || AI.ASS_ITEM_NAME ITEM_NAME, AI.ASS_ITEM_ID, ASS_ITEM_NAME, ATP.ASSIS_NATURE, ASS_EMP_ASSISMENT,"
        //                + "AI.ASS_ITEM_SET_CODE FROM HR_ASSESSMENT_ITEM AI, HR_ASSESSMENT_TYPE ATP, HR_ASSESSMENT_EMPLOYEE AE "
        //                + "WHERE AI.ASSIS_TYPE_ID=ATP.ASSIS_TYPE_ID AND ATP.ASSIS_TYPE_ID='" + strtypeID + "' AND ASS_ITEM_TYPE='G' "
        //                + "AND AI.ASS_ITEM_ID=AE.ASS_ITEM_ID(+) AND EMP_ID(+)='" + EmpId + "' AND YR_ID(+)='" + YearId + "' AND ASSIS_PERSON_TYPE(+)='" + perId + "' "
        //                + " AND (AI.CMP_BRANCH_ID='" + branchId + "' OR AI.CMP_BRANCH_ID='" + AllBranch + "') ORDER by ASS_ITEM_SET_CODE ";


        //asad

        string strSql = "SELECT ATP.COMMENT_STATUS, CAST(AI.ASS_ITEM_SET_CODE AS VARCHAR(1000)) + ' - ' + CAST(AI.ASS_ITEM_NAME AS VARCHAR(2000)) ITEM_NAME, "
                        + "AI.ASS_ITEM_ID,ASS_ITEM_NAME,ATP.ASSIS_NATURE,ASS_EMP_ASSISMENT,AI.ASS_ITEM_SET_CODE "
                        + "FROM  HR_ASSESSMENT_ITEM AI  LEFT OUTER JOIN  HR_ASSESSMENT_EMPLOYEE AE  ON  AI.ASS_ITEM_ID  = AE.ASS_ITEM_ID "
                        + "AND	EMP_ID  = '" + EmpId + "' AND YR_ID  = '" + YearId + "' AND	ASSIS_PERSON_TYPE  = '" + perId + "' , "
                        + "HR_ASSESSMENT_TYPE ATP WHERE	 AI.ASSIS_TYPE_ID  = ATP.ASSIS_TYPE_ID AND	ATP.ASSIS_TYPE_ID  = '" + strtypeID + "' "
                        + "AND	ASS_ITEM_TYPE  = 'G' AND (AI.CMP_BRANCH_ID  = '" + branchId + "' OR	AI.CMP_BRANCH_ID  = '" + AllBranch + "') "
                        + "ORDER BY ASS_ITEM_SET_CODE";

        try
        {
            con = new SqlConnection(cn);
            con.Open();
            DataSet oDS = new DataSet();
            // Create the DataTable "Orders" in the Dataset and the OrdersDataAdapter
            SqlDataAdapter oOrdersDataAdapter = new SqlDataAdapter(new SqlCommand(strSql, con));
            SqlCommandBuilder oOrdersCmdBuilder = new SqlCommandBuilder(oOrdersDataAdapter);
            oOrdersDataAdapter.Fill(oDS, "ASSESSMENT_GROUP");
            return oDS;
        }
        catch (Exception ex)
        {

            return null;
        }
        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }


    }


    public DataSet GetAsseGroupValueForDraft(string branchId, string AllBranch, string strtypeID,     /*** added by : Polash, 10-12-2018, Decription : Select Assessment Group Draft ***/
                                      string YearId, string EmpId, string perId,string draftstatus)
    {


        //string strSql = "SELECT AI.ASS_ITEM_SET_CODE ||' - ' || AI.ASS_ITEM_NAME ITEM_NAME, AI.ASS_ITEM_ID, ASS_ITEM_NAME, ATP.ASSIS_NATURE, ASS_EMP_ASSISMENT,"
        //                + "AI.ASS_ITEM_SET_CODE FROM HR_ASSESSMENT_ITEM AI, HR_ASSESSMENT_TYPE ATP, HR_ASSESSMENT_EMPLOYEE AE "
        //                + "WHERE AI.ASSIS_TYPE_ID=ATP.ASSIS_TYPE_ID AND ATP.ASSIS_TYPE_ID='" + strtypeID + "' AND ASS_ITEM_TYPE='G' "
        //                + "AND AI.ASS_ITEM_ID=AE.ASS_ITEM_ID(+) AND EMP_ID(+)='" + EmpId + "' AND YR_ID(+)='" + YearId + "' AND ASSIS_PERSON_TYPE(+)='" + perId + "' "
        //                + " AND (AI.CMP_BRANCH_ID='" + branchId + "' OR AI.CMP_BRANCH_ID='" + AllBranch + "') ORDER by ASS_ITEM_SET_CODE ";


        //asad

        string strSql = "SELECT ATP.COMMENT_STATUS, CAST(AI.ASS_ITEM_SET_CODE AS VARCHAR(1000)) + ' - ' + CAST(AI.ASS_ITEM_NAME AS VARCHAR(2000)) ITEM_NAME, "
                        + "AI.ASS_ITEM_ID,ASS_ITEM_NAME,ATP.ASSIS_NATURE,ASS_EMP_ASSISMENT,AI.ASS_ITEM_SET_CODE "
                        + "FROM  HR_ASSESSMENT_ITEM AI  LEFT OUTER JOIN  HR_ASSESSMENT_EMPLOYEE AE  ON  AI.ASS_ITEM_ID  = AE.ASS_ITEM_ID "
                        + "AND	EMP_ID  = '" + EmpId + "' AND YR_ID  = '" + YearId + "' AND	ASSIS_PERSON_TYPE  = '" + perId + "' , "
                        + "HR_ASSESSMENT_TYPE ATP WHERE AE.DRAFT_STATUS='" + draftstatus + "' and  AI.ASSIS_TYPE_ID  = ATP.ASSIS_TYPE_ID AND	ATP.ASSIS_TYPE_ID  = '" + strtypeID + "' "
                        + "AND	ASS_ITEM_TYPE  = 'G' AND (AI.CMP_BRANCH_ID  = '" + branchId + "' OR	AI.CMP_BRANCH_ID  = '" + AllBranch + "') "
                        + "ORDER BY ASS_ITEM_SET_CODE";

        try
        {
            con = new SqlConnection(cn);
            con.Open();
            DataSet oDS = new DataSet();
            // Create the DataTable "Orders" in the Dataset and the OrdersDataAdapter
            SqlDataAdapter oOrdersDataAdapter = new SqlDataAdapter(new SqlCommand(strSql, con));
            SqlCommandBuilder oOrdersCmdBuilder = new SqlCommandBuilder(oOrdersDataAdapter);
            oOrdersDataAdapter.Fill(oDS, "ASSESSMENT_GROUP");
            return oDS;
        }
        catch (Exception ex)
        {

            return null;
        }
        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }


    }

    public DataSet GetAsseChildValue(string branchId, string branAll, string strtypeID, string code,      /*** added by : Niaz Morshed, 08-Oct-2018, Decription : Select Assessment Child ***/
                                      string EmpId, string YearId, string perId)
    {


        //string strSql = "SELECT ASS_ITEM_SET_CODE ||' - ' || ASS_ITEM_NAME ITEM_NAME, AI.ASS_ITEM_ID, ASS_ITEM_TYPE, ASS_ITEM_NAME, "
        //                + "ASS_ITEM_SET_CODE, ASS_EMP_ASSISMENT FROM HR_ASSESSMENT_ITEM AI, HR_ASSESSMENT_EMPLOYEE AE "
        //                + "WHERE AI.ASS_ITEM_SET_CODE!='" + code + "' AND AI.ASS_ITEM_SET_CODE LIKE '" + code + "%' AND AI.ASS_ITEM_TYPE='P' "
        //                + "AND AE.ASS_ITEM_ID(+)=AI.ASS_ITEM_ID AND EMP_ID(+)='" + EmpId + "' AND YR_ID(+)='" + YearId + "' AND ASSIS_PERSON_TYPE(+)='" + perId + "'"
        //                + "AND AI.ASSIS_TYPE_ID='" + strtypeID + "' AND (AI.CMP_BRANCH_ID='" + branchId + "' OR AI.CMP_BRANCH_ID='" + branAll + "') order by ASS_ITEM_SET_CODE ";

        string strSql = "SELECT AE.COMMENTS,(CAST(ASS_ITEM_SET_CODE AS VARCHAR(1000)) + ' - ' + CAST(ASS_ITEM_NAME AS VARCHAR(2000))) ITEM_NAME,AI.ASS_ITEM_ID, "
                        + "ASS_ITEM_TYPE, ASS_ITEM_NAME, ASS_ITEM_SET_CODE, ASS_EMP_ASSISMENT "
                        + "FROM  HR_ASSESSMENT_EMPLOYEE AE  RIGHT OUTER JOIN  HR_ASSESSMENT_ITEM AI  ON  AE.ASS_ITEM_ID  = AI.ASS_ITEM_ID "
                        + "AND	EMP_ID  = '" + EmpId + "' AND	YR_ID  = '" + YearId + "'  "
                        + "WHERE ae.ASSE_STATUS='LM' and	 AI.ASS_ITEM_SET_CODE  != '" + code + "' AND	AI.ASS_ITEM_SET_CODE  LIKE '" + code + "%' "
                        + "AND	AI.ASS_ITEM_TYPE  = 'P' AND	AI.ASSIS_TYPE_ID  = '" + strtypeID + "' AND	(AI.CMP_BRANCH_ID  = '" + branchId + "' "
                        + "OR	AI.CMP_BRANCH_ID  = '" + branAll + "') ORDER BY ASS_ITEM_SET_CODE";

        try   //  CAST(ASS_ITEM_NAME AS VARCHAR(1000))
        {
            con = new SqlConnection(cn);
            con.Open();
            DataSet oDS = new DataSet();
            // Create the DataTable "Orders" in the Dataset and the OrdersDataAdapter
            SqlDataAdapter oOrdersDataAdapter = new SqlDataAdapter(new SqlCommand(strSql, con));
            SqlCommandBuilder oOrdersCmdBuilder = new SqlCommandBuilder(oOrdersDataAdapter);
            oOrdersDataAdapter.Fill(oDS, "ASSESSMENT_CHILD");
            return oDS;
        }
        catch (Exception ex)
        {

            return null;
        }
        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }
    }


    public DataSet GetAsseChildValueDraft(string branchId, string branAll, string strtypeID, string code,      /*** added by : Polash, 10-12-2018, Decription : Select Assessment Child Draft ***/
                                     string EmpId, string YearId, string perId,string draftStatus)
    {


        //string strSql = "SELECT ASS_ITEM_SET_CODE ||' - ' || ASS_ITEM_NAME ITEM_NAME, AI.ASS_ITEM_ID, ASS_ITEM_TYPE, ASS_ITEM_NAME, "
        //                + "ASS_ITEM_SET_CODE, ASS_EMP_ASSISMENT FROM HR_ASSESSMENT_ITEM AI, HR_ASSESSMENT_EMPLOYEE AE "
        //                + "WHERE AI.ASS_ITEM_SET_CODE!='" + code + "' AND AI.ASS_ITEM_SET_CODE LIKE '" + code + "%' AND AI.ASS_ITEM_TYPE='P' "
        //                + "AND AE.ASS_ITEM_ID(+)=AI.ASS_ITEM_ID AND EMP_ID(+)='" + EmpId + "' AND YR_ID(+)='" + YearId + "' AND ASSIS_PERSON_TYPE(+)='" + perId + "'"
        //                + "AND AI.ASSIS_TYPE_ID='" + strtypeID + "' AND (AI.CMP_BRANCH_ID='" + branchId + "' OR AI.CMP_BRANCH_ID='" + branAll + "') order by ASS_ITEM_SET_CODE ";

        string strSql = "SELECT AE.COMMENTS,(CAST(ASS_ITEM_SET_CODE AS VARCHAR(1000)) + ' - ' + CAST(ASS_ITEM_NAME AS VARCHAR(2000))) ITEM_NAME,AI.ASS_ITEM_ID, "
                        + "ASS_ITEM_TYPE, ASS_ITEM_NAME, ASS_ITEM_SET_CODE, ASS_EMP_ASSISMENT "
                        + "FROM  HR_ASSESSMENT_EMPLOYEE AE  RIGHT OUTER JOIN  HR_ASSESSMENT_ITEM AI  ON  AE.ASS_ITEM_ID  = AI.ASS_ITEM_ID "
                        + "AND	EMP_ID  = '" + EmpId + "' AND	YR_ID  = '" + YearId + "'  "
                        + "WHERE ae.DRAFT_STATUS='" + draftStatus + "' and	 AI.ASS_ITEM_SET_CODE  != '" + code + "' AND	AI.ASS_ITEM_SET_CODE  LIKE '" + code + "%' "
                        + "AND	AI.ASS_ITEM_TYPE  = 'P' AND	AI.ASSIS_TYPE_ID  = '" + strtypeID + "' AND	(AI.CMP_BRANCH_ID  = '" + branchId + "' "
                        + "OR	AI.CMP_BRANCH_ID  = '" + branAll + "') ORDER BY ASS_ITEM_SET_CODE";

        try   //  CAST(ASS_ITEM_NAME AS VARCHAR(1000))
        {
            con = new SqlConnection(cn);
            con.Open();
            DataSet oDS = new DataSet();
            // Create the DataTable "Orders" in the Dataset and the OrdersDataAdapter
            SqlDataAdapter oOrdersDataAdapter = new SqlDataAdapter(new SqlCommand(strSql, con));
            SqlCommandBuilder oOrdersCmdBuilder = new SqlCommandBuilder(oOrdersDataAdapter);
            oOrdersDataAdapter.Fill(oDS, "ASSESSMENT_CHILD");
            return oDS;
        }
        catch (Exception ex)
        {

            return null;
        }
        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }
    }


    public DataSet GetAsseChildValueForLineManager(string branchId, string branAll, string strtypeID, string code,      /*** added by : polash, 08-12-2018, Decription : Select Assessment Child ***/
                                     string EmpId, string YearId, string perId)
    {



        string strSql = "SELECT AE.COMMENTS,(CAST(ASS_ITEM_SET_CODE AS VARCHAR(1000)) + ' - ' + CAST(ASS_ITEM_NAME AS VARCHAR(2000))) ITEM_NAME,AI.ASS_ITEM_ID, "
                        + "ASS_ITEM_TYPE, ASS_ITEM_NAME, ASS_ITEM_SET_CODE, ASS_EMP_ASSISMENT "
                        + "FROM  HR_ASSESSMENT_EMPLOYEE AE  RIGHT OUTER JOIN  HR_ASSESSMENT_ITEM AI  ON  AE.ASS_ITEM_ID  = AI.ASS_ITEM_ID "
                        + "AND	EMP_ID  = '" + EmpId + "' AND	YR_ID  = '" + YearId + "'  "
                        + "WHERE ae.ASSE_STATUS='LM' and	 AI.ASS_ITEM_SET_CODE  != '" + code + "' AND	AI.ASS_ITEM_SET_CODE  LIKE '" + code + "%' "
                        + "AND	AI.ASS_ITEM_TYPE  = 'P' AND	AI.ASSIS_TYPE_ID  = '" + strtypeID + "' AND	(AI.CMP_BRANCH_ID  = '" + branchId + "' "
                        + "OR	AI.CMP_BRANCH_ID  = '" + branAll + "') ORDER BY ASS_ITEM_SET_CODE";

        try   //  CAST(ASS_ITEM_NAME AS VARCHAR(1000))
        {
            con = new SqlConnection(cn);
            con.Open();
            DataSet oDS = new DataSet();
            // Create the DataTable "Orders" in the Dataset and the OrdersDataAdapter
            SqlDataAdapter oOrdersDataAdapter = new SqlDataAdapter(new SqlCommand(strSql, con));
            SqlCommandBuilder oOrdersCmdBuilder = new SqlCommandBuilder(oOrdersDataAdapter);
            oOrdersDataAdapter.Fill(oDS, "ASSESSMENT_CHILD");
            return oDS;
        }
        catch (Exception ex)
        {

            return null;
        }
        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }
    }

    public DataSet GetAsseChildValueForDptHead(string branchId, string branAll, string strtypeID, string code,      /*** added by : polash, 08-12-2018, Decription : Select Assessment Child ***/
                                    string EmpId, string YearId, string perId)
    {


    
        string strSql = "SELECT AE.COMMENTS,(CAST(ASS_ITEM_SET_CODE AS VARCHAR(1000)) + ' - ' + CAST(ASS_ITEM_NAME AS VARCHAR(2000))) ITEM_NAME,AI.ASS_ITEM_ID, "
                        + "ASS_ITEM_TYPE, ASS_ITEM_NAME, ASS_ITEM_SET_CODE, ASS_EMP_ASSISMENT "
                        + "FROM  HR_ASSESSMENT_EMPLOYEE AE  RIGHT OUTER JOIN  HR_ASSESSMENT_ITEM AI  ON  AE.ASS_ITEM_ID  = AI.ASS_ITEM_ID "
                        + "AND	EMP_ID  = '" + EmpId + "' AND	YR_ID  = '" + YearId + "'  "
                        + "WHERE ae.ASSE_STATUS='DH' and	 AI.ASS_ITEM_SET_CODE  != '" + code + "' AND	AI.ASS_ITEM_SET_CODE  LIKE '" + code + "%' "
                        + "AND	AI.ASS_ITEM_TYPE  = 'P' AND	AI.ASSIS_TYPE_ID  = '" + strtypeID + "' AND	(AI.CMP_BRANCH_ID  = '" + branchId + "' "
                        + "OR	AI.CMP_BRANCH_ID  = '" + branAll + "') ORDER BY ASS_ITEM_SET_CODE";

        try   //  CAST(ASS_ITEM_NAME AS VARCHAR(1000))
        {
            con = new SqlConnection(cn);
            con.Open();
            DataSet oDS = new DataSet();
            // Create the DataTable "Orders" in the Dataset and the OrdersDataAdapter
            SqlDataAdapter oOrdersDataAdapter = new SqlDataAdapter(new SqlCommand(strSql, con));
            SqlCommandBuilder oOrdersCmdBuilder = new SqlCommandBuilder(oOrdersDataAdapter);
            oOrdersDataAdapter.Fill(oDS, "ASSESSMENT_CHILD");
            return oDS;
        }
        catch (Exception ex)
        {

            return null;
        }
        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }
    }

    public string UpdateAssessment(string branchId, string strtypeID, string EmpId,      /*** added by : Niaz Morshed, 08-Oct-2013, Decription : Select Assessment Child ***/
                                   string YearId, string perId, string ItemId, string Value)
    {
        string updateString;

        updateString = "UPDATE HR_ASSESSMENT_EMPLOYEE SET ASS_EMP_ASSISMENT = '" + Value + "' "
                     + "WHERE EMP_ID='" + EmpId + "' AND YR_ID='" + YearId + "' "
                     + "AND ASSIS_PERSON_TYPE='" + perId + "' AND  ASS_ITEM_ID='" + ItemId + "' AND ASSIS_TYPE_ID='" + strtypeID + "' ";



        string strReturn = "";
        try
        {
            con = new SqlConnection(cn);
            con.Open();
            dbTransaction = con.BeginTransaction();
            SqlCommand cmd = new SqlCommand(updateString, con, dbTransaction);
            cmd.Connection = con;
            cmd.ExecuteNonQuery();
            dbTransaction.Commit();
            //con.Close();
        }
        catch (Exception ex)
        {
            strReturn = ex.Message.ToString();
        }
        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }
        return strReturn;
    }

    public string UpdateAssessment(string branchId, string strtypeID, string EmpId,      /*** added by : Kamruzzaman Polash, 10-Oct-2018, Decription : Select Assessment Child ***/
                                   string YearId, string perId, string ItemId, string Value, string comments,string status,string draftStatus,string draft)
    {
        string updateString;

        updateString = "UPDATE HR_ASSESSMENT_EMPLOYEE SET ASS_EMP_ASSISMENT = '" + Value + "', COMMENTS='" + comments + "',ASSE_STATUS='" + status + "',DRAFT_STATUS='"+draftStatus+"' "
                     + "WHERE EMP_ID='" + EmpId + "' AND YR_ID='" + YearId + "' "
                     + "AND ASSIS_PERSON_TYPE='" + perId + "' AND  ASS_ITEM_ID='" + ItemId + "' AND ASSIS_TYPE_ID='" + strtypeID + "' and  DRAFT_STATUS='"+draft+"'";

        var con = new SqlConnection(cn);

        string strReturn = "";
        try
        {
            con.Open();
            dbTransaction = con.BeginTransaction();
            SqlCommand cmd = new SqlCommand(updateString, con, dbTransaction);
            cmd.Connection = con;
            cmd.ExecuteNonQuery();
            dbTransaction.Commit();
            //con.Close();
        }
        catch (Exception ex)
        {
            strReturn = ex.Message.ToString();
        }
        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }
        return strReturn;
    }

    public DataSet getYearID(string bId, string year)// Developed By Md. Sydur Rahman (29-Oct-13)
    {
        string strQuery = "Select YR_ID from HR_YEAR where YR_YEAR='" + year + "' and CMP_BRANCH_ID='" + bId + "'";

        try
        {
            con = new SqlConnection(cn);
            con.Open();

            DataSet oDS = new DataSet();
            SqlDataAdapter odaData = new SqlDataAdapter(new SqlCommand(strQuery, con));
            odaData.Fill(oDS, "HR_YEAR");
            return oDS;
        }
        catch (Exception ex)
        {
            return null;
        }
        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }
    }
    public string CloseCurrentYear(string bId, string empId, string yId, string typId, string YendBal) //  Close Current Year
    {
        string strSql;

        strSql = "UPDATE PR_LEAVE_ALLOWED SET LEAVE_YEAR_END = '" + YendBal + "' "
                  + "WHERE EMP_ID = '" + empId + "' AND CMP_BRANCH_ID='" + bId + "' AND YR_ID='" + yId + "' AND PRLT_ID='" + typId + "'";

        SqlConnection con = new SqlConnection(cn);

        try
        {
            con.Open();
            dbTransaction = con.BeginTransaction();
            SqlCommand cmd = new SqlCommand(strSql, con, dbTransaction);
            cmd.Connection = con;
            cmd.ExecuteNonQuery();
            dbTransaction.Commit();
            return "Current year successfully closed.";
        }
        catch (Exception ex)
        {
            return ex.Message.ToString();
        }
        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }

    }
    public string CloseCurrentYear(string bId, string empId, string yId, string typId, string Yearlyallowed, string monAllowed, string YendBal) //  Close Current Year
    {
        string strSql;


        strSql = "SELECT CMP_BRANCH_ID, EMP_ID, YR_ID, PRLT_ID, LEAVEALL_ALLOWED, LEAVE_ALLOWED_MON, LEAVE_YEAR_END FROM PR_LEAVE_ALLOWED";


        try
        {
            // Payroll Detail
            DataRow oOrderRow;

            con = new SqlConnection(cn);
            con.Open();
            dbTransaction = con.BeginTransaction();
            DataSet oDS = new DataSet();
            SqlDataAdapter oOrdersDataAdapter = new SqlDataAdapter(new SqlCommand(strSql, con, dbTransaction));
            SqlCommandBuilder oOrdersCmdBuilder = new SqlCommandBuilder(oOrdersDataAdapter);
            //oOrdersDataAdapter.MissingSchemaAction = MissingSchemaAction.AddWithKey;
            //oOrdersDataAdapter.Fill(oDS, "PR_LEAVE_ALLOWED");
            oOrdersDataAdapter.FillSchema(oDS, SchemaType.Source, "PR_LEAVE_ALLOWED");
            oOrderRow = oDS.Tables["PR_LEAVE_ALLOWED"].NewRow();

            // 5 fields
            oOrderRow["CMP_BRANCH_ID"] = bId;
            oOrderRow["EMP_ID"] = empId;
            oOrderRow["YR_ID"] = yId;
            oOrderRow["PRLT_ID"] = typId;

            if (Yearlyallowed != "")
            {
                oOrderRow["LEAVEALL_ALLOWED"] = Yearlyallowed;
            }
            else
            {
                oOrderRow["LEAVEALL_ALLOWED"] = 0;
            }
            if (monAllowed != "")
            {
                oOrderRow["LEAVE_ALLOWED_MON"] = monAllowed;
            }
            else
            {
                oOrderRow["LEAVE_ALLOWED_MON"] = 0;
            }
            if (YendBal != "")
            {
                oOrderRow["LEAVE_YEAR_END"] = YendBal;
            }
            else
            {
                oOrderRow["LEAVE_YEAR_END"] = 0;
            }

            oDS.Tables["PR_LEAVE_ALLOWED"].Rows.Add(oOrderRow);
            oOrdersDataAdapter.Update(oDS, "PR_LEAVE_ALLOWED");
            dbTransaction.Commit();
            //con.Close();
            return "Current year successfully closed.";
        }
        catch (Exception ex)
        {
            return ex.Message.ToString();
        }
        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }

    }
    public string ApplyNextYear(string bId, string empId, string yId, string typId, string totYearlyallowed, string Yearlyallowed, string monAllowed, string YerBigBal) // Apply For Next Year
    {
        string strSql;


        strSql = "SELECT ACTUAL_ALLOWED, CMP_BRANCH_ID, EMP_ID, YR_ID, PRLT_ID, LEAVEALL_ALLOWED, LEAVE_ALLOWED_MON, LEAVE_YEAR_BGN FROM PR_LEAVE_ALLOWED";

        con = new SqlConnection(cn);
        try
        {
            // Leave Allowed
            DataRow oOrderRow;

            con.Open();
            dbTransaction = con.BeginTransaction();
            DataSet oDS = new DataSet();
            SqlDataAdapter oOrdersDataAdapter = new SqlDataAdapter(new SqlCommand(strSql, con, dbTransaction));
            SqlCommandBuilder oOrdersCmdBuilder = new SqlCommandBuilder(oOrdersDataAdapter);
            oOrdersDataAdapter.FillSchema(oDS, SchemaType.Source, "PR_LEAVE_ALLOWED");
            oOrderRow = oDS.Tables["PR_LEAVE_ALLOWED"].NewRow();

            // 5 fields
            oOrderRow["CMP_BRANCH_ID"] = bId;
            oOrderRow["EMP_ID"] = empId;
            oOrderRow["YR_ID"] = yId;
            oOrderRow["PRLT_ID"] = typId;

            if (totYearlyallowed != "")
            {
                oOrderRow["LEAVEALL_ALLOWED"] = totYearlyallowed;
            }
            else
            {
                oOrderRow["LEAVEALL_ALLOWED"] = 0;
            }
            if (Yearlyallowed != "")
            {
                oOrderRow["ACTUAL_ALLOWED"] = Yearlyallowed;
            }
            else
            {
                oOrderRow["ACTUAL_ALLOWED"] = 0;
            }
            if (monAllowed != "")
            {
                oOrderRow["LEAVE_ALLOWED_MON"] = monAllowed;
            }
            else
            {
                oOrderRow["LEAVE_ALLOWED_MON"] = 0;
            }
            if (YerBigBal != "")
            {
                oOrderRow["LEAVE_YEAR_BGN"] = YerBigBal;
            }
            else
            {
                oOrderRow["LEAVE_YEAR_BGN"] = 0;
            }

            oDS.Tables["PR_LEAVE_ALLOWED"].Rows.Add(oOrderRow);
            oOrdersDataAdapter.Update(oDS, "PR_LEAVE_ALLOWED");
            dbTransaction.Commit();
            return "Successfully Applied.";
        }
        catch (Exception ex)
        {
            return ex.Message.ToString();
        }
        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }
    }
    public string ApplyNextYear(string bId, string empId, string yId, string typId, string Yearlyallowed, string monAllowed, string YerBigBal) // Apply For Next Year
    {
        string strSql;


        strSql = "SELECT CMP_BRANCH_ID, EMP_ID, YR_ID, PRLT_ID, LEAVEALL_ALLOWED, LEAVE_ALLOWED_MON, LEAVE_YEAR_BGN FROM PR_LEAVE_ALLOWED";


        try
        {
            // Payroll Detail
            DataRow oOrderRow;

            con = new SqlConnection(cn);
            con.Open();
            dbTransaction = con.BeginTransaction();
            DataSet oDS = new DataSet();
            SqlDataAdapter oOrdersDataAdapter = new SqlDataAdapter(new SqlCommand(strSql, con, dbTransaction));
            SqlCommandBuilder oOrdersCmdBuilder = new SqlCommandBuilder(oOrdersDataAdapter);
            //oOrdersDataAdapter.MissingSchemaAction = MissingSchemaAction.AddWithKey;
            //oOrdersDataAdapter.Fill(oDS, "PR_LEAVE_ALLOWED");
            oOrdersDataAdapter.FillSchema(oDS, SchemaType.Source, "PR_LEAVE_ALLOWED");
            oOrderRow = oDS.Tables["PR_LEAVE_ALLOWED"].NewRow();

            // 5 fields
            oOrderRow["CMP_BRANCH_ID"] = bId;
            oOrderRow["EMP_ID"] = empId;
            oOrderRow["YR_ID"] = yId;
            oOrderRow["PRLT_ID"] = typId;

            if (Yearlyallowed != "")
            {
                oOrderRow["LEAVEALL_ALLOWED"] = Yearlyallowed;
            }
            else
            {
                oOrderRow["LEAVEALL_ALLOWED"] = 0;
            }
            if (monAllowed != "")
            {
                oOrderRow["LEAVE_ALLOWED_MON"] = monAllowed;
            }
            else
            {
                oOrderRow["LEAVE_ALLOWED_MON"] = 0;
            }
            if (YerBigBal != "")
            {
                oOrderRow["LEAVE_YEAR_BGN"] = YerBigBal;
            }
            else
            {
                oOrderRow["LEAVE_YEAR_BGN"] = 0;
            }

            oDS.Tables["PR_LEAVE_ALLOWED"].Rows.Add(oOrderRow);
            oOrdersDataAdapter.Update(oDS, "PR_LEAVE_ALLOWED");
            dbTransaction.Commit();
            //con.Close();
            return "Successfully Applied.";
        }
        catch (Exception ex)
        {
            return ex.Message.ToString();
        }
        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }
    }

    public DataSet FindCarryForward(string bId, string eId, string year, string tId, string eDsg)// Developed By Md. Sydur Rahman (28-Oct-13)
    {
        string strQuery = "SELECT  ISNULL(LA.LEAVE_YEAR_END, '0') AS LEAVE_YEAR_END," +
                                   "ISNULL(LT.PRLP_CFORWARD, 'No') AS PRLP_CFORWARD," +
                                   "LT.DSG_ID," +
                                   "LT.PRLT_ID," +
                                   "Y.YR_YEAR," +
                                   "LA.EMP_ID " +
                            "FROM   PR_LEAVE_ALLOWED LA INNER JOIN PR_LEAVE_POLICY LT " +
                                   "ON LA.CMP_BRANCH_ID = LT.CMP_BRANCH_ID AND LA.PRLT_ID = LT.PRLT_ID INNER JOIN HR_YEAR Y " +
                                   "ON LA.YR_ID = Y.YR_ID " +
                           "WHERE   LA.CMP_BRANCH_ID = '" + bId + "' " +
                                   "AND LT.DSG_ID = '" + eDsg + "' " +
                                   "AND LA.EMP_ID = '" + eId + "' " +
                                   "AND LT.PRLT_ID = '" + tId + "' " +
                                   "AND Y.YR_YEAR = '" + year + "'";



        try
        {
            con = new SqlConnection(cn);
            con.Open();
            DataSet oDS = new DataSet();
            SqlDataAdapter odaData = new SqlDataAdapter(new SqlCommand(strQuery, con));
            odaData.Fill(oDS, "EMP_LEAVE_TYPE");
            return oDS;
        }
        catch (Exception ex)
        {
            return null;
        }
        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }
    }

    public string InsertSelfEmpRoster(string branch, string empID, string rType, string rSche, string rDetails) //Developed By: Md. Sydur rahman (05-Nov-13)
    {
        string strSql;
        strSql = "SELECT CMP_BRANCH_ID, EMP_ID, PRR_ID, PR_ROSTER_ID, PREMP_ROS_DETAILS FROM PR_EMP_ROSTER ";


        try
        {
            DataRow oOrderRow;

            con = new SqlConnection(cn);
            con.Open();
            dbTransaction = con.BeginTransaction();
            DataSet oDS = new DataSet();
            SqlDataAdapter oOrdersDataAdapter = new SqlDataAdapter(new SqlCommand(strSql, con, dbTransaction));
            SqlCommandBuilder oOrdersCmdBuilder = new SqlCommandBuilder(oOrdersDataAdapter);
            oOrdersDataAdapter.FillSchema(oDS, SchemaType.Source);

            DataTable dtAddSchedule = oDS.Tables["Table"];
            dtAddSchedule.TableName = "PR_EMP_ROSTER";

            oOrderRow = oDS.Tables["PR_EMP_ROSTER"].NewRow();

            oOrderRow["CMP_BRANCH_ID"] = branch;
            oOrderRow["EMP_ID"] = empID;
            oOrderRow["PRR_ID"] = rType;
            oOrderRow["PR_ROSTER_ID"] = rSche;
            oOrderRow["PREMP_ROS_DETAILS"] = rDetails;


            oDS.Tables["PR_EMP_ROSTER"].Rows.Add(oOrderRow);
            oOrdersDataAdapter.Update(oDS, "PR_EMP_ROSTER");
            dbTransaction.Commit();
            //con.Close();
            return "Saved Successfully.";
        }
        catch (Exception ex)
        {
            return ex.Message.ToString();
        }
        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }
    }
    /*asad 
     * dated: 16-Nov-14
     * last Month last 7days roster info of a year
     */
    public DataSet GetLast7daysRosterInfoHrYr(string empId, string HrYrId, string bid) /*  asad */
    {

        string strQuery = " SELECT DATENAME(weekday, ROSTER_DATE) as [day_name], ROSTER_DATE, PRR_ID, EMP_ID, COUNT(*) OVER() AS [Total_Rows]  FROM PR_EMP_ROSTER "
               + " WHERE  MONTH_ID=(SELECT MAX( MONTH_ID) FROM HR_MONTH WHERE YR_ID='" + HrYrId + "')AND EMP_ID='" + empId + "' and CMP_BRANCH_ID='" + bid + "' "
               + " and ROSTER_DATE >= DATEADD(day,-6,  "
               + "     ( "
               + "     SELECT MONTH_END_DATE FROM HR_MONTH WHERE YR_ID='" + HrYrId + "' "
               + "     and MONTH_ID=(SELECT MAX( MONTH_ID) FROM HR_MONTH WHERE YR_ID='" + HrYrId + "' "
               + "     ) "
               + " )) "
               + "  order by ROSTER_DATE  desc ";

        var con = new SqlConnection(cn);
        try
        {

            DataSet oDS = new DataSet();
            SqlDataAdapter odaData = new SqlDataAdapter(new SqlCommand(strQuery, con));
            odaData.Fill(oDS, "PR_EMP_ROSTER");
            return oDS;
        }
        catch (Exception ex)
        {
            return null;
        }
        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }
    }

    public DataSet GetHrYrMonthInfo(string HrYr, string MonthVal,string branchId) /*  asad */
    {

        string strQuery = " SELECT YR_ID, MONTH_ID, MONTH_START_DATE,MONTH_END_DATE,MONTH_TOTAL_DAYS FROM HR_MONTH "
                         + " WHERE  MONTH_VALUE='" + MonthVal + "'  and YR_ID=(SELECT YR_ID FROM HR_YEAR WHERE  CMP_BRANCH_ID='" + branchId + "' and YR_YEAR='" + HrYr + "') ";
        var con = new SqlConnection(cn);
        try
        {

            DataSet oDS = new DataSet();
            SqlDataAdapter odaData = new SqlDataAdapter(new SqlCommand(strQuery, con));
            odaData.Fill(oDS, "HR_MONTH");
            return oDS;
        }
        catch (Exception ex)
        {
            return null;
        }
        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }
    }
    public string GetDayName(string date) /*  asad */
    {
        string dayName = "";
        string strQuery = "SELECT   DATENAME(weekday, '" + date + "') [day_name] ";
        var con = new SqlConnection(cn);
        try
        {
            DataSet oDS = new DataSet();
            SqlDataAdapter odaData = new SqlDataAdapter(new SqlCommand(strQuery, con));
            odaData.Fill(oDS, "dayName");
            dayName = oDS.Tables[0].Rows[0]["day_name"].ToString();
        }
        catch (Exception ex)
        {
            return null;
        }
        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }
        return dayName;
    }


    public string InsertEmpHistory(string branch, string year, string empID, //Developed By: Md. Sydur rahman (06-Nov-13)
                                    string preDesg, string efDate, string amountType,
                                    string newDesg, string preStatus, string newStatus,
                                    string amount, string curAppDate, string nextAppDate,
                                    string attFile, string remarks)
    {

        string strSql;
        strSql = "SELECT CMP_BRANCH_ID, YR_ID, EMP_ID, DSG_ID, PREH_ASSIGNING_DATE, PREH_AMOUNT_TYPE, NEW_DSG_ID,"
                + " PREH_P_STATUS, PREH_N_STATUS, PREH_AMOUNT, CURRENT_APPRAISAL_DATE, NEXT_APPRAISAL_DATE, ATTACH_FILE, PREH_REMARKS  FROM PR_EMPLOYEE_HISTORY ";
        try
        {
            DataRow oOrderRow;
            con = new SqlConnection(cn);
            con.Open();
            dbTransaction = con.BeginTransaction();
            DataSet oDS = new DataSet();
            SqlDataAdapter oOrdersDataAdapter = new SqlDataAdapter(new SqlCommand(strSql, con, dbTransaction));
            SqlCommandBuilder oOrdersCmdBuilder = new SqlCommandBuilder(oOrdersDataAdapter);
            oOrdersDataAdapter.FillSchema(oDS, SchemaType.Source);

            DataTable dtAddSchedule = oDS.Tables["Table"];
            dtAddSchedule.TableName = "PR_EMPLOYEE_HISTORY";

            oOrderRow = oDS.Tables["PR_EMPLOYEE_HISTORY"].NewRow();

            oOrderRow["CMP_BRANCH_ID"] = branch;
            oOrderRow["YR_ID"] = year;
            oOrderRow["EMP_ID"] = empID;
            oOrderRow["DSG_ID"] = preDesg;
            oOrderRow["PREH_ASSIGNING_DATE"] = efDate;
            oOrderRow["PREH_AMOUNT_TYPE"] = amountType;
            oOrderRow["NEW_DSG_ID"] = newDesg;
            oOrderRow["PREH_P_STATUS"] = preStatus;
            oOrderRow["PREH_N_STATUS"] = newStatus;
            oOrderRow["PREH_AMOUNT"] = amount;
            oOrderRow["CURRENT_APPRAISAL_DATE"] = curAppDate;
            oOrderRow["NEXT_APPRAISAL_DATE"] = nextAppDate;
            oOrderRow["ATTACH_FILE"] = attFile;
            oOrderRow["PREH_REMARKS"] = remarks;

            oDS.Tables["PR_EMPLOYEE_HISTORY"].Rows.Add(oOrderRow);
            oOrdersDataAdapter.Update(oDS, "PR_EMPLOYEE_HISTORY");
            dbTransaction.Commit();
            return "Saved Successfully.";
        }
        catch (Exception ex)
        {
            return ex.Message.ToString();
        }
        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }
    }

    public string InsertEmpAction(string branch, string empID, string ActionType, string aDate, string aReason, string aRemarks) //Developed By: Md. Sydur rahman (13-Nov-13)
    {

        string strSql;
        strSql = "SELECT CMP_BRANCH_ID, EMP_ID, EDA_TYPE_OF_ACTION, EDA_DATE, EDA_REASON, EDA_REMARKS FROM HR_EMP_DISCIPLINARY_ACTION ";
        try
        {
            DataRow oOrderRow;
            con = new SqlConnection(cn);
            con.Open();
            dbTransaction = con.BeginTransaction();
            DataSet oDS = new DataSet();
            SqlDataAdapter oOrdersDataAdapter = new SqlDataAdapter(new SqlCommand(strSql, con, dbTransaction));
            SqlCommandBuilder oOrdersCmdBuilder = new SqlCommandBuilder(oOrdersDataAdapter);
            oOrdersDataAdapter.FillSchema(oDS, SchemaType.Source);

            DataTable dtAddSchedule = oDS.Tables["Table"];
            dtAddSchedule.TableName = "HR_EMP_DISCIPLINARY_ACTION";

            oOrderRow = oDS.Tables["HR_EMP_DISCIPLINARY_ACTION"].NewRow();

            oOrderRow["CMP_BRANCH_ID"] = branch;
            oOrderRow["EMP_ID"] = empID;
            oOrderRow["EDA_TYPE_OF_ACTION"] = ActionType;
            oOrderRow["EDA_DATE"] = aDate;
            oOrderRow["EDA_REASON"] = aReason;
            oOrderRow["EDA_REMARKS"] = aRemarks;


            oDS.Tables["HR_EMP_DISCIPLINARY_ACTION"].Rows.Add(oOrderRow);
            oOrdersDataAdapter.Update(oDS, "HR_EMP_DISCIPLINARY_ACTION");
            dbTransaction.Commit();
            return "Saved Successfully.";
        }
        catch (Exception ex)
        {
            return ex.Message.ToString();
        }
        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }
    }

    public DataSet PromotionHistory(string eID)
    {
        string strProm = "SELECT EH.DSG_ID, PD.DSG_TITLE, EH.PREH_ASSIGNING_DATE "
                       + "FROM PR_EMPLOYEE_HISTORY EH, PR_DESIGNATION PD "
                       + "WHERE EH.DSG_ID=PD.DSG_ID AND  EH.PREH_AMOUNT_TYPE='P' AND  EH.EMP_ID='" + eID + "' ";



        try
        {
            con = new SqlConnection(cn);
            con.Open();
            DataSet oDS = new DataSet();
            SqlDataAdapter odaData = new SqlDataAdapter(new SqlCommand(strProm, con));
            odaData.Fill(oDS, "HR_PROMOTION_HISTORY");
            return oDS;
        }
        catch (Exception ex)
        {
            return null;
        }
        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }
    }

    public DataSet GetEmpStatus(string bId, string empId)
    {
        string strQuery = "select EMP_STATUS from pr_employee_list where CMP_BRANCH_ID='" + bId + "' AND EMP_ID='" + empId + "'";

        try
        {
            con = new SqlConnection(cn);
            con.Open();

            DataSet oDS = new DataSet();
            SqlDataAdapter odaData = new SqlDataAdapter(new SqlCommand(strQuery, con));
            odaData.Fill(oDS, "EMPLOYEE_STATUS");
            return oDS;
        }
        catch (Exception ex)
        {
            return null;
        }
        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }
    }


    public void RelaodFirstEdit(string bId, string yID, string eId)
    {

        string updateString = "UPDATE PR_LEAVE_ALLOWED SET  LEAVEALL_ALLOWED = '0', LEAVE_ALLOWED_MON = '0' "
                        + "WHERE EMP_ID = '" + eId + "' AND CMP_BRANCH_ID = '" + bId + "' AND YR_ID = '" + yID + "'";

        try
        {
            con = new SqlConnection(cn);
            con.Open();
            con.Open();
            SqlCommand cmd = new SqlCommand(updateString, con);
            cmd.ExecuteNonQuery();
        }
        catch (Exception)
        {
        }
        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }
    }

    public DataSet GetLeaveType(string bId, string empId, string deg, string hrYear, string empSta)
    {
        string strQuery = "select LP.PRLT_ID, PRLT_TITLE, PRLP_DAYS, PRLP_ALLOWANCE, PRLP_MON_DAYS, TYP_CODE from PR_LEAVE_POLICY LP, PR_LEAVE_TYPE LT "
                        + "where DSG_ID='" + deg + "' and YR_ID='" + hrYear + "' and TYP_CODE='" + empSta + "' and LP.CMP_BRANCH_ID='" + bId + "' AND LP.PRLT_ID=LT.PRLT_ID ";

        try
        {
            con = new SqlConnection(cn);
            con.Open();

            DataSet oDS = new DataSet();
            SqlDataAdapter odaData = new SqlDataAdapter(new SqlCommand(strQuery, con));
            odaData.Fill(oDS, "LEAVE_TYPE");
            return oDS;
        }
        catch (Exception ex)
        {
            return null;
        }
        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }
    }
    public string ReloadLeaveAllowed(string bId, string yID, string eId, string lvTypID, string YrlyDays, string MonDays, string dept)
    {
        var con = new SqlConnection(cn);
        string updateString = "UPDATE PR_LEAVE_ALLOWED SET  LEAVEALL_ALLOWED = '" + YrlyDays + "', LEAVE_ALLOWED_MON = '" + MonDays + "' "
                        + "WHERE EMP_ID = '" + eId + "' AND CMP_BRANCH_ID = '" + bId + "' AND YR_ID = '" + yID + "' AND PRLT_ID = '" + lvTypID + "' "
                        + "AND DPT_ID='" + dept + "'";

        try
        {
            con.Open();
            SqlCommand cmd = new SqlCommand(updateString, con);
            cmd.ExecuteNonQuery();
            string dd = "";
            return "Data reloaded successfully...";
        }
        catch (Exception ex)
        {
            return ex.Message.ToString();
        }
        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }
    }
    public DataSet ChkEmpGeneralLeave(string bId, string eId, string yId, string dept)
    {
        string strQuery = "SELECT * FROM PR_LEAVE_ALLOWED "
                            + "WHERE CMP_BRANCH_ID='" + bId + "' AND  EMP_ID='" + eId + "' AND "
                            + "YR_ID='" + yId + "' AND DPT_ID='" + dept + "'";
        var con = new SqlConnection(cn);

        try
        {

            DataSet oDS = new DataSet();
            SqlDataAdapter odaData = new SqlDataAdapter(new SqlCommand(strQuery, con));
            odaData.Fill(oDS, "PR_LEAVE_ALLOWED");
            return oDS;
        }
        catch (Exception ex)
        {
            return null;
        }
        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }
    }
    public string RemoveLeaveGeneral(string bId, string eId, string yId, string dept)
    {
        string strSql;

        strSql = "DELETE from PR_LEAVE_ALLOWED "
                + "WHERE CMP_BRANCH_ID='" + bId + "' AND  EMP_ID='" + eId + "' AND "
                            + "YR_ID='" + yId + "' AND DPT_ID='" + dept + "'";
        var con = new SqlConnection(cn);
        try
        {

            con.Open();
            dbTransaction = con.BeginTransaction();
            SqlCommand olcmd = new SqlCommand(strSql, con, dbTransaction);
            olcmd.Connection = con;
            olcmd.ExecuteNonQuery();
            dbTransaction.Commit();
            //con.Close();

            return "Saved successfully";
        }
        catch (Exception ex)
        {
            return ex.Message.ToString();
        }
        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }
    }
    public DataSet GetDeptEmpInfo(string bId, string dept)
    {


        string strQuery = "SELECT SET_LEVEL, EMP_ID,CAST(ISNULL(EMP_TITLE, '') AS VARCHAR(100)) + ' ' + CAST(EMP_NAME AS VARCHAR(500)) AS ENAME, EL.DSG_ID, "
                            + "DP.DPT_ID, DP.DPT_NAME, CAST(SET_CODE AS VARCHAR) + ' ' + CAST(DSG_TITLE AS VARCHAR(200)) AS DNAME, ET.TYP_TYPE, "
                            + "EMP_GENDER, EMP_STATUS, TOT_INCOME, TAX_PER_MONTH, TOT_TAX "
                            + "FROM PR_EMPLOYEE_LIST EL, PR_DESIGNATION DG, HR_EMP_TYPE ET, PR_DEPARTMENT DP "
                            + "WHERE EL.DSG_ID=DG.DSG_ID AND EL.CMP_BRANCH_ID='" + bId + "' AND DP.DPT_ID = EL.DPT_ID  "
                            + "AND EL.DPT_ID = '" + dept + "' "
                            + "AND EL.EMP_STATUS=ET.TYP_CODE AND ET.PAYROLL_ALLOWED='Y' "
                            + "ORDER BY CONVERT(NUMERIC(8, 2), SET_LEVEL), ENAME ASC";

        var con = new SqlConnection(cn);
        try
        {

            DataSet oDS = new DataSet();
            SqlDataAdapter odaData = new SqlDataAdapter(new SqlCommand(strQuery, con));
            odaData.Fill(oDS, "PR_EMPLOYEE_LIST");
            return oDS;
        }
        catch (Exception ex)
        {
            return null;
        }
        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }
    }


    public DataSet GetDeptEmpInfo(string bId, string dept, string strFilter, string blank, string blank2)
    {
        string strQuery = "SELECT SET_LEVEL, EMP_ID,CAST(ISNULL(EMP_TITLE, '') AS VARCHAR(100)) + ' ' + CAST(EMP_NAME AS VARCHAR(500)) AS ENAME, EL.DSG_ID, "
                            + "DP.DPT_ID, DP.DPT_NAME, CAST(SET_CODE AS VARCHAR) + ' ' + CAST(DSG_TITLE AS VARCHAR(200)) AS DNAME, ET.TYP_TYPE, "
                            + "EMP_GENDER, EMP_STATUS, TOT_INCOME, TAX_PER_MONTH, TOT_TAX "
                            + "FROM PR_EMPLOYEE_LIST EL, PR_DESIGNATION DG, HR_EMP_TYPE ET, PR_DEPARTMENT DP "
                            + "WHERE EL.DSG_ID=DG.DSG_ID AND EL.CMP_BRANCH_ID='" + bId + "' AND DP.DPT_ID = EL.DPT_ID " + strFilter + "  "
                            + "AND EL.DPT_ID = '" + dept + "' "
                            + "AND EL.EMP_STATUS=ET.TYP_CODE AND ET.PAYROLL_ALLOWED='Y' "
                            + "ORDER BY CONVERT(NUMERIC(8, 2), SET_LEVEL), ENAME ASC";

        var con = new SqlConnection(cn);
        try
        {

            DataSet oDS = new DataSet();
            SqlDataAdapter odaData = new SqlDataAdapter(new SqlCommand(strQuery, con));
            odaData.Fill(oDS, "PR_EMPLOYEE_LIST");
            return oDS;
        }
        catch (Exception ex)
        {
            return null;
        }
        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }
    }


    public DataSet GetDeptEmpInfoForMissingRoster(string bId, string dept, string strFilter, string hrYearId, string MonthId,string rosterDate)
    {
        string strQuery = @"SELECT SET_LEVEL, RO.ROSTER_DATE, El.EMP_ID,CAST(ISNULL(EMP_TITLE, '') AS VARCHAR(100)) + ' ' + CAST(EMP_NAME AS VARCHAR(500)) AS ENAME, EL.DSG_ID, "
                            + "DP.DPT_ID, DP.DPT_NAME, CAST(SET_CODE AS VARCHAR) + ' ' + CAST(DSG_TITLE AS VARCHAR(200)) AS DNAME, ET.TYP_TYPE, "
                            + "EMP_GENDER, EMP_STATUS, TOT_INCOME, TAX_PER_MONTH, TOT_TAX "
                            + " FROM PR_EMPLOYEE_LIST EL Left Join PR_EMP_ROSTER as RO on RO.EMP_ID=EL.EMP_ID and RO.CMP_BRANCH_ID='" + bId + "' AND "
                            + " RO.YR_ID='" + hrYearId + "' AND RO.MONTH_ID='" + MonthId + "' AND RO.ROSTER_DATE='" + rosterDate + "'" 
                            + " , PR_DESIGNATION DG, HR_EMP_TYPE ET, PR_DEPARTMENT DP "
                            + "WHERE EL.DSG_ID=DG.DSG_ID AND EL.CMP_BRANCH_ID='" + bId + "' AND DP.DPT_ID = EL.DPT_ID " + strFilter + "  "
                            + "AND EL.DPT_ID = '" + dept + "' "
                            + "AND EL.EMP_STATUS=ET.TYP_CODE AND ET.PAYROLL_ALLOWED='Y' "
                            + "ORDER BY CONVERT(NUMERIC(8, 2), SET_LEVEL), ENAME ASC";
     



        var con = new SqlConnection(cn);
        try
        {

            DataSet oDS = new DataSet();
            SqlDataAdapter odaData = new SqlDataAdapter(new SqlCommand(strQuery, con));
            odaData.Fill(oDS, "PR_EMPLOYEE_LIST");
            return oDS;
        }
        catch (Exception ex)
        {
            return null;
        }
        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }
    }

    public DataSet GetDeptEmpInfo(string bId, string dept, string gId)
    {
        string strQuery = "SELECT SET_LEVEL, EMP_ID,CAST(ISNULL(EMP_TITLE, '') AS VARCHAR) + ' ' + CAST(EMP_NAME AS VARCHAR) AS ENAME, EL.DSG_ID, "
                            + "DP.DPT_ID, DP.DPT_NAME, CAST(SET_CODE AS VARCHAR) + ' ' + CAST(DSG_TITLE AS VARCHAR) AS DNAME, ET.TYP_TYPE, "
                            + "EMP_GENDER, EMP_STATUS, TOT_INCOME, TAX_PER_MONTH, TOT_TAX "
                            + "FROM PR_EMPLOYEE_LIST EL, PR_DESIGNATION DG, HR_EMP_TYPE ET, PR_DEPARTMENT DP "
                            + "WHERE EL.DSG_ID=DG.DSG_ID AND EL.CMP_BRANCH_ID='" + bId + "' AND DP.DPT_ID = EL.DPT_ID "
                            + "AND EL.DPT_ID = '" + dept + "' AND EL.DSG_ID='" + gId + "' "
                            + "AND EL.EMP_STATUS=ET.TYP_CODE AND ET.PAYROLL_ALLOWED='Y' "
                            + "ORDER BY CONVERT(NUMERIC(8, 2), SET_LEVEL), ENAME ASC";

        var con = new SqlConnection(cn);
        try
        {

            DataSet oDS = new DataSet();
            SqlDataAdapter odaData = new SqlDataAdapter(new SqlCommand(strQuery, con));
            odaData.Fill(oDS, "PR_EMPLOYEE_LIST");
            return oDS;
        }
        catch (Exception ex)
        {
            return null;
        }
        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }
    }
    public DataSet GetDeptEmpInfo(string bId, string dept, string gId, string eId)
    {
        string strQuery = "SELECT SET_LEVEL, EMP_ID,CAST(ISNULL(EMP_TITLE, '') AS VARCHAR) + ' ' + CAST(EMP_NAME AS VARCHAR) AS ENAME, EL.DSG_ID, "
                            + "DP.DPT_ID, DP.DPT_NAME, CAST(SET_CODE AS VARCHAR) + ' ' + CAST(DSG_TITLE AS VARCHAR) AS DNAME, ET.TYP_TYPE, "
                            + "EMP_GENDER, EMP_STATUS, TOT_INCOME, TAX_PER_MONTH, TOT_TAX "
                            + "FROM PR_EMPLOYEE_LIST EL, PR_DESIGNATION DG, HR_EMP_TYPE ET, PR_DEPARTMENT DP "
                            + "WHERE EL.DSG_ID=DG.DSG_ID AND EL.CMP_BRANCH_ID='" + bId + "' AND DP.DPT_ID = EL.DPT_ID "
                            + "AND EL.DPT_ID = '" + dept + "' AND EL.DSG_ID='" + gId + "' AND EL.EMP_ID='" + eId + "' "
                            + "AND EL.EMP_STATUS=ET.TYP_CODE AND ET.PAYROLL_ALLOWED='Y' "
                            + "ORDER BY CONVERT(NUMERIC(8, 2), SET_LEVEL), ENAME ASC";

        var con = new SqlConnection(cn);
        try
        {

            DataSet oDS = new DataSet();
            SqlDataAdapter odaData = new SqlDataAdapter(new SqlCommand(strQuery, con));
            odaData.Fill(oDS, "PR_EMPLOYEE_LIST");
            return oDS;
        }
        catch (Exception ex)
        {
            return null;
        }
        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }
    }

    public DataSet GetAllEmpInfoLv(string bId, string gId)
    {
        string strQuery = "SELECT SET_LEVEL, EMP_ID,CAST(ISNULL(EMP_TITLE, '') AS VARCHAR(100)) + ' ' + CAST(EMP_NAME AS VARCHAR(500)) AS ENAME, EL.DSG_ID, "
                            + "DP.DPT_ID, DP.DPT_NAME, CAST(SET_CODE AS VARCHAR) + ' ' + CAST(DSG_TITLE AS VARCHAR) AS DNAME, ET.TYP_TYPE, "
                            + "EMP_GENDER, EMP_STATUS, TOT_INCOME, TAX_PER_MONTH, TOT_TAX "
                            + "FROM PR_EMPLOYEE_LIST EL, PR_DESIGNATION DG, HR_EMP_TYPE ET, PR_DEPARTMENT DP "
                            + "WHERE EL.DSG_ID=DG.DSG_ID AND EL.CMP_BRANCH_ID='" + bId + "' AND DP.DPT_ID = EL.DPT_ID "
                            + "AND EL.EMP_STATUS=ET.TYP_CODE AND ET.PAYROLL_ALLOWED='Y' "
                            + "EL.DSG_ID='" + gId + "' "
                            + "ORDER BY CONVERT(NUMERIC(8, 2), SET_LEVEL), ENAME ASC";

        var con = new SqlConnection(cn);
        try
        {

            DataSet oDS = new DataSet();
            SqlDataAdapter odaData = new SqlDataAdapter(new SqlCommand(strQuery, con));
            odaData.Fill(oDS, "PR_EMPLOYEE_LIST");
            return oDS;
        }
        catch (Exception ex)
        {
            return null;
        }
        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }
    }
    public DataSet GetAllEmpInfoLv(string bId, string gId, string eId)
    {
        string strQuery = "SELECT SET_LEVEL, EMP_ID,CAST(ISNULL(EMP_TITLE, '') AS VARCHAR) + ' ' + CAST(EMP_NAME AS VARCHAR) AS ENAME, EL.DSG_ID, "
                            + "DP.DPT_ID, DP.DPT_NAME, CAST(SET_CODE AS VARCHAR) + ' ' + CAST(DSG_TITLE AS VARCHAR) AS DNAME, ET.TYP_TYPE, "
                            + "EMP_GENDER, EMP_STATUS, TOT_INCOME, TAX_PER_MONTH, TOT_TAX "
                            + "FROM PR_EMPLOYEE_LIST EL, PR_DESIGNATION DG, HR_EMP_TYPE ET, PR_DEPARTMENT DP "
                            + "WHERE EL.DSG_ID=DG.DSG_ID AND EL.CMP_BRANCH_ID='" + bId + "' AND DP.DPT_ID = EL.DPT_ID "
                            + "AND EL.EMP_STATUS=ET.TYP_CODE AND ET.PAYROLL_ALLOWED='Y' "
                            + "EL.DSG_ID='" + gId + "' AND EL.EMP_ID='" + eId + "' "
                            + "ORDER BY CONVERT(NUMERIC(8, 2), SET_LEVEL), ENAME ASC";

        var con = new SqlConnection(cn);
        try
        {

            DataSet oDS = new DataSet();
            SqlDataAdapter odaData = new SqlDataAdapter(new SqlCommand(strQuery, con));
            odaData.Fill(oDS, "PR_EMPLOYEE_LIST");
            return oDS;
        }
        catch (Exception ex)
        {
            return null;
        }
        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }
    }


    public string ApplyNextYear(string bId, string empId, string yId, string typId, string totYearlyallowed, string Yearlyallowed, string monAllowed, string YerBigBal, string dept) // Apply For Next Year
    {
        string strSql;


        strSql = "SELECT DPT_ID, ACTUAL_ALLOWED, CMP_BRANCH_ID, EMP_ID, YR_ID, PRLT_ID, LEAVEALL_ALLOWED, LEAVE_ALLOWED_MON, LEAVE_YEAR_BGN FROM PR_LEAVE_ALLOWED";

        var con = new SqlConnection(cn);
        try
        {
            // Leave Allowed
            DataRow oOrderRow;

            con.Open();
            dbTransaction = con.BeginTransaction();
            DataSet oDS = new DataSet();
            SqlDataAdapter oOrdersDataAdapter = new SqlDataAdapter(new SqlCommand(strSql, con, dbTransaction));
            SqlCommandBuilder oOrdersCmdBuilder = new SqlCommandBuilder(oOrdersDataAdapter);
            oOrdersDataAdapter.FillSchema(oDS, SchemaType.Source, "PR_LEAVE_ALLOWED");
            oOrderRow = oDS.Tables["PR_LEAVE_ALLOWED"].NewRow();

            // 5 fields
            oOrderRow["CMP_BRANCH_ID"] = bId;
            oOrderRow["EMP_ID"] = empId;
            oOrderRow["DPT_ID"] = dept;
            oOrderRow["YR_ID"] = yId;
            oOrderRow["PRLT_ID"] = typId;

            if (totYearlyallowed != "")
            {
                oOrderRow["LEAVEALL_ALLOWED"] = totYearlyallowed;
            }
            else
            {
                oOrderRow["LEAVEALL_ALLOWED"] = 0;
            }
            if (Yearlyallowed != "")
            {
                oOrderRow["ACTUAL_ALLOWED"] = Yearlyallowed;
            }
            else
            {
                oOrderRow["ACTUAL_ALLOWED"] = 0;
            }
            if (monAllowed != "")
            {
                oOrderRow["LEAVE_ALLOWED_MON"] = monAllowed;
            }
            else
            {
                oOrderRow["LEAVE_ALLOWED_MON"] = 0;
            }
            if (YerBigBal != "")
            {
                oOrderRow["LEAVE_YEAR_BGN"] = YerBigBal;
            }
            else
            {
                oOrderRow["LEAVE_YEAR_BGN"] = 0;
            }

            oDS.Tables["PR_LEAVE_ALLOWED"].Rows.Add(oOrderRow);
            oOrdersDataAdapter.Update(oDS, "PR_LEAVE_ALLOWED");
            dbTransaction.Commit();
            return "Successfully Applied.";
        }
        catch (Exception ex)
        {
            return ex.Message.ToString();
        }
        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }
    }


    public string CloseCurrentYear(string bId, string empId, string yId, string typId, string YendBal, string dept) //  Close Current Year
    {
        string strSql;

        strSql = "UPDATE PR_LEAVE_ALLOWED SET LEAVE_YEAR_END = '" + YendBal + "' "
                  + "WHERE EMP_ID = '" + empId + "' AND CMP_BRANCH_ID='" + bId + "' AND YR_ID='" + yId + "' AND PRLT_ID='" + typId + "' "
                  + "AND DPT_ID='" + dept + "'";

        SqlConnection con = new SqlConnection(cn);

        try
        {
            con.Open();
            dbTransaction = con.BeginTransaction();
            SqlCommand cmd = new SqlCommand(strSql, con, dbTransaction);
            cmd.Connection = con;
            cmd.ExecuteNonQuery();
            dbTransaction.Commit();
            return "Current year successfully closed.";
        }
        catch (Exception ex)
        {
            return ex.Message.ToString();
        }
        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }

    }
    public string RemoveAllowedLeave(string bId, string hrYear, string dept)
    {
        string strSql;

        strSql = "DELETE from PR_LEAVE_ALLOWED "
                + "WHERE CMP_BRANCH_ID='" + bId + "' AND YR_ID='" + hrYear + "' AND DPT_ID='" + dept + "'";
        var con = new SqlConnection(cn);
        try
        {

            con.Open();
            dbTransaction = con.BeginTransaction();
            SqlCommand olcmd = new SqlCommand(strSql, con, dbTransaction);
            olcmd.Connection = con;
            olcmd.ExecuteNonQuery();
            dbTransaction.Commit();
            //con.Close();

            return "Deleted Successfully";
        }
        catch (Exception ex)
        {
            return ex.Message.ToString();
        }
        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }
    }
    public string SaveEmployeeLeave(string bId, string empId, string yId, string typId, string totYearlyallowed, string Yearlyallowed, string monAllowed, string dept) //  Employee LEAVE
    {
        string strSql;


        strSql = "SELECT DPT_ID, ACTUAL_ALLOWED, CMP_BRANCH_ID, EMP_ID, YR_ID, PRLT_ID, LEAVEALL_ALLOWED, LEAVE_ALLOWED_MON FROM PR_LEAVE_ALLOWED";

        var con = new SqlConnection(cn);
        try
        {
            // Payroll Detail
            DataRow oOrderRow;

            con.Open();
            dbTransaction = con.BeginTransaction();
            DataSet oDS = new DataSet();
            SqlDataAdapter oOrdersDataAdapter = new SqlDataAdapter(new SqlCommand(strSql, con, dbTransaction));
            SqlCommandBuilder oOrdersCmdBuilder = new SqlCommandBuilder(oOrdersDataAdapter);
            //oOrdersDataAdapter.MissingSchemaAction = MissingSchemaAction.AddWithKey;
            //oOrdersDataAdapter.Fill(oDS, "PR_LEAVE_ALLOWED");
            oOrdersDataAdapter.FillSchema(oDS, SchemaType.Source, "PR_LEAVE_ALLOWED");
            oOrderRow = oDS.Tables["PR_LEAVE_ALLOWED"].NewRow();

            // 5 fields
            oOrderRow["CMP_BRANCH_ID"] = bId;
            oOrderRow["EMP_ID"] = empId;
            oOrderRow["DPT_ID"] = dept;
            oOrderRow["YR_ID"] = yId;
            oOrderRow["PRLT_ID"] = typId;

            if (totYearlyallowed != "")
            {
                oOrderRow["LEAVEALL_ALLOWED"] = totYearlyallowed;
            }
            else
            {
                oOrderRow["LEAVEALL_ALLOWED"] = 0;
            }

            if (Yearlyallowed != "")
            {
                oOrderRow["ACTUAL_ALLOWED"] = Yearlyallowed;
            }
            else
            {
                oOrderRow["ACTUAL_ALLOWED"] = 0;
            }
            if (monAllowed != "")
            {
                oOrderRow["LEAVE_ALLOWED_MON"] = monAllowed;
            }
            else
            {
                oOrderRow["LEAVE_ALLOWED_MON"] = 0;
            }

            oDS.Tables["PR_LEAVE_ALLOWED"].Rows.Add(oOrderRow);
            oOrdersDataAdapter.Update(oDS, "PR_LEAVE_ALLOWED");
            dbTransaction.Commit();
            //con.Close();
            return "Saved successfully";
        }
        catch (Exception ex)
        {
            return ex.Message.ToString();
        }
        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }
    }
    public string ReloadLeaveAllowed(string bId, string yID, string eId, string lvTypID, string YrlyDays, string MonDays)
    {

        string updateString = "UPDATE PR_LEAVE_ALLOWED SET  LEAVEALL_ALLOWED = '" + YrlyDays + "', LEAVE_ALLOWED_MON = '" + MonDays + "' "
                        + "WHERE EMP_ID = '" + eId + "' AND CMP_BRANCH_ID = '" + bId + "' AND YR_ID = '" + yID + "' AND PRLT_ID = '" + lvTypID + "'";

        try
        {
            con = new SqlConnection(cn);
            con.Open();
            SqlCommand cmd = new SqlCommand(updateString, con);
            cmd.ExecuteNonQuery();
            string dd = "";
            return "Data reloaded successfully...";
        }
        catch (Exception ex)
        {
            return ex.Message.ToString();
        }
        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }
    }


    #region PROVIDEND_FUND
    /// <summary>
    /// Developer: Md. Asaduzzaman
    /// Dated: 24-Nov-2013
    /// </summary>
    public string SaveProvidendFundPolicy(params string[] strParam)
    {
        string strSql = string.Empty; ;
        strSql = "SELECT PFP_PROV_FUND_EMPE,PFP_PROV_FUND_EMPR,PFP_PENSION_SCHME_EMPE,PFP_PENSION_SCHME_EMPR, "
                + " PFP_DEPO_LINK_INSU_EMPE,PFP_DEPO_LINK_INSU_EMPR,PFP_ADMIN_CHARGE_EMPE,PFP_ADMIN_CHARGE_EMPR,PFP_DEPO_LINK_INSU_SCHME_EMPE, "
                + " PFP_DEPO_LINK_INSU_SCHME_EMPR,PFP_DESCRIPTION,YR_ID,CMP_BRANCH_ID,YR_STATUS,PR_ITEM_ID FROM HR_PROVIDEND_FUND_POLICIY ";

        try
        {
            con = new SqlConnection(cn);
            con.Open();
            DataRow oOrderRow;
            dbTransaction = con.BeginTransaction();
            DataSet oDS = new DataSet();
            SqlDataAdapter oOrdersDataAdapter = new SqlDataAdapter(new SqlCommand(strSql, con, dbTransaction));
            SqlCommandBuilder oOrdersCmdBuilder = new SqlCommandBuilder(oOrdersDataAdapter);
            oOrdersDataAdapter.FillSchema(oDS, SchemaType.Source, "HR_PROVIDEND_FUND_POLICIY");
            oOrderRow = oDS.Tables["HR_PROVIDEND_FUND_POLICIY"].NewRow();

            oOrderRow["PFP_PROV_FUND_EMPE"] = strParam[0].ToString();
            oOrderRow["PFP_PROV_FUND_EMPR"] = strParam[1].ToString();
            oOrderRow["PFP_PENSION_SCHME_EMPE"] = strParam[2].ToString();
            oOrderRow["PFP_PENSION_SCHME_EMPR"] = strParam[3].ToString();
            oOrderRow["PFP_DEPO_LINK_INSU_EMPE"] = strParam[4].ToString();
            oOrderRow["PFP_DEPO_LINK_INSU_EMPR"] = strParam[5].ToString();
            oOrderRow["PFP_ADMIN_CHARGE_EMPE"] = strParam[6].ToString();
            oOrderRow["PFP_ADMIN_CHARGE_EMPR"] = strParam[7].ToString();
            oOrderRow["PFP_DEPO_LINK_INSU_SCHME_EMPE"] = strParam[8].ToString();
            oOrderRow["PFP_DEPO_LINK_INSU_SCHME_EMPR"] = strParam[9].ToString();
            oOrderRow["PFP_DESCRIPTION"] = strParam[10].ToString();
            oOrderRow["YR_ID"] = strParam[11].ToString();
            oOrderRow["CMP_BRANCH_ID"] = strParam[12].ToString();
            oOrderRow["YR_STATUS"] = strParam[13].ToString();
            oOrderRow["PR_ITEM_ID"] = strParam[14].ToString();

            oDS.Tables["HR_PROVIDEND_FUND_POLICIY"].Rows.Add(oOrderRow);
            oOrdersDataAdapter.Update(oDS, "HR_PROVIDEND_FUND_POLICIY");
            dbTransaction.Commit();
            return "Saved successfully";

        }
        catch (Exception ex)
        {
            // return ex.Message.ToString();
            return "Error in process...";
        }
        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }
    }

    /// <summary>
    /// Author: Tanjil Alam
    /// Date: 25/11/2013
    /// </summary>
    /// <param name="bId"></param>
    /// <param name="year"></param>
    /// <returns></returns>
    public DataSet GetItem(string bId, string year)
    {
        string strQuery = "select PF.PR_ITEM_ID,PI.PR_ITEM_TITLE from HR_PROVIDEND_FUND_POLICIY PF, PR_PAYROLL_ITEM PI  "
                        + "where PF.PR_ITEM_ID=PI.PR_ITEM_ID AND PF.YR_ID='" + year + "' "
                        + "AND PF.CMP_BRANCH_ID='" + bId + "' AND PF.YR_STATUS='R'";

        try
        {
            con = new SqlConnection(cn);
            con.Open();
            DataSet oDS = new DataSet();
            SqlDataAdapter odaData = new SqlDataAdapter(new SqlCommand(strQuery, con));
            odaData.Fill(oDS, "PROVIDENT_FUND");
            return oDS;
        }
        catch (Exception ex)
        {
            return null;
        }
        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }
    }

    /// <summary>
    ///  Author: Tanjil Alam
    /// Date: 25/11/2013
    /// </summary>
    /// <param name="bId"></param>
    /// <param name="year"></param>
    /// <returns></returns>

    public DataSet GetEmployeeProvident(string hrYear, string bID, string pMonth, string ItemID)
    {
        string strQuery = "SELECT EMP_NAME, PRST_AMT, EL.EMP_ID FROM PR_PAYROLL_DETAIL PD, PR_EMPLOYEE_LIST EL "
                        + "WHERE PD.CMP_BRANCH_ID='" + bID + "' AND PR_ITEM_ID='" + ItemID + "' AND PRM_ID='" + pMonth + "' AND "
                        + "PD.EMP_ID=EL.EMP_ID";

        try
        {
            con = new SqlConnection(cn);
            con.Open();
            DataSet oDS = new DataSet();
            SqlDataAdapter odaData = new SqlDataAdapter(new SqlCommand(strQuery, con));
            odaData.Fill(oDS, "EMP_PROVIDENT_FUND");
            return oDS;
        }
        catch (Exception ex)
        {
            return null;
        }
        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }
    }

    /// <summary>
    ///  Author: Tanjil Alam
    /// Date: 26/11/2013
    /// </summary>
    /// <param name="bId"></param>
    /// <param name="year"></param>
    /// <returns></returns>

    public DataSet GetProvidentPolicy(string hrYear, string bID, string ItemID)
    {
        string strQuery = "select PFP_PROV_FUND_EMPE,PFP_PROV_FUND_EMPR,PFP_PENSION_SCHME_EMPE,PFP_PENSION_SCHME_EMPR,PFP_DEPO_LINK_INSU_EMPE, "
                        + "PFP_DEPO_LINK_INSU_EMPR,PFP_ADMIN_CHARGE_EMPE,PFP_ADMIN_CHARGE_EMPR,PFP_DEPO_LINK_INSU_SCHME_EMPE, "
                        + "PFP_DEPO_LINK_INSU_SCHME_EMPR FROM HR_PROVIDEND_FUND_POLICIY "
                        + "where YR_ID='" + hrYear + "'AND CMP_BRANCH_ID='" + bID + "' AND PR_ITEM_ID='" + ItemID + "' ";

        try
        {
            con = new SqlConnection(cn);
            con.Open();
            DataSet oDS = new DataSet();
            SqlDataAdapter odaData = new SqlDataAdapter(new SqlCommand(strQuery, con));
            odaData.Fill(oDS, "EMP_PROVIDENT_POLICY");
            return oDS;
        }
        catch (Exception ex)
        {
            return null;
        }
        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }
    }


    #endregion

    #region MailSend
    //#######################################
    /*
     Developer: Asad   Dec,10-2013
     */
    public string SendMailCRMgeneral(string userName, string mailFrom, string mailPass, string mailSub, string message, string smtpHost, string smtpPort, string[] mob_email, string[] allEmailCC)
    {
        string FrmMail = "";
        string PassMail = "";
        string HostMail = "";
        string PortMail = "";
        int port;
        string SenderName = "";
        string SubEmail = "";
        string emailTO = "";
        int c = mob_email.Count();
        if (c > 0)
        {
            port = int.Parse(smtpPort);
            for (int i = 0; i < c; i++)
            {
                emailTO = mob_email[i].ToString();
                try
                {
                    MailAddress objFrom = new MailAddress(mailFrom, userName);
                    MailAddress objTo = new MailAddress(emailTO);

                    MailMessage msgMail = new MailMessage(objFrom, objTo);
                    //######################### CC ##############################
                    int cc = allEmailCC.Count();
                    if (cc > 0 && i == 0)
                    {
                        for (int j = 0; j < cc; j++)
                        {
                            MailAddress objBCC = new MailAddress(allEmailCC[j].ToString());
                            msgMail.Bcc.Add(objBCC);
                        }
                    }
                    //########################### End CC #################################
                    msgMail.IsBodyHtml = true;
                    msgMail.Priority = MailPriority.High;

                    msgMail.Subject = mailSub;
                    msgMail.Body = message;

                    SmtpClient objSMTP = new SmtpClient();
                    objSMTP.EnableSsl = false;
                    objSMTP.Timeout = 10000;
                    objSMTP.DeliveryMethod = SmtpDeliveryMethod.Network;
                    objSMTP.UseDefaultCredentials = false;

                    System.Net.NetworkCredential NetworkCred = new System.Net.NetworkCredential();
                    NetworkCred.UserName = mailFrom;
                    NetworkCred.Password = mailPass;
                    objSMTP.Credentials = NetworkCred;

                    objSMTP.Host = smtpHost;
                    objSMTP.Port = port;

                    objSMTP.Send(msgMail);

                }
                catch (Exception ex)
                {
                    return ex.Message.ToString();
                }

            }
        }
        return "Mail sent successfully...";
    }
    //########################################

    #endregion

    public DataSet GetTaskProgressInfo(string strQry) // Employee List from Payroll Sheet
    {
        string strQuery = strQry;


        try
        {
            con = new SqlConnection(cn);
            con.Open();
            DataSet oDS = new DataSet();
            SqlDataAdapter odaData = new SqlDataAdapter(new SqlCommand(strQuery, con));
            odaData.Fill(oDS, "task");
            return oDS;
        }
        catch (Exception ex)
        {
            return null;
        }
        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }
    }

    public string InsertEmpTransfer(string empID, string PreBranch, string NewBranch, string Description, string PosHeld, string DurFromDate, string DurToDate) //Developed By: Md. Sydur rahman (12-Dec-13)
    {

        string strSql;
        strSql = "SELECT EMP_ID, PREA_CMP_BRANCH_ID, NEW_CMP_BRANCH_ID, EMP_TRANSFER_DETAILS, POSITION_HELD, DURATION_FROM, DURATIO_TO FROM HR_EMP_TRANSFER ";
        try
        {
            DataRow oOrderRow;
            con = new SqlConnection(cn);
            con.Open();
            dbTransaction = con.BeginTransaction();
            DataSet oDS = new DataSet();
            SqlDataAdapter oOrdersDataAdapter = new SqlDataAdapter(new SqlCommand(strSql, con, dbTransaction));
            SqlCommandBuilder oOrdersCmdBuilder = new SqlCommandBuilder(oOrdersDataAdapter);
            oOrdersDataAdapter.FillSchema(oDS, SchemaType.Source);

            DataTable dtAddSchedule = oDS.Tables["Table"];
            dtAddSchedule.TableName = "HR_EMP_TRANSFER";

            oOrderRow = oDS.Tables["HR_EMP_TRANSFER"].NewRow();

            oOrderRow["EMP_ID"] = empID;
            oOrderRow["PREA_CMP_BRANCH_ID"] = PreBranch;
            oOrderRow["NEW_CMP_BRANCH_ID"] = NewBranch;
            oOrderRow["EMP_TRANSFER_DETAILS"] = Description;
            oOrderRow["POSITION_HELD"] = PosHeld;
            oOrderRow["DURATION_FROM"] = DurFromDate;
            oOrderRow["DURATIO_TO"] = DurToDate;


            oDS.Tables["HR_EMP_TRANSFER"].Rows.Add(oOrderRow);
            oOrdersDataAdapter.Update(oDS, "HR_EMP_TRANSFER");
            dbTransaction.Commit();
            return "Saved Successfully.";
        }
        catch (Exception ex)
        {
            return ex.Message.ToString();
        }
        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }
    }

    public string InsertEmpTargate(string Branch, string empID, string TerType, string TerAmountYearly, string TerAmountMon, string YrID) //Developed By: Md. Sydur rahman (17-Dec-13)
    {

        string strSql;
        strSql = "SELECT CMP_BRANCH_ID, EMP_ID, TER_TYPE, TER_YEAR_AMMOUNT, TER_MONTH_AMMOUNT, YR_ID FROM HR_EMP_TARGET ";
        try
        {
            DataRow oOrderRow;
            con = new SqlConnection(cn);
            con.Open();
            dbTransaction = con.BeginTransaction();
            DataSet oDS = new DataSet();
            SqlDataAdapter oOrdersDataAdapter = new SqlDataAdapter(new SqlCommand(strSql, con, dbTransaction));
            SqlCommandBuilder oOrdersCmdBuilder = new SqlCommandBuilder(oOrdersDataAdapter);
            oOrdersDataAdapter.FillSchema(oDS, SchemaType.Source);

            DataTable dtAddSchedule = oDS.Tables["Table"];
            dtAddSchedule.TableName = "HR_EMP_TARGET";

            oOrderRow = oDS.Tables["HR_EMP_TARGET"].NewRow();

            oOrderRow["CMP_BRANCH_ID"] = Branch;
            oOrderRow["EMP_ID"] = empID;
            oOrderRow["TER_TYPE"] = TerType;
            oOrderRow["TER_YEAR_AMMOUNT"] = TerAmountYearly;
            oOrderRow["TER_MONTH_AMMOUNT"] = TerAmountMon;
            oOrderRow["YR_ID"] = YrID;

            oDS.Tables["HR_EMP_TARGET"].Rows.Add(oOrderRow);
            oOrdersDataAdapter.Update(oDS, "HR_EMP_TARGET");
            dbTransaction.Commit();
            return "Saved Successfully.";
        }
        catch (Exception ex)
        {
            return ex.Message.ToString();
        }
        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }
    }

    public DataSet Target_AMT(string eID, string Bid, string Yid, string Type)
    {
        string strTA = "SELECT TER_TYPE, TER_YEAR_AMMOUNT FROM HR_EMP_TARGET WHERE EMP_ID='" + eID + "' "
                       + "AND CMP_BRANCH_ID='" + Bid + "' AND YR_ID='" + Yid + "' AND TER_TYPE='" + Type + "' ";

        try
        {
            con = new SqlConnection(cn);
            con.Open();

            DataSet oDS = new DataSet();
            SqlDataAdapter odaData = new SqlDataAdapter(new SqlCommand(strTA, con));
            odaData.Fill(oDS, "HR_TARGET_ACHV");
            return oDS;
        }
        catch (Exception ex)
        {
            return null;
        }

        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }
    }

    public DataSet Action(string eID, string Bid)
    {
        string strTA = "SELECT EDA_TYPE_OF_ACTION, EDA_REASON, EDA_DATE, EDA_REMARKS FROM HR_EMP_DISCIPLINARY_ACTION "
                       + "WHERE EMP_ID='" + eID + "' AND CMP_BRANCH_ID='" + Bid + "' ";

        try
        {
            con = new SqlConnection(cn);
            con.Open();

            DataSet oDS = new DataSet();
            SqlDataAdapter odaData = new SqlDataAdapter(new SqlCommand(strTA, con));
            odaData.Fill(oDS, "HR_EMP_DISCIPLINARY_ACTION");
            return oDS;
        }
        catch (Exception ex)
        {
            return null;
        }

        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }
    }

    public DataSet TotAssper(string eID, string Bid, string yId, string TypeId)
    {
        string strTA = "SELECT DISTINCT ASSIS_PERSON_TYPE FROM HR_ASSESSMENT_EMPLOYEE "
                       + "WHERE ASSIS_TYPE_ID='" + TypeId + "' AND ASSIS_PERSON_TYPE!=4 AND YR_ID='" + yId + "' "
                       + "AND CMP_BRANCH_ID='" + Bid + "' AND EMP_ID='" + eID + "' ";

        try
        {
            con = new SqlConnection(cn);
            con.Open();

            DataSet oDS = new DataSet();
            SqlDataAdapter odaData = new SqlDataAdapter(new SqlCommand(strTA, con));
            odaData.Fill(oDS, "TOT_ASSESSMENT_PERSON");
            return oDS;
        }
        catch (Exception ex)
        {
            return null;
        }

        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }
    }

    #region Off---Prothom-Alo (Attendance Report)
    //// ################### Attendance Information ####################
    //// Developed by: Md. Asaduzzaman Dated: 17-Dec-2013
    //public DataSet GetAttendanceInfo(string Br, string yr, string empId, string AtTypeIn, string FrmDt) // 
    //{
    //    string strQuery = "SELECT   CB.CMP_BRANCH_NAME, DP.DPT_NAME, (SET_CODE || ' - ' || DSG_TITLE) DNAME,(EMP_TITLE || ' ' || EMP_NAME) ENAME,EA.EMP_ID, "
    //         + " EA.ATT_LATE_TIME,EA.ATT_DATE_TIME,EA.ATT_COMMENTS, "
    //         + " YR.YR_OFFICE_HOUR,EL.EMP_CODE, EA.ATT_ID, EA.ATT_TYPE, EA.ATT_LATE_STATUS "
    //         + " FROM   PR_EMP_ATTENDENCE EA,PR_EMPLOYEE_LIST EL,CM_CMP_BRANCH CB, PR_DESIGNATION DE, PR_DEPARTMENT DP,HR_YEAR YR "
    //         + " WHERE   EA.EMP_ID = EL.EMP_ID AND EA.CMP_BRANCH_ID = CB.CMP_BRANCH_ID "
    //         + " AND ( (YR.CMP_BRANCH_ID = EL.CMP_BRANCH_ID AND YR.YR_YEAR = '" + yr + "') "
    //         + " OR (YR.CMP_BRANCH_ID = '" + Br + "' AND YR.YR_YEAR = '" + yr + "')) "
    //         + " AND DE.DSG_ID = EL.DSG_ID "
    //         + " AND EL.DPT_ID = DP.DPT_ID "
    //         + " AND EA.ATT_TYPE = '" + AtTypeIn + "' "
    //         + " AND TO_DATE (TO_CHAR (EA.ATT_DATE_TIME, 'DD/MM/YYYY'), 'DD/MM/YYYY') = TO_DATE('" + FrmDt + "','DD/MM/YYYY')  "
    //         + " AND CB.CMP_BRANCH_ID = '" + Br + "' AND EL.EMP_ID = '" + empId + "' ";
    //   
    //    try
    //    {
    //        DataSet oDS = new DataSet();
    //        SqlDataAdapter odaData = new SqlDataAdapter(new SqlCommand(strQuery, con));
    //        odaData.Fill(oDS, "AttendanceInfo");
    //        return oDS;
    //    }
    //    catch (Exception ex)
    //    {
    //        return null;
    //    }
    //    finally
    //    {
    //        con.Close();
    //        con = null;
    //    }
    //}
    //public DataSet GetROSTER_ENDTIME(string BRANCH, string EMPID, string STARTDATE, string DAYOFWEEK)
    //{
    //    string strQuery = " SELECT  NVL(ROSTER_ENDTIME_OVERTIME('" + BRANCH + "' ,'" + EMPID + "' ,to_date('" + STARTDATE + "','DD/MM/YYYY') ,'" + DAYOFWEEK + "' ),'0')ROSTER_ENDTIME FROM DUAL  ";

    //   
    //    try
    //    {
    //        DataSet oDS = new DataSet();
    //        SqlDataAdapter odaData = new SqlDataAdapter(new SqlCommand(strQuery, con));
    //        odaData.Fill(oDS, "ROSTER_outTIME");
    //        return oDS;
    //    }
    //    catch (Exception ex)
    //    {
    //        return null;
    //    }
    //    finally
    //    {
    //        con.Close();
    //        con = null;
    //    }
    //}
    //public DataSet GetMonthlyAttendanceList(string BRANCH, string STARTDATE, string ENDDATE)
    //{
    //    string strQuery = " SELECT DISTINCT  EL.EMP_ID,(EMP_TITLE || ' ' || EMP_NAME) ENAME, PD.DSG_TITLE AS DESIGNATION "
    //                      + " FROM PR_EMPLOYEE_LIST EL, PR_EMP_ATTENDENCE EA,PR_DESIGNATION PD "
    //                      + " WHERE EL.EMP_ID=EA.EMP_ID AND ATT_TYPE='IN' AND EL.DSG_ID=PD.DSG_ID  AND TRUNC(EA.ATT_DATE_TIME)  BETWEEN TO_DATE('" + STARTDATE + "','DD/MM/YYYY') AND TO_DATE('" + ENDDATE + "','DD/MM/YYYY') AND  EA.CMP_BRANCH_ID = '" + BRANCH + "' "
    //                      + " ORDER BY ENAME ";

    //   
    //    try
    //    {
    //        DataSet oDS = new DataSet();
    //        SqlDataAdapter odaData = new SqlDataAdapter(new SqlCommand(strQuery, con));
    //        odaData.Fill(oDS, "AttendanceList");
    //        return oDS;
    //    }
    //    catch (Exception ex)
    //    {
    //        return null;
    //    }
    //    finally
    //    {
    //        con.Close();
    //        con = null;
    //    }
    //}
    //public string TotalWorkingDaysEmp(string BRANCH, string STARTDATE, string ENDDATE, string empId)
    //{
    //    string strQuery = " select NVL(max(rownum),0) TOTAL from (SELECT DISTINCT EL.EMP_ID,EL.EMP_NAME,TRUNC(EA.ATT_DATE_TIME)  "
    //                      + " FROM PR_EMPLOYEE_LIST EL, PR_EMP_ATTENDENCE EA "
    //                      + " WHERE EL.EMP_ID=EA.EMP_ID AND ATT_TYPE='IN' and EL.EMP_ID='" + empId + "'  AND TRUNC(EA.ATT_DATE_TIME) BETWEEN  TO_DATE('" + STARTDATE + "','DD/MM/YYYY') AND TO_DATE('" + ENDDATE + "','DD/MM/YYYY') AND  EA.CMP_BRANCH_ID = '" + BRANCH + "'  ORDER BY EMP_NAME) ";
    //   
    //    try
    //    {
    //        DataSet oDS = new DataSet();
    //        SqlDataAdapter odaData = new SqlDataAdapter(new SqlCommand(strQuery, con));
    //        odaData.Fill(oDS, "workingDays");
    //        return oDS.Tables["workingDays"].Rows[0]["TOTAL"].ToString();
    //    }
    //    catch (Exception ex)
    //    {
    //        return null;
    //    }
    //    finally
    //    {
    //        con.Close();
    //        con = null;
    //    }
    //}
    //public string TotalLateDaysEmp(string BRANCH, string STARTDATE, string ENDDATE, string empId)
    //{
    //    string strQuery = " select NVL(max(rownum),0) TOTAL from (SELECT DISTINCT EL.EMP_ID,EL.EMP_NAME,TRUNC(EA.ATT_DATE_TIME)  "
    //                      + " FROM PR_EMPLOYEE_LIST EL, PR_EMP_ATTENDENCE EA "
    //                      + " WHERE EL.EMP_ID=EA.EMP_ID AND ATT_TYPE='IN' and EL.EMP_ID='" + empId + "'  AND TRUNC(EA.ATT_DATE_TIME) BETWEEN  TO_DATE('" + STARTDATE + "','DD/MM/YYYY') AND TO_DATE('" + ENDDATE + "','DD/MM/YYYY') AND  EA.CMP_BRANCH_ID = '" + BRANCH + "' AND EA.ATT_LATE_TIME !=''  ORDER BY EMP_NAME) ";
    //   
    //    try
    //    {
    //        DataSet oDS = new DataSet();
    //        SqlDataAdapter odaData = new SqlDataAdapter(new SqlCommand(strQuery, con));
    //        odaData.Fill(oDS, "LateDays");
    //        return oDS.Tables["LateDays"].Rows[0]["TOTAL"].ToString();
    //    }
    //    catch (Exception ex)
    //    {
    //        return null;
    //    }
    //    finally
    //    {
    //        con.Close();
    //        con = null;
    //    }
    //}
    //public string TotalLeaveEmp(string BRANCH, string STARTDATE, string ENDDATE, string empId)
    //{
    //    string strQuery = " select NVL(sum(pr.total),0)totalLv from ((select LVE_APPROVED_DAY total from PR_LEAVE  "
    //                     + "where EMP_ID='" + empId + "' and CMP_BRANCH_ID='" + BRANCH + "' and TO_DATE('" + STARTDATE + "','DD/MM/YYYY') >=  TO_DATE(LVE_FROM_DATE)  and TO_DATE('" + ENDDATE + "','DD/MM/YYYY') <=  TO_DATE(LVE_TO_DATE))pr) ";
    //   
    //    try
    //    {
    //        DataSet oDS = new DataSet();
    //        SqlDataAdapter odaData = new SqlDataAdapter(new SqlCommand(strQuery, con));
    //        odaData.Fill(oDS, "totalLeave");
    //        return oDS.Tables["totalLeave"].Rows[0]["totalLv"].ToString();
    //    }
    //    catch (Exception ex)
    //    {
    //        return null;
    //    }
    //    finally
    //    {
    //        con.Close();
    //        con = null;
    //    }
    //}
    //public DataSet GetRosterEmpwise(string empid, string branch, string STARTDATE, string EndDate)
    //{
    //    string strQuery = " SELECT DISTINCT PRR_TYPE || '(' || PRR_TIME_FROM ||' to '||PRR_TIME_TO || ')' as timespan,GRACE_TIME || 'min('|| PRR_TYPE || ')'  as considertime "
    //                        + " FROM PR_EMP_ROSTER PR,PR_ROSTER_SETUP PS,PR_ROSTER_SEHEDULE RSE "
    //                        + " WHERE PR.PRR_ID=PS.PRR_ID AND PR.CMP_BRANCH_ID=PS.CMP_BRANCH_ID  AND RSE.PRR_ID=PS.PRR_ID AND PR.PR_ROSTER_ID=RSE.PR_ROSTER_ID "
    //                        + " AND TO_DATE('" + STARTDATE + "','DD/MM/YYYY') >= TO_DATE(RSE.PRS_START_DATE) AND  TO_DATE('" + EndDate + "','DD/MM/YYYY') <= TO_DATE(RSE.PRS_END_DATE) "
    //                        + " AND PR.EMP_ID='" + empid + "' AND PR.CMP_BRANCH_ID='" + branch + "' ";

    //   
    //    try
    //    {
    //        DataSet oDS = new DataSet();
    //        SqlDataAdapter odaData = new SqlDataAdapter(new SqlCommand(strQuery, con));
    //        odaData.Fill(oDS, "RosterEmpwise");
    //        return oDS;
    //    }
    //    catch (Exception ex)
    //    {
    //        return null;
    //    }
    //    finally
    //    {
    //        con.Close();
    //        con = null;
    //    }
    //}

    #endregion

    #region Prothom-Alo (Attendance Report)
    // ################### Attendance Information ####################
    // Developed by: Md. Asaduzzaman Dated: 17-Dec-2013
    public DataSet GetMonthlyStartEndDate(string MonthId) //  asad
    {
        string strQuery = " SELECT M.MONTH_START_DATE , M.MONTH_END_DATE, M.MONTH_TOTAL_DAYS FROM HR_MONTH M WHERE M.MONTH_ID='" + MonthId + "'  ";

        try
        {
            con = new SqlConnection(cn);
            con.Open();
            DataSet oDS = new DataSet();
            SqlDataAdapter odaData = new SqlDataAdapter(new SqlCommand(strQuery, con));
            odaData.Fill(oDS, "MstartEnd");
            return oDS;
        }
        catch (Exception ex)
        {
            return null;
        }
        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }
    }
    public string TotalWorkingDays(string FrmDt, string ToDt, string EmpId) // asad
    {
        string strQuery = "select COUNT(*)total from PR_EMP_ATTENDENCE at where (at.ATT_TYPE= 'IN')  and EMP_ID='" + EmpId + "' and  "
                         + "convert(date, CONVERT(DATETIME, CONVERT(VARCHAR(23), at.ATT_DATE_TIME))) between  convert(date, CONVERT(DATETIME, CONVERT(VARCHAR(23), '" + FrmDt + "')))  and  convert(date, CONVERT(DATETIME, CONVERT(VARCHAR(23),'" + ToDt + "'))) AND at.PREV_OUT_FLAG='NO' AND  at.ACCESS_METHOD !='FD' ";
        string totalPresent = "0";
        try
        {
            con = new SqlConnection(cn);
            con.Open();
            DataSet oDS = new DataSet();
            SqlDataAdapter odaData = new SqlDataAdapter(new SqlCommand(strQuery, con));
            odaData.Fill(oDS, "workingDays");
            if (oDS.Tables[0].Rows.Count > 0)
            {
                totalPresent = oDS.Tables[0].Rows[0]["total"].ToString();
            }
        }
        catch (Exception ex)
        {
            return null;
        }
        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }
        return totalPresent;
    }
    public int WorkingDaysCheckDaily_v2(string FrmDt, string EmpId) // asad
    {
        string strQuery = " set deadlock_priority low; select  COUNT(*)total from PR_EMP_ATTENDENCE at with(nolock)  where (at.ATT_TYPE= 'IN' OR at.ATT_TYPE= 'OUT' )  and EMP_ID='" + EmpId + "' and  "
                         + "convert(date, CONVERT(DATETIME, CONVERT(VARCHAR(23), at.ATT_DATE_TIME))) =  convert(date, CONVERT(DATETIME, CONVERT(VARCHAR(23), '" + FrmDt + "')))  AND at.PREV_OUT_FLAG in ('NO','PRE')  AND  at.ACCESS_METHOD !='FD' ";  //
        int totalPresent = 0;
        try
        {
            con = new SqlConnection(cn);
            con.Open();
            DataSet oDS = new DataSet();
            SqlDataAdapter odaData = new SqlDataAdapter(new SqlCommand(strQuery, con));
            odaData.Fill(oDS, "workingDays");
            string chkWDays = oDS.Tables[0].Rows[0]["total"].ToString();
            if (int.Parse(chkWDays) > 0)
            {
                totalPresent = 1;
            }
        }
        catch (Exception ex)
        {
            return 0;
        }
        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }
        return totalPresent;
    }
    public int WorkingDaysCheckDaily(string FrmDt, string EmpId) // asad
    {
        //string strQuery = " set deadlock_priority low; select  COUNT(*)total from PR_EMP_ATTENDENCE at with(nolock)  where (at.ATT_TYPE= 'IN' OR at.ATT_TYPE= 'OUT' )  and EMP_ID='" + EmpId + "' and  "
        //                 + "convert(date, CONVERT(DATETIME, CONVERT(VARCHAR(23), at.ATT_DATE_TIME))) =  convert(date, CONVERT(DATETIME, CONVERT(VARCHAR(23), '" + FrmDt + "')))   AND (at.PREV_OUT_FLAG = 'NO' ) AND  at.ACCESS_METHOD !='FD' "; //or at.PREV_OUT_FLAG='PRE'
        string strQuery = " set deadlock_priority low; select  COUNT(*)total from PR_EMP_ATTENDENCE at with(nolock)  where (at.ATT_TYPE= 'IN' OR at.ATT_TYPE= 'OUT' )  and EMP_ID='" + EmpId + "' and  "
                        + "convert(date,  at.ATT_DATE_TIME) =  convert(date,  '" + FrmDt + "')   AND (at.PREV_OUT_FLAG = 'NO' ) AND  at.ACCESS_METHOD !='FD' "; //or at.PREV_OUT_FLAG='PRE'
        int totalPresent = 0;
        try
        {
            con = new SqlConnection(cn);
            con.Open();
            DataSet oDS = new DataSet();
            SqlDataAdapter odaData = new SqlDataAdapter(new SqlCommand(strQuery, con));
            odaData.Fill(oDS, "workingDays");
            string chkWDays = oDS.Tables[0].Rows[0]["total"].ToString();
            if (int.Parse(chkWDays) > 0)
            {
                totalPresent = 1;
            }
        }
        catch (Exception ex)
        {
            return 0;
        }
        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }
        return totalPresent;
    }

    public int WorkingDaysCheckDaily(string FrmDt, string EmpId, string AttType) // asad
    {
        //string strQuery = " set deadlock_priority low; select  COUNT(*)total from PR_EMP_ATTENDENCE at with(nolock)  where at.ATT_TYPE= '" + AttType + "' and EMP_ID='" + EmpId + "' and  "
        //                 + "convert(date, CONVERT(DATETIME, CONVERT(VARCHAR(23), at.ATT_DATE_TIME))) =  convert(date, CONVERT(DATETIME, CONVERT(VARCHAR(23), '" + FrmDt + "')))   AND at.PREV_OUT_FLAG='NO' ";
        string strQuery = " set deadlock_priority low; select  COUNT(*)total from PR_EMP_ATTENDENCE at with(nolock)  where at.ATT_TYPE= '" + AttType + "' and EMP_ID='" + EmpId + "' and  "
                         + "convert(date,  at.ATT_DATE_TIME) =  convert(date,  '" + FrmDt + "')   AND at.PREV_OUT_FLAG='NO' ";
        int totalPresent = 0;
        try
        {
            con = new SqlConnection(cn);
            con.Open();
            DataSet oDS = new DataSet();
            SqlDataAdapter odaData = new SqlDataAdapter(new SqlCommand(strQuery, con));
            odaData.Fill(oDS, "workingDays");
            string chkWDays = oDS.Tables[0].Rows[0]["total"].ToString();
            if (int.Parse(chkWDays) > 0)
            {
                totalPresent = 1;
            }
        }
        catch (Exception ex)
        {

        }
        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }
        return totalPresent;
    }


    public int LateCheckDaily(string FrmDt, string EmpId) // asad
    {
        //string strQuery = " set deadlock_priority low; SELECT COUNT(*) total  FROM    PR_EMP_ATTENDENCE AT  with(nolock) WHERE "
        //                + " convert(date, CONVERT(DATETIME, CONVERT(VARCHAR(23), AT.ATT_DATE_TIME))) "
        //                + " = convert(date, CONVERT(DATETIME, CONVERT(VARCHAR(23), '" + FrmDt + "'))) "
        //                + " AND AT.EMP_ID='" + EmpId + "'  AND AT.ATT_TYPE='IN' and at.ACCESS_METHOD !='FD'  AND AT.ATT_LATE_STATUS='YES' ";
        string strQuery = " set deadlock_priority low; SELECT COUNT(*) total  FROM    PR_EMP_ATTENDENCE AT  with(nolock) WHERE "
                       + " convert(date,  AT.ATT_DATE_TIME) "
                       + " = convert(date,  '" + FrmDt + "') "
                       + " AND AT.EMP_ID='" + EmpId + "'  AND AT.ATT_TYPE='IN' and at.ACCESS_METHOD !='FD'  AND AT.ATT_LATE_STATUS='YES' ";
        int totalPresent = 0;
        try
        {
            con = new SqlConnection(cn);
            con.Open();
            DataSet oDS = new DataSet();
            SqlDataAdapter odaData = new SqlDataAdapter(new SqlCommand(strQuery, con));
            odaData.Fill(oDS, "workingDays");
            string chkWDays = oDS.Tables[0].Rows[0]["total"].ToString();
            if (int.Parse(chkWDays) > 0)
            {
                totalPresent = 1;
            }
        }
        catch (Exception ex)
        {
            return 0;
        }
        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }
        return totalPresent;
    }
    public bool LateCheckDaily_bool(string FrmDt, string EmpId) // asad
    {
        //string strQuery = " set deadlock_priority low; SELECT COUNT(*) total  FROM    PR_EMP_ATTENDENCE AT  with(nolock) WHERE "
        //                + " convert(date, CONVERT(DATETIME, CONVERT(VARCHAR(23), AT.ATT_DATE_TIME))) "
        //                + " = convert(date, CONVERT(DATETIME, CONVERT(VARCHAR(23), '" + FrmDt + "'))) "
        //                + " AND AT.EMP_ID='" + EmpId + "'  AND AT.ATT_TYPE='IN' and at.ACCESS_METHOD !='FD'  AND AT.ATT_LATE_STATUS='YES' ";
        string strQuery = " set deadlock_priority low; SELECT COUNT(*) total  FROM    PR_EMP_ATTENDENCE AT  with(nolock) WHERE "
                       + " convert(date,  AT.ATT_DATE_TIME) "
                       + " = convert(date,  '" + FrmDt + "') "
                       + " AND AT.EMP_ID='" + EmpId + "'  AND AT.ATT_TYPE='IN' and at.ACCESS_METHOD !='FD'  AND AT.ATT_LATE_STATUS='YES' ";
        bool totalPresent = false;
        try
        {
            con = new SqlConnection(cn);
            con.Open();
            DataSet oDS = new DataSet();
            SqlDataAdapter odaData = new SqlDataAdapter(new SqlCommand(strQuery, con));
            odaData.Fill(oDS, "workingDays");
            string chkWDays = oDS.Tables[0].Rows[0]["total"].ToString();
            if (int.Parse(chkWDays) > 0)
            {
                totalPresent = true;
            }
        }
        catch (Exception ex)
        {
            return false;
        }
        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }
        return totalPresent;
    }
    public int OSDCheckDaily(string FrmDt, string EmpId) // asad
    {
        //string strQuery = " set deadlock_priority low; SELECT  COUNT(*) total  FROM    PR_EMP_ATTENDENCE AT with(nolock) WHERE "
        //                + "  convert(date, CONVERT(DATETIME, CONVERT(VARCHAR(23), AT.ATT_DATE_TIME))) "
        //                + "  = convert(date, CONVERT(DATETIME, CONVERT(VARCHAR(23), '" + FrmDt + "'))) "
        //                + "  AND AT.EMP_ID='" + EmpId + "'  AND AT.ATT_TYPE='OSD' and AT.ACCESS_METHOD ='FD'  AND AT.APPROVAL_STATUS='AV' ";
        string strQuery = " set deadlock_priority low; SELECT  COUNT(*) total  FROM    PR_EMP_ATTENDENCE AT with(nolock) WHERE "
                        + "  convert(date,  AT.ATT_DATE_TIME) "
                        + "  = convert(date,  '" + FrmDt + "') "
                        + "  AND AT.EMP_ID='" + EmpId + "'  AND AT.ATT_TYPE='OSD' and AT.ACCESS_METHOD ='FD'  AND AT.APPROVAL_STATUS='AV' ";
        int totalPresent = 0;
        try
        {
            con = new SqlConnection(cn);
            con.Open();
            DataSet oDS = new DataSet();
            SqlDataAdapter odaData = new SqlDataAdapter(new SqlCommand(strQuery, con));
            odaData.Fill(oDS, "workingDays");
            string chkWDays = oDS.Tables[0].Rows[0]["total"].ToString();
            if (int.Parse(chkWDays) > 0)
            {
                totalPresent = 1;
            }
        }
        catch (Exception ex)
        {
            return 0;
        }
        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }
        return totalPresent;
    }
    public int FDLateCheckDaily(string FrmDt, string EmpId) // asad
    {
        string strQuery = "set deadlock_priority low;  SELECT  COUNT(*) total  FROM    PR_EMP_ATTENDENCE AT with(nolock) WHERE "
                        + "  convert(date, AT.ATT_DATE_TIME) "
                        + "  = convert(date,  '" + FrmDt + "') "
                        + "  AND AT.EMP_ID='" + EmpId + "'  AND AT.ATT_TYPE='IN' and AT.ACCESS_METHOD ='FD' AND AT.APPROVAL_STATUS='AV'  AND AT.ATT_LATE_STATUS='YES'  ";
        int totalPresent = 0;
        try
        {
            con = new SqlConnection(cn);
            con.Open();
            DataSet oDS = new DataSet();
            SqlDataAdapter odaData = new SqlDataAdapter(new SqlCommand(strQuery, con));
            odaData.Fill(oDS, "workingDays");
            string chkWDays = oDS.Tables[0].Rows[0]["total"].ToString();
            if (int.Parse(chkWDays) > 0)
            {
                totalPresent = 1;
            }
        }
        catch (Exception ex)
        {
            return 0;
        }
        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }
        return totalPresent;
    }
    public int ApprovedWorkDaysDailyChk(string FrmDt, string EmpId) // asad
    {
        //string strQuery = " set deadlock_priority low; select COUNT(*)total from PR_EMP_ATTENDENCE at  with(nolock) where (at.ATT_TYPE= 'IN' OR at.ATT_TYPE= 'OUT' OR at.ATT_TYPE= 'OSD') and at.ACCESS_METHOD ='FD' and at.APPROVAL_STATUS='AV' and EMP_ID='" + EmpId + "' and "
        //                 + " convert(date, CONVERT(DATETIME, CONVERT(VARCHAR(23), at.ATT_DATE_TIME))) =  convert(date, CONVERT(DATETIME, CONVERT(VARCHAR(23), '" + FrmDt + "'))) ";
        string strQuery = " set deadlock_priority low; select COUNT(*)total from PR_EMP_ATTENDENCE at  with(nolock) where (at.ATT_TYPE= 'IN' OR at.ATT_TYPE= 'OUT' OR at.ATT_TYPE= 'OSD') and at.ACCESS_METHOD ='FD' and at.APPROVAL_STATUS='AV' and EMP_ID='" + EmpId + "' and "
                         + " convert(date, at.ATT_DATE_TIME) =  convert(date, '" + FrmDt + "') ";
        int totalPresent = 0;
        try
        {
            con = new SqlConnection(cn);
            con.Open();
            DataSet oDS = new DataSet();
            SqlDataAdapter odaData = new SqlDataAdapter(new SqlCommand(strQuery, con));
            odaData.Fill(oDS, "workingDays");
            string chkWDays = oDS.Tables[0].Rows[0]["total"].ToString();
            if (int.Parse(chkWDays) > 0)
            {
                totalPresent = 1;
            }
        }
        catch (Exception ex)
        {
            return 0;
        }
        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }
        return totalPresent;
    }

    public string TotalApprovedWorkingDays(string FrmDt, string ToDt, string EmpId) // asad
    {
        string strQuery = "select COUNT(*)total from PR_EMP_ATTENDENCE at where at.ATT_TYPE= 'IN' and at.ACCESS_METHOD ='FD' and at.APPROVAL_STATUS='AV' and EMP_ID='" + EmpId + "' and "
                         + " convert(date, CONVERT(DATETIME, CONVERT(VARCHAR(23), at.ATT_DATE_TIME))) between  convert(date, CONVERT(DATETIME, CONVERT(VARCHAR(23), '" + FrmDt + "')))  and  convert(date, CONVERT(DATETIME, CONVERT(VARCHAR(23),'" + ToDt + "'))) ";
        string totalPresent = "0";
        try
        {
            con = new SqlConnection(cn);
            con.Open();
            DataSet oDS = new DataSet();
            SqlDataAdapter odaData = new SqlDataAdapter(new SqlCommand(strQuery, con));
            odaData.Fill(oDS, "workingDays");
            if (oDS.Tables[0].Rows.Count > 0)
            {
                totalPresent = oDS.Tables[0].Rows[0]["total"].ToString();
            }
        }
        catch (Exception ex)
        {
            return null;
        }
        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }
        return totalPresent;
    }
    public int CheckWorkingDays(string FrmDt, string EmpId) // asad
    {
        string strQuery = "select COUNT(*)total from PR_EMP_ATTENDENCE at where at.ATT_TYPE= 'IN'  and EMP_ID='" + EmpId + "' and "
                         + " convert(date, CONVERT(DATETIME, CONVERT(VARCHAR(23), at.ATT_DATE_TIME))) =  convert(date, CONVERT(DATETIME, CONVERT(VARCHAR(23), '" + FrmDt + "')))  ";
        int totalPresent = 0;
        try
        {
            con = new SqlConnection(cn);
            con.Open();
            DataSet oDS = new DataSet();
            SqlDataAdapter odaData = new SqlDataAdapter(new SqlCommand(strQuery, con));
            odaData.Fill(oDS, "workingDays");
            if (oDS.Tables[0].Rows.Count > 0)
            {
                totalPresent = int.Parse(oDS.Tables[0].Rows[0]["total"].ToString());
            }
        }
        catch (Exception ex)
        {
            return 0;
        }
        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }
        return totalPresent;
    }
    public int CheckFrontDeskAttendance(string FrmDt, string EmpId) // asad
    {
        string strQuery = "select COUNT(*)total from PR_EMP_ATTENDENCE at where at.ATT_TYPE= 'IN'  and EMP_ID='" + EmpId + "' and "
                         + " convert(date, CONVERT(DATETIME, CONVERT(VARCHAR(23), at.ATT_DATE_TIME))) =  convert(date, CONVERT(DATETIME, CONVERT(VARCHAR(23), '" + FrmDt + "'))) and ACCESS_METHOD ='FD' and APPROVAL_STATUS='AV'  ";
        int totalPresent = 0;
        try
        {
            con = new SqlConnection(cn);
            con.Open();
            DataSet oDS = new DataSet();
            SqlDataAdapter odaData = new SqlDataAdapter(new SqlCommand(strQuery, con));
            odaData.Fill(oDS, "workingDays");
            if (oDS.Tables[0].Rows.Count > 0)
            {
                totalPresent = int.Parse(oDS.Tables[0].Rows[0]["total"].ToString());
            }
        }
        catch (Exception ex)
        {
            return 0;
        }
        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }
        return totalPresent;
    }

    public string TotalWorkingHrs(string BRANCH, string EmpId, string FrmDt, string yrId) // asad
    {
        string strQuery = " set deadlock_priority low; select rs.PRR_TIME_FROM,rs.PRR_TIME_TO from PR_ROSTER_SETUP rs where rs.PRR_ID in "
                       + " ( "
                       + " select PRR_ID from PR_EMP_ROSTER with(nolock) where EMP_ID='" + EmpId + "'  and  CMP_BRANCH_ID='" + BRANCH + "' and "
                       + " convert(date, CONVERT(DATETIME, CONVERT(VARCHAR(23), ROSTER_DATE)))  "
                       + " = convert(date, CONVERT(DATETIME, CONVERT(VARCHAR(23), '" + FrmDt + "'))) "
                       + " ) ";
        string totalWorkingHr = "0";
        string frmTime = "";
        string toTime = "";
        try
        {
            con = new SqlConnection(cn);
            con.Open();
            DataSet oDS = new DataSet();
            SqlDataAdapter odaData = new SqlDataAdapter(new SqlCommand(strQuery, con));
            odaData.Fill(oDS, "workingDays");
            if (oDS.Tables[0].Rows.Count > 0)
            {
                frmTime = oDS.Tables[0].Rows[0]["PRR_TIME_FROM"].ToString();
                toTime = oDS.Tables[0].Rows[0]["PRR_TIME_TO"].ToString();
            }

            if ((frmTime == "" && toTime == "") || (frmTime == "00:00" && toTime == "00:00"))
            {
                string strQuery3 = " select LEFT(DATEADD(mi,CONVERT(NUMERIC(8, 2), 0), (CONVERT(TIME(0),YR_OFFICE_HOUR))),5)  + ' ' + RIGHT(CONVERT(VARCHAR(30), CAST(DATEADD(MINUTE, "
                                    + " CONVERT(NUMERIC(8, 2), 0), YR_OFFICE_HOUR) AS time(0)), 9), 2) startTime, "
                                    + " LEFT(DATEADD(mi,CONVERT(NUMERIC(8, 2), 0), (CONVERT(TIME(0),YR_OFFICE_HOUR_END))),5)  + ' ' + RIGHT(CONVERT(VARCHAR(30), CAST(DATEADD(MINUTE, "
                                    + " CONVERT(NUMERIC(8, 2), 0), YR_OFFICE_HOUR_END) AS time(0)), 9), 2) endTime "
                                    + " from HR_YEAR hr where YR_ID='" + yrId + "' ";
                DataSet oDS3 = new DataSet();
                SqlDataAdapter odaData3 = new SqlDataAdapter(new SqlCommand(strQuery3, con));
                odaData3.Fill(oDS3, "workingHRoutofRoster");
                if (oDS3.Tables[0].Rows.Count > 0)
                {
                    frmTime = oDS3.Tables[0].Rows[0]["startTime"].ToString();
                    toTime = oDS3.Tables[0].Rows[0]["endTime"].ToString();
                }
            }
            //##########################################################################################################
            string strQuery2 = " select dbo.OFC_TIME_DIFFERENCE('" + frmTime + "', '" + toTime + "')totalHr ";
            DataSet oDS2 = new DataSet();
            SqlDataAdapter odaData2 = new SqlDataAdapter(new SqlCommand(strQuery2, con));
            odaData2.Fill(oDS2, "workingHr");
            if (oDS2.Tables[0].Rows.Count > 0)
            {
                totalWorkingHr = oDS2.Tables[0].Rows[0]["totalHr"].ToString();  // returns in minutes
            }
            //###########################################################################################################
        }
        catch (Exception ex)
        {
            return "0";
        }
        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }
        if (totalWorkingHr == "")
        {
            totalWorkingHr = "0";
        }
        return totalWorkingHr;
    }



    public DataSet RosterWiseInOutTime(string BRANCH, string EmpId, string FrmDt, string yrId) // asad
    {
        //Roster check
        string strQuery = "select rs.PRR_TIME_FROM,rs.PRR_TIME_TO,rs.GRACE_TIME from PR_ROSTER_SETUP rs where rs.PRR_ID in "
                       + " ( "
                       + " select PRR_ID from PR_EMP_ROSTER where EMP_ID='" + EmpId + "'  and  CMP_BRANCH_ID='" + BRANCH + "' and "
                       + " convert(date, CONVERT(DATETIME, CONVERT(VARCHAR(23), ROSTER_DATE)))  "
                       + " = convert(date, CONVERT(DATETIME, CONVERT(VARCHAR(23), '" + FrmDt + "'))) "
                       + " ) ";
        string totalWorkingHr = "0";
        string frmTime = "";
        string toTime = "";
        string frmTimeOffice = "";
        string toTimeOffice = "";
        DataSet oDS = new DataSet();
        try
        {
            con = new SqlConnection(cn);
            con.Open();

            SqlDataAdapter odaData = new SqlDataAdapter(new SqlCommand(strQuery, con));
            odaData.Fill(oDS, "workingDays");
            if (oDS.Tables[0].Rows.Count > 0)
            {
                frmTime = oDS.Tables[0].Rows[0]["PRR_TIME_FROM"].ToString();
                toTime = oDS.Tables[0].Rows[0]["PRR_TIME_TO"].ToString();
            }

            //If no roster
            if (frmTime == "" && toTime == "")
            {
                string strQuery3 = " select hr.GRACE_TIME,LEFT(DATEADD(mi,CONVERT(NUMERIC(8, 2), 0), (CONVERT(TIME(0),YR_OFFICE_HOUR))),5)  + ' ' + RIGHT(CONVERT(VARCHAR(30), CAST(DATEADD(MINUTE, "
                                    + " CONVERT(NUMERIC(8, 2), 0), YR_OFFICE_HOUR) AS time(0)), 9), 2) PRR_TIME_FROM, "
                                    + " LEFT(DATEADD(mi,CONVERT(NUMERIC(8, 2), 0), (CONVERT(TIME(0),YR_OFFICE_HOUR_END))),5)  + ' ' + RIGHT(CONVERT(VARCHAR(30), CAST(DATEADD(MINUTE, "
                                    + " CONVERT(NUMERIC(8, 2), 0), YR_OFFICE_HOUR_END) AS time(0)), 9), 2) PRR_TIME_TO "
                                    + " from HR_YEAR hr where YR_ID='" + yrId + "' ";

                SqlDataAdapter odaData3 = new SqlDataAdapter(new SqlCommand(strQuery3, con));
                odaData3.Fill(oDS, "workingHRoutofRoster");
                if (oDS.Tables[0].Rows.Count > 0)
                {
                    frmTimeOffice = oDS.Tables[0].Rows[0]["PRR_TIME_FROM"].ToString();
                    toTimeOffice = oDS.Tables[0].Rows[0]["PRR_TIME_TO"].ToString();
                }
            }

        }
        catch (Exception ex)
        {

        }
        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }
        return oDS;


    }


    public string AttendanceTime(string EmpId, string AttDate, string attType) // asad
    {
        //string strQuery = " set deadlock_priority low;  SELECT DISTINCT A.EMP_ID,LEFT(DATEADD(mi,CONVERT(NUMERIC(8, 2), 0), (CONVERT(TIME(0),A.ATT_DATE_TIME))),5)  + ' ' + RIGHT(CONVERT(VARCHAR(30), CAST(DATEADD(MINUTE, "
        //                    + " CONVERT(NUMERIC(8, 2), 0), A.ATT_DATE_TIME) AS time(0)), 9), 2)timeInout, A.ATT_COMMENTS, A.OSD_TIME_IN,A.OSD_TIME_OUT,A.ATT_TYPE,	APPROVAL_STATUS = CASE A.APPROVAL_STATUS WHEN 'AP' THEN 'APPLIED' WHEN 'NAV' THEN 'APPROVED TO WAIT FOR HR' "
        //                    + " WHEN 'AV' THEN 'APPROVED' WHEN 'CN' THEN 'CANCELLED' ELSE 'NOT MENTIONED' END "
        //                    + " FROM  PR_EMP_ATTENDENCE A  with(nolock)  "
        //                    + " WHERE convert(date, CONVERT(DATETIME, CONVERT(VARCHAR(23), A.ATT_DATE_TIME))) =  "
        //                    + " convert(date, CONVERT(DATETIME, CONVERT(VARCHAR(23), '" + AttDate + "'))) "
        //                    + " AND A.ATT_TYPE='" + attType + "'  AND A.EMP_ID='" + EmpId + "' AND A.PREV_OUT_FLAG ='NO' ";
        string strQuery = " set deadlock_priority low;  SELECT DISTINCT A.EMP_ID,LEFT(DATEADD(mi,CONVERT(NUMERIC(8, 2), 0), (CONVERT(TIME(0),A.ATT_DATE_TIME))),5)  + ' ' + RIGHT(CONVERT(VARCHAR(30), CAST(DATEADD(MINUTE, "
                           + " CONVERT(NUMERIC(8, 2), 0), A.ATT_DATE_TIME) AS time(0)), 9), 2)timeInout, A.ATT_COMMENTS, A.OSD_TIME_IN,A.OSD_TIME_OUT,A.ATT_TYPE,	APPROVAL_STATUS = CASE A.APPROVAL_STATUS WHEN 'AP' THEN 'APPLIED' WHEN 'NAV' THEN 'APPROVED TO WAIT FOR HR' "
                           + " WHEN 'AV' THEN 'APPROVED' WHEN 'CN' THEN 'CANCELLED' ELSE 'NOT MENTIONED' END "
                           + " FROM  PR_EMP_ATTENDENCE A  with(nolock)  "
                           + " WHERE convert(date, A.ATT_DATE_TIME) =  "
                           + " convert(date,  '" + AttDate + "') "
                           + " AND A.ATT_TYPE='" + attType + "'  AND A.EMP_ID='" + EmpId + "' AND A.PREV_OUT_FLAG ='NO' ";
        string TimeInout = "";
        try
        {
            con = new SqlConnection(cn);
            con.Open();
            DataSet oDS = new DataSet();
            SqlDataAdapter odaData = new SqlDataAdapter(new SqlCommand(strQuery, con));
            odaData.Fill(oDS, "TimeINOUT");
            if (oDS.Tables[0].Rows.Count > 0)
            {
                string type = oDS.Tables[0].Rows[0]["ATT_TYPE"].ToString();
                if (type == "OSD")
                {
                    string inT = oDS.Tables[0].Rows[0]["OSD_TIME_IN"].ToString();
                    string ouT = oDS.Tables[0].Rows[0]["OSD_TIME_OUT"].ToString();
                    string approval = oDS.Tables[0].Rows[0]["APPROVAL_STATUS"].ToString();
                    string Atcomments = oDS.Tables[0].Rows[0]["ATT_COMMENTS"].ToString();
                    TimeInout = inT + ";" + ouT + ";" + approval + ";" + Atcomments;

                }
                else
                {
                    TimeInout = oDS.Tables[0].Rows[0]["timeInout"].ToString();
                }
            }
        }
        catch (Exception ex)
        {
            return "";
        }
        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }
        return TimeInout;
    }



    //public string TimeDuration(string intime, string outTime) // polash
    //{
    //    DateTime start=start.ToFileTime("00:00");

    //    string strQuery = " SELECT DATEADD(SECOND, - DATEDIFF(SECOND,'" + outTime + "', '" + intime + "'), '"+start+"') as WorkingHour ";

    //    string TimeInout = "";
    //    try
    //    {
    //        con = new SqlConnection(cn);
    //        con.Open();
    //        DataSet oDS = new DataSet();
    //        SqlDataAdapter odaData = new SqlDataAdapter(new SqlCommand(strQuery, con));
    //        odaData.Fill(oDS, "WorkingHour");
    //        if (oDS.Tables[0].Rows.Count > 0)
    //        {
    //            //string type = oDS.Tables[0].Rows[0]["ATT_TYPE"].ToString();
    //            //if (type == "OSD")
    //            //{
    //            //    string inT = oDS.Tables[0].Rows[0]["OSD_TIME_IN"].ToString();
    //            //    string ouT = oDS.Tables[0].Rows[0]["OSD_TIME_OUT"].ToString();
    //            //    string approval = oDS.Tables[0].Rows[0]["APPROVAL_STATUS"].ToString();
    //            //    string Atcomments = oDS.Tables[0].Rows[0]["ATT_COMMENTS"].ToString();
    //            //    TimeInout = inT + ";" + ouT + ";" + approval + ";" + Atcomments;

    //            //}
    //            //else
    //            //{
    //            TimeInout = oDS.Tables[0].Rows[0]["WorkingHour"].ToString();
    //            // }
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        return "";
    //    }
    //    finally
    //    {
    //        con.Close();
    //        con.Dispose();
    //        SqlConnection.ClearPool(con);
    //        con = null;
    //    }
    //    return TimeInout;
    //}
    public string AttendanceTimeOUTprevious(string EmpId, string AttDate, string attType) // asad
    {
        //string strQuery = " set deadlock_priority low; SELECT DISTINCT A.EMP_ID,LEFT(DATEADD(mi,CONVERT(NUMERIC(8, 2), 0), (CONVERT(TIME(0),A.ATT_DATE_TIME))),5)  + ' ' + RIGHT(CONVERT(VARCHAR(30), CAST(DATEADD(MINUTE, "
        //                    + " CONVERT(NUMERIC(8, 2), 0), A.ATT_DATE_TIME) AS time(0)), 9), 2)timeInout "
        //                    + " FROM  PR_EMP_ATTENDENCE A with(nolock)  "
        //                    + " WHERE convert(date, CONVERT(DATETIME, CONVERT(VARCHAR(23), A.ATT_DATE_TIME))) =  "
        //                    + " convert(date, CONVERT(DATETIME, CONVERT(VARCHAR(23), '" + AttDate + "'))) "
        //                    + " AND A.ATT_TYPE='" + attType + "'  AND A.EMP_ID='" + EmpId + "' AND A.PREV_OUT_FLAG='PRE' ";
        string strQuery = " set deadlock_priority low; SELECT DISTINCT A.EMP_ID,LEFT(DATEADD(mi,CONVERT(NUMERIC(8, 2), 0), (CONVERT(TIME(0),A.ATT_DATE_TIME))),5)  + ' ' + RIGHT(CONVERT(VARCHAR(30), CAST(DATEADD(MINUTE, "
                           + " CONVERT(NUMERIC(8, 2), 0), A.ATT_DATE_TIME) AS time(0)), 9), 2)timeInout "
                           + " FROM  PR_EMP_ATTENDENCE A with(nolock)  "
                           + " WHERE convert(date,  A.ATT_DATE_TIME) =  "
                           + " convert(date,  '" + AttDate + "') "
                           + " AND A.ATT_TYPE='" + attType + "'  AND A.EMP_ID='" + EmpId + "' AND A.PREV_OUT_FLAG='PRE' ";
        string TimeInout = "";
        try
        {
            con = new SqlConnection(cn);
            con.Open();
            DataSet oDS = new DataSet();
            SqlDataAdapter odaData = new SqlDataAdapter(new SqlCommand(strQuery, con));
            odaData.Fill(oDS, "TimeINOUT");
            if (oDS.Tables[0].Rows.Count > 0)
            {
                TimeInout = oDS.Tables[0].Rows[0]["timeInout"].ToString();
            }
        }
        catch (Exception ex)
        {
            return "";
        }
        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }
        return TimeInout;
    }
    public string DurationOfworks(string startTime, string endTime, bool preflag) // asad
    {
        con = new SqlConnection(cn);
        con.Open();
        string totalWorkingHr = "0";
        try
        {
            string strQuery2 = string.Empty;
            bool AM1 = startTime.Contains("AM");
            bool AM2 = endTime.Contains("AM");
            if (preflag == true && AM1 == true && AM2 == true)
            {
                strQuery2 = "set deadlock_priority low; select dbo.OFC_TIME_DIFFERENCE_AM_AM_PREVIOUS_DAYS('" + startTime + "', '" + endTime + "')totalHr ";
            }
            else
            {
                strQuery2 = "set deadlock_priority low; select dbo.OFC_TIME_DIFFERENCE('" + startTime + "', '" + endTime + "')totalHr ";
            }
            DataSet oDS2 = new DataSet();
            SqlDataAdapter odaData2 = new SqlDataAdapter(new SqlCommand(strQuery2, con));
            odaData2.Fill(oDS2, "workingHr");
            if (oDS2.Tables[0].Rows.Count > 0)
            {
                totalWorkingHr = oDS2.Tables[0].Rows[0]["totalHr"].ToString(); // returns minutes
            }
        }
        catch (Exception)
        {

            return "0";
        }
        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }
        return totalWorkingHr;
    }
    public string DurationOfworks(string startTime, string endTime) // asad
    {
        con = new SqlConnection(cn);
        con.Open();
        string totalWorkingHr = "0";
        try
        {
            string strQuery2 = "set deadlock_priority low; select dbo.OFC_TIME_DIFFERENCE('" + startTime + "', '" + endTime + "')totalHr ";
            DataSet oDS2 = new DataSet();
            SqlDataAdapter odaData2 = new SqlDataAdapter(new SqlCommand(strQuery2, con));
            odaData2.Fill(oDS2, "workingHr");
            if (oDS2.Tables[0].Rows.Count > 0)
            {
                totalWorkingHr = oDS2.Tables[0].Rows[0]["totalHr"].ToString(); // returns minutes
            }
        }
        catch (Exception)
        {

            return "0";
        }
        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }
        return totalWorkingHr;
    }
    public string YearId(string Yearname) // asad
    {
        con = new SqlConnection(cn);
        con.Open();
        string yrId = "";
        try
        {
            string strQuery2 = "select  YR_ID from HR_YEAR where YR_YEAR ='" + Yearname + "' ";
            DataSet oDS2 = new DataSet();
            SqlDataAdapter odaData2 = new SqlDataAdapter(new SqlCommand(strQuery2, con));
            odaData2.Fill(oDS2, "yearId");
            if (oDS2.Tables[0].Rows.Count > 0)
            {
                yrId = oDS2.Tables[0].Rows[0]["YR_ID"].ToString();
            }
        }
        catch (Exception)
        {
            return "";
        }
        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }
        return yrId;
    }
    public DataSet GetAttendanceInfo(string Br, string yr, string empId, string AtTypeIn, string FrmDt) // 
    {
        string strQuery = "SELECT   CB.CMP_BRANCH_NAME, DP.DPT_NAME, (CAST(SET_CODE AS VARCHAR) + ' - ' + CAST(DSG_TITLE AS VARCHAR)) DNAME, "
             + " (CAST(EMP_TITLE AS VARCHAR) + ' ' + CAST(EMP_NAME AS VARCHAR)) ENAME,EA.EMP_ID, "
             + " EA.ATT_LATE_TIME,EA.ATT_DATE_TIME,EA.ATT_COMMENTS, "
             + " YR.YR_OFFICE_HOUR,EL.EMP_CODE, EA.ATT_ID, EA.ATT_TYPE, EA.ATT_LATE_STATUS "
             + " FROM   PR_EMP_ATTENDENCE EA,PR_EMPLOYEE_LIST EL,CM_CMP_BRANCH CB, PR_DESIGNATION DE, PR_DEPARTMENT DP,HR_YEAR YR "
             + " WHERE   EA.EMP_ID = EL.EMP_ID AND EA.CMP_BRANCH_ID = CB.CMP_BRANCH_ID "
             + " AND ( (YR.CMP_BRANCH_ID = EL.CMP_BRANCH_ID AND YR.YR_YEAR = '" + yr + "') "
             + " OR (YR.CMP_BRANCH_ID = '" + Br + "' AND YR.YR_YEAR = '" + yr + "')) "
             + " AND DE.DSG_ID = EL.DSG_ID "
             + " AND EL.DPT_ID = DP.DPT_ID "
             + " AND EA.ATT_TYPE = '" + AtTypeIn + "' "
             + " AND CONVERT(DATETIME, CONVERT(VARCHAR(23), EA.ATT_DATE_TIME, 103), 103) = CONVERT(DATETIME, '" + FrmDt + "', 103)  "
             + " AND CB.CMP_BRANCH_ID = '" + Br + "' AND EL.EMP_ID = '" + empId + "' ";

        try
        {
            con = new SqlConnection(cn);
            con.Open();
            DataSet oDS = new DataSet();
            SqlDataAdapter odaData = new SqlDataAdapter(new SqlCommand(strQuery, con));
            odaData.Fill(oDS, "AttendanceInfo");
            return oDS;
        }
        catch (Exception ex)
        {
            return null;
        }
        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }
    }
    public DataSet GetROSTER_ENDTIME(string BRANCH, string EMPID, string STARTDATE, string DAYOFWEEK)
    {
        string strQuery = " SELECT ISNULL(DBO.ROSTER_ENDTIME_OVERTIME('" + BRANCH + "', '" + EMPID + "', CONVERT(DATETIME, '" + STARTDATE + "', 103), '" + DAYOFWEEK + "'), '0') ROSTER_ENDTIME  ";


        try
        {
            con = new SqlConnection(cn);
            con.Open();
            DataSet oDS = new DataSet();
            SqlDataAdapter odaData = new SqlDataAdapter(new SqlCommand(strQuery, con));
            odaData.Fill(oDS, "ROSTER_outTIME");
            return oDS;
        }
        catch (Exception ex)
        {
            return null;
        }
        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }
    }
    public DataSet GetMonthlyAttendanceList(string BRANCH, string STARTDATE, string ENDDATE)
    {
        string strQuery = " SELECT DISTINCT  EL.EMP_ID,(CAST(EMP_TITLE AS VARCHAR(100)) + ' ' + CAST(EMP_NAME AS VARCHAR(500))) ENAME, PD.DSG_TITLE AS DESIGNATION "
                          + " FROM PR_EMPLOYEE_LIST EL, PR_EMP_ATTENDENCE EA,PR_DESIGNATION PD "
                          + " WHERE EL.EMP_ID=EA.EMP_ID AND ATT_TYPE='IN' AND EL.DSG_ID=PD.DSG_ID  AND CONVERT(DATETIME, EA.ATT_DATE_TIME, 103)  BETWEEN CONVERT(DATETIME, '" + STARTDATE + "', 103)  AND  CONVERT(DATETIME, '" + ENDDATE + "', 103) AND  EA.CMP_BRANCH_ID = '" + BRANCH + "' "
                          + " ORDER BY ENAME ";

        try
        {
            con = new SqlConnection(cn);
            con.Open();
            DataSet oDS = new DataSet();
            SqlDataAdapter odaData = new SqlDataAdapter(new SqlCommand(strQuery, con));
            odaData.Fill(oDS, "AttendanceList");
            return oDS;
        }
        catch (Exception ex)
        {
            return null;
        }
        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }
    }
    public string TotalWorkingDaysEmp(string BRANCH, string STARTDATE, string ENDDATE, string empId)
    {
        string strQuery = " select count(*)as TOTAL "
                          + " FROM PR_EMPLOYEE_LIST EL, PR_EMP_ATTENDENCE EA "
                          + " WHERE EL.EMP_ID=EA.EMP_ID AND ATT_TYPE='IN' and EL.EMP_ID='" + empId + "'  AND FLOOR(EA.ATT_DATE_TIME)  BETWEEN CONVERT(DATETIME, '" + STARTDATE + "', 103)  AND  CONVERT(DATETIME, '" + ENDDATE + "', 103) AND  EA.CMP_BRANCH_ID = '" + BRANCH + "'  ORDER BY EMP_NAME ";

        try
        {
            con = new SqlConnection(cn);
            con.Open();
            DataSet oDS = new DataSet();
            SqlDataAdapter odaData = new SqlDataAdapter(new SqlCommand(strQuery, con));
            odaData.Fill(oDS, "workingDays");
            return oDS.Tables["workingDays"].Rows[0]["TOTAL"].ToString();
        }
        catch (Exception ex)
        {
            return null;
        }
        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }
    }
    public string TotalLateDaysEmp(string BRANCH, string STARTDATE, string ENDDATE, string empId) //asad
    {
        STARTDATE = String.Format("{0:yyyy/MM/dd}", Convert.ToDateTime(STARTDATE));
        ENDDATE = String.Format("{0:yyyy/MM/dd}", Convert.ToDateTime(ENDDATE));
        string strQuery = " SELECT dbo.TOTAL_LATE_PRESENT_MONTH('" + empId + "', '" + BRANCH + "', '" + STARTDATE + "', '" + ENDDATE + "')TOTAL ";
        string Total = "0";
        try
        {
            con = new SqlConnection(cn);
            con.Open();
            DataSet oDS = new DataSet();
            SqlDataAdapter odaData = new SqlDataAdapter(new SqlCommand(strQuery, con));
            odaData.Fill(oDS, "LateDays");
            Total = oDS.Tables[0].Rows[0]["TOTAL"].ToString();
        }
        catch (Exception ex)
        {
            return null;
        }
        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }
        return Total;
    }
    public string TotalOSDapprovedDaysEmp(string BRANCH, string STARTDATE, string ENDDATE, string empId) //asad
    {
        STARTDATE = String.Format("{0:yyyy/MM/dd}", Convert.ToDateTime(STARTDATE));
        ENDDATE = String.Format("{0:yyyy/MM/dd}", Convert.ToDateTime(ENDDATE));
        string strQuery = " SELECT dbo.TOTAL_LATE_PRESENT_MONTH('" + empId + "', '" + BRANCH + "', '" + STARTDATE + "', '" + ENDDATE + "')TOTAL ";
        string Total = "0";
        try
        {
            con = new SqlConnection(cn);
            con.Open();
            DataSet oDS = new DataSet();
            SqlDataAdapter odaData = new SqlDataAdapter(new SqlCommand(strQuery, con));
            odaData.Fill(oDS, "LateDays");
            Total = oDS.Tables[0].Rows[0]["TOTAL"].ToString();
        }
        catch (Exception ex)
        {
            return null;
        }
        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }
        return Total;
    }
    public string TotalLateApprovedFrontDesk(string BRANCH, string STARTDATE, string ENDDATE, string empId) //asad
    {
        string strQuery = " SELECT dbo.TOTAL_APPROVED_FRONTDESK_LATE_PRESENT_MONTH('" + empId + "', '" + BRANCH + "', '" + STARTDATE + "', '" + ENDDATE + "')TOTAL ";
        string Total = "0";
        try
        {
            con = new SqlConnection(cn);
            con.Open();
            DataSet oDS = new DataSet();
            SqlDataAdapter odaData = new SqlDataAdapter(new SqlCommand(strQuery, con));
            odaData.Fill(oDS, "LateDays");
            Total = oDS.Tables[0].Rows[0]["TOTAL"].ToString();
        }
        catch (Exception ex)
        {
            return null;
        }
        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }
        return Total;
    }
    public DataSet ChkFrontdeskAttendanceApproved(string BranchId, string empId, string DateOfAttendance)
    {
        string strQuery = " set deadlock_priority low; SELECT * FROM PR_EMP_ATTENDENCE AT with(nolock) "
                         + " WHERE convert(date, CONVERT(DATETIME, CONVERT(VARCHAR(23), AT.ATT_DATE_TIME))) = convert(date, CONVERT(DATETIME, CONVERT(VARCHAR(23), '" + DateOfAttendance + "'))) "
                         + " AND AT.EMP_ID='" + empId + "'  AND AT.CMP_BRANCH_ID= '" + BranchId + "' and APPROVAL_STATUS='AV' ";
        DataSet oDS = new DataSet();
        try
        {
            con = new SqlConnection(cn);
            con.Open();
            SqlDataAdapter odaData = new SqlDataAdapter(new SqlCommand(strQuery, con));
            odaData.Fill(oDS, "Approved");
        }
        catch (Exception ex)
        {

        }
        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }
        return oDS;
    }
    public string TotalLeaveEmp(string BRANCH, string STARTDATE, string ENDDATE, string empId)  //asad
    {
        STARTDATE = String.Format("{0:yyyy/MM/dd}", Convert.ToDateTime(STARTDATE));
        ENDDATE = String.Format("{0:yyyy/MM/dd}", Convert.ToDateTime(ENDDATE));
        string strQuery = " Select dbo.TOTAL_LEAVE_USED_MONTH('" + empId + "', '" + BRANCH + "', '" + STARTDATE + "', '" + ENDDATE + "')totalLv ";
        string totalLeave = "0";

        try
        {
            con = new SqlConnection(cn);
            con.Open();
            DataSet oDS = new DataSet();
            SqlDataAdapter odaData = new SqlDataAdapter(new SqlCommand(strQuery, con));
            odaData.Fill(oDS, "totalLeave");
            if (oDS.Tables[0].Rows.Count > 0)
            {
                totalLeave = oDS.Tables[0].Rows[0]["totalLv"].ToString();
                if (totalLeave == "")
                {
                    totalLeave = "0";
                }
            }
        }
        catch (Exception ex)
        {
            return null;
        }
        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }
        return totalLeave;
    }
    public int TotalWeekend(string BRANCH, string STARTDATE, string empId)  //asad
    {
        //string strQuery = " set deadlock_priority low; Select dbo.TOTAL_WEEKEND_DAILY_CHECK('" + empId + "', '" + BRANCH + "', '" + STARTDATE + "')totalWeekend ";
        string strQuery = @"select  count(*)totalweekend from pr_roster_setup rs,pr_emp_roster er
		                    where  rs.prr_id=er.prr_id and  rtrim(ltrim(convert(varchar(10), rs.prr_time_from, 20)))='00:00'
	                     and er.emp_id='" + empId + "'  and er.cmp_branch_id='" + BRANCH + "'  and convert(date,  er.roster_date) = convert(date, '" + STARTDATE + "') ";
        int totalOffDays = 0;

        try
        {
            con = new SqlConnection(cn);
            con.Open();
            DataSet oDS = new DataSet();
            SqlDataAdapter odaData = new SqlDataAdapter(new SqlCommand(strQuery, con));
            odaData.Fill(oDS, "totWeekend");
            if (oDS.Tables[0].Rows.Count > 0)
            {
                totalOffDays = int.Parse(oDS.Tables[0].Rows[0]["totalWeekend"].ToString());
            }
        }
        catch (Exception ex)
        {
            return 0;
        }
        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }
        return totalOffDays;
    }
    public string TotalHolidays(string BRANCH, string STARTDATE, string ENDDATE, string empId)  //asad
    {
        string strQuery = " Select dbo.TOTAL_HOLIDAYS_MONTH('" + empId + "', '" + BRANCH + "', '" + STARTDATE + "', '" + ENDDATE + "')totalHolidays ";
        string totalHoliday = "0";
        try
        {
            con = new SqlConnection(cn);
            con.Open();
            DataSet oDS = new DataSet();
            SqlDataAdapter odaData = new SqlDataAdapter(new SqlCommand(strQuery, con));
            odaData.Fill(oDS, "totHolidays");
            if (oDS.Tables[0].Rows.Count > 0)
            {
                totalHoliday = oDS.Tables[0].Rows[0]["totalHolidays"].ToString();
                if (totalHoliday == "")
                {
                    totalHoliday = "0";
                }
            }
        }
        catch (Exception ex)
        {
            return null;
        }
        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }
        return totalHoliday;
    }
    public DataSet GetRosterEmpwise(string empid, string branch, string STARTDATE, string EndDate)
    {
        string strQuery = " SELECT DISTINCT  CAST(PRR_TYPE AS VARCHAR) + '(' + CAST(PRR_TIME_FROM AS VARCHAR) + ' to ' +  CAST(PRR_TIME_TO AS VARCHAR) + ')' as timespan, CAST(GRACE_TIME AS VARCHAR) + 'min(' + CAST(PRR_TYPE AS VARCHAR) + ')' as considertime "
                            + " FROM PR_EMP_ROSTER PR,PR_ROSTER_SETUP PS,PR_ROSTER_SEHEDULE RSE "
                            + " WHERE PR.PRR_ID=PS.PRR_ID AND PR.CMP_BRANCH_ID=PS.CMP_BRANCH_ID  AND RSE.PRR_ID=PS.PRR_ID AND PR.PR_ROSTER_ID=RSE.PR_ROSTER_ID "
                            + " AND CONVERT(DATETIME, '" + STARTDATE + "', 103)  >= CONVERT(DATETIME, RSE.PRS_START_DATE) AND	CONVERT(DATETIME, '" + EndDate + "', 103)  <= CONVERT(DATETIME, RSE.PRS_END_DATE)"
                            + " AND PR.EMP_ID='" + empid + "' AND PR.CMP_BRANCH_ID='" + branch + "' ";


        try
        {
            con = new SqlConnection(cn);
            con.Open();
            DataSet oDS = new DataSet();
            SqlDataAdapter odaData = new SqlDataAdapter(new SqlCommand(strQuery, con));
            odaData.Fill(oDS, "RosterEmpwise");
            return oDS;
        }
        catch (Exception ex)
        {
            return null;
        }
        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }
    }
    public DataSet GetAttendanceList(string Br, string yr, string empId, string AtTypeIn, string FrmDt, string ToDt) // 
    {
        string strQuery = "SELECT   CB.CMP_BRANCH_NAME, DP.DPT_NAME,  (CAST(SET_CODE AS VARCHAR) + ' - ' + CAST(DSG_TITLE AS VARCHAR)) DNAME, (CAST(EMP_TITLE AS VARCHAR) + ' ' + CAST(EMP_NAME AS VARCHAR)) ENAME,EA.EMP_ID, "
             + " EA.ATT_LATE_TIME,EA.ATT_DATE_TIME,EA.ATT_COMMENTS, "
             + " YR.YR_OFFICE_HOUR,EL.EMP_CODE, EA.ATT_ID, EA.ATT_TYPE, EA.ATT_LATE_STATUS "
             + " FROM   PR_EMP_ATTENDENCE EA,PR_EMPLOYEE_LIST EL,CM_CMP_BRANCH CB, PR_DESIGNATION DE, PR_DEPARTMENT DP,HR_YEAR YR "
             + " WHERE   EA.EMP_ID = EL.EMP_ID AND EA.CMP_BRANCH_ID = CB.CMP_BRANCH_ID "
             + " AND ( (YR.CMP_BRANCH_ID = EL.CMP_BRANCH_ID AND YR.YR_YEAR = '" + yr + "') "
             + " OR (YR.CMP_BRANCH_ID = '" + Br + "' AND YR.YR_YEAR = '" + yr + "')) "
             + " AND DE.DSG_ID = EL.DSG_ID "
             + " AND EL.DPT_ID = DP.DPT_ID "
             + " AND EA.ATT_TYPE = '" + AtTypeIn + "' "
             + " AND Convert(Datetime,EA.ATT_DATE_TIME,103) between Convert (Datetime,'" + FrmDt + "',103) and Convert(Datetime,'" + ToDt + "',103) "
             + " AND CB.CMP_BRANCH_ID = '" + Br + "' AND EL.EMP_ID = '" + empId + "' ";

        try
        {
            con = new SqlConnection(cn);
            con.Open();
            DataSet oDS = new DataSet();
            SqlDataAdapter odaData = new SqlDataAdapter(new SqlCommand(strQuery, con));
            odaData.Fill(oDS, "AttendanceInfo");
            return oDS;
        }
        catch (Exception ex)
        {
            return null;
        }
        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }
    }

    //################################# MD. ASADUZZAMAN DATED:14-JUL-2014 ##############################
    public string GetAttendanceInOutTime(string BranchId, string empId, string DateOfAttendance, string AtType) // 
    {
        //string strQuery = " set deadlock_priority low;  SELECT dbo.EMP_ATTENDANCE_INOUT('" + empId + "','" + BranchId + "','" + DateOfAttendance + "','" + AtType + "')INOUT ";
        string strQuery = " set deadlock_priority low; SELECT (LEFT((CONVERT(TIME(0),AT.ATT_DATE_TIME)),5)  + ' ' + RIGHT(CONVERT(VARCHAR(30), CAST(DATEADD(MINUTE,CONVERT(NUMERIC(8, 2), 0), AT.ATT_DATE_TIME) AS time(0)), 9), 2) )INOUT  "
        + " FROM    PR_EMP_ATTENDENCE AT with(nolock)  	WHERE	convert(date,  AT.ATT_DATE_TIME)  = convert(date, '" + DateOfAttendance + "')  AND AT.EMP_ID='" + empId + "'  AND AT.CMP_BRANCH_ID='" + BranchId + "' AND AT.ATT_TYPE='" + AtType + "'  ";
        string result = "";
        try
        {
            con = new SqlConnection(cn);
            con.Open();
            DataSet oDS = new DataSet();
            SqlDataAdapter odaData = new SqlDataAdapter(new SqlCommand(strQuery, con));
            odaData.Fill(oDS, "AtTime");
            if (oDS.Tables[0].Rows.Count > 0)
            {
                result = oDS.Tables[0].Rows[0]["INOUT"].ToString();
            }
        }
        catch (Exception ex)
        {
            return "";
        }
        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }
        return result;
    }


    public string CheckDailyLeave(string BranchId, string empId, string Today) // asad
    {
        string strQuery = "set deadlock_priority low; SELECT dbo.CHECK_LEAVE_APPROVED_DAILY('" + empId + "','" + BranchId + "','" + Today + "')LV ";
        //string strQuery = @"SELECT COUNT(*)LV  FROM PR_LEAVE pl with(nolock) WHERE convert(date,  pl.LVE_FROM_DATE) <= convert(date,  '" + Today + "') " +
        //                               " and convert(date,  pl.LVE_TO_DATE)  >=  convert(date, '" + Today + "') and pl.EMP_ID='" + empId + "' and pl.CMP_BRANCH_ID='" + BranchId + "' AND pl.LVE_STATUS='R'   GROUP BY pl.EMP_ID";
        string result = "NO";
        try
        {
            con = new SqlConnection(cn);
            con.Open();
            DataSet oDS = new DataSet();
            SqlDataAdapter odaData = new SqlDataAdapter(new SqlCommand(strQuery, con));
            odaData.Fill(oDS, "Leave");
            int count = 0;
            if (oDS.Tables[0].Rows.Count > 0)
            {
                string lvCount = oDS.Tables[0].Rows[0]["LV"].ToString();
                count = int.Parse(lvCount == "" ? "0" : lvCount);
            }
            if (count > 0)
            {
                result = "YES";
            }
            else
            {
                result = "NO";
            }
        }
        catch (Exception ex)
        {
            return "";
        }
        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }
        return result;
    }
    public string InterimHolidayCheck(string BranchId, string empId, string Today) // asad
    {
        //string strQuery = " set deadlock_priority low; select  distinct PRLP_IHCOUNT InterimCount from PR_LEAVE_POLICY lp with(nolock)  "
        //      + "  where lp.PRLT_ID in ( "
        //        + " SELECT PRLT_ID FROM PR_LEAVE pl WHERE "
        //        + " convert(date, CONVERT(DATETIME, CONVERT(VARCHAR(23), pl.LVE_FROM_DATE))) "
        //        + " <= "
        //        + " convert(date, CONVERT(DATETIME, CONVERT(VARCHAR(23), '" + Today + "')))  "
        //        + " and  "
        //        + " convert(date, CONVERT(DATETIME, CONVERT(VARCHAR(23), pl.LVE_TO_DATE))) "
        //        + " >= "
        //        + " convert(date, CONVERT(DATETIME, CONVERT(VARCHAR(23), '" + Today + "')))  "
        //        + " and pl.EMP_ID='" + empId + "' and pl.CMP_BRANCH_ID='" + BranchId + "' AND pl.LVE_STATUS='R' "
        //     + " ) and  "
        //     + "  lp.DSG_ID in (select DSG_ID grade from PR_EMPLOYEE_LIST  where EMP_ID='" + empId + "') ";

                  string   strQuery=@"set deadlock_priority low; select  distinct PRLP_IHCOUNT InterimCount from PR_LEAVE_POLICY lp with(nolock)
      where lp.PRLT_ID in (  SELECT PRLT_ID FROM PR_LEAVE pl WHERE  convert(date, pl.LVE_FROM_DATE) <=  convert(date, '" + Today + "') and   convert(date,  pl.LVE_TO_DATE)  >=  convert(date, '" + Today + "')  and pl.EMP_ID='" + empId + "' and pl.CMP_BRANCH_ID='" + BranchId + "' AND pl.LVE_STATUS='R'  ) and  "
          + "  lp.DSG_ID in (select DSG_ID grade from PR_EMPLOYEE_LIST  where EMP_ID='" + empId + "') ";
        string IHCount = "Yes";
        try
        {
            con = new SqlConnection(cn);
            con.Open();
            DataSet oDS = new DataSet();
            SqlDataAdapter odaData = new SqlDataAdapter(new SqlCommand(strQuery, con));
            odaData.Fill(oDS, "Leave");
            if (oDS.Tables[0].Rows.Count > 0)
            {
                IHCount = oDS.Tables[0].Rows[0]["InterimCount"].ToString();
            }
        }
        catch (Exception ex)
        {
            return "";
        }
        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }
        return IHCount;
    }
    public string GetAttHolidaysAsRosterDate(string BranchId, string empId, string FrmDt) // 
    {
        string strQuery = " set deadlock_priority low; SELECT dbo.HOLIDAY_CHK_ROSTERDATE('" + empId + "','" + BranchId + "','" + FrmDt + "')Holidays  ";
        //string strQuery = "SELECT   COUNT(*)Holidays FROM PR_ROSTER_SETUP RS,PR_EMP_ROSTER ER WHERE RS.PRR_ID=ER.PRR_ID AND	RTRIM(LTRIM(CONVERT(varchar(10), RS.PRR_TIME_FROM, 20)))='00:00' "
        //+ " AND  ER.EMP_ID='" + empId + "' AND  ER.CMP_BRANCH_ID='" + BranchId + "' AND convert(date, ER.ROSTER_DATE) = convert(date,  '" + FrmDt + "') ";
        string result = "0";
        try
        {
            con = new SqlConnection(cn);
            con.Open();
            DataSet oDS = new DataSet();
            SqlDataAdapter odaData = new SqlDataAdapter(new SqlCommand(strQuery, con));
            odaData.Fill(oDS, "holiday");
            if (oDS.Tables[0].Rows.Count > 0)
            {
                result = oDS.Tables[0].Rows[0]["Holidays"].ToString();
            }
        }
        catch (Exception ex)
        {
            return "0";
        }
        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }
        if (result == "")
        {
            result = "0";
        }
        return result;
    }
    public string GetAttWeekendAsRosterDate(string BranchId, string empId, string FrmDt) // 
    {


        string strQuery = " set deadlock_priority low; SELECT dbo.WEEKEND_CHECK_ROSTERDATE('" + empId + "','" + BranchId + "','" + FrmDt + "')Weekend  ";
        string result = "0";
        try
        {
            con = new SqlConnection(cn);
            con.Open();
            DataSet oDS = new DataSet();
            SqlDataAdapter odaData = new SqlDataAdapter(new SqlCommand(strQuery, con));
            odaData.Fill(oDS, "weekend");
            if (oDS.Tables[0].Rows.Count > 0)
            {
                result = oDS.Tables[0].Rows[0]["Weekend"].ToString();
            }
        }
        catch (Exception ex)
        {
            return "0";
        }
        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }
        if (result == "")
        {
            result = "0";
        }
        return result;
    }

    public string GetAttendanceLateCount(string BranchId, string empId, string DateOfAttendance, string AtType) // 
    {
        string strQuery = " set deadlock_priority low; SELECT dbo.EMP_ATTENDANCE_LATECOUNT('" + empId + "','" + BranchId + "','" + DateOfAttendance + "','" + AtType + "')latecount ";
        string result = "";
        try
        {
            con = new SqlConnection(cn);
            con.Open();
            DataSet oDS = new DataSet();
            SqlDataAdapter odaData = new SqlDataAdapter(new SqlCommand(strQuery, con));
            odaData.Fill(oDS, "AtLate");
            if (oDS.Tables[0].Rows.Count > 0)
            {
                result = oDS.Tables[0].Rows[0]["latecount"].ToString();
            }
        }
        catch (Exception ex)
        {
            return "";
        }
        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }
        return result;
    }
    public string GetAttendanceLateCountApprovedFD(string BranchId, string empId, string DateOfAttendance, string AtType) // 
    {
        string strQuery = " SELECT dbo.EMP_ATTENDANCE_LATECOUNT_APPROVED_FRONTDESK('" + empId + "','" + BranchId + "','" + DateOfAttendance + "','" + AtType + "')latecount ";
        string result = "";
        try
        {
            con = new SqlConnection(cn);
            con.Open();
            DataSet oDS = new DataSet();
            SqlDataAdapter odaData = new SqlDataAdapter(new SqlCommand(strQuery, con));
            odaData.Fill(oDS, "AtLate");
            if (oDS.Tables[0].Rows.Count > 0)
            {
                result = oDS.Tables[0].Rows[0]["latecount"].ToString();
            }
        }
        catch (Exception ex)
        {
            return "";
        }
        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }
        return result;
    }
    public DataSet GetAttendanceGeneralInfo(string BranchId, string empId, string DateOfAttendance, string AtType) // 
    {
        string strQuery = "  set deadlock_priority low; SELECT DISTINCT ACCESS_METHOD as ACCESSMETHOD, ACCESS_METHOD  = CASE AT.ACCESS_METHOD  WHEN 'FD' THEN 'FD' WHEN 'FP' THEN 'FINGER PRINT' WHEN 'SI' THEN 'SOFTWARE INPUT' WHEN 'RDOOR' THEN 'RDOOR' END, isnull(AT.ATT_COMMENTS,'')ATT_COMMENTS "
                         + " FROM PR_EMP_ATTENDENCE AT with(nolock) "
                         + " WHERE  "
                         + " convert(date, CONVERT(DATETIME, CONVERT(VARCHAR(23), AT.ATT_DATE_TIME))) = convert(date, CONVERT(DATETIME, CONVERT(VARCHAR(23), '" + DateOfAttendance + "'))) "
                         + " AND AT.EMP_ID='" + empId + "'  AND AT.CMP_BRANCH_ID= '" + BranchId + "'  AND AT.ATT_TYPE='" + AtType + "' ";
        DataSet oDS = new DataSet();
        try
        {
            con = new SqlConnection(cn);
            con.Open();
            SqlDataAdapter odaData = new SqlDataAdapter(new SqlCommand(strQuery, con));
            odaData.Fill(oDS, "AtGeneral");
        }
        catch (Exception ex)
        {

        }
        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }
        return oDS;
    }
    public DataSet GetBasicInfoForAttendance(string empId)
    {
        string strQuery = @"  select el.EMP_NAME_BANGLA, DPT.DPT_NAME, el.EMP_FATHER_NAME,el.EMP_MOTHER_NAME,el.FATHERS_NAME_BANGLA,el.MOTHERS_NAME_BANGLA,el.EMP_PER_ADDRESS,DE.DSG_TITLE as Grade,
       el.EMP_PRE_ADDRES, CAST(ISNULL(el.EMP_CODE, '') AS VARCHAR(100)) as empId,CAST(ISNULL(EMP_TITLE, '') AS VARCHAR(100)) + ' ' + CAST(EMP_NAME AS VARCHAR(500)) AS ENAME, 
       LTRIM(RTRIM(el.EMP_LATE_SHOW)) EMP_LATE_SHOW,  pd.CMP_BRANCH_NAME, 
       (SELECT DSG_TITLE FROM PR_DESIGNATION p WHERE p.SET_TYPE='D' AND p.DSG_ID=el.DSG_ID_MAIN)DSG_TITLE, (SELECT DSG_TITLE_BANGLA FROM PR_DESIGNATION p WHERE p.SET_TYPE='D' AND p.DSG_ID=el.DSG_ID_MAIN)DSG_TITLE_BANGLA,  CONVERT(varchar, EMP_JOINING_DATE, 107) EMP_JOINING_DATE ,
       CONVERT(varchar, el.EMP_BIRTHDAY, 107) EMP_BIRTHDAY ,el.EMP_BLOODGRP, CONVERT(varchar, el.EMP_CONFIR_DATE, 107) EMP_CONFIR_DATE,el.EMP_CONTACT_NUM,el.EMP_EMAIL,el.EMP_EMAIL_PERSONAL ,
       [dbo].[CALCULATE_SERVICETIME](el.EMP_JOINING_DATE,GETDATE()) as ExperianceInPalo,[dbo].[CALCULATE_SERVICETIME](el.EMP_BIRTHDAY,GETDATE()) Age

       from PR_EMPLOYEE_LIST el left join PR_DESIGNATION as DE on DE.DSG_ID=el.DSG_ID and DE.SET_TYPE='G'  
       left join PR_DEPARTMENT as DPT on DPT.DPT_ID=el.DPT_ID ,CM_CMP_BRANCH pd "
                              + " where  "
                              + " el.CMP_BRANCH_ID=pd.CMP_BRANCH_ID and el.EMP_ID='" + empId + "' order by convert(int, isnull( DPT.ORDER_BY,'1000000')) ,  convert(int, isnull(EL.ORDER_BY,'10000000'))   ";
        try
        {
            con = new SqlConnection(cn);
            con.Open();
            DataSet oDS = new DataSet();
            SqlDataAdapter odaData = new SqlDataAdapter(new SqlCommand(strQuery, con));
            odaData.Fill(oDS, "attBasic");
            return oDS;
        }
        catch (Exception ex)
        {
            return null;
        }
        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }
    }
    #endregion


    #region for Special Leave
    ///<summary>
    /// Author:Tanjil Alam
    /// Date: 08/01/2014
    /// 
    /// </summary>
    /// 
    /// <returns></returns>
    /// 

    //sydur 23-02-2014
    public DataSet GetSpecialLeaveType(string bID)
    {

        string strQuery = "select PRLT_ID, PRLT_TITLE from PR_LEAVE_TYPE where NATURE_TYPE='SL' AND CMP_BRANCH_ID='" + bID + "'";
        try
        {
            con = new SqlConnection(cn);
            con.Open();
            DataSet oDS = new DataSet();
            SqlDataAdapter odaData = new SqlDataAdapter(new SqlCommand(strQuery, con));
            odaData.Fill(oDS, "LEAVE_TYPE");
            return oDS;
        }
        catch (Exception ex)
        {
            return null;
        }
        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }
    }

    public bool ChkSchedule(string bId, string eId, string claimId)  /*** added by : Niaz Morshed, 29-Jan-2014, Decription : Chaque Duplicate Schedule  ***/
    {
        con = new SqlConnection(cn);
        con.Open();
        string strSql;
        bool result = false;
        strSql = "SELECT DISTINCT ADV_AMOUNT FROM HR_CLAIM_ADVANCE_LIST CA, HR_CLAIM_SCHEDULE CS "
               + "WHERE CA.CMP_BRANCH_ID='" + bId + "' AND CA.EMP_ID='" + eId + "' AND CA.CL_ID='" + claimId + "' "
               + "AND CA.EMP_ID=CS.EMP_ID AND CA.CL_ID= CA.CL_ID ";
        try
        {
            //DataRow oOrderRow;
            DataSet oDS = new DataSet();
            SqlDataAdapter oOrdersDataAdapter = new SqlDataAdapter(new SqlCommand(strSql, con));
            SqlCommandBuilder oOrdersCmdBuilder = new SqlCommandBuilder(oOrdersDataAdapter);
            oOrdersDataAdapter.MissingSchemaAction = MissingSchemaAction.AddWithKey;
            oOrdersDataAdapter.Fill(oDS, "HR_CLAIM_ADVANCE_LIST");
            DataTable tbl_AD = oDS.Tables["HR_CLAIM_ADVANCE_LIST"];
            // already exists
            if (tbl_AD.Rows.Count > 0) { result = true; }
            else { result = false; }
        }
        catch (Exception ex)
        {
            return false;
        }
        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }
        return result;
    }

    //public DataSet GetSpecialLeaveType(string bID)
    //{
    //    string strQuery = "select PRLT_ID, PRLT_TITLE from PR_LEAVE_TYPE where NATURE_TYPE='SL' AND CMP_BRANCH_ID='" + bID + "'";
    //    try
    //    {
    //        con = new SqlConnection(cn);
    //        con.Open();  
    //        DataSet oDS = new DataSet();
    //        SqlDataAdapter odaData = new SqlDataAdapter(new SqlCommand(strQuery, con));
    //        odaData.Fill(oDS, "LEAVE_TYPE");
    //        return oDS;
    //    }
    //    catch (Exception ex)
    //    {
    //        return null;
    //    }
    //}
    ///<summary>
    /// Author:Tanjil Alam
    /// Date: 08/01/2014
    /// 
    /// </summary>
    /// 
    /// <returns></returns>
    public DataSet GetEmployeeInfo(string bId)
    {
        string strQuery = "select EMP_ID,EL.DSG_ID,EMP_NAME,EMP_STATUS,EL.CMP_BRANCH_ID,EMP_FINAL_CONFIR_DATE,DSG_TITLE,TYP_TYPE from PR_EMPLOYEE_LIST EL, "
                        + "PR_DESIGNATION PD, HR_EMP_TYPE ET "
                        + "WHERE EL.DSG_ID=PD.DSG_ID AND EL.EMP_STATUS=ET.TYP_CODE AND EL.CMP_BRANCH_ID='" + bId + "' AND EMP_STATUS='2'";


        try
        {

            con = new SqlConnection(cn);
            con.Open();
            DataSet oDS = new DataSet();
            SqlDataAdapter odaData = new SqlDataAdapter(new SqlCommand(strQuery, con));
            odaData.Fill(oDS, "PR_EMPLOYEE_INFO");
            return oDS;
        }
        catch (Exception ex)
        {
            return null;
        }
        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }
    }
    public DataSet GetEmployeeInfoSpecialLV(string bId)
    {
        string strQuery = "SELECT EL.DPT_ID,(SELECT DPT_NAME FROM PR_DEPARTMENT WHERE DPT_ID=EL.DPT_ID)DPT_NAME, EL.EMP_ID, EL.DSG_ID, EMP_NAME,EMP_STATUS,EL.CMP_BRANCH_ID,EMP_FINAL_CONFIR_DATE,DSG_TITLE,TYP_TYPE from PR_EMPLOYEE_LIST EL, "
                        + "PR_DESIGNATION PD, HR_EMP_TYPE ET,PR_LEAVE_POLICY LP "
                        + "WHERE EL.DSG_ID=PD.DSG_ID AND EL.EMP_STATUS=ET.TYP_CODE AND EL.CMP_BRANCH_ID='" + bId + "' AND EMP_STATUS='2' and LP.EMP_ID=EL.EMP_ID";


        try
        {

            con = new SqlConnection(cn);
            con.Open();
            DataSet oDS = new DataSet();
            SqlDataAdapter odaData = new SqlDataAdapter(new SqlCommand(strQuery, con));
            odaData.Fill(oDS, "PR_EMPLOYEE_INFO");
            return oDS;
        }
        catch (Exception ex)
        {
            return null;
        }
        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }
    }
    ///<summary>
    /// Author:Tanjil Alam
    /// Date: 08/01/2014
    /// 
    /// </summary>
    /// 
    /// <returns></returns>
    //public DataSet GetLeavePolicy(string leaveTypeID, string degID, string empType)
    //{
    //    string strQuery = "select PRLP_ALLOWANCE, PRLP_FREQUANCY_LEAVE, PRLP_VALID_DAY from PR_LEAVE_POLICY "
    //                     + "where PRLT_ID='" + leaveTypeID + "' AND DSG_ID='" + degID + "' AND TYP_CODE='" + empType + "' AND NATURE_TYPE='SL'";


    //    try
    //    {

    //        DataSet oDS = new DataSet();
    //        SqlDataAdapter odaData = new SqlDataAdapter(new SqlCommand(strQuery, con));
    //        odaData.Fill(oDS, "LEAVE_POLICY");
    //        return oDS;
    //    }
    //    catch (Exception ex)
    //    {
    //        return null;
    //    }
    //    finally
    //    {
    //        con.Close();
    //        con = null;
    //    }
    //}


    //sydur 24-02-2014
    public DataSet GetLeavePolicy(string leaveTypeID, string degID, string empType)
    {
        string strQuery = "select PRLP_ALLOWANCE, PRLP_FREQUANCY_LEAVE, PRLP_VALID_DAY from PR_LEAVE_POLICY "
                         + "where PRLT_ID='" + leaveTypeID + "' AND DSG_ID='" + degID + "' AND TYP_CODE='" + empType + "' AND NATURE_TYPE='SL'";


        try
        {

            con = new SqlConnection(cn);
            con.Open();
            DataSet oDS = new DataSet();
            SqlDataAdapter odaData = new SqlDataAdapter(new SqlCommand(strQuery, con));
            odaData.Fill(oDS, "LEAVE_POLICY");
            return oDS;
        }
        catch (Exception ex)
        {
            return null;
        }
        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }
    }

    ///<summary>
    /// Author:Tanjil Alam
    /// Date: 08/01/2014
    /// 
    /// </summary>
    /// 
    /// <returns></returns>

    //sydur 24-02-2014
    public DataSet GetEmployeeInfo(string bId, string degID, string eID)
    {
        string strQuery = "select EMP_ID,EL.DSG_ID,EMP_NAME,EMP_STATUS,EL.CMP_BRANCH_ID,EMP_FINAL_CONFIR_DATE,DSG_TITLE,TYP_TYPE from PR_EMPLOYEE_LIST EL, "
                        + "PR_DESIGNATION PD, HR_EMP_TYPE ET "
                        + "WHERE EL.DSG_ID=PD.DSG_ID AND EL.EMP_STATUS=ET.TYP_CODE AND EL.CMP_BRANCH_ID='" + bId + "' "
                        + "AND EL.DSG_ID='" + degID + "' AND EL.EMP_ID='" + eID + "' AND EMP_STATUS='2'";


        try
        {
            con = new SqlConnection(cn);
            con.Open();
            DataSet oDS = new DataSet();
            SqlDataAdapter odaData = new SqlDataAdapter(new SqlCommand(strQuery, con));
            odaData.Fill(oDS, "PR_EMPLOYEE_INFO");
            return oDS;
        }
        catch (Exception ex)
        {
            return null;
        }
        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }
    }
    public DataSet GetEmployeeInfoSpecialLV(string bId, string degID, string eID)
    {
        string strQuery = "select EL.EMP_ID,EL.DSG_ID,EMP_NAME,EMP_STATUS,EL.CMP_BRANCH_ID,EMP_FINAL_CONFIR_DATE,DSG_TITLE,TYP_TYPE from PR_EMPLOYEE_LIST EL, "
                        + "PR_DESIGNATION PD, HR_EMP_TYPE ET ,PR_LEAVE_POLICY LP"
                        + "WHERE EL.DSG_ID=PD.DSG_ID AND EL.EMP_STATUS=ET.TYP_CODE AND EL.CMP_BRANCH_ID='" + bId + "' "
                        + "AND EL.DSG_ID='" + degID + "' AND EL.EMP_ID='" + eID + "' AND EMP_STATUS='2' and LP.EMP_ID=EL.EMP_ID ";


        try
        {
            con = new SqlConnection(cn);
            con.Open();
            DataSet oDS = new DataSet();
            SqlDataAdapter odaData = new SqlDataAdapter(new SqlCommand(strQuery, con));
            odaData.Fill(oDS, "PR_EMPLOYEE_INFO");
            return oDS;
        }
        catch (Exception ex)
        {
            return null;
        }
        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }
    }
    ///<summary>
    /// Author:Tanjil Alam
    /// Date: 08/01/2014
    /// 
    /// </summary>
    /// 
    /// <returns></returns>
    public DataSet GetEmployeeInfo(string bId, string degID)
    {
        string strQuery = "select EMP_ID,EL.DSG_ID,EMP_NAME,EMP_STATUS,EL.CMP_BRANCH_ID,EMP_FINAL_CONFIR_DATE,DSG_TITLE,TYP_TYPE from PR_EMPLOYEE_LIST EL, "
                        + "PR_DESIGNATION PD, HR_EMP_TYPE ET "
                        + "WHERE EL.DSG_ID=PD.DSG_ID AND EL.EMP_STATUS=ET.TYP_CODE AND EL.CMP_BRANCH_ID='" + bId + "' "
                        + "AND EL.DSG_ID='" + degID + "' AND EMP_STATUS='2'";


        try
        {
            con = new SqlConnection(cn);
            con.Open();
            DataSet oDS = new DataSet();
            SqlDataAdapter odaData = new SqlDataAdapter(new SqlCommand(strQuery, con));
            odaData.Fill(oDS, "PR_EMPLOYEE_INFO");
            return oDS;
        }
        catch (Exception ex)
        {
            return null;
        }
        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }
    }
    public DataSet GetEmployeeInfoSpecialLV(string bId, string degID)
    {
        string strQuery = "select EL.EMP_ID,EL.DSG_ID,EMP_NAME,EMP_STATUS,EL.CMP_BRANCH_ID,EMP_FINAL_CONFIR_DATE,DSG_TITLE,TYP_TYPE from PR_EMPLOYEE_LIST EL, "
                        + "PR_DESIGNATION PD, HR_EMP_TYPE ET,PR_LEAVE_POLICY LP "
                        + "WHERE LP.EMP_ID=EL.EMP_ID and EL.DSG_ID=PD.DSG_ID AND EL.EMP_STATUS=ET.TYP_CODE AND EL.CMP_BRANCH_ID='" + bId + "' "
                        + "AND EL.DSG_ID='" + degID + "' AND EMP_STATUS='2'";


        try
        {
            con = new SqlConnection(cn);
            con.Open();
            DataSet oDS = new DataSet();
            SqlDataAdapter odaData = new SqlDataAdapter(new SqlCommand(strQuery, con));
            odaData.Fill(oDS, "PR_EMPLOYEE_INFO");
            return oDS;
        }
        catch (Exception ex)
        {
            return null;
        }
        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }
    }
    #endregion

    ///<summary>
    /// Author:Tanjil Alam
    /// Date: 08/01/2014
    /// 
    /// </summary>
    /// 
    /// <returns></returns>
    public string InsertLeaveInfo(string strLeaveType, string frmDate, string toDate, string joinDate, string totDuration, string totLeaveCount,        //Developed By: Md. Sydur rahman (17-Dec-13)
                                  string strResPrsn, string strLeavePurpose, string strphoneNum, string strAddress, string strEmpID,
                                   string strBranchID, string strLeaveStatus, string strCycelNo)
    {

        string strSql = "";
        strSql = "select PRLT_ID, LVE_FROM_DATE, LVE_TO_DATE, LVE_JOIN_DATE,LVE_DURATION,LVE_COUNT,LVE_RESPERSON, "
               + "LVE_PURPOSE,LVE_TELEPHONE,LVE_ADDRESS,EMP_ID,CMP_BRANCH_ID,LVE_STATUS,CYCEL_NO from PR_LEAVE";
        try
        {
            DataRow oOrderRow;
            con = new SqlConnection(cn);
            con.Open();
            dbTransaction = con.BeginTransaction();
            DataSet oDS = new DataSet();
            SqlDataAdapter oOrdersDataAdapter = new SqlDataAdapter(new SqlCommand(strSql, con, dbTransaction));
            SqlCommandBuilder oOrdersCmdBuilder = new SqlCommandBuilder(oOrdersDataAdapter);
            oOrdersDataAdapter.FillSchema(oDS, SchemaType.Source);

            DataTable dtAddSchedule = oDS.Tables["Table"];
            dtAddSchedule.TableName = "PR_LEAVE";

            oOrderRow = oDS.Tables["PR_LEAVE"].NewRow();
            oOrderRow["PRLT_ID"] = strLeaveType;
            oOrderRow["LVE_FROM_DATE"] = frmDate;
            oOrderRow["LVE_TO_DATE"] = toDate;
            oOrderRow["LVE_JOIN_DATE"] = joinDate;
            oOrderRow["LVE_DURATION"] = totDuration;
            oOrderRow["LVE_COUNT"] = totLeaveCount;
            oOrderRow["LVE_RESPERSON"] = strResPrsn;
            oOrderRow["LVE_PURPOSE"] = strLeavePurpose;
            oOrderRow["LVE_TELEPHONE"] = strphoneNum;
            oOrderRow["LVE_ADDRESS"] = strAddress;
            oOrderRow["EMP_ID"] = strEmpID;
            oOrderRow["CMP_BRANCH_ID"] = strBranchID;
            oOrderRow["LVE_STATUS"] = strLeaveStatus;
            oOrderRow["CYCEL_NO"] = strCycelNo;

            oDS.Tables["PR_LEAVE"].Rows.Add(oOrderRow);
            oOrdersDataAdapter.Update(oDS, "PR_LEAVE");
            dbTransaction.Commit();
            return "Saved Successfully.";
        }
        catch (Exception ex)
        {
            return ex.Message.ToString();
        }
        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }
    }


    public string InsertLeaveInfoForBulk(string strLeaveType, string frmDate, string toDate, string joinDate, string totDuration, string totLeaveCount,        //Developed By: Md. Sydur rahman (17-Dec-13)
                                  string strResPrsn, string strLeavePurpose, string strphoneNum, string strAddress, string strEmpID,
                                   string strBranchID, string strLeaveStatus, string strCycelNo, string leaveApprovedDate)
    {

        string strSql = "";
        strSql = "select PRLT_ID, LVE_FROM_DATE,LVE_APPROVED_DAY,LVE_APPROVED_DATE, LVE_TO_DATE, LVE_JOIN_DATE,LVE_DURATION,LVE_COUNT,LVE_RESPERSON, "
               + "LVE_PURPOSE,LVE_TELEPHONE,LVE_ADDRESS,EMP_ID,CMP_BRANCH_ID,LVE_STATUS,CYCEL_NO from PR_LEAVE";
        try
        {
            DataRow oOrderRow;
            con = new SqlConnection(cn);
            con.Open();
            dbTransaction = con.BeginTransaction();
            DataSet oDS = new DataSet();
            SqlDataAdapter oOrdersDataAdapter = new SqlDataAdapter(new SqlCommand(strSql, con, dbTransaction));
            SqlCommandBuilder oOrdersCmdBuilder = new SqlCommandBuilder(oOrdersDataAdapter);
            oOrdersDataAdapter.FillSchema(oDS, SchemaType.Source);

            DataTable dtAddSchedule = oDS.Tables["Table"];
            dtAddSchedule.TableName = "PR_LEAVE";

            oOrderRow = oDS.Tables["PR_LEAVE"].NewRow();
            oOrderRow["PRLT_ID"] = strLeaveType;
            oOrderRow["LVE_FROM_DATE"] = frmDate;
            oOrderRow["LVE_TO_DATE"] = toDate;
            oOrderRow["LVE_JOIN_DATE"] = joinDate;
            oOrderRow["LVE_DURATION"] = totDuration;
            oOrderRow["LVE_COUNT"] = totLeaveCount;
            oOrderRow["LVE_RESPERSON"] = strResPrsn;
            oOrderRow["LVE_PURPOSE"] = strLeavePurpose;
            oOrderRow["LVE_TELEPHONE"] = strphoneNum;
            oOrderRow["LVE_ADDRESS"] = strAddress;
            oOrderRow["EMP_ID"] = strEmpID;
            oOrderRow["CMP_BRANCH_ID"] = strBranchID;
            oOrderRow["LVE_STATUS"] = strLeaveStatus;
            oOrderRow["CYCEL_NO"] = strCycelNo;
            oOrderRow["LVE_APPROVED_DAY"] = totLeaveCount;
            oOrderRow["LVE_APPROVED_DATE"] = leaveApprovedDate;

            oDS.Tables["PR_LEAVE"].Rows.Add(oOrderRow);
            oOrdersDataAdapter.Update(oDS, "PR_LEAVE");
            dbTransaction.Commit();
            return "Saved Successfully.";
        }
        catch (Exception ex)
        {
            return ex.Message.ToString();
        }
        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }
    }

    public string InsertLeaveInfo(string strLeaveType, string frmDate, string toDate, string joinDate, string totDuration, string totLeaveCount,        //Developed By: Md. Sydur rahman (17-Dec-13)
                                  string strResPrsn, string strLeavePurpose, string strphoneNum, string strAddress, string strEmpID,
                                   string strBranchID, string strLeaveStatus, string strCycelNo, string deleteAllwRcLv)
    {

        string strSql = "";
        strSql = "select PRLT_ID, LVE_FROM_DATE, LVE_TO_DATE, LVE_JOIN_DATE,LVE_DURATION,LVE_COUNT,LVE_RESPERSON, "
               + "LVE_PURPOSE,LVE_TELEPHONE,LVE_ADDRESS,EMP_ID,CMP_BRANCH_ID,LVE_STATUS,CYCEL_NO,DeleteAllowRecommendedLv,SYS_USR_ID,LVE_DOCUMENT,CANCEL_REMARKS,CANCEL_DOCUMENT_PATH from PR_LEAVE";
        try
        {
            DataRow oOrderRow;
            con = new SqlConnection(cn);
            con.Open();
            dbTransaction = con.BeginTransaction();
            DataSet oDS = new DataSet();
            SqlDataAdapter oOrdersDataAdapter = new SqlDataAdapter(new SqlCommand(strSql, con, dbTransaction));
            SqlCommandBuilder oOrdersCmdBuilder = new SqlCommandBuilder(oOrdersDataAdapter);
            oOrdersDataAdapter.FillSchema(oDS, SchemaType.Source);

            DataTable dtAddSchedule = oDS.Tables["Table"];
            dtAddSchedule.TableName = "PR_LEAVE";

            oOrderRow = oDS.Tables["PR_LEAVE"].NewRow();
            oOrderRow["PRLT_ID"] = strLeaveType;
            oOrderRow["LVE_FROM_DATE"] = frmDate;
            oOrderRow["LVE_TO_DATE"] = toDate;
            oOrderRow["LVE_JOIN_DATE"] = joinDate;
            oOrderRow["LVE_DURATION"] = totDuration;
            oOrderRow["LVE_COUNT"] = totLeaveCount;
            oOrderRow["LVE_RESPERSON"] = strResPrsn;
            oOrderRow["LVE_PURPOSE"] = strLeavePurpose;
            oOrderRow["LVE_TELEPHONE"] = strphoneNum;
            oOrderRow["LVE_ADDRESS"] = strAddress;
            oOrderRow["EMP_ID"] = strEmpID;
            oOrderRow["CMP_BRANCH_ID"] = strBranchID;
            oOrderRow["LVE_STATUS"] = strLeaveStatus;
            oOrderRow["CYCEL_NO"] = strCycelNo;
            oOrderRow["DeleteAllowRecommendedLv"] = deleteAllwRcLv;
            oOrderRow["SYS_USR_ID"] = "";
            oOrderRow["LVE_DOCUMENT"] = "";
            oOrderRow["CANCEL_REMARKS"] = "";
            oOrderRow["CANCEL_DOCUMENT_PATH"] = "";

            oDS.Tables["PR_LEAVE"].Rows.Add(oOrderRow);
            oOrdersDataAdapter.Update(oDS, "PR_LEAVE");
            dbTransaction.Commit();
            return "Saved Successfully.";
        }
        catch (Exception ex)
        {
            return ex.Message.ToString();
        }
        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }
    }
    public string InsertLeaveInfoDeleted(
                                            string branch, string emplId, string LvType,
                                            string FrmDate, string ToDate, string joinDate, string duration,
                                            string Approveddays, string status, string ApprovedDate, string ApprovedByHR,
                                            string Purpose, string remarksApproved, string phone, string address, string ReasonDel
                                        )
    {

        string strSql = "";
        strSql = "SELECT CMP_BRANCH_ID,EMP_ID, PRLT_ID, LVE_FROM_DATE, LVE_TO_DATE, LVE_JOIN_DATE,LVE_DURATION, "
                + " LVE_APPROVED_DAY, LVE_STATUS,LVE_APPROVED_DATE, "
                + " LVE_APPROVED_HR,LVE_PURPOSE,LVE_APPROVAL_REMARKS, LVE_TELEPHONE,LVE_ADDRESS,DeleteReason from PR_LEAVE_DELETED";
        try
        {
            DataRow oOrderRow;
            con = new SqlConnection(cn);
            con.Open();
            dbTransaction = con.BeginTransaction();
            DataSet oDS = new DataSet();
            SqlDataAdapter oOrdersDataAdapter = new SqlDataAdapter(new SqlCommand(strSql, con, dbTransaction));
            SqlCommandBuilder oOrdersCmdBuilder = new SqlCommandBuilder(oOrdersDataAdapter);
            oOrdersDataAdapter.FillSchema(oDS, SchemaType.Source);
            //--------------------------------------
            DataTable dtAddSchedule = oDS.Tables["Table"];
            dtAddSchedule.TableName = "PR_LEAVE_DELETED";
            //-------------------------------------
            oOrderRow = oDS.Tables["PR_LEAVE_DELETED"].NewRow();
            //--------------------------------------
            oOrderRow["CMP_BRANCH_ID"] = branch;
            oOrderRow["EMP_ID"] = emplId;
            oOrderRow["PRLT_ID"] = LvType;
            oOrderRow["LVE_FROM_DATE"] = FrmDate;
            oOrderRow["LVE_TO_DATE"] = ToDate;
            oOrderRow["LVE_JOIN_DATE"] = joinDate;
            oOrderRow["LVE_DURATION"] = duration;
            oOrderRow["LVE_APPROVED_DAY"] = Approveddays;
            oOrderRow["LVE_STATUS"] = status;
            oOrderRow["LVE_APPROVED_DATE"] = ApprovedDate;
            oOrderRow["LVE_APPROVED_HR"] = ApprovedByHR;
            oOrderRow["LVE_PURPOSE"] = Purpose;
            oOrderRow["LVE_APPROVAL_REMARKS"] = remarksApproved;
            oOrderRow["LVE_TELEPHONE"] = phone;
            oOrderRow["LVE_ADDRESS"] = address;
            oOrderRow["DeleteReason"] = ReasonDel;
            //-----------------------------------------
            oDS.Tables["PR_LEAVE_DELETED"].Rows.Add(oOrderRow);
            oOrdersDataAdapter.Update(oDS, "PR_LEAVE_DELETED");
            dbTransaction.Commit();
            return "Saved Successfully.";
        }
        catch (Exception ex)
        {
            return ex.Message.ToString();
        }
        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }
    }

    ///<summary>
    /// Author:Tanjil Alam
    /// Date: 08/01/2014
    /// 
    /// </summary>
    /// 
    /// <returns></returns>
    public string InsertLeaveType(string strCmpBranch, string strNatureType, string strLeaveCodeOdr, string strLeaveCode, string strLeaveType,
                                      string strWithPay, string strEnchasable, string strDetail)
    {

        string strSql = "";
        strSql = "select DOCUMENT_MENDATORY, PRLT_TITLE,PRLT_DETAILS,PRLT_IS_ENCASHABLE,PRLT_IS_PAYABLE,CMP_BRANCH_ID,PRTL_CODE, NATURE_TYPE,PRLT_CODE_NAME from PR_LEAVE_TYPE ";
        try
        {
            DataRow oOrderRow;
            con = new SqlConnection(cn);
            con.Open();
            dbTransaction = con.BeginTransaction();
            DataSet oDS = new DataSet();
            SqlDataAdapter oOrdersDataAdapter = new SqlDataAdapter(new SqlCommand(strSql, con, dbTransaction));
            SqlCommandBuilder oOrdersCmdBuilder = new SqlCommandBuilder(oOrdersDataAdapter);
            oOrdersDataAdapter.FillSchema(oDS, SchemaType.Source);

            DataTable dtAddSchedule = oDS.Tables["Table"];
            dtAddSchedule.TableName = "PR_LEAVE_TYPE";

            oOrderRow = oDS.Tables["PR_LEAVE_TYPE"].NewRow();
            oOrderRow["PRLT_TITLE"] = strLeaveType;
            oOrderRow["PRLT_DETAILS"] = strDetail;
            oOrderRow["PRLT_IS_ENCASHABLE"] = strEnchasable;
            oOrderRow["PRLT_IS_PAYABLE"] = strWithPay;
            oOrderRow["CMP_BRANCH_ID"] = strCmpBranch;
            oOrderRow["PRTL_CODE"] = strLeaveCodeOdr;
            oOrderRow["NATURE_TYPE"] = strNatureType;
            oOrderRow["PRLT_CODE_NAME"] = strLeaveCode;
            oOrderRow["DOCUMENT_MENDATORY"] = "";

            oDS.Tables["PR_LEAVE_TYPE"].Rows.Add(oOrderRow);
            oOrdersDataAdapter.Update(oDS, "PR_LEAVE_TYPE");
            dbTransaction.Commit();
            return "Saved Successfully.";
        }
        catch (Exception ex)
        {
            return ex.Message.ToString();
        }
        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }
    }

    ///<summary>
    /// Author:Tanjil Alam
    /// Date: 08/01/2014
    /// 
    /// </summary>
    /// 
    /// <returns></returns>
    public string ChkCodeOrder(string bID, string strLeaveCodeOdr, string strNatureType)
    {
        string massage = "";
        string strQuery = "select PRTL_CODE,PRLT_CODE_NAME from PR_LEAVE_TYPE "
                        + "where PRTL_CODE='" + strLeaveCodeOdr + "' AND NATURE_TYPE='" + strNatureType + "' AND CMP_BRANCH_ID in('" + bID + "')";


        try
        {
            con = new SqlConnection(cn);
            con.Open();
            DataSet oDS = new DataSet();
            SqlDataAdapter odaData = new SqlDataAdapter(new SqlCommand(strQuery, con));
            odaData.Fill(oDS, "PR_LEAVE_TYPE");

            if (oDS.Tables["PR_LEAVE_TYPE"].Rows.Count > 0)
            {
                massage = "Data Exist";
            }
            else
            {
                massage = "";
            }

            return massage;
        }
        catch (Exception ex)
        {
            return null;
        }
        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }
    }
    ///<summary>
    /// Author:Tanjil Alam
    /// Date: 08/01/2014
    /// 
    /// </summary>
    /// 
    /// <returns></returns>
    public string ChkCodeName(string bID, string strLeaveCode, string strNatureType)
    {
        string massage = "";
        string strQuery = "select PRTL_CODE,PRLT_CODE_NAME from PR_LEAVE_TYPE "
                        + "where PRLT_CODE_NAME='" + strLeaveCode + "' AND NATURE_TYPE='" + strNatureType + "' AND CMP_BRANCH_ID in('" + bID + "')";


        try
        {
            con = new SqlConnection(cn);
            con.Open();
            DataSet oDS = new DataSet();
            SqlDataAdapter odaData = new SqlDataAdapter(new SqlCommand(strQuery, con));
            odaData.Fill(oDS, "PR_LEAVE_TYPE");

            if (oDS.Tables["PR_LEAVE_TYPE"].Rows.Count > 0)
            {
                massage = "Data Exist";
            }
            else
            {
                massage = "";
            }

            return massage;
        }
        catch (Exception ex)
        {
            return null;
        }
        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }
    }

    ///<summary>
    /// Author:Tanjil Alam
    /// Date: 27/01/2014
    /// 
    /// </summary>
    /// 
    /// <returns></returns>
    /// 

    public DataSet GetYearStatus(string yId)
    {
        string strTA = "select YR_STATUS from HR_YEAR where YR_ID='" + yId + "'";

        try
        {
            con = new SqlConnection(cn);
            con.Open();
            DataSet oDS = new DataSet();
            SqlDataAdapter odaData = new SqlDataAdapter(new SqlCommand(strTA, con));
            odaData.Fill(oDS, "YEAR_STATUS");
            return oDS;
        }
        catch (Exception ex)
        {
            return null;
        }

        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }
    }
    ///<summary>
    /// Author:Tanjil Alam
    /// Date: 27/01/2014
    /// 
    /// </summary>
    /// 
    /// <returns></returns>
    /// 

    //public DataSet GetEmpAssingnInfo(string bId, string eId)
    //    {
    //        string strQuery = "SELECT EL.EMP_ID, (EMP_TITLE || ' ' || EMP_NAME) ENAME, CB.CMP_BRANCH_NAME, ASSIGN_PERSON, ITEM_SET_CODE, "
    //                        + "  EL.EMP_CODE, SET_CODE || ' - ' || DSG_TITLE AS DNAME, DP.DPT_NAME, EL.EMP_EMAIL, EL.EMP_REPORTING_ID  "
    //                        + "   FROM PR_EMPLOYEE_LIST EL, CM_CMP_BRANCH CB, PR_DESIGNATION DE, PR_DEPARTMENT DP , CM_WORKFLOW_SETUP EA "
    //                        + " WHERE EL.CMP_BRANCH_ID = CB.CMP_BRANCH_ID AND EL.DSG_ID=DE.DSG_ID AND EL.DPT_ID=DP.DPT_ID AND EA.DESTINATION_EMP_ID=EL.EMP_ID "
    //                        + " AND CB.CMP_BRANCH_ID ='" + bId + "'  AND EL.EMP_ID ='" + eId + "' AND EA.MAIN_TYPE_ID='01' ORDER BY ITEM_SET_CODE ASC";
    //        con = new SqlConnection(cn);
    //        try
    //        {

    //            DataSet oDS = new DataSet();
    //            SqlDataAdapter odaData = new SqlDataAdapter(new SqlCommand(strQuery, con));
    //            odaData.Fill(oDS, "EMPLOYEE_DETAIL_INFO");
    //            return oDS;
    //        }
    //        catch (Exception ex)
    //        {
    //            return null;
    //        }

    //        finally
    //        {
    //            con.Close();
    //            con = null;
    //        }
    //    }
    public DataSet GetConfirDate(string eId, string bId) // whether alrady defined or not
    {
        string strQuery = "select ET.TYP_TYPE, EMP_FINAL_CONFIR_DATE,EMP_STATUS,DSG_ID from PR_EMPLOYEE_LIST EL, HR_EMP_TYPE ET where EL.EMP_STATUS=ET.TYP_CODE AND EMP_ID='" + eId + "' AND CMP_BRANCH_ID='" + bId + "'";
        var con = new SqlConnection(cn);
        try
        {

            con.Open();
            DataSet oDS = new DataSet();
            SqlDataAdapter odaData = new SqlDataAdapter(new SqlCommand(strQuery, con));
            odaData.Fill(oDS, "EMP_STATUS");
            return oDS;
        }
        catch (Exception ex)
        {
            return null;
        }
        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }
    }
    ///<summary>
    /// Author:Tanjil Alam
    /// Date: 27/01/2014
    /// 
    /// </summary>
    /// 
    /// <returns></returns>
    /// 


    public DataSet GetSpecialLeaves(string empType, string desigID, string bId)
    {


        string strQuery = " SELECT DISTINCT LP.PRLT_ID,LT.PRLT_TITLE,LP.PRLP_ALLOWANCE,LP.PRLP_FREQUANCY_LEAVE,LP.PRLP_VALID_DAY "
                       + " from PR_LEAVE_TYPE LT, PR_LEAVE_POLICY LP, HR_EMP_TYPE ET "
                       + " where ET.TYP_CODE=LP.TYP_CODE AND LT.PRLT_ID=LP.PRLT_ID  AND LP.NATURE_TYPE='SL' AND LP.CMP_BRANCH_ID='" + bId + "' AND DSG_ID='" + desigID + "' AND ET.TYP_CODE='" + empType + "'";




        try
        {
            con = new SqlConnection(cn);
            con.Open();
            DataSet oDS = new DataSet();
            SqlDataAdapter odaData = new SqlDataAdapter(new SqlCommand(strQuery, con));
            odaData.Fill(oDS, "SPECIAL_LEAVE");
            return oDS;
        }
        catch (Exception ex)
        {
            return null;
        }
        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }
    }
    public DataSet GetSpecialLeavesEmp(string empType, string desigID, string bId, string empId)
    {


        string strQuery = " SELECT DISTINCT LP.PRLT_ID,LT.PRLT_TITLE,LP.PRLP_ALLOWANCE,LP.PRLP_FREQUANCY_LEAVE,LP.PRLP_VALID_DAY "
                       + " from PR_LEAVE_TYPE LT, PR_LEAVE_POLICY LP, HR_EMP_TYPE ET "
                       + " where ET.TYP_CODE=LP.TYP_CODE AND LT.PRLT_ID=LP.PRLT_ID  AND LP.NATURE_TYPE='SL' "
                       + " and LP.EMP_ID='" + empId + "'  AND LP.CMP_BRANCH_ID='" + bId + "' AND DSG_ID='" + desigID + "' AND ET.TYP_CODE='" + empType + "'";




        try
        {
            con = new SqlConnection(cn);
            con.Open();
            DataSet oDS = new DataSet();
            SqlDataAdapter odaData = new SqlDataAdapter(new SqlCommand(strQuery, con));
            odaData.Fill(oDS, "SPECIAL_LEAVE");
            return oDS;
        }
        catch (Exception ex)
        {
            return null;
        }
        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }
    }

    public DataSet GetMonthWiseSalary(string empCode, string month, string yr)  /*  Md. Asaduzzaman  */
    {
        string strQuery = " SELECT distinct  EMP_CODE,upper(ltrim(rtrim([Month])))mn,ltrim(rtrim([Year]))yr, isnull([Basic],0) Basic, isnull([Gross Salary],0) Gross, isnull([Com PF Con],0) PF,isnull([Com Tax],0) Tax, isnull([Net Salary/ Bank Transfer],0)NetSalary,  "
                        + " (convert(float,ltrim(rtrim(isnull([Gross Salary],0))))-  "
                        + " (convert(float,ltrim(rtrim(isnull([Com PF Con],0))))+ convert(float,ltrim(rtrim(isnull([Com Tax],0))))))Actual "
                        + " FROM  PR_SalaryInformationSheet  where upper(ltrim(rtrim([Month])))= upper(ltrim(rtrim('" + month + "')))  and EMP_CODE ='" + empCode + "' and ltrim(rtrim([Year]))= ltrim(rtrim('" + yr + "'))  ";
        try
        {
            con = new SqlConnection(cn);
            con.Open();
            DataSet oDS = new DataSet();
            SqlDataAdapter odaData = new SqlDataAdapter(new SqlCommand(strQuery, con));
            odaData.Fill(oDS, "Salary");
            return oDS;
        }
        catch (Exception ex)
        {
            return null;
        }
        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }
    }

    public DataSet GetCurrentSalaryMonthYr(string empCode)  /*  Md. Asaduzzaman  */
    {
        string strQuery = " SELECT Max(datepart(MM,[Month] + ' 01 ' + [Year]))MN ,[Year]YR "
                            + " from PR_SalaryInformationSheet  "
                            + " where [Year]= (SELECT max([Year]) from PR_SalaryInformationSheet WHERE EMP_CODE='" + empCode + "') AND  EMP_CODE='" + empCode + "' "
                            + " group by [Year] ";
        try
        {
            con = new SqlConnection(cn);
            con.Open();
            DataSet oDS = new DataSet();
            SqlDataAdapter odaData = new SqlDataAdapter(new SqlCommand(strQuery, con));
            odaData.Fill(oDS, "SalaryMonthYr");
            return oDS;
        }
        catch (Exception ex)
        {
            return null;
        }
        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }
    }


    ///<summary>
    /// Author:Tanjil Alam
    /// Date: 27/01/2014
    /// 
    /// </summary>
    /// 
    /// <returns></returns>
    /// 
    public DataSet GetSpecialLeaves(string strLeaveType, string empType, string desigID, string bId)
    {
        string strQuery = "select LP.PRLT_ID,LT.PRLT_TITLE,LP.PRLP_ALLOWANCE,LP.PRLP_FREQUANCY_LEAVE,LP.PRLP_VALID_DAY "
                        + "from PR_LEAVE_TYPE LT, PR_LEAVE_POLICY LP "
                        + "where LT.PRLT_ID=LP.PRLT_ID AND LP.PRLT_ID='" + strLeaveType + "' AND LP.NATURE_TYPE='SL' AND LP.CMP_BRANCH_ID='" + bId + "' AND DSG_ID='" + desigID + "' AND TYP_CODE='" + empType + "'";

        try
        {
            con = new SqlConnection(cn);
            con.Open();
            DataSet oDS = new DataSet();
            SqlDataAdapter odaData = new SqlDataAdapter(new SqlCommand(strQuery, con));
            odaData.Fill(oDS, "SPECIAL_LEAVE");
            return oDS;
        }
        catch (Exception ex)
        {
            return null;
        }
        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }
    }



    ///<summary>
    /// Author:Tanjil Alam
    /// Date: 27/01/2014
    /// 
    /// </summary>
    /// 
    /// <returns></returns>
    /// 

    public DataSet GetLeaveNature(string strLeaveType)
    {
        string strQuery = "select NATURE_TYPE from PR_LEAVE_TYPE where PRLT_ID='" + strLeaveType + "'";

        try
        {
            con = new SqlConnection(cn);
            con.Open();
            DataSet oDS = new DataSet();
            SqlDataAdapter odaData = new SqlDataAdapter(new SqlCommand(strQuery, con));
            odaData.Fill(oDS, "SPECIAL_LEAVE");
            return oDS;
        }
        catch (Exception ex)
        {
            return null;
        }
        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }
    }

    public DataSet GetLeaveInfo(string LvId)
    {
        string strQuery = "SELECT * from PR_LEAVE where LVE_ID='" + LvId + "'";

        try
        {
            con = new SqlConnection(cn);
            con.Open();
            DataSet oDS = new DataSet();
            SqlDataAdapter odaData = new SqlDataAdapter(new SqlCommand(strQuery, con));
            odaData.Fill(oDS, "PR_LEAVE");
            return oDS;
        }
        catch (Exception ex)
        {
            return null;
        }
        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }
    }
    ///<summary>
    /// Author:Tanjil Alam
    /// Date: 27/01/2014
    /// 
    /// </summary>
    /// 
    /// <returns></returns>
    /// 
    public DataSet GetTotSpLeaveApproved(string bId, string eId, string ltypeId, string totCycle)
    {

        string strQuery = "SELECT ISNULL(SUM(CONVERT(FLOAT, LVE_APPROVED_DAY)), 0) LEAVE_APPROVED "
                      + "FROM PR_LEAVE PL "
                      + "WHERE PL.CMP_BRANCH_ID ='" + bId + "' AND PL.EMP_ID ='" + eId + "' "
                      + "AND PL.PRLT_ID='" + ltypeId + "' AND LVE_STATUS='R' "
                      + "AND CYCEL_NO='" + totCycle + "'";


        try
        {
            con = new SqlConnection(cn);
            con.Open();
            DataSet oDS = new DataSet();
            SqlDataAdapter odaData = new SqlDataAdapter(new SqlCommand(strQuery, con));
            odaData.Fill(oDS, "TOT_SP_APPROVED_LEAVE");
            return oDS;
        }
        catch (Exception ex)
        {
            return null;

        }
        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }
    }
    #region Leave_ClaimRecommendationSetup

    public string LCInsertProductCategory(string ItemCode,
                                string ItemName,
                                string ItemType,
                                string ItemCategory,
                                string ItemDesc,
                                string cmpBranch
                                )
    {

        var con = new SqlConnection(cn);
        try
        {
            string strSql = "SELECT WM_ITEM_NAME, WM_ITEM_PARENT_CODE, WM_ITEM_SET_CODE, "
                                + "WM_ITEM_DESCRIPTION, WM_ITEM_TYPE, CMP_BRANCH_ID FROM CM_WORKFLOW_MANAGE_TYPE";

            DataRow oOrderRow;

            con.Open();
            dbTransaction = con.BeginTransaction();
            DataSet oDs = new DataSet();
            SqlDataAdapter oDbAdapter = new SqlDataAdapter(new SqlCommand(strSql, con, dbTransaction));
            SqlCommandBuilder oDbCmdBuilder = new SqlCommandBuilder(oDbAdapter);
            //oDbAdapter.MissingSchemaAction = MissingSchemaAction.AddWithKey;
            oDbAdapter.FillSchema(oDs, SchemaType.Source, "CM_WORKFLOW_MANAGE_TYPE");

            #region Insert Data
            oOrderRow = oDs.Tables["CM_WORKFLOW_MANAGE_TYPE"].NewRow();

            //7 fields
            oOrderRow["WM_ITEM_SET_CODE"] = ItemCode;
            oOrderRow["WM_ITEM_NAME"] = ItemName;
            oOrderRow["WM_ITEM_TYPE"] = ItemType;
            oOrderRow["WM_ITEM_PARENT_CODE"] = ItemCategory;

            oOrderRow["WM_ITEM_DESCRIPTION"] = ItemDesc;
            oOrderRow["CMP_BRANCH_ID"] = cmpBranch;
            //oOrderRow["LCS_TYPE_ID"] = AssessType;

            oDs.Tables["CM_WORKFLOW_MANAGE_TYPE"].Rows.Add(oOrderRow);
            oDbAdapter.Update(oDs, "CM_WORKFLOW_MANAGE_TYPE");
            dbTransaction.Commit();
            //con.Close();
            #endregion
        }
        catch (Exception ex)
        {

            return "Err:" + ex.Message.ToString();
        }

        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }

        return "Success";
    }

    public DataSet LCGetCategoryItem(string strType,
                                          string strBranchID,
                                          string strParentCode)
    {
        string strQuery = "SELECT CMP_BRANCH_ID, WM_ITEM_PARENT_CODE, WM_ITEM_TYPE, WM_ITEM_ID, WM_ITEM_SET_CODE, "
                        + "CASE WHEN WM_ITEM_SET_CODE IS NULL THEN WM_ITEM_NAME ELSE '  ' + CAST(WM_ITEM_SET_CODE AS VARCHAR) + ' - ' + CAST(WM_ITEM_NAME AS VARCHAR) END PNAME"
                        + " FROM CM_WORKFLOW_MANAGE_TYPE "
                        + "WHERE WM_ITEM_TYPE='" + strType + "' AND WM_ITEM_PARENT_CODE='" + strParentCode + "' "
                        + "AND CMP_BRANCH_ID = '" + strBranchID + "'  "
                        + "ORDER BY CMP_BRANCH_ID, WM_ITEM_SET_CODE";

        var con = new SqlConnection(cn);

        try
        {

            DataSet oDS = new DataSet();
            SqlDataAdapter odaData = new SqlDataAdapter(new SqlCommand(strQuery, con));
            odaData.Fill(oDS, "PARENT_LIST");
            return oDS;
        }
        catch (Exception ex)
        {
            return null;
        }
        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }
    }

    public DataSet LCGetChildCategoryItem(string strType,
                                            string strBranchID,
                                            string strParentCode)
    {
        string strQuery = "SELECT CMP_BRANCH_ID, WM_ITEM_PARENT_CODE, WM_ITEM_TYPE, WM_ITEM_ID, WM_ITEM_SET_CODE, "
                        + "CASE WHEN WM_ITEM_SET_CODE IS NULL THEN WM_ITEM_NAME ELSE '  ' + CAST(WM_ITEM_SET_CODE AS VARCHAR) + ' - ' + CAST(WM_ITEM_NAME AS VARCHAR) END PNAME "
                        + "FROM CM_WORKFLOW_MANAGE_TYPE "
                        + "WHERE WM_ITEM_TYPE='" + strType + "' AND WM_ITEM_PARENT_CODE='" + strParentCode + "' "
                        + "AND CMP_BRANCH_ID = '" + strBranchID + "'  "
                        + "ORDER BY CMP_BRANCH_ID, WM_ITEM_SET_CODE";


        var con = new SqlConnection(cn);

        try
        {

            DataSet oDS = new DataSet();
            SqlDataAdapter odaData = new SqlDataAdapter(new SqlCommand(strQuery, con));
            odaData.Fill(oDS, "CHILD_LIST");
            return oDS;
        }
        catch (Exception ex)
        {
            return null;
        }
        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }
    }
    public string InsertRecommendLC(string branch, string appType, string SettleType, string EmpId, string DegId, string Details)
    {
        string strSql;
        strSql = "SELECT CMP_BRANCH_ID, LC_APP_TYPE, LC_SETTLED_TYPE, LC_SET_BY_EMPID, LC_SET_BY_DSG_ID,LC_DESCRIPTION  FROM EMP_LEAVE_CLAIM_SETTLED_BY ";

        var con = new SqlConnection(cn);
        try
        {
            DataRow oOrderRow;
            con.Open();
            dbTransaction = con.BeginTransaction();
            DataSet oDS = new DataSet();
            SqlDataAdapter oOrdersDataAdapter = new SqlDataAdapter(new SqlCommand(strSql, con, dbTransaction));
            SqlCommandBuilder oOrdersCmdBuilder = new SqlCommandBuilder(oOrdersDataAdapter);
            oOrdersDataAdapter.FillSchema(oDS, SchemaType.Source);
            //######################################################
            DataTable dtSchedule = oDS.Tables["Table"];
            dtSchedule.TableName = "EMP_LEAVE_CLAIM_SETTLED_BY";
            //######################################################
            oOrderRow = oDS.Tables["EMP_LEAVE_CLAIM_SETTLED_BY"].NewRow();
            oOrderRow["CMP_BRANCH_ID"] = branch;
            oOrderRow["LC_APP_TYPE"] = appType;
            oOrderRow["LC_SETTLED_TYPE"] = SettleType;
            oOrderRow["LC_SET_BY_EMPID"] = EmpId;
            oOrderRow["LC_SET_BY_DSG_ID"] = DegId;
            oOrderRow["LC_DESCRIPTION"] = Details;
            //#########################################################
            oDS.Tables["EMP_LEAVE_CLAIM_SETTLED_BY"].Rows.Add(oOrderRow);
            oOrdersDataAdapter.Update(oDS, "EMP_LEAVE_CLAIM_SETTLED_BY");
            dbTransaction.Commit();
            return "Saved Successfully.";
        }
        catch (Exception ex)
        {
            return ex.Message.ToString();
        }
        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }
    }
    #region OSD
    /// <summary>
    /// #################################################################### OSD #########################################################
    /// ASAD Dated:17-09-2014
    /// </summary>

    public bool chkOSDattendance(string empId, DateTime date) // get employee id from user id & branch id
    {
        string strQuery = " select COUNT(*)present from PR_EMP_ATTENDENCE at where at.EMP_ID='" + empId + "' and at.ATT_TYPE='OSD' "
                         + " and convert(date, CONVERT(DATETIME, CONVERT(VARCHAR(23), at.ATT_DATE_TIME)))= convert(date, CONVERT(DATETIME, CONVERT(VARCHAR(23), '" + date + "'))) ";
        bool exist = false;
        string Present = "";
        try
        {
            con = new SqlConnection(cn);
            con.Open();
            DataSet oDS = new DataSet();
            SqlDataAdapter odaData = new SqlDataAdapter(new SqlCommand(strQuery, con));
            odaData.Fill(oDS, "PR_EMP_ATTENDENCE");
            if (oDS.Tables[0].Rows.Count > 0)
            {
                Present = oDS.Tables[0].Rows[0]["present"].ToString();
                if (Present != "0")
                {
                    exist = true;
                }
            }
        }
        catch (Exception ex)
        {
            return false;
        }
        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }
        return exist;
    }

    public string InsertAttendanceOSD(string branch, DateTime atDate, string EmpId, string OsDIn, string OsdOut, string comments)
    {
        string strSql;
        strSql = "SELECT AT.ATT_DATE_TIME,AT.ACCESS_METHOD,AT.APPLY_LATE_DEDUCT,AT.ATT_LATE_STATUS, "
                + " AT.ATT_TYPE,AT.CMP_BRANCH_ID,AT.EMP_ID,AT.OSD_TIME_IN,AT.OSD_TIME_OUT,AT.ATT_COMMENTS FROM PR_EMP_ATTENDENCE AT ";

        var con = new SqlConnection(cn);
        try
        {
            DataRow oOrderRow;
            con.Open();
            dbTransaction = con.BeginTransaction();
            DataSet oDS = new DataSet();
            SqlDataAdapter oOrdersDataAdapter = new SqlDataAdapter(new SqlCommand(strSql, con, dbTransaction));
            SqlCommandBuilder oOrdersCmdBuilder = new SqlCommandBuilder(oOrdersDataAdapter);
            oOrdersDataAdapter.FillSchema(oDS, SchemaType.Source);
            //######################################################
            DataTable dtSchedule = oDS.Tables["Table"];
            dtSchedule.TableName = "PR_EMP_ATTENDENCE";
            //######################################################
            oOrderRow = oDS.Tables["PR_EMP_ATTENDENCE"].NewRow();
            oOrderRow["ATT_DATE_TIME"] = atDate;
            oOrderRow["ACCESS_METHOD"] = "FD";
            oOrderRow["APPLY_LATE_DEDUCT"] = "N";
            oOrderRow["ATT_LATE_STATUS"] = "NO";
            oOrderRow["ATT_TYPE"] = "OSD";
            oOrderRow["CMP_BRANCH_ID"] = branch;
            oOrderRow["EMP_ID"] = EmpId;
            oOrderRow["OSD_TIME_IN"] = OsDIn;
            oOrderRow["OSD_TIME_OUT"] = OsdOut;
            oOrderRow["ATT_COMMENTS"] = comments;
            //#########################################################
            oDS.Tables["PR_EMP_ATTENDENCE"].Rows.Add(oOrderRow);
            oOrdersDataAdapter.Update(oDS, "PR_EMP_ATTENDENCE");
            dbTransaction.Commit();
            return "Saved Successfully.";
        }
        catch (Exception ex)
        {
            return ex.Message.ToString();
        }
        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }
    }
    public string DeleteAttendanceOSD(string attId)
    {
        string updateString;
        updateString = "Delete PR_EMP_ATTENDENCE  WHERE ATT_ID = '" + attId + "'";
        string strReturn = "";
        try
        {
            con = new SqlConnection(cn);
            con.Open();
            dbTransaction = con.BeginTransaction();
            SqlCommand cmd = new SqlCommand(updateString, con, dbTransaction);
            cmd.Connection = con;
            cmd.ExecuteNonQuery();
            dbTransaction.Commit();
            //con.Close();
        }
        catch (Exception ex)
        {
            strReturn = ex.Message.ToString();
        }
        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }
        return strReturn;
    }

    public string UpdateAttendanceOSD(string attId, string status, string comments, string Approvedby)
    {
        string updateString;
        updateString = "UPDATE PR_EMP_ATTENDENCE SET APPROVAL_STATUS='" + status + "', APPROVAL_COMMENT='" + comments + "', OSD_Approvedby_Initial='" + Approvedby + "'   WHERE ATT_ID = '" + attId + "' ";
        string strReturn = "";
        try
        {
            con = new SqlConnection(cn);
            con.Open();
            dbTransaction = con.BeginTransaction();
            SqlCommand cmd = new SqlCommand(updateString, con, dbTransaction);
            cmd.Connection = con;
            cmd.ExecuteNonQuery();
            dbTransaction.Commit();
        }
        catch (Exception ex)
        {
            strReturn = "";
        }
        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }
        strReturn = "Updated successfully";
        return strReturn;
    }
    public string UpdateAttendanceOSDHR(string attId, string status, string comments, string Approvedby)
    {
        string updateString;
        updateString = "UPDATE PR_EMP_ATTENDENCE SET APPROVAL_STATUS='" + status + "', APPROVAL_COMMENT='" + comments + "', OSD_Approvedby_HR='" + Approvedby + "'   WHERE ATT_ID = '" + attId + "' ";
        string strReturn = "";
        try
        {
            con = new SqlConnection(cn);
            con.Open();
            dbTransaction = con.BeginTransaction();
            SqlCommand cmd = new SqlCommand(updateString, con, dbTransaction);
            cmd.Connection = con;
            cmd.ExecuteNonQuery();
            dbTransaction.Commit();
        }
        catch (Exception ex)
        {
            strReturn = "";
        }
        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }
        strReturn = "Updated successfully";
        return strReturn;
    }
    #endregion

    #endregion

    public string DeleteAppliedLV(string LVid)
    {
        string updateString;
        updateString = "Delete PR_LEAVE  WHERE LVE_ID = '" + LVid + "'";
        string strReturn = "";
        try
        {
            con = new SqlConnection(cn);
            con.Open();
            dbTransaction = con.BeginTransaction();
            SqlCommand cmd = new SqlCommand(updateString, con, dbTransaction);
            cmd.Connection = con;
            cmd.ExecuteNonQuery();
            dbTransaction.Commit();
            //con.Close();
        }
        catch (Exception ex)
        {
            strReturn = ex.Message.ToString();
        }
        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }
        return strReturn;
    }

    public string DeleteSalarySheet(string updateString)
    {
        //string updateString;
        //updateString = "Delete PR_LEAVE  WHERE LVE_ID = '" + LVid + "'";
        string strReturn = "Deleted successfully";
        try
        {
            con = new SqlConnection(cn);
            con.Open();
            dbTransaction = con.BeginTransaction();
            SqlCommand cmd = new SqlCommand(updateString, con, dbTransaction);
            cmd.Connection = con;
            cmd.ExecuteNonQuery();
            dbTransaction.Commit();
            //con.Close();
        }
        catch (Exception ex)
        {
            strReturn = ex.Message.ToString();
        }
        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }
        return strReturn;
    }



    public string DeleteCertificate(string CFTid)
    {
        string updateString;
        updateString = " Delete HR_EMP_JOB_CERTIFICATE  WHERE CFT_ID = '" + CFTid + "'";
        string strReturn = "";
        try
        {
            con = new SqlConnection(cn);
            con.Open();
            dbTransaction = con.BeginTransaction();
            SqlCommand cmd = new SqlCommand(updateString, con, dbTransaction);
            cmd.Connection = con;
            cmd.ExecuteNonQuery();
            dbTransaction.Commit();
            //con.Close();
        }
        catch (Exception ex)
        {
            strReturn = ex.Message.ToString();
        }
        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }
        return strReturn;
    }

    ///<summary>
    /// Author:Niaz Morshed
    /// Date: 23/02/2014
    /// 
    /// </summary>
    /// 
    /// <returns></returns>
    /// 
    public string AddNewEmployee(string bId, string tId, string empCode, string UserId, string title,
                                  string EmpName, string gender, string birth, string department, string designation,
                                  string joiningDate, string PrvtionPeriod,
                                  string EmpConDate, string bankAccNo, string comments)
    {
        con = new SqlConnection(cn);
        string strSql = "";
        strSql = "SELECT CMP_BRANCH_ID, EMP_STATUS, EMP_CODE, SYS_USR_ID, EMP_TITLE, EMP_NAME, "
               + "EMP_GENDER, EMP_BIRTHDAY, DPT_ID, DSG_ID, EMP_JOINING_DATE, EMP_PROVISION_PERIOD, "
               + "EMP_CONFIR_DATE, EMP_BANK_ACC_NO, EMP_COMMENTS FROM PR_EMPLOYEE_LIST ";
        try
        {
            DataRow oOrderRow;
            con.Open();
            dbTransaction = con.BeginTransaction();
            DataSet oDS = new DataSet();
            SqlDataAdapter oOrdersDataAdapter = new SqlDataAdapter(new SqlCommand(strSql, con, dbTransaction));
            SqlCommandBuilder oOrdersCmdBuilder = new SqlCommandBuilder(oOrdersDataAdapter);
            oOrdersDataAdapter.FillSchema(oDS, SchemaType.Source);

            DataTable dtAddSchedule = oDS.Tables["Table"];
            dtAddSchedule.TableName = "PR_EMPLOYEE_LIST";

            oOrderRow = oDS.Tables["PR_EMPLOYEE_LIST"].NewRow();

            oOrderRow["CMP_BRANCH_ID"] = bId;
            oOrderRow["EMP_STATUS"] = tId;
            oOrderRow["EMP_CODE"] = empCode;
            oOrderRow["SYS_USR_ID"] = UserId;
            oOrderRow["EMP_TITLE"] = title;
            oOrderRow["EMP_NAME"] = EmpName;
            oOrderRow["EMP_GENDER"] = gender;
            oOrderRow["EMP_BIRTHDAY"] = birth;
            oOrderRow["DPT_ID"] = department;
            oOrderRow["DSG_ID"] = designation;
            oOrderRow["EMP_JOINING_DATE"] = joiningDate;
            oOrderRow["EMP_PROVISION_PERIOD"] = Convert.ToDouble(PrvtionPeriod);
            if (EmpConDate.ToString() != string.Empty)
            {
                oOrderRow["EMP_CONFIR_DATE"] = Convert.ToDateTime(EmpConDate);
            }
            oOrderRow["EMP_BANK_ACC_NO"] = bankAccNo;
            oOrderRow["EMP_COMMENTS"] = comments;

            oDS.Tables["PR_EMPLOYEE_LIST"].Rows.Add(oOrderRow);
            oOrdersDataAdapter.Update(oDS, "PR_EMPLOYEE_LIST");
            dbTransaction.Commit();
            return "Success";
        }
        catch (Exception ex)
        {
            return ex.Message.ToString();
        }
        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }
    }
    public string AddNewEmployee(string bId, string tId, string empCode, string UserId, string title,
                                  string EmpName, string empNameBangla, string gender, string birth, string department, string designation, string MainDesignation,
                                  string joiningDate, string PrvtionPeriod,
                                  string EmpConDate, string bankAccNo, string comments)
    {
        con = new SqlConnection(cn);
        string strSql = "";
        strSql = "SELECT EMP_NAME_BANGLA, FACEBOOK_ID,LINKEDIN_ID, REFERENCE,BATCH_NUMBER, CMP_BRANCH_ID, EMP_STATUS, EMP_CODE, SYS_USR_ID, EMP_TITLE, EMP_NAME, "
               + "EMP_GENDER, EMP_BIRTHDAY, DPT_ID, DSG_ID,DSG_ID_MAIN, EMP_JOINING_DATE, EMP_PROVISION_PERIOD, "
               + "EMP_CONFIR_DATE, EMP_BANK_ACC_NO, EMP_COMMENTS FROM PR_EMPLOYEE_LIST ";
        try
        {
            DataRow oOrderRow;
            con.Open();
            dbTransaction = con.BeginTransaction();
            DataSet oDS = new DataSet();
            SqlDataAdapter oOrdersDataAdapter = new SqlDataAdapter(new SqlCommand(strSql, con, dbTransaction));
            SqlCommandBuilder oOrdersCmdBuilder = new SqlCommandBuilder(oOrdersDataAdapter);
            oOrdersDataAdapter.FillSchema(oDS, SchemaType.Source);

            DataTable dtAddSchedule = oDS.Tables["Table"];
            dtAddSchedule.TableName = "PR_EMPLOYEE_LIST";

            oOrderRow = oDS.Tables["PR_EMPLOYEE_LIST"].NewRow();

            oOrderRow["CMP_BRANCH_ID"] = bId;
            oOrderRow["EMP_STATUS"] = tId;
            oOrderRow["EMP_CODE"] = empCode;
            oOrderRow["SYS_USR_ID"] = UserId;
            oOrderRow["EMP_TITLE"] = title;
            oOrderRow["EMP_NAME"] = EmpName;
            oOrderRow["EMP_GENDER"] = gender;
            oOrderRow["EMP_BIRTHDAY"] = birth;
            oOrderRow["DPT_ID"] = department;
            oOrderRow["DSG_ID"] = designation;
            oOrderRow["DSG_ID_MAIN"] = MainDesignation;
            oOrderRow["EMP_JOINING_DATE"] = joiningDate;
            oOrderRow["EMP_NAME_BANGLA"] = empNameBangla;

            oOrderRow["REFERENCE"] = "";
            oOrderRow["BATCH_NUMBER"] = "";

            oOrderRow["FACEBOOK_ID"] = "";
            oOrderRow["LINKEDIN_ID"] = "";

            oOrderRow["EMP_PROVISION_PERIOD"] = Convert.ToDouble(PrvtionPeriod);
            if (EmpConDate.ToString() != string.Empty)
            {
                oOrderRow["EMP_CONFIR_DATE"] = Convert.ToDateTime(EmpConDate);
            }
            oOrderRow["EMP_BANK_ACC_NO"] = bankAccNo;
            oOrderRow["EMP_COMMENTS"] = comments;

            oDS.Tables["PR_EMPLOYEE_LIST"].Rows.Add(oOrderRow);
            oOrdersDataAdapter.Update(oDS, "PR_EMPLOYEE_LIST");
            dbTransaction.Commit();
            return "Success";
        }
        catch (Exception ex)
        {
            return ex.Message.ToString();
        }
        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }
    }

    public string InsertEmpRegister(string bId, string tId, string empCode, string UserId, string title,
                                      string EmpName, string FatherName, string motherName, string gender, string birth,
                                      string nationality, string religion, string bloodGroup, string department, string designation, string repotingId,
                                      string joiningDate, string meritalStatus, string spouseName, string empQty, string PrvtionPeriod,
                                      string EmpConDate, string bankAccNo, string EmpIndividual, string comments)
    {
        con = new SqlConnection(cn);
        string strSql = "";
        strSql = "SELECT CMP_BRANCH_ID, EMP_STATUS, EMP_CODE, SYS_USR_ID, EMP_TITLE, EMP_NAME, EMP_FATHER_NAME, EMP_MOTHER_NAME, "
               + "EMP_GENDER, EMP_BIRTHDAY, EMP_NATIONALITY, EMP_RELIGION, EMP_BLOODGRP, DPT_ID, DSG_ID, EMP_REPORTING_ID, EMP_JOINING_DATE, "
               + "EMP_MARITAL_STATAS, EMP_SPOUSE_NAME, EMP_QUANTITY, EMP_PROVISION_PERIOD, EMP_CONFIR_DATE, EMP_BANK_ACC_NO, "
               + "EMP_INDIVIDUAL, EMP_COMMENTS FROM PR_EMPLOYEE_LIST ";
        try
        {
            DataRow oOrderRow;
            con.Open();
            dbTransaction = con.BeginTransaction();
            DataSet oDS = new DataSet();
            SqlDataAdapter oOrdersDataAdapter = new SqlDataAdapter(new SqlCommand(strSql, con, dbTransaction));
            SqlCommandBuilder oOrdersCmdBuilder = new SqlCommandBuilder(oOrdersDataAdapter);
            oOrdersDataAdapter.FillSchema(oDS, SchemaType.Source);

            DataTable dtAddSchedule = oDS.Tables["Table"];
            dtAddSchedule.TableName = "PR_EMPLOYEE_LIST";

            oOrderRow = oDS.Tables["PR_EMPLOYEE_LIST"].NewRow();

            oOrderRow["CMP_BRANCH_ID"] = bId;
            oOrderRow["EMP_STATUS"] = tId;
            oOrderRow["EMP_CODE"] = empCode;
            oOrderRow["SYS_USR_ID"] = UserId;
            oOrderRow["EMP_TITLE"] = title;
            oOrderRow["EMP_NAME"] = EmpName;
            oOrderRow["EMP_FATHER_NAME"] = FatherName;
            oOrderRow["EMP_MOTHER_NAME"] = motherName;
            oOrderRow["EMP_GENDER"] = gender;
            oOrderRow["EMP_BIRTHDAY"] = birth;


            oOrderRow["EMP_NATIONALITY"] = nationality;
            oOrderRow["EMP_RELIGION"] = religion;
            oOrderRow["EMP_BLOODGRP"] = bloodGroup;


            oOrderRow["DPT_ID"] = department;
            oOrderRow["DSG_ID"] = designation;
            oOrderRow["EMP_REPORTING_ID"] = repotingId;
            oOrderRow["EMP_JOINING_DATE"] = joiningDate;
            oOrderRow["EMP_MARITAL_STATAS"] = meritalStatus;
            oOrderRow["EMP_SPOUSE_NAME"] = spouseName;
            oOrderRow["EMP_QUANTITY"] = Convert.ToDouble(empQty);
            oOrderRow["EMP_PROVISION_PERIOD"] = Convert.ToDouble(PrvtionPeriod);
            if (EmpConDate.ToString() != string.Empty)
            {
                oOrderRow["EMP_CONFIR_DATE"] = Convert.ToDateTime(EmpConDate);
            }
            oOrderRow["EMP_BANK_ACC_NO"] = bankAccNo;
            oOrderRow["EMP_INDIVIDUAL"] = EmpIndividual;
            oOrderRow["EMP_COMMENTS"] = comments;

            oDS.Tables["PR_EMPLOYEE_LIST"].Rows.Add(oOrderRow);
            oOrdersDataAdapter.Update(oDS, "PR_EMPLOYEE_LIST");
            dbTransaction.Commit();
            return "Success";
        }
        catch (Exception ex)
        {
            return ex.Message.ToString();
        }
        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }
    }
    //saydur rahman
    //to add employee info
    public bool insertemployeeinfo(string empid, string fathersname, string mothersname, string nationality, string religion, string actualdate, string marital, string spouse, string blood, string CertificateBD)
    {
        bool flag = false;
        try
        {
            DataBaseClassSql db = new DataBaseClassSql();
            string qry = " UPDATE PR_EMPLOYEE_LIST SET EMP_FATHER_NAME='" + fathersname + "',EMP_MOTHER_NAME='" + mothersname + "', "
                        + " EMP_NATIONALITY='" + nationality + "',EMP_RELIGION='" + religion + "',ACTUAL_BIRTHDAY='" + actualdate + "', EMP_BIRTHDAY='" + CertificateBD + "', EMP_MARITAL_STATAS='" + marital + "', "
                        + " EMP_SPOUSE_NAME='" + spouse + "',EMP_BLOODGRP='" + blood + "' WHERE EMP_ID='" + empid + "' ";
            db.ConnectDataBaseToInsert(qry);
            flag = true;
        }
        catch
        {
            flag = false;
        }
        return flag;
    }
    public bool insertemployeeinfo(string empid, string fathersname, string mothersname, string nationality, string religion, string actualdate, string marital, string spouse, string blood, string CertificateBD, string marriageDt, string emailPersonal, string dobSpouse)
    {
        bool flag = false;
        try
        {
            DataBaseClassSql db = new DataBaseClassSql();
            string qry = " UPDATE PR_EMPLOYEE_LIST SET EMP_FATHER_NAME='" + fathersname + "',EMP_MOTHER_NAME='" + mothersname + "', "
                        + " EMP_NATIONALITY='" + nationality + "',EMP_RELIGION='" + religion + "',ACTUAL_BIRTHDAY='" + actualdate + "', EMP_BIRTHDAY='" + CertificateBD + "', EMP_MARITAL_STATAS='" + marital + "', "
                        + " EMP_SPOUSE_NAME='" + spouse + "',EMP_BLOODGRP='" + blood + "', MARRIAGE_DATE='" + marriageDt + "', EMP_EMAIL_PERSONAL='" + emailPersonal + "', EMP_SPOUSE_DOB='" + dobSpouse + "'  WHERE EMP_ID='" + empid + "' ";

            db.ConnectDataBaseToInsert(qry);
            flag = true;
        }
        catch
        {
            flag = false;
        }
        return flag;
    }
    public DataTable getemployeeinfo(string empid)
    {
        DataTable dt = new DataTable();
        DataBaseClassSql db = new DataBaseClassSql();
        string qry = "SELECT DPT_ID FROM PR_EMPLOYEE_LIST WHERE EMP_ID='" + empid + "'";
        dt = db.ConnectDataBaseReturnDT(qry);
        return dt;
    }
    //asad
    public int GetDeptName(string deptName)
    {
        string strQuery = "SELECT DPT_NAME FROM PR_DEPARTMENT "
                            + "WHERE UPPER(DPT_NAME) = UPPER('" + deptName + "') ";

        int getValue = 0;

        try
        {
            con = new SqlConnection(cn);
            con.Open();
            DataSet oDS = new DataSet();
            SqlDataAdapter odaData = new SqlDataAdapter(new SqlCommand(strQuery, con));
            odaData.Fill(oDS, "CM_CMP_BRANCH");
            if (oDS.Tables[0].Rows.Count > 0)
            {
                getValue = 1;
            }

        }
        catch (Exception ex)
        {

        }

        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }
        return getValue;
    }
    public DataSet GetOrganogramInfo(string SET_CODE, string CMP_BRANCH_ID)  /* ASAD DATED:04-APR-2014 */
    {
        string strQuery = "SELECT * FROM  PR_DESIGNATION  "
                          + " WHERE  SET_CODE='" + SET_CODE + "' AND CMP_BRANCH_ID='" + CMP_BRANCH_ID + "'  ";
        try
        {
            con = new SqlConnection(cn);
            con.Open();
            DataSet oDS = new DataSet();
            SqlDataAdapter odaData = new SqlDataAdapter(new SqlCommand(strQuery, con));
            odaData.Fill(oDS, "PR_DESIGNATION");
            return oDS;
        }
        catch (Exception ex)
        {
            return null;
        }

        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }
    }


    public int GetDayDifference(string FrmDt, string ToDt)  /* ASAD DATED:04-APR-2014 */
    {
        int days = 0;
        try
        {
            double datetimes = (DateTime.Parse(ToDt) - DateTime.Parse(FrmDt)).TotalDays;
            days = Convert.ToInt32(datetimes);
        }
        catch (Exception ex)
        {
            return 0;
        }

        finally
        {
            //con.Close();
            //con = null;
        }
        return days;
    }
    public DataSet GetEmployeeInfoDept(string bId, string dept)
    {
        //string strQuery = "select EMP_ID,EL.DSG_ID,EMP_NAME,EMP_STATUS,EL.CMP_BRANCH_ID,EMP_FINAL_CONFIR_DATE,DSG_TITLE,TYP_TYPE "
        //                + "from PR_EMPLOYEE_LIST EL, PR_DESIGNATION PD, HR_EMP_TYPE ET "
        //                + "WHERE EL.DSG_ID=PD.DSG_ID AND EL.EMP_STATUS=ET.TYP_CODE AND EL.CMP_BRANCH_ID='" + bId + "' AND EMP_STATUS='2' "
        //                + "AND EL.DPT_ID='" + dept + "' ";

        string strQuery = "select EMP_ID,EL.DSG_ID,EMP_NAME,EMP_STATUS,EL.CMP_BRANCH_ID,EMP_FINAL_CONFIR_DATE,DSG_TITLE,TYP_TYPE "
                       + "from PR_EMPLOYEE_LIST EL, PR_DESIGNATION PD, HR_EMP_TYPE ET "
                       + "WHERE EL.DSG_ID_MAIN=PD.DSG_ID AND EL.EMP_STATUS=ET.TYP_CODE AND EL.CMP_BRANCH_ID='" + bId + "' AND EMP_STATUS='2' "
                       + "AND EL.DPT_ID='" + dept + "' ";


        var con = new SqlConnection(cn);
        try
        {

            DataSet oDS = new DataSet();
            SqlDataAdapter odaData = new SqlDataAdapter(new SqlCommand(strQuery, con));
            odaData.Fill(oDS, "PR_EMPLOYEE_INFO");
            return oDS;
        }
        catch (Exception ex)
        {
            return null;
        }
        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }
    }
    public DataSet GetEmployeeInfoDeptSpecialLv(string bId, string dept)
    {
        //string strQuery = "select EMP_ID,EL.DSG_ID,EMP_NAME,EMP_STATUS,EL.CMP_BRANCH_ID,EMP_FINAL_CONFIR_DATE,DSG_TITLE,TYP_TYPE "
        //                + "from PR_EMPLOYEE_LIST EL, PR_DESIGNATION PD, HR_EMP_TYPE ET "
        //                + "WHERE EL.DSG_ID=PD.DSG_ID AND EL.EMP_STATUS=ET.TYP_CODE AND EL.CMP_BRANCH_ID='" + bId + "' AND EMP_STATUS='2' "
        //                + "AND EL.DPT_ID='" + dept + "' ";

        string strQuery = "SELECT EL.DPT_ID,(SELECT DPT_NAME FROM PR_DEPARTMENT WHERE DPT_ID=EL.DPT_ID)DPT_NAME, EL.EMP_ID,EL.DSG_ID,EMP_NAME,EMP_STATUS,EL.CMP_BRANCH_ID,EMP_FINAL_CONFIR_DATE,DSG_TITLE,TYP_TYPE "
                       + "from PR_EMPLOYEE_LIST EL, PR_DESIGNATION PD, HR_EMP_TYPE ET,PR_LEAVE_POLICY LP "
                       + "WHERE EL.EMP_ID=LP.EMP_ID AND EL.DSG_ID_MAIN=PD.DSG_ID AND EL.EMP_STATUS=ET.TYP_CODE AND EL.CMP_BRANCH_ID='" + bId + "' AND EMP_STATUS='2' "
                       + "AND EL.DPT_ID='" + dept + "' ";


        var con = new SqlConnection(cn);
        try
        {

            DataSet oDS = new DataSet();
            SqlDataAdapter odaData = new SqlDataAdapter(new SqlCommand(strQuery, con));
            odaData.Fill(oDS, "PR_EMPLOYEE_INFO");
            return oDS;
        }
        catch (Exception ex)
        {
            return null;
        }
        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }
    }
    public DataSet GetEmployeeInfoDept(string bId, string degID, string dept)
    {
        string strQuery = "select EMP_ID,EL.DSG_ID,EMP_NAME,EMP_STATUS,EL.CMP_BRANCH_ID,EMP_FINAL_CONFIR_DATE,DSG_TITLE,TYP_TYPE "
                        + "from PR_EMPLOYEE_LIST EL, PR_DESIGNATION PD, HR_EMP_TYPE ET "
                        + "WHERE EL.DSG_ID=PD.DSG_ID AND EL.EMP_STATUS=ET.TYP_CODE AND EL.CMP_BRANCH_ID='" + bId + "' "
                        + "AND EL.DSG_ID='" + degID + "' AND EMP_STATUS='2' AND EL.DPT_ID='" + dept + "'";

        var con = new SqlConnection(cn);
        try
        {

            DataSet oDS = new DataSet();
            SqlDataAdapter odaData = new SqlDataAdapter(new SqlCommand(strQuery, con));
            odaData.Fill(oDS, "PR_EMPLOYEE_INFO");
            return oDS;
        }
        catch (Exception ex)
        {
            return null;
        }
        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }
    }
    public DataSet GetEmployeeInfoDeptSpecialLV(string bId, string degID, string dept)
    {
        string strQuery = "select EL.EMP_ID,EL.DSG_ID,EMP_NAME,EMP_STATUS,EL.CMP_BRANCH_ID,EMP_FINAL_CONFIR_DATE,DSG_TITLE,TYP_TYPE "
                        + "from PR_EMPLOYEE_LIST EL, PR_DESIGNATION PD, HR_EMP_TYPE ET ,PR_LEAVE_POLICY LP"
                        + "WHERE LP.EMP_ID=EL.EMP_ID and EL.DSG_ID=PD.DSG_ID AND EL.EMP_STATUS=ET.TYP_CODE AND EL.CMP_BRANCH_ID='" + bId + "' "
                        + "AND EL.DSG_ID='" + degID + "' AND EMP_STATUS='2' AND EL.DPT_ID='" + dept + "'";

        var con = new SqlConnection(cn);
        try
        {

            DataSet oDS = new DataSet();
            SqlDataAdapter odaData = new SqlDataAdapter(new SqlCommand(strQuery, con));
            odaData.Fill(oDS, "PR_EMPLOYEE_INFO");
            return oDS;
        }
        catch (Exception ex)
        {
            return null;
        }
        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }
    }
    public DataSet GetEmployeeInfoDept(string bId, string degID, string eID, string dept)
    {
        string strQuery = "select EMP_ID,EL.DSG_ID,EMP_NAME,EMP_STATUS,EL.CMP_BRANCH_ID,EMP_FINAL_CONFIR_DATE,DSG_TITLE,TYP_TYPE "
                        + "from PR_EMPLOYEE_LIST EL, PR_DESIGNATION PD, HR_EMP_TYPE ET "
                        + "WHERE EL.DSG_ID=PD.DSG_ID AND EL.EMP_STATUS=ET.TYP_CODE AND EL.CMP_BRANCH_ID='" + bId + "' "
                        + "AND EL.DSG_ID='" + degID + "' AND EL.EMP_ID='" + eID + "' AND EMP_STATUS='2' AND EL.DPT_ID='" + dept + "'";

        var con = new SqlConnection(cn);
        try
        {

            DataSet oDS = new DataSet();
            SqlDataAdapter odaData = new SqlDataAdapter(new SqlCommand(strQuery, con));
            odaData.Fill(oDS, "PR_EMPLOYEE_INFO");
            return oDS;
        }
        catch (Exception ex)
        {
            return null;
        }
        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }
    }
    public DataSet GetEmployeeInfoDeptSpecialLV(string bId, string degID, string eID, string dept)
    {
        string strQuery = "select EL.EMP_ID,EL.DSG_ID,EMP_NAME,EMP_STATUS,EL.CMP_BRANCH_ID,EMP_FINAL_CONFIR_DATE,DSG_TITLE,TYP_TYPE "
                        + "from PR_EMPLOYEE_LIST EL, PR_DESIGNATION PD, HR_EMP_TYPE ET,PR_LEAVE_POLICY LP "
                        + "WHERE EL.DSG_ID=PD.DSG_ID AND EL.EMP_STATUS=ET.TYP_CODE AND EL.CMP_BRANCH_ID='" + bId + "' "
                        + "AND EL.DSG_ID='" + degID + "' AND EL.EMP_ID='" + eID + "' AND EMP_STATUS='2' AND EL.DPT_ID='" + dept + "' and LP.EMP_ID=EL.EMP_ID ";

        var con = new SqlConnection(cn);
        try
        {

            DataSet oDS = new DataSet();
            SqlDataAdapter odaData = new SqlDataAdapter(new SqlCommand(strQuery, con));
            odaData.Fill(oDS, "PR_EMPLOYEE_INFO");
            return oDS;
        }
        catch (Exception ex)
        {
            return null;
        }
        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }
    }
    public string getEmpDeptById(string Empid)
    {
        string deptid = "";
        try
        {
            string qry = "SELECT DPT_ID FROM PR_EMPLOYEE_LIST WHERE EMP_ID='" + Empid + "'";
            DataBaseClassSql db = new DataBaseClassSql();
            DataTable dt = new DataTable();
            dt = db.ConnectDataBaseReturnDT(qry);
            deptid = dt.Rows[0]["DPT_ID"].ToString();

        }
        catch (Exception)
        {


        }
        return deptid;
    }




    #region Employee Basic Property
    //saydur rahman
    // 6-4-2014
    //to insert employee property
    public string InsertPropertytype(string BRANCH_ID, string TypeCode, string TypeName, string Nature, string Details)
    {
        string strSql;
        strSql = "SELECT PROPERTY_NAME, PROPERTY_CODE, PROPERTY_TYPE, CMP_BRANCH_ID, PROPERTY_DETAIL FROM HR_EMP_PROPERTY_SETUP ";

        con = new SqlConnection(cn);
        try
        {
            DataRow oOrderRow;

            con.Open();
            dbTransaction = con.BeginTransaction();
            DataSet oDS = new DataSet();
            SqlDataAdapter oOrdersDataAdapter = new SqlDataAdapter(new SqlCommand(strSql, con, dbTransaction));
            SqlCommandBuilder oOrdersCmdBuilder = new SqlCommandBuilder(oOrdersDataAdapter);
            oOrdersDataAdapter.FillSchema(oDS, SchemaType.Source);

            DataTable dtSchedule = oDS.Tables["Table"];
            dtSchedule.TableName = "HR_EMP_PROPERTY_SETUP";

            oOrderRow = oDS.Tables["HR_EMP_PROPERTY_SETUP"].NewRow();

            oOrderRow["CMP_BRANCH_ID"] = BRANCH_ID;
            oOrderRow["PROPERTY_NAME"] = TypeName;
            oOrderRow["PROPERTY_CODE"] = TypeCode;
            oOrderRow["PROPERTY_TYPE"] = Nature;
            oOrderRow["PROPERTY_DETAIL"] = Details;




            oDS.Tables["HR_EMP_PROPERTY_SETUP"].Rows.Add(oOrderRow);
            oOrdersDataAdapter.Update(oDS, "HR_EMP_PROPERTY_SETUP");
            dbTransaction.Commit();
            //con.Close();
            return "1";
        }
        catch (Exception ex)
        {
            return ex.Message.ToString();
        }

        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }
    }
    //saydur rahman
    // 6-4-2014
    //to insert employee property value
    public string InsertPropertyValue(string PROPERTY_ID, string PROPERTY_VALUE, string PROPERTY_DETAIL)
    {
        string strSql;
        strSql = "SELECT PROPERTY_ID, PROPERTY_VALUE, PROPERTY_DETAIL FROM HR_PROPERTY_TYPE_VALUE ";

        con = new SqlConnection(cn);
        try
        {
            DataRow oOrderRow;

            con.Open();
            dbTransaction = con.BeginTransaction();
            DataSet oDS = new DataSet();
            SqlDataAdapter oOrdersDataAdapter = new SqlDataAdapter(new SqlCommand(strSql, con, dbTransaction));
            SqlCommandBuilder oOrdersCmdBuilder = new SqlCommandBuilder(oOrdersDataAdapter);
            oOrdersDataAdapter.FillSchema(oDS, SchemaType.Source);

            DataTable dtSchedule = oDS.Tables["Table"];
            dtSchedule.TableName = "HR_PROPERTY_TYPE_VALUE";

            oOrderRow = oDS.Tables["HR_PROPERTY_TYPE_VALUE"].NewRow();

            oOrderRow["PROPERTY_ID"] = PROPERTY_ID;
            oOrderRow["PROPERTY_VALUE"] = PROPERTY_VALUE;
            oOrderRow["PROPERTY_DETAIL"] = PROPERTY_DETAIL;


            oDS.Tables["HR_PROPERTY_TYPE_VALUE"].Rows.Add(oOrderRow);
            oOrdersDataAdapter.Update(oDS, "HR_PROPERTY_TYPE_VALUE");
            dbTransaction.Commit();
            //con.Close();
            return "1";
        }
        catch (Exception ex)
        {
            return ex.Message.ToString();
        }

        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }
    }
    public DataSet GetLeavePolicy(string leaveTypeID, string degID, string empType, string dept)
    {
        string strQuery = "select PRLP_ALLOWANCE, PRLP_FREQUANCY_LEAVE, PRLP_VALID_DAY from PR_LEAVE_POLICY "
                         + "where PRLT_ID='" + leaveTypeID + "' AND DSG_ID='" + degID + "' AND TYP_CODE='" + empType + "' AND NATURE_TYPE='SL' "
                         + "AND DPT_ID='" + dept + "'";

        var con = new SqlConnection(cn);
        try
        {

            DataSet oDS = new DataSet();
            SqlDataAdapter odaData = new SqlDataAdapter(new SqlCommand(strQuery, con));
            odaData.Fill(oDS, "LEAVE_POLICY");
            return oDS;
        }
        catch (Exception ex)
        {
            return null;
        }
        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }
    }
    public DataSet GetLeavePolicyEmp(string leaveTypeID, string degID, string empType, string dept, string empId)
    {
        string strQuery = "select PRLP_ALLOWANCE, PRLP_FREQUANCY_LEAVE, PRLP_VALID_DAY from PR_LEAVE_POLICY "
                         + "where PRLT_ID='" + leaveTypeID + "' AND DSG_ID='" + degID + "' AND TYP_CODE='" + empType + "' AND NATURE_TYPE='SL' "
                         + "AND DPT_ID='" + dept + "' and EMP_ID='" + empId + "' ";

        var con = new SqlConnection(cn);
        try
        {

            DataSet oDS = new DataSet();
            SqlDataAdapter odaData = new SqlDataAdapter(new SqlCommand(strQuery, con));
            odaData.Fill(oDS, "LEAVE_POLICY");
            return oDS;
        }
        catch (Exception ex)
        {
            return null;
        }
        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }
    }
    #endregion
    #region RECOMMAND, APPROVAL PROCESS
    ///<summary>
    /// Author:Tanjil Alam
    /// Date: 10/04/2014
    /// 
    /// </summary>
    /// 
    /// <returns></returns>
    /// 
    public DataSet GetNumOfRcPer1(string empDesig, string empID, string bid)
    {
        string strQuery = "select SOURCE_EMP_ID, SOURCE_DSG_ID,ITEM_SET_CODE from CM_WORKFLOW_SETUP "
                        + "where (DESTINATION_EMP_ID='" + empID + "' or DESTINATION_DSG_ID='" + empDesig + "') AND MAIN_TYPE_ID='01' AND ITEM_SET_CODE='0103' "
                        + "AND CMP_BRANCH_ID='" + bid + "' ORDER BY ITEM_SET_CODE ";
        var con = new SqlConnection(cn);
        try
        {

            DataSet oDS = new DataSet();
            SqlDataAdapter odaData = new SqlDataAdapter(new SqlCommand(strQuery, con));
            odaData.Fill(oDS, "EMPLOYEE_RECOMMEND_INFO");
            return oDS;
        }
        catch (Exception ex)
        {
            return null;
        }
        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }
    }
    public DataSet GetSetCode(string empID, string empDesigID, string rcPerID, string rcDesigID, string bid)
    {
        string strQuery = "select ITEM_SET_CODE from CM_WORKFLOW_SETUP "
                        + "where (DESTINATION_EMP_ID='" + empID + "' or DESTINATION_DSG_ID='" + empDesigID + "') "
                        + "AND (SOURCE_DSG_ID='" + rcDesigID + "' or SOURCE_EMP_ID='" + rcPerID + "') AND MAIN_TYPE_ID='01' AND CMP_BRANCH_ID='" + bid + "'";
        var con = new SqlConnection(cn);
        try
        {

            DataSet oDS = new DataSet();
            SqlDataAdapter odaData = new SqlDataAdapter(new SqlCommand(strQuery, con));
            odaData.Fill(oDS, "EMPLOYEE_RECOMMEND_INFO");
            return oDS;
        }
        catch (Exception ex)
        {
            return null;
        }
        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }
    }
    public bool CheckEmpLeaveAvability(string eid, string hrYear, string lvStatus, string bId, string chk)
    {
        string strSql = "";
        int hryr = int.Parse(hrYear) + 1;
        if (chk == "0101" || chk == "0103" || chk == "0104")
        {
            strSql = "SELECT EMP_ID FROM PR_LEAVE "
                   + "WHERE EMP_ID='" + eid + "' AND LVE_STATUS='" + lvStatus + "' AND CMP_BRANCH_ID='" + bId + "'  "
                   + "AND DATEPART(YYYY, LVE_FROM_DATE)  = '" + hrYear + "' AND	 ( DATEPART(YYYY, LVE_TO_DATE)= '" + hrYear + "' OR DATEPART(YYYY, LVE_TO_DATE)= '" + hryr.ToString() + "' )   ";
        }
        else if (chk == "0102")
        {
            strSql = "SELECT EMP_ID FROM PR_LEAVE "
                   + "WHERE EMP_ID='" + eid + "' AND LVE_STATUS='A1' AND CMP_BRANCH_ID='" + bId + "'  "
                   + "AND DATEPART(YYYY, LVE_FROM_DATE)  = '" + hrYear + "' AND	( DATEPART(YYYY, LVE_TO_DATE)= '" + hrYear + "' OR DATEPART(YYYY, LVE_TO_DATE)= '" + hryr.ToString() + "' )   ";
        }

        var con = new SqlConnection(cn);
        try
        {
            DataSet oDS = new DataSet();
            SqlDataAdapter oOrdersDataAdapter = new SqlDataAdapter(new SqlCommand(strSql, con));
            SqlCommandBuilder oOrdersCmdBuilder = new SqlCommandBuilder(oOrdersDataAdapter);
            oOrdersDataAdapter.MissingSchemaAction = MissingSchemaAction.AddWithKey;
            oOrdersDataAdapter.Fill(oDS, "LEAVE_AVAIBILITY");

            DataTable tbl_AD = oDS.Tables["LEAVE_AVAIBILITY"];

            // already exists
            if (tbl_AD.Rows.Count > 0) { return true; }
            else { return false; }
        }
        catch (Exception ex)
        {
            return false;
        }
        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }
    }

    //public bool CheckEmpLeaveAvability(string eid, string hrYear, string lvStatus, string bId, string chk)
    //{
    //    string strSql = "";

    //    if (chk == "0101" || chk == "0103" || chk == "0104")
    //    {
    //        strSql = "SELECT EMP_ID FROM PR_LEAVE "
    //               + "WHERE EMP_ID='" + eid + "' AND LVE_STATUS='" + lvStatus + "' AND CMP_BRANCH_ID='" + bId + "'  "
    //               + "AND DATEPART(YYYY, LVE_FROM_DATE)  = '" + hrYear + "' AND	DATEPART(YYYY, LVE_TO_DATE)= '" + hrYear + "'";
    //    }
    //    else if (chk == "0102")
    //    {
    //        strSql = "SELECT EMP_ID FROM PR_LEAVE "
    //               + "WHERE EMP_ID='" + eid + "' AND LVE_STATUS='A1' AND CMP_BRANCH_ID='" + bId + "'  "
    //               + "AND DATEPART(YYYY, LVE_FROM_DATE)  = '" + hrYear + "' AND	DATEPART(YYYY, LVE_TO_DATE)= '" + hrYear + "'";
    //    }

    //    var con = new SqlConnection(cn);
    //    try
    //    {
    //        DataSet oDS = new DataSet();
    //        SqlDataAdapter oOrdersDataAdapter = new SqlDataAdapter(new SqlCommand(strSql, con));
    //        SqlCommandBuilder oOrdersCmdBuilder = new SqlCommandBuilder(oOrdersDataAdapter);
    //        oOrdersDataAdapter.MissingSchemaAction = MissingSchemaAction.AddWithKey;
    //        oOrdersDataAdapter.Fill(oDS, "LEAVE_AVAIBILITY");

    //        DataTable tbl_AD = oDS.Tables["LEAVE_AVAIBILITY"];

    //        // already exists
    //        if (tbl_AD.Rows.Count > 0) { return true; }
    //        else { return false; }
    //    }
    //    catch (Exception ex)
    //    {
    //        return false;
    //    }

    //    finally
    //    {
    //        con.Close();
    //        con = null;
    //    }
    //}


    public bool ChkEmpAttendance(string eid, string attStatus, string bId)
    {
        string strSql = "";
        strSql = "select * from PR_EMP_ATTENDENCE where APPROVAL_STATUS='" + attStatus + "' and EMP_ID='" + eid + "' and CMP_BRANCH_ID='" + bId + "' ";
        var con = new SqlConnection(cn);
        try
        {
            DataSet oDS = new DataSet();
            SqlDataAdapter oOrdersDataAdapter = new SqlDataAdapter(new SqlCommand(strSql, con));
            SqlCommandBuilder oOrdersCmdBuilder = new SqlCommandBuilder(oOrdersDataAdapter);
            oOrdersDataAdapter.MissingSchemaAction = MissingSchemaAction.AddWithKey;
            oOrdersDataAdapter.Fill(oDS, "PR_EMP_ATTENDENCE");

            DataTable tbl_AD = oDS.Tables["PR_EMP_ATTENDENCE"];

            // already exists
            if (tbl_AD.Rows.Count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        catch (Exception ex)
        {
            return false;
        }
        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }
    }

    public bool ChkEmpAttendance(string eid, string attStatus, string bId, string AccessMethod)
    {
        string strSql = "";
        strSql = "select * from PR_EMP_ATTENDENCE where APPROVAL_STATUS='" + attStatus + "' and EMP_ID='" + eid + "' and CMP_BRANCH_ID='" + bId + "'   and ACCESS_METHOD='" + AccessMethod + "'  ";
        var con = new SqlConnection(cn);
        try
        {
            DataSet oDS = new DataSet();
            SqlDataAdapter oOrdersDataAdapter = new SqlDataAdapter(new SqlCommand(strSql, con));
            SqlCommandBuilder oOrdersCmdBuilder = new SqlCommandBuilder(oOrdersDataAdapter);
            oOrdersDataAdapter.MissingSchemaAction = MissingSchemaAction.AddWithKey;
            oOrdersDataAdapter.Fill(oDS, "PR_EMP_ATTENDENCE");

            DataTable tbl_AD = oDS.Tables["PR_EMP_ATTENDENCE"];

            // already exists
            if (tbl_AD.Rows.Count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        catch (Exception ex)
        {
            return false;
        }
        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }
    }

    public bool ChkEMPPerformance(string eid, string attStatus, string bId)
    {
         string strSql = "";
        DataSet manage = new DataSet();
        string ManagementEmp = string.Empty;
        manage = GetEmpInfo(bId, eid);
        if (manage.Tables[0].Rows.Count > 0)
        {
            ManagementEmp = manage.Tables["EMPLOYEE_INFO"].Rows[0]["MANAGEMENT_EMPLOYEE"].ToString().Trim();
        }
        if (ManagementEmp == "N")
        {
            strSql = @"select count(EMP_ID) EMP_ID  from [dbo].[HR_ASSESSMENT_EMPLOYEE_MASTER] where EMP_ID ='"+eid+"' and CMP_BRANCH_ID='"+bId+"' and STATUS='"+attStatus+"' " +
" having count(EMP_ID)=(select Count(ASSIS_TYPE_ID) from HR_ASSESSMENT_TYPE where ASSIS_EMP_NATURE='E') ";
        }
        else
        {
            strSql = @"select count(EMP_ID) EMP_ID  from [dbo].[HR_ASSESSMENT_EMPLOYEE_MASTER] where EMP_ID ='" + eid + "' and CMP_BRANCH_ID='" + bId + "' and STATUS='" + attStatus + "' " +
" having count(EMP_ID)=(select Count(ASSIS_TYPE_ID) from HR_ASSESSMENT_TYPE where ASSIS_EMP_NATURE='I') ";
        }


       
      
        var con = new SqlConnection(cn);
        try
        {
            DataSet oDS = new DataSet();
            SqlDataAdapter oOrdersDataAdapter = new SqlDataAdapter(new SqlCommand(strSql, con));
            SqlCommandBuilder oOrdersCmdBuilder = new SqlCommandBuilder(oOrdersDataAdapter);
            oOrdersDataAdapter.MissingSchemaAction = MissingSchemaAction.AddWithKey;
            oOrdersDataAdapter.Fill(oDS, "HR_ASSESSMENT_EMPLOYEE_MASTER");

            DataTable tbl_AD = oDS.Tables["HR_ASSESSMENT_EMPLOYEE_MASTER"];

            // already exists
            if (tbl_AD.Rows.Count>0)
            {
                return true;
            }            
            else
            {
                return false;
            }
        }
        catch (Exception ex)
        {
            return false;
        }
        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }
    }


    //public DataSet GetRecommendInfo(string empID, string dId, string dpId, string bID)
    public DataSet GetRecommendInfo(string empID, string dId, string bID, string hrYear)
    {

        int hryr = int.Parse(hrYear) + 1;
        string strQuery = @"SELECT DISTINCT  DESTINATION_EMP_ID,ITEM_SET_CODE,  "
                        + "DESTINATION_DSG_ID "
                        + "FROM   CM_WORKFLOW_SETUP WS "
                        + "WHERE  "
                        + "(SOURCE_EMP_ID = '" + empID + "' OR SOURCE_DSG_ID='" + dId + "') "
            //+ "AND WS.DPT_ID='" + dpId + "' "
                        + "AND MAIN_TYPE_ID='01' AND CMP_BRANCH_ID='" + bID + "'" +

      "  and  WS.DESTINATION_EMP_ID in( SELECT EMP_ID FROM PR_LEAVE WHERE LVE_STATUS not in ('R') AND CMP_BRANCH_ID='" + bID + "' AND DATEPART(YYYY, LVE_FROM_DATE)  = '" + hrYear + "' AND	 ( DATEPART(YYYY, LVE_TO_DATE)= '" + hrYear + "' OR DATEPART(YYYY, LVE_TO_DATE)= '" + hryr.ToString() + "' )) order by WS.DESTINATION_EMP_ID";

        var con = new SqlConnection(cn);
        try
        {

            DataSet oDS = new DataSet();
            SqlDataAdapter odaData = new SqlDataAdapter(new SqlCommand(strQuery, con));
            odaData.Fill(oDS, "EMPLOYEE_RECOMMEND_INFO");
            return oDS;
        }
        catch (Exception ex)
        {
            return null;
        }

        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }
    }
    public DataSet GetRecommendInfo_admin(string empID, string dId, string bID, string hrYear)
    {

        int hryr = int.Parse(hrYear) + 1;
        string strQuery = @"SELECT DISTINCT  DESTINATION_EMP_ID,ITEM_SET_CODE,  "
                        + "DESTINATION_DSG_ID "
                        + "FROM   CM_WORKFLOW_SETUP WS "
                        + "WHERE  "       
                        + "  MAIN_TYPE_ID='01' AND CMP_BRANCH_ID='" + bID + "'" +

      "  and  WS.DESTINATION_EMP_ID in( SELECT EMP_ID FROM PR_LEAVE WHERE LVE_STATUS not in ('R') AND CMP_BRANCH_ID='" + bID + "' AND DATEPART(YYYY, LVE_FROM_DATE)  = '" + hrYear + "' AND	 ( DATEPART(YYYY, LVE_TO_DATE)= '" + hrYear + "' OR DATEPART(YYYY, LVE_TO_DATE)= '" + hryr.ToString() + "' )) order by WS.DESTINATION_EMP_ID";

        var con = new SqlConnection(cn);
        try
        {

            DataSet oDS = new DataSet();
            SqlDataAdapter odaData = new SqlDataAdapter(new SqlCommand(strQuery, con));
            odaData.Fill(oDS, "EMPLOYEE_RECOMMEND_INFO");
            return oDS;
        }
        catch (Exception ex)
        {
            return null;
        }

        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }
    }

    public DataSet GetEmpListLvAllowed(string YrID, string branchFilter)
    {
        string strQuery = "SELECT DISTINCT EL.EMP_ID,LTRIM(CAST(ISNULL(EL.EMP_TITLE, '') AS VARCHAR(100)) + ' ' + CAST(EL.EMP_NAME AS VARCHAR(500))) + '  [' + EL.EMP_CODE + ']'  ENAME "
                         + " FROM PR_EMPLOYEE_LIST EL, PR_LEAVE_ALLOWED LA "
                         + " WHERE EL.EMP_ID=LA.EMP_ID AND LA.YR_ID='" + YrID + "' " + branchFilter + " ";
        var con = new SqlConnection(cn);
        try
        {

            DataSet oDS = new DataSet();
            SqlDataAdapter odaData = new SqlDataAdapter(new SqlCommand(strQuery, con));
            odaData.Fill(oDS, "EmpList");
            return oDS;
        }
        catch (Exception ex)
        {
            return null;
        }

        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }
    }
    public DataSet GetUserEmpId(string uId, string bId, string chk) // get employee id from user id & branch id
    {
        string strQuery = "";
        if (chk == "empId")
        {
            strQuery = "SELECT EMP_ID, DSG_ID,EMP_BIRTHDAY_NOTIFY_SHOW FROM PR_EMPLOYEE_LIST "
                                + "WHERE SYS_USR_ID = '" + uId + "' AND CMP_BRANCH_ID='" + bId + "'";
        }

        else if (chk == "desig")
        {
            strQuery = "SELECT EMP_ID, DSG_ID FROM PR_EMPLOYEE_LIST "
                                + "WHERE SYS_USR_ID = '" + uId + "' AND CMP_BRANCH_ID='" + bId + "'";
        }
        else if (chk == "dpt")
        {
            strQuery = "SELECT EMP_ID, DSG_ID,DPT_ID FROM PR_EMPLOYEE_LIST "
                                + "WHERE SYS_USR_ID = '" + uId + "' AND CMP_BRANCH_ID='" + bId + "'";
        }

        var con = new SqlConnection(cn);

        try
        {

            DataSet oDS = new DataSet();
            SqlDataAdapter odaData = new SqlDataAdapter(new SqlCommand(strQuery, con));
            odaData.Fill(oDS, "PR_EMPLOYEE_LIST");
            return oDS;
        }
        catch (Exception ex)
        {
            return null;
        }
        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }
    }

    public DataSet GetUserEmpIdForBulkLeave(string uId, string bId, string chk) 
    {
        string strQuery = "";
        if (chk == "empId")
        {
            strQuery = "SELECT EMP_ID, DSG_ID,EMP_BIRTHDAY_NOTIFY_SHOW FROM PR_EMPLOYEE_LIST "
                                + "WHERE EMP_ID = '" + uId + "' AND CMP_BRANCH_ID='" + bId + "'";
        }

        else if (chk == "desig")
        {
            strQuery = "SELECT EMP_ID, DSG_ID FROM PR_EMPLOYEE_LIST "
                                + "WHERE EMP_ID = '" + uId + "' AND CMP_BRANCH_ID='" + bId + "'";
        }
        else if (chk == "dpt")
        {
            strQuery = "SELECT EMP_ID, DSG_ID,DPT_ID FROM PR_EMPLOYEE_LIST "
                                + "WHERE EMP_ID = '" + uId + "' AND CMP_BRANCH_ID='" + bId + "'";
        }

        var con = new SqlConnection(cn);

        try
        {

            DataSet oDS = new DataSet();
            SqlDataAdapter odaData = new SqlDataAdapter(new SqlCommand(strQuery, con));
            odaData.Fill(oDS, "PR_EMPLOYEE_LIST");
            return oDS;
        }
        catch (Exception ex)
        {
            return null;
        }
        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }
    }
    public DataSet GetEmpAssingnInfo(string bId, string eId)
    {
        string strQuery = " SELECT EL.EMP_ID,LTRIM(CAST(ISNULL(EL.EMP_TITLE, '') AS VARCHAR(100)) + ' ' + CAST(EL.EMP_NAME AS VARCHAR(500))) ENAME, CB.CMP_BRANCH_NAME, ASSIGN_PERSON, ITEM_SET_CODE, "
                        + " EL.EMP_CODE,CAST(DSG_TITLE AS VARCHAR) AS DNAME, DP.DPT_NAME, EL.EMP_EMAIL, EL.EMP_REPORTING_ID  "
                        + " FROM PR_EMPLOYEE_LIST EL, CM_CMP_BRANCH CB, PR_DESIGNATION DE, PR_DEPARTMENT DP , CM_WORKFLOW_SETUP EA "
                        + " WHERE EL.CMP_BRANCH_ID = CB.CMP_BRANCH_ID AND EL.DSG_ID_MAIN=DE.DSG_ID AND DE.SET_TYPE='D'  AND EL.DPT_ID=DP.DPT_ID AND EA.DESTINATION_EMP_ID=EL.EMP_ID "
                        + " AND CB.CMP_BRANCH_ID ='" + bId + "'  AND EL.EMP_ID ='" + eId + "' AND EA.MAIN_TYPE_ID='01' ORDER BY ITEM_SET_CODE ASC ";
        var con = new SqlConnection(cn);
        try
        {

            DataSet oDS = new DataSet();
            SqlDataAdapter odaData = new SqlDataAdapter(new SqlCommand(strQuery, con));
            odaData.Fill(oDS, "EMPLOYEE_DETAIL_INFO");
            return oDS;
        }
        catch (Exception ex)
        {
            return null;
        }

        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }
    }
    public DataSet GetNumOfRcPer(string empDesig, string empID, string bid, string rcPer)
    {
        string strQuery = "";
        if (rcPer == "0102" || rcPer == "0103")
        {
            strQuery = "select SOURCE_EMP_ID, SOURCE_DSG_ID,ITEM_SET_CODE from CM_WORKFLOW_SETUP "
                            + "where (DESTINATION_EMP_ID='" + empID + "' or DESTINATION_DSG_ID='" + empDesig + "') AND MAIN_TYPE_ID='01' AND ITEM_SET_CODE='" + rcPer + "' "
                            + "AND CMP_BRANCH_ID='" + bid + "' ORDER BY ITEM_SET_CODE ";
        }
        else if (rcPer == "0301" || rcPer == "0302")//Attendance approval check
        {
            strQuery = "select SOURCE_EMP_ID, SOURCE_DSG_ID,ITEM_SET_CODE from CM_WORKFLOW_SETUP "
                               + "where (DESTINATION_EMP_ID='" + empID + "' or DESTINATION_DSG_ID='" + empDesig + "') AND MAIN_TYPE_ID='03' AND ITEM_SET_CODE='" + rcPer + "' "
                               + "AND CMP_BRANCH_ID='" + bid + "' ORDER BY ITEM_SET_CODE ";
        }
        else
        {
            strQuery = "select SOURCE_EMP_ID, SOURCE_DSG_ID,ITEM_SET_CODE from CM_WORKFLOW_SETUP "
                            + "where (DESTINATION_EMP_ID='" + empID + "' or DESTINATION_DSG_ID='" + empDesig + "') AND MAIN_TYPE_ID='01' "
                            + "AND CMP_BRANCH_ID='" + bid + "' ORDER BY ITEM_SET_CODE ";
        }
        var con = new SqlConnection(cn);
        try
        {

            DataSet oDS = new DataSet();
            SqlDataAdapter odaData = new SqlDataAdapter(new SqlCommand(strQuery, con));
            odaData.Fill(oDS, "EMPLOYEE_RECOMMEND_INFO");
            return oDS;
        }
        catch (Exception ex)
        {
            return null;
        }

        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }
    }

    public DataSet GetNumOfRcPer(string empDesig, string empID, string bid)
    {
        string strQuery = "";
        //strQuery = "select SOURCE_EMP_ID, SOURCE_DSG_ID,ITEM_SET_CODE from CM_WORKFLOW_SETUP "
        //                + "where (DESTINATION_EMP_ID='" + empID + "' or DESTINATION_DSG_ID='" + empDesig + "') AND MAIN_TYPE_ID='01' "
        //                + "AND CMP_BRANCH_ID='" + bid + "' AND (ITEM_SET_CODE='0101' OR ITEM_SET_CODE='0102') ORDER BY ITEM_SET_CODE ";

        strQuery = "SELECT SOURCE_EMP_ID, SOURCE_DSG_ID,ITEM_SET_CODE from CM_WORKFLOW_SETUP "
                           + "where (DESTINATION_EMP_ID='" + empID + "') AND MAIN_TYPE_ID='01' "
                           + "AND CMP_BRANCH_ID='" + bid + "' AND (ITEM_SET_CODE='0101' OR ITEM_SET_CODE='0102') ORDER BY ITEM_SET_CODE ";


        var con = new SqlConnection(cn);
        try
        {

            DataSet oDS = new DataSet();
            SqlDataAdapter odaData = new SqlDataAdapter(new SqlCommand(strQuery, con));
            odaData.Fill(oDS, "EMPLOYEE_RECOMMEND_INFO");
            return oDS;
        }
        catch (Exception ex)
        {
            return null;
        }
        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }
    }

    //public DataSet GetWFAttendance(string eId, string grdId, string deptId, string bID)
    public DataSet GetWFAttendance(string eId, string grdId, string bID, string Hryr)
    {
        string strQuery = "SELECT DESTINATION_EMP_ID,"
                        + "ITEM_SET_CODE, "
                        + "DESTINATION_DSG_ID "
                        + "FROM   CM_WORKFLOW_SETUP WS "
                        + "WHERE  "
                        + "(SOURCE_EMP_ID = '" + eId + "' OR SOURCE_DSG_ID='" + grdId + "') "
            //+ "AND WS.DPT_ID='" + deptId + "' "
                        + "AND MAIN_TYPE_ID='03' AND CMP_BRANCH_ID='" + bID + "' and DESTINATION_EMP_ID in (select EMP_ID from PR_EMP_ATTENDENCE where APPROVAL_STATUS in ('NAV','AP')  and CMP_BRANCH_ID='" + bID + "'   and ACCESS_METHOD='FD' and  DATEPART(YYYY, ATT_DATE_TIME) ='" + Hryr + "' )";
        var con = new SqlConnection(cn);
        try
        {

            DataSet oDS = new DataSet();
            SqlDataAdapter odaData = new SqlDataAdapter(new SqlCommand(strQuery, con));
            odaData.Fill(oDS, "EMPLOYEE_RECOMMEND_INFO");
            return oDS;
        }
        catch (Exception ex)
        {
            return null;
        }

        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }
    }

    public DataSet GetPerformanceWorkflow(string eId, string grdId, string bID)
    {
        string strQuery = "SELECT DESTINATION_EMP_ID,"
                        + "ITEM_SET_CODE, "
                        + "DESTINATION_DSG_ID "
                        + "FROM   CM_WORKFLOW_SETUP WS "
                        + "WHERE  "
                        + "(SOURCE_EMP_ID = '" + eId + "' OR SOURCE_DSG_ID='" + grdId + "') "
            //+ "AND WS.DPT_ID='" + deptId + "' "
                        + "AND MAIN_TYPE_ID='04' AND CMP_BRANCH_ID='" + bID + "' ";
        var con = new SqlConnection(cn);
        try
        {

            DataSet oDS = new DataSet();
            SqlDataAdapter odaData = new SqlDataAdapter(new SqlCommand(strQuery, con));
            odaData.Fill(oDS, "EMPLOYEE_RECOMMEND_INFO");
            return oDS;
        }
        catch (Exception ex)
        {
            return null;
        }

        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }
    }
    public DataSet GetWFApplication(string eId, string grdId, string bID, string Type, string itmsFilter)
    {
        string strQuery = "";
        strQuery = "SELECT distinct DESTINATION_EMP_ID,DESTINATION_DSG_ID "
                   + " FROM   CM_WORKFLOW_SETUP WS   WHERE  "
                   + " (SOURCE_EMP_ID = '" + eId + " ' OR SOURCE_DSG_ID='" + grdId + " ') AND    DESTINATION_EMP_ID in (SELECT a.APP_EMP_ID FROM HR_APPLICATION_REPORT  a WHERE STATUS='" + Type + "') AND  "
                   + " MAIN_TYPE_ID in (SELECT APP_TYPE_ID FROM HR_APPLICATION_REPORT WHERE STATUS='" + Type + "') AND CMP_BRANCH_ID='" + bID + "'  " + itmsFilter + "  ";
        var con = new SqlConnection(cn);
        try
        {

            DataSet oDS = new DataSet();
            SqlDataAdapter odaData = new SqlDataAdapter(new SqlCommand(strQuery, con));
            odaData.Fill(oDS, "APPLICATIONS");
            return oDS;
        }
        catch (Exception ex)
        {
            return null;
        }
        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }
    }

    public DataSet GetWFApplication(string eId, string grdId, string bID, string Type)
    {
        string strQuery = "";
        strQuery = "SELECT distinct DESTINATION_EMP_ID,DESTINATION_DSG_ID "
                   + " FROM   CM_WORKFLOW_SETUP WS   WHERE  "
                   + " (SOURCE_EMP_ID = '" + eId + " ' OR SOURCE_DSG_ID='" + grdId + " ') AND    DESTINATION_EMP_ID in (SELECT a.APP_EMP_ID FROM HR_APPLICATION_REPORT  a WHERE STATUS='" + Type + "') AND  "
                   + " MAIN_TYPE_ID in (SELECT APP_TYPE_ID FROM HR_APPLICATION_REPORT WHERE STATUS='" + Type + "') AND CMP_BRANCH_ID='" + bID + "'    ";
        var con = new SqlConnection(cn);
        try
        {

            DataSet oDS = new DataSet();
            SqlDataAdapter odaData = new SqlDataAdapter(new SqlCommand(strQuery, con));
            odaData.Fill(oDS, "APPLICATIONS");
            return oDS;
        }
        catch (Exception ex)
        {
            return null;
        }
        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }
    }




    public DataSet GetRcPer_Application_Process(string empDesig, string empID, string bid, string AppType, string ItemSetCode)   /*  asad   dated:25-Nov-2014   */
    {
        string strQuery = "";
        strQuery = "select SOURCE_EMP_ID, SOURCE_DSG_ID,ITEM_SET_CODE from CM_WORKFLOW_SETUP "
                  + " where (DESTINATION_EMP_ID='" + empID + "' or DESTINATION_DSG_ID='" + empDesig + "') AND MAIN_TYPE_ID='" + AppType + "' "
                  + " AND CMP_BRANCH_ID='" + bid + "' AND (ITEM_SET_CODE='" + ItemSetCode + "') ORDER BY ITEM_SET_CODE ";
        var con = new SqlConnection(cn);
        try
        {
            DataSet oDS = new DataSet();
            SqlDataAdapter odaData = new SqlDataAdapter(new SqlCommand(strQuery, con));
            odaData.Fill(oDS, "App_Approval");
            return oDS;
        }
        catch (Exception ex)
        {
            return null;
        }
        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }
    }


    public DataSet Get_Assessment_Process(string empDesig, string empID, string bid, string AppType, string ItemSetCode)   /*  asad   dated:25-Nov-2014   */
    {
        string strQuery = "";
        strQuery = "select SOURCE_EMP_ID, SOURCE_DSG_ID,ITEM_SET_CODE from CM_WORKFLOW_SETUP "
                  + " where (DESTINATION_EMP_ID='" + empID + "' or DESTINATION_DSG_ID='" + empDesig + "') AND MAIN_TYPE_ID='" + AppType + "' "
                  + " AND CMP_BRANCH_ID='" + bid + "' AND (ITEM_SET_CODE='" + ItemSetCode + "') ORDER BY ITEM_SET_CODE ";
        var con = new SqlConnection(cn);
        try
        {
            DataSet oDS = new DataSet();
            SqlDataAdapter odaData = new SqlDataAdapter(new SqlCommand(strQuery, con));
            odaData.Fill(oDS, "App_Approval");
            return oDS;
        }
        catch (Exception ex)
        {
            return null;
        }
        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }
    }

    public DataSet GetRcPerAsDesignation(string desigID, string bID, string eID)
    {
        string strQuery = "SELECT EMP_ID,EMP_NAME, EMP_EMAIL FROM PR_EMPLOYEE_LIST WHERE DSG_ID='" + desigID + "' AND CMP_BRANCH_ID='" + bID + "' "
                        + " AND EMP_ID <> '" + eID + "' ";
        var con = new SqlConnection(cn);
        try
        {

            DataSet oDS = new DataSet();
            SqlDataAdapter odaData = new SqlDataAdapter(new SqlCommand(strQuery, con));
            odaData.Fill(oDS, "EMPLOYEE_DETAIL_INFO");
            return oDS;
        }
        catch (Exception ex)
        {
            return null;
        }
        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }
    }
    public DataSet GetEmpAsDesig(string desDegID, string desEmpID, string bId)
    {
        string strQuery = "SELECT distinct LTRIM(CAST(ISNULL(L.EMP_TITLE, '') AS VARCHAR(100)) + ' ' + CAST(L.EMP_NAME AS VARCHAR(500))) + '  [' + L.EMP_CODE + ']' as EMP_NAME, EMP_ID FROM PR_EMPLOYEE_LIST L "
                        + "WHERE DSG_ID='" + desDegID + "' AND CMP_BRANCH_ID = '" + bId + "' AND EMP_ID <> '" + desEmpID + "'  order by EMP_NAME ";
        var con = new SqlConnection(cn);
        try
        {

            DataSet oDS = new DataSet();
            SqlDataAdapter odaData = new SqlDataAdapter(new SqlCommand(strQuery, con));
            odaData.Fill(oDS, "EMPLOYEE_INFO");
            return oDS;
        }
        catch (Exception ex)
        {
            return null;
        }
        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }
    }
    public DataSet GetEmpAsId(string desEmpID, string bId)
    {
        string strQuery = "SELECT distinct LTRIM(CAST(ISNULL(L.EMP_TITLE, '') AS VARCHAR(100)) + ' ' + CAST(L.EMP_NAME AS VARCHAR(500))) + '  [' + L.EMP_CODE + ']' as EMP_NAME,EMP_ID FROM PR_EMPLOYEE_LIST L "
                        + "WHERE CMP_BRANCH_ID = '" + bId + "' AND EMP_ID = '" + desEmpID + "' order by EMP_NAME ";
        var con = new SqlConnection(cn);
        try
        {

            DataSet oDS = new DataSet();
            SqlDataAdapter odaData = new SqlDataAdapter(new SqlCommand(strQuery, con));
            odaData.Fill(oDS, "EMPLOYEE_INFO");
            return oDS;
        }
        catch (Exception ex)
        {
            return null;
        }
        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }
    }

    #endregion

    #region Leave Application
    public DataSet GetEmpbasicInfo(string branch, string empId) /* asad 22-Apr-2014 */
    {
        string strQuery = "SELECT EMP_STATUS,DPT_ID from PR_EMPLOYEE_LIST "
                            + "WHERE CMP_BRANCH_ID ='" + branch + "' and EMP_ID='" + empId + "' ";


        con = new SqlConnection(cn);
        try
        {

            DataSet oDS = new DataSet();
            SqlDataAdapter odaData = new SqlDataAdapter(new SqlCommand(strQuery, con));
            odaData.Fill(oDS, "PR_EMPLOYEE_LIST");
            return oDS;
        }
        catch (Exception ex)
        {
            return null;
        }
        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }
    }
    public DataSet GetHolidayInterimCountInfo(string strSql) /* asad 22-Apr-2014 */
    {
        con = new SqlConnection(cn);
        try
        {
            DataSet oDS = new DataSet();
            SqlDataAdapter odaData = new SqlDataAdapter(new SqlCommand(strSql, con));
            odaData.Fill(oDS, "Employee");
            return oDS;
        }
        catch (Exception ex)
        {
            return null;
        }
        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }
    }
    public int GetHolidayCount(string strSql) /* asad 22-Apr-2014 */
    {
        con = new SqlConnection(cn);
        int holiday = 0;
        try
        {
            DataSet oDS = new DataSet();
            SqlDataAdapter odaData = new SqlDataAdapter(new SqlCommand(strSql, con));
            odaData.Fill(oDS, "Holiday");
            holiday = int.Parse(oDS.Tables[0].Rows[0]["TOTAL_HOLIDAY"].ToString());
        }
        catch (Exception ex)
        {

        }
        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }
        return holiday;
    }
    public DataSet GetWeeklyHolidaysCount(string HRyrID, string BranchId) /* asad 23-Apr-2014 */
    {
        con = new SqlConnection(cn);
        string strSql = "SELECT YR_WEEKEND_START,YR_WEEKEND_END FROM HR_YEAR "
                        + "WHERE YR_ID='" + HRyrID + "' AND CMP_BRANCH_ID='" + BranchId + "'  ";
        DataSet oDS = new DataSet();
        try
        {
            SqlDataAdapter odaData = new SqlDataAdapter(new SqlCommand(strSql, con));
            odaData.Fill(oDS, "HR_YEAR");
        }
        catch (Exception ex)
        {
        }
        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }
        return oDS;
    }
    #endregion

    /*  niaz */
    public string InsertAppReport(string branch, string DepId, string empID, //Developed By: Md. Niaz Morshed (27-Apr-14)
                                      string AppTypeId, string AppSubject, string EmpCode,
                                      string EmpDepartName, string AppDetails, string AppToDate,
                                      string AppFromDate, string AppOtherDate, string AppToTime,
                                      string AppFromTime, string AppOtherTime, string ProtinidhiAnchal,
                                      string DepartMentHead, string Amount, string MonthlyAmt, string existingDate, string modifyDate,string description)
    {
        SqlConnection con = new SqlConnection(cn);
        string strSql;
        strSql = "SELECT CMP_BRANCH_ID, DPT_ID, APP_EMP_ID, APP_EMP_CODE, APP_TYPE_ID, APP_SUBJECT, APP_EMP_DEPARTMENT,DESCRIPTION, "
                + " APP_DETAILS, APP_TO_DATE, APP_FROM_DATE, APP_OTHER_DATE, APP_TO_TIME, APP_FROM_TIME, APP_OTHER_TIME, "
                + " APP_PROTINIDHI_ANCHAL, APP_DEPARTMENT_HEAD, APP_LOAN_AMOUNT, APP_MONTHLY_AMT,EXISTING_DATE,MODIFY_DATE FROM HR_APPLICATION_REPORT ";
        try
        {
            DataRow oOrderRow;
            con.Open();
            dbTransaction = con.BeginTransaction();
            DataSet oDS = new DataSet();
            SqlDataAdapter oOrdersDataAdapter = new SqlDataAdapter(new SqlCommand(strSql, con, dbTransaction));
            SqlCommandBuilder oOrdersCmdBuilder = new SqlCommandBuilder(oOrdersDataAdapter);
            oOrdersDataAdapter.FillSchema(oDS, SchemaType.Source);

            DataTable dtAddSchedule = oDS.Tables["Table"];
            dtAddSchedule.TableName = "HR_APPLICATION_REPORT";

            oOrderRow = oDS.Tables["HR_APPLICATION_REPORT"].NewRow();

            oOrderRow["CMP_BRANCH_ID"] = branch;
            oOrderRow["DPT_ID"] = DepId;
            oOrderRow["APP_EMP_ID"] = empID;
            oOrderRow["APP_TYPE_ID"] = AppTypeId;
            oOrderRow["APP_SUBJECT"] = AppSubject;
            oOrderRow["APP_EMP_CODE"] = EmpCode;
            oOrderRow["APP_EMP_DEPARTMENT"] = EmpDepartName;
            oOrderRow["APP_DETAILS"] = AppDetails;
            if (AppToDate != "")
            {
                oOrderRow["APP_TO_DATE"] = AppToDate;
            }
            if (AppFromDate != "")
            {
                oOrderRow["APP_FROM_DATE"] = AppFromDate;
            }
            if (AppOtherDate != "")
            {
                oOrderRow["APP_OTHER_DATE"] = AppOtherDate;
            }
            oOrderRow["APP_TO_TIME"] = AppToTime;
            oOrderRow["APP_FROM_TIME"] = AppFromTime;
            oOrderRow["APP_OTHER_TIME"] = AppOtherTime;
            oOrderRow["APP_PROTINIDHI_ANCHAL"] = ProtinidhiAnchal;
            oOrderRow["APP_DEPARTMENT_HEAD"] = DepartMentHead;
            oOrderRow["APP_LOAN_AMOUNT"] = Amount;
            oOrderRow["APP_MONTHLY_AMT"] = MonthlyAmt;
            oOrderRow["EXISTING_DATE"] = existingDate;
            oOrderRow["MODIFY_DATE"] = modifyDate;
            oOrderRow["DESCRIPTION"] = description;



            oDS.Tables["HR_APPLICATION_REPORT"].Rows.Add(oOrderRow);
            oOrdersDataAdapter.Update(oDS, "HR_APPLICATION_REPORT");
            dbTransaction.Commit();
            return "Saved Successfully.";
        }
        catch (Exception ex)
        {
            return ex.Message.ToString();
        }
        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }
    }



    public DataSet GetLeaveItems()
    {
        string strQuery = "SELECT DISTINCT WM_ITEM_NAME, WM_ITEM_SET_CODE  FROM CM_WORKFLOW_MANAGE_TYPE  WHERE (WM_ITEM_TYPE = 'I')  AND  (WM_ITEM_PARENT_CODE = '01') ORDER BY WM_ITEM_SET_CODE";

        try
        {
            con = new SqlConnection(cn);
            con.Open();
            DataSet oDS = new DataSet();
            SqlDataAdapter odaData = new SqlDataAdapter(new SqlCommand(strQuery, con));
            odaData.Fill(oDS, "LEAVE_ITEMS");
            return oDS;
        }
        catch (Exception ex)
        {
            return null;
        }

        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }
    }

    public string GetEmpId(string branchId, string EmpCode)      /*** added by : Niaz Morshed, 23-Oct-2013, Decription : Select EmployeeDetails ***/
    {
        string strSql = "SELECT EMP_ID FROM PR_EMPLOYEE_LIST WHERE EMP_CODE='" + EmpCode + "' AND CMP_BRANCH_ID='" + branchId + "' ";
        string empId = "";
        DataSet oDS = new DataSet();
        try
        {
            con = new SqlConnection(cn);
            con.Open();
            // Create the DataTable "Orders" in the Dataset and the OrdersDataAdapter
            SqlDataAdapter oOrdersDataAdapter = new SqlDataAdapter(new SqlCommand(strSql, con));
            SqlCommandBuilder oOrdersCmdBuilder = new SqlCommandBuilder(oOrdersDataAdapter);
            oOrdersDataAdapter.Fill(oDS, "EMP_ID");

            if (oDS.Tables[0].Rows.Count > 0)
            {
                empId = oDS.Tables[0].Rows[0]["EMP_ID"].ToString();
            }
            return empId;
        }
        catch (Exception ex)
        {

            return null;
        }
        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }
    }

    public string CheckExist(string bId, string empid)
    {
        string strQuery = "SELECT EMP_ID FROM PR_EMPLOYEE_LIST WHERE (CMP_BRANCH_ID = '" + bId + "' and EMP_ID='" + empid + "') ";

        string result = "";
        try
        {
            con = new SqlConnection(cn);
            con.Open();
            DataSet oDS = new DataSet();
            SqlDataAdapter odaData = new SqlDataAdapter(new SqlCommand(strQuery, con));
            odaData.Fill(oDS, "PR_EMPLOYEE_LIST");
            if (oDS.Tables[0].Rows.Count > 0)
            {
                result = "exist";
            }
        }
        catch (Exception ex)
        {
            return null;
        }
        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }
        return result;
    }

    public DataSet CheckType(string typCode)
    {
        string strQuery = "select TYP_TYPE, TYP_CODE from HR_EMP_TYPE where TYP_CODE='" + typCode + "'";

        string result = "";
        try
        {
            con = new SqlConnection(cn);
            con.Open();
            DataSet oDS = new DataSet();
            SqlDataAdapter odaData = new SqlDataAdapter(new SqlCommand(strQuery, con));
            odaData.Fill(oDS, "HR_EMPLOYEE_TYPE");
            return oDS;

        }
        catch (Exception ex)
        {
            return null;
        }
        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }

    }

    #region Employee_Details_Info

    public DataSet EmployeeDetails(string strFilter)   /* Md. Asaduzzaman Dated: 17-June-2014  */
    {

        string strQuery = " select distinct list.EMP_ID, list.EMP_CODE,CAST(ISNULL(list.EMP_TITLE, '') AS VARCHAR) + ' ' + CAST(list.EMP_NAME AS VARCHAR) AS EMPNAME, "
                         + "  CASE list.EMP_STATUS WHEN  '1' THEN 'Trainee' when  '2' THEN 'Permanent' when '3' THEN 'Contractual' "
                         + "   when  '4' THEN 'Part-time' when  '5' THEN 'Probation' when  '6' THEN 'Intern' when  '7' THEN 'Contributor' when '8' THEN 'WB' END  Status "
                         + "  , list.DSG_ID_MAIN DSG_ID, PDpt.DPT_NAME, CONVERT(date, list.EMP_JOINING_DATE)JoinDt, CONVERT(date, list.EMP_CONFIR_DATE)ConfirmationDt, "
                         + "  list.EMP_BLOODGRP,list.EMP_RELIGION, "
                         + "  CASE list.EMP_MARITAL_STATAS WHEN  'U' THEN 'Single' when  'M' THEN 'Married'  END  EMP_MARITAL_STATAS, "
                         + "  list.EMP_CONTACT_NUM,list.EMP_EMAIL,list.EMP_PRE_ADDRES,list.EMP_PER_ADDRESS "
                         + "  ,CONVERT(date, list.EMP_BIRTHDAY)BirthDayCertify,CONVERT(date, list.ACTUAL_BIRTHDAY)BirthDayOriginal, list.EMP_EMAIL_PERSONAL, list.EMP_SPOUSE_DOB, list.MARRIAGE_DATE"
            // +"  ,ref.REF_NAME Nominee_Name,ref.REF_RELATION Nominee_Relation,ref.REF_MOBILE Nominee_Mobile "
                         + "  from PR_EMPLOYEE_LIST list,  "
                         + "  PR_DESIGNATION PD , "  // --right outer join PR_EMPLOYEE_LIST pll on PD.DSG_ID=pll.DSG_ID,
            //+"  HR_EMP_JOB_REFERENCE ref right outer join PR_EMPLOYEE_LIST pl on ref.EMP_ID=pl.EMP_ID, "
                         + "  HR_EMP_JOB_ACADEMIC_QUA Edu right outer join PR_EMPLOYEE_LIST pli on Edu.EMP_ID=pli.EMP_ID, PR_DEPARTMENT PDpt "
                         + "  WHERE PD.DSG_ID=list.DSG_ID  and PDpt.DPT_ID=list.DPT_ID " + strFilter + " order by PDpt.DPT_NAME  ";

        try
        {
            con = new SqlConnection(cn);
            con.Open();
            DataSet oDS = new DataSet();
            SqlDataAdapter odaData = new SqlDataAdapter(new SqlCommand(strQuery, con));
            odaData.Fill(oDS, "EMP_DETAILS");
            return oDS;
        }
        catch (Exception ex)
        {
            return null;
        }

        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }
    }
    public string ServiceTime(DateTime EMP_JOINING_DATE)   /* Md. Asaduzzaman Dated: 17-June-2014  */
    {

        string strQuery = "SELECT dbo.CALCULATE_YRMONTH(CONVERT(date, '" + EMP_JOINING_DATE + "'))ServiceTime ";
        DataSet oDS = new DataSet();
        string result = "";
        try
        {
            con = new SqlConnection(cn);
            con.Open();

            SqlDataAdapter odaData = new SqlDataAdapter(new SqlCommand(strQuery, con));
            odaData.Fill(oDS, "Service");
            if (oDS.Tables[0].Rows.Count > 0)
            {
                result = oDS.Tables[0].Rows[0]["ServiceTime"].ToString();
            }
        }
        catch (Exception ex)
        {
            return null;
        }
        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }

        return result;
    }
    public string PreviousJobExperience(string empId)   /* Md. Asaduzzaman Dated: 17-June-2014  */
    {

        string strQuery = "SELECT dbo.EDU_HONORS('" + empId + "')HONORS ";
        DataSet oDS = new DataSet();
        string result = "";
        try
        {
            con = new SqlConnection(cn);
            con.Open();

            SqlDataAdapter odaData = new SqlDataAdapter(new SqlCommand(strQuery, con));
            odaData.Fill(oDS, "Service");
            if (oDS.Tables[0].Rows.Count > 0)
            {
                result = oDS.Tables[0].Rows[0]["HONORS"].ToString();
            }
        }
        catch (Exception ex)
        {
            return null;
        }

        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }

        return result;
    }
    public string PreviousJobHistory(string empId)   /* Md. Asaduzzaman Dated: 17-June-2014  */
    {
        string strQuery = "SELECT dbo.ServiceHistory('" + empId + "')PastJob ";
        DataSet oDS = new DataSet();
        string result = "";
        try
        {
            con = new SqlConnection(cn);
            con.Open();

            SqlDataAdapter odaData = new SqlDataAdapter(new SqlCommand(strQuery, con));
            odaData.Fill(oDS, "Service");
            if (oDS.Tables[0].Rows.Count > 0)
            {
                result = oDS.Tables[0].Rows[0]["PastJob"].ToString();
            }
        }
        catch (Exception ex)
        {
            return null;
        }

        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }

        return result;
    }

    public string PreviousJobHistoryProthomalo(string empId)   /* Md. Asaduzzaman Dated: 17-June-2014  */
    {
        string strQuery = "SELECT dbo.ServiceHistory_Prothomalo('" + empId + "')PastJob ";
        DataSet oDS = new DataSet();
        string result = "";
        try
        {
            con = new SqlConnection(cn);
            con.Open();

            SqlDataAdapter odaData = new SqlDataAdapter(new SqlCommand(strQuery, con));
            odaData.Fill(oDS, "Service");
            if (oDS.Tables[0].Rows.Count > 0)
            {
                result = oDS.Tables[0].Rows[0]["PastJob"].ToString();
            }
        }
        catch (Exception ex)
        {
            return null;
        }

        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }

        return result;
    }


    public string Designation(string DesigId)   /* Md. Asaduzzaman Dated: 17-June-2014  */
    {

        string strQuery = "select dbo.EMP_DESIG('" + DesigId + "')DSG_TITLE";
        DataSet oDS = new DataSet();
        string result = "";
        try
        {
            con = new SqlConnection(cn);
            con.Open();

            SqlDataAdapter odaData = new SqlDataAdapter(new SqlCommand(strQuery, con));
            odaData.Fill(oDS, "Service");
            if (oDS.Tables[0].Rows.Count > 0)
            {
                result = oDS.Tables[0].Rows[0]["DSG_TITLE"].ToString();
            }
        }
        catch (Exception ex)
        {
            return null;
        }

        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }

        return result;
    }
    public string SSC(string empId)   /* Md. Asaduzzaman Dated: 17-June-2014  */
    {

        string strQuery = "SELECT dbo.EDU_SSC('" + empId + "')SSC ";
        DataSet oDS = new DataSet();
        string result = "";
        try
        {
            con = new SqlConnection(cn);
            con.Open();

            SqlDataAdapter odaData = new SqlDataAdapter(new SqlCommand(strQuery, con));
            odaData.Fill(oDS, "SSC");
            if (oDS.Tables[0].Rows.Count > 0)
            {
                result = oDS.Tables[0].Rows[0]["SSC"].ToString();
            }
        }
        catch (Exception ex)
        {
            return null;
        }

        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }

        return result;
    }
    public string HSC(string empId)   /* Md. Asaduzzaman Dated: 17-June-2014  */
    {

        string strQuery = "SELECT dbo.EDU_HSC('" + empId + "')HSC ";
        DataSet oDS = new DataSet();
        string result = "";
        try
        {
            con = new SqlConnection(cn);
            con.Open();

            SqlDataAdapter odaData = new SqlDataAdapter(new SqlCommand(strQuery, con));
            odaData.Fill(oDS, "Service");
            if (oDS.Tables[0].Rows.Count > 0)
            {
                result = oDS.Tables[0].Rows[0]["HSC"].ToString();
            }
        }
        catch (Exception ex)
        {
            return null;
        }
        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }

        return result;
    }
    public string DIPLOMA(string empId)   /* Md. Asaduzzaman Dated: 17-June-2014  */
    {

        string strQuery = "SELECT dbo.EDU_DIPLOMA('" + empId + "')DIPLOMA ";
        DataSet oDS = new DataSet();
        string result = "";
        try
        {
            con = new SqlConnection(cn);
            con.Open();

            SqlDataAdapter odaData = new SqlDataAdapter(new SqlCommand(strQuery, con));
            odaData.Fill(oDS, "Service");
            if (oDS.Tables[0].Rows.Count > 0)
            {
                result = oDS.Tables[0].Rows[0]["DIPLOMA"].ToString();
            }
        }
        catch (Exception ex)
        {
            return null;
        }
        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }

        return result;
    }
    public string MASTERS(string empId)   /* Md. Asaduzzaman Dated: 17-June-2014  */
    {

        string strQuery = "SELECT dbo.EDU_MASTERS('" + empId + "')MASTERS ";
        DataSet oDS = new DataSet();
        string result = "";
        try
        {
            con = new SqlConnection(cn);
            con.Open();

            SqlDataAdapter odaData = new SqlDataAdapter(new SqlCommand(strQuery, con));
            odaData.Fill(oDS, "Service");
            if (oDS.Tables[0].Rows.Count > 0)
            {
                result = oDS.Tables[0].Rows[0]["MASTERS"].ToString();
            }
        }
        catch (Exception ex)
        {
            return null;
        }

        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }
        return result;
    }
    public string HONORS(string empId)   /* Md. Asaduzzaman Dated: 17-June-2014  */
    {

        string strQuery = "SELECT dbo.EDU_HONORS('" + empId + "')HONORS ";
        DataSet oDS = new DataSet();
        string result = "";
        try
        {
            con = new SqlConnection(cn);
            con.Open();

            SqlDataAdapter odaData = new SqlDataAdapter(new SqlCommand(strQuery, con));
            odaData.Fill(oDS, "Service");
            if (oDS.Tables[0].Rows.Count > 0)
            {
                result = oDS.Tables[0].Rows[0]["HONORS"].ToString();
            }
        }
        catch (Exception ex)
        {
            return null;
        }

        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }
        return result;
    }
    public string NOMINEE_INFO(string empId, string nomineetype)   /* Md. Asaduzzaman Dated: 17-June-2014  */
    {

        string strQuery = "SELECT dbo.NOMINEE_INFO('" + empId + "' , '" + nomineetype + "')NOMINEE ";
        DataSet oDS = new DataSet();
        string result = "";
        try
        {
            con = new SqlConnection(cn);
            con.Open();

            SqlDataAdapter odaData = new SqlDataAdapter(new SqlCommand(strQuery, con));
            odaData.Fill(oDS, "NomInfo");
            if (oDS.Tables[0].Rows.Count > 0)
            {
                result = oDS.Tables[0].Rows[0]["NOMINEE"].ToString();
            }
        }
        catch (Exception ex)
        {
            return null;
        }
        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }

        return result;
    }

    public string InsertAcademicQua(string bId, string eId, string strAcqLevelEducation, string strAcqDegreeTitle, string strAcqConcertration, string strAcqInstituteName, string strAcqResult, string strAcqCGPA, string strAcqScal, string strAcqYearPassing, string strAcqDuration, string strAcqAchievement, string order)
    {


        try
        {
            string strSql = "SELECT EMP_ID, CMP_BRANCH_ID, ACQ_LEVEL_OF_EDUCATION, ACQ_DEGREE_TITLE, ACQ_CONCENTRATION, ACQ_INSTITUTE_NAME, ACQ_RESULT, ACQ_CGPA, ACQ_SCALE, ACQ_YEAR_PASSING, ACQ_DURATION, ACQ_ACHIEVEMENT,ACQ_ORDER FROM HR_EMP_JOB_ACADEMIC_QUA ";
            DataRow oOrderRow;

            con = new SqlConnection(cn);
            con.Open();
            dbTransaction = con.BeginTransaction();
            DataSet oDs = new DataSet();
            SqlDataAdapter oDbAdapter = new SqlDataAdapter(new SqlCommand(strSql, con, dbTransaction));
            SqlCommandBuilder oDbCmdBuilder = new SqlCommandBuilder(oDbAdapter);
            //oDbAdapter.MissingSchemaAction = MissingSchemaAction.AddWithKey;
            //oDbAdapter.Fill(oDs, "HR_EMP_JOB_ACADEMIC_QUA");
            oDbAdapter.FillSchema(oDs, SchemaType.Source, "HR_EMP_JOB_ACADEMIC_QUA");

            // Insert the Data
            oOrderRow = oDs.Tables["HR_EMP_JOB_ACADEMIC_QUA"].NewRow();

            // 12 fields
            oOrderRow["ACQ_LEVEL_OF_EDUCATION"] = strAcqLevelEducation;
            oOrderRow["ACQ_DEGREE_TITLE"] = strAcqDegreeTitle;
            oOrderRow["ACQ_CONCENTRATION"] = strAcqConcertration;
            oOrderRow["ACQ_INSTITUTE_NAME"] = strAcqInstituteName;
            oOrderRow["ACQ_RESULT"] = strAcqResult;
            oOrderRow["ACQ_CGPA"] = strAcqCGPA;
            oOrderRow["ACQ_SCALE"] = strAcqScal;
            oOrderRow["ACQ_YEAR_PASSING"] = strAcqYearPassing;
            oOrderRow["ACQ_DURATION"] = strAcqDuration;
            oOrderRow["ACQ_ACHIEVEMENT"] = strAcqAchievement;
            oOrderRow["EMP_ID"] = eId;
            oOrderRow["CMP_BRANCH_ID"] = bId;
            oOrderRow["ACQ_ORDER"] = order;

            oDs.Tables["HR_EMP_JOB_ACADEMIC_QUA"].Rows.Add(oOrderRow);
            oDbAdapter.Update(oDs, "HR_EMP_JOB_ACADEMIC_QUA");
            dbTransaction.Commit();
            //con.Close();
        }
        catch (Exception ex)
        {

            return null;
        }
        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }
        return "Success";
    }

    #endregion

    #region RosterManagement
    public string START_DATE(int month, int year)   /* Md. Asaduzzaman Dated: 17-June-2014  */
    {

        string strQuery = "SELECT dbo.START_DATE('" + month + "','" + year + "')START_DATE ";
        DataSet oDS = new DataSet();
        string result = "";
        try
        {
            con = new SqlConnection(cn);
            con.Open();

            SqlDataAdapter odaData = new SqlDataAdapter(new SqlCommand(strQuery, con));
            odaData.Fill(oDS, "START_DATE");
            if (oDS.Tables[0].Rows.Count > 0)
            {
                result = oDS.Tables[0].Rows[0]["START_DATE"].ToString();
            }
        }
        catch (Exception ex)
        {
            return null;
        }

        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }
        return result;
    }
    public string END_DATE(int month, int year)   /* Md. Asaduzzaman Dated: 17-June-2014  */
    {

        string strQuery = "SELECT dbo.END_DATE('" + month + "','" + year + "')END_DATE ";
        DataSet oDS = new DataSet();
        string result = "";
        try
        {
            con = new SqlConnection(cn);
            con.Open();

            SqlDataAdapter odaData = new SqlDataAdapter(new SqlCommand(strQuery, con));
            odaData.Fill(oDS, "END_DATE");
            if (oDS.Tables[0].Rows.Count > 0)
            {
                result = oDS.Tables[0].Rows[0]["END_DATE"].ToString();
            }
        }
        catch (Exception ex)
        {
            return null;
        }

        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }

        return result;
    }
    //public string SaveHRmonth(string YrId, string MonthName, int Monthvalue, DateTime startDt, DateTime endDt,int TotalDays)
    //{


    //    try
    //    {
    //        string strSql = " SELECT  YR_ID, MONTH_NAME, MONTH_VALUE, MONTH_START_DATE, MONTH_END_DATE, MONTH_TOTAL_DAYS FROM HR_MONTH ";
    //        DataRow oOrderRow;

    //        con = new SqlConnection(cn);
    //        con.Open();
    //        dbTransaction = con.BeginTransaction();
    //        DataSet oDs = new DataSet();
    //        SqlDataAdapter oDbAdapter = new SqlDataAdapter(new SqlCommand(strSql, con, dbTransaction));
    //        SqlCommandBuilder oDbCmdBuilder = new SqlCommandBuilder(oDbAdapter);

    //        oDbAdapter.FillSchema(oDs, SchemaType.Source, "HR_MONTH");
    //        // Insert the Data
    //        oOrderRow = oDs.Tables["HR_MONTH"].NewRow();


    //        oOrderRow["YR_ID"] = YrId;
    //        oOrderRow["MONTH_NAME"] = MonthName;
    //        oOrderRow["MONTH_VALUE"] = Monthvalue;
    //        oOrderRow["MONTH_START_DATE"] = startDt;
    //        oOrderRow["MONTH_END_DATE"] = endDt;
    //        oOrderRow["MONTH_TOTAL_DAYS"] = TotalDays;

    //        oDs.Tables["HR_MONTH"].Rows.Add(oOrderRow);
    //        oDbAdapter.Update(oDs, "HR_MONTH");
    //        dbTransaction.Commit();
    //        //con.Close();
    //    }
    //    catch (Exception ex)
    //    {

    //        return null;
    //    }

    //    finally
    //    {
    //        con.Close();
    //        con = null;
    //    }

    //    return "Success";
    //}
    public DataSet GetHRMonthInfo(string YrId, string MonthId)
    {
        string strQuery = "SELECT MONTH_TOTAL_DAYS, MONTH_START_DATE,MONTH_END_DATE from HR_MONTH  where MONTH_ID='" + MonthId + "' and YR_ID='" + YrId + "' ";
        var con = new SqlConnection(cn);
        try
        {
            DataSet oDS = new DataSet();
            SqlDataAdapter odaData = new SqlDataAdapter(new SqlCommand(strQuery, con));
            odaData.Fill(oDS, "HR_MONTH");
            return oDS;
        }
        catch (Exception ex)
        {
            return null;
        }
        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }
    }
    public DataSet getMonthPartInfo(string monthid, string monthpartId)
    {
        string strQuery = "SELECT MONTH_ID,WEEKENED_START_DATE,WEEKENED_END_DATE, DATEDIFF(DAY,WEEKENED_START_DATE,WEEKENED_END_DATE)+1 TOTAL_DAYS from HR_WEEKENED where MONTH_ID='" + monthid + "' and WEEKENED_ID='" + monthpartId + "'";
        var con = new SqlConnection(cn);
        try
        {
            DataSet oDS = new DataSet();
            SqlDataAdapter odaData = new SqlDataAdapter(new SqlCommand(strQuery, con));
            odaData.Fill(oDS, "HR_MONTH");
            return oDS;
        }
        catch (Exception ex)
        {
            return null;
        }
        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }
    }
    // Priscilla apu
    public DataSet EmpRosterInfo(string bId, string empId, string hrYearId, string MonthId, string rosterDate)
    {
        string strQuery = "SELECT * FROM PR_EMP_ROSTER "
                            + "WHERE CMP_BRANCH_ID='" + bId + "' AND  EMP_ID='" + empId + "' AND "
                            + "YR_ID='" + hrYearId + "' AND MONTH_ID='" + MonthId + "' AND ROSTER_DATE='" + rosterDate + "'";
        try
        {
            con = new SqlConnection(cn);
            con.Open();
            DataSet oDS = new DataSet();
            SqlDataAdapter odaData = new SqlDataAdapter(new SqlCommand(strQuery, con));
            odaData.Fill(oDS, "EMP_ROSTER");
            return oDS;
        }
        catch (Exception ex)
        {
            return null;
        }
        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }
    }
    public DataSet ChkEmpRoster(string bId, string empId, string hrYearId, string MonthId)
    {
        string strQuery = "SELECT * FROM PR_EMP_ROSTER "
                            + "WHERE CMP_BRANCH_ID='" + bId + "' AND  EMP_ID='" + empId + "' AND "
                            + "YR_ID='" + hrYearId + "' AND MONTH_ID='" + MonthId + "'";
        try
        {
            con = new SqlConnection(cn);
            con.Open();
            DataSet oDS = new DataSet();
            SqlDataAdapter odaData = new SqlDataAdapter(new SqlCommand(strQuery, con));
            odaData.Fill(oDS, "EMP_ROSTER");
            return oDS;
        }
        catch (Exception ex)
        {
            return null;
        }
        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }
    }
    public string RemoveRoster(string bId, string empId, string hrYearId, string MonthId)
    {
        string strSql;

        strSql = "DELETE from PR_EMP_ROSTER "
                + "WHERE CMP_BRANCH_ID='" + bId + "' AND  EMP_ID='" + empId + "' AND "
                            + "YR_ID='" + hrYearId + "' AND MONTH_ID='" + MonthId + "' ";

        //strSql = "DELETE from PR_EMP_ROSTER "
        //         + "WHERE CMP_BRANCH_ID='" + bId + "' AND  EMP_ID='" + empId + "' AND "
        //         + "YR_ID='" + hrYearId + "' AND MONTH_ID='" + MonthId + "' AND convert(date, CONVERT(DATETIME, CONVERT(VARCHAR(23), ROSTER_DATE))) >= "
        //         + " SELECT DISTINCT   convert(date, CONVERT(DATETIME, CONVERT(VARCHAR(23), H.WEEKENED_START_DATE))) FROM HR_WEEKENED H WHERE H.MONTH_ID='" + MonthId + "' AND H.WEEKENED_ID='" + weekendId + "' ";

        try
        {

            con = new SqlConnection(cn);
            con.Open();
            dbTransaction = con.BeginTransaction();
            SqlCommand olcmd = new SqlCommand(strSql, con, dbTransaction);
            olcmd.Connection = con;
            olcmd.ExecuteNonQuery();
            dbTransaction.Commit();

            return "Deleted successfully";
        }
        catch (Exception ex)
        {
            return ex.Message.ToString();
        }
        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }
    }

    public string RemoveRoster(string bId, string empId, string MonthId, string weekendId, string empty, string empt)
    {
        string strSql;
        //strSql = "DELETE from PR_EMP_ROSTER "
        //         + "WHERE CMP_BRANCH_ID='" + bId + "' AND  EMP_ID='" + empId + "' AND "
        //         + "YR_ID='" + hrYearId + "' AND MONTH_ID='" + MonthId + "' AND convert(date, CONVERT(DATETIME, CONVERT(VARCHAR(23), ROSTER_DATE))) >= "
        //         + " (SELECT DISTINCT convert(date, CONVERT(DATETIME, CONVERT(VARCHAR(23), H.WEEKENED_START_DATE))) FROM HR_WEEKENED H WHERE H.MONTH_ID='" + MonthId + "' AND H.WEEKENED_ID='" + weekendId + "') "
        //         + " AND convert(date, CONVERT(DATETIME, CONVERT(VARCHAR(23), ROSTER_DATE))) <= "
        //         + " (SELECT DISTINCT convert(date, CONVERT(DATETIME, CONVERT(VARCHAR(23), H.WEEKENED_END_DATE))) FROM HR_WEEKENED H WHERE H.MONTH_ID='" + MonthId + "' AND H.WEEKENED_ID='" + weekendId + "') ";


        strSql = "DELETE from PR_EMP_ROSTER WHERE CMP_BRANCH_ID='" + bId + "'  "
                  + "AND  EMP_ID='" + empId + "'   "
                  + "AND convert(date, CONVERT(DATETIME, CONVERT(VARCHAR(23), ROSTER_DATE)))  "
                  + "BETWEEN   "
                  + "( SELECT DISTINCT convert(date, CONVERT(DATETIME, CONVERT(VARCHAR(23), H.WEEKENED_START_DATE)))  "
                  + "FROM HR_WEEKENED H WHERE H.MONTH_ID='" + MonthId + "' AND H.WEEKENED_ID='" + weekendId + "' )    "
                  + "AND  "
                  + "(SELECT DISTINCT convert(date, CONVERT(DATETIME, CONVERT(VARCHAR(23), H.WEEKENED_END_DATE)))  "
                  + "FROM HR_WEEKENED H WHERE H.MONTH_ID='" + MonthId + "' AND H.WEEKENED_ID='" + weekendId + "'  ) ";

        try
        {

            con = new SqlConnection(cn);
            con.Open();
            dbTransaction = con.BeginTransaction();
            SqlCommand olcmd = new SqlCommand(strSql, con, dbTransaction);
            olcmd.Connection = con;
            olcmd.ExecuteNonQuery();
            dbTransaction.Commit();

            return "Deleted successfully";
        }
        catch (Exception ex)
        {
            return ex.Message.ToString();
        }
        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }
    }
    public string SaveEmployeeRoster(string bId, string empId, string hrYearId, string MonthId, string rosterID, string rosterDate) //  Employee Roster
    {
        string strSql;


        strSql = "SELECT CMP_BRANCH_ID, EMP_ID, YR_ID, MONTH_ID, ROSTER_DATE, PRR_ID FROM PR_EMP_ROSTER";

        try
        {
            // Payroll Detail
            DataRow oOrderRow;

            con = new SqlConnection(cn);
            con.Open();
            dbTransaction = con.BeginTransaction();
            DataSet oDS = new DataSet();
            SqlDataAdapter oOrdersDataAdapter = new SqlDataAdapter(new SqlCommand(strSql, con, dbTransaction));
            SqlCommandBuilder oOrdersCmdBuilder = new SqlCommandBuilder(oOrdersDataAdapter);
            //oOrdersDataAdapter.MissingSchemaAction = MissingSchemaAction.AddWithKey;
            //oOrdersDataAdapter.Fill(oDS, "PR_PAYROLL_GENERUL");

            oOrdersDataAdapter.FillSchema(oDS, SchemaType.Source, "EMP_ROSTER");
            oOrderRow = oDS.Tables["EMP_ROSTER"].NewRow();

            // 6 fields
            oOrderRow["CMP_BRANCH_ID"] = bId;
            oOrderRow["EMP_ID"] = empId;
            oOrderRow["YR_ID"] = hrYearId;
            oOrderRow["MONTH_ID"] = MonthId;
            oOrderRow["PRR_ID"] = rosterID;
            oOrderRow["ROSTER_DATE"] = rosterDate;

            oDS.Tables["EMP_ROSTER"].Rows.Add(oOrderRow);
            oOrdersDataAdapter.Update(oDS, "EMP_ROSTER");

            dbTransaction.Commit();
            //con.Close();
            return "Saved successfully";

        }
        catch (Exception ex)
        {
            return ex.Message.ToString();
        }
        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }
    }
    public string SaveHRmonth(string YrId, string MonthName, int Monthvalue, DateTime startDt, DateTime endDt, int TotalDays)
    {


        try
        {
            string strSql = " SELECT  YR_ID, MONTH_NAME, MONTH_VALUE, MONTH_START_DATE, MONTH_END_DATE, MONTH_TOTAL_DAYS FROM HR_MONTH ";
            DataRow oOrderRow;

            con = new SqlConnection(cn);
            con.Open();
            dbTransaction = con.BeginTransaction();
            DataSet oDs = new DataSet();
            SqlDataAdapter oDbAdapter = new SqlDataAdapter(new SqlCommand(strSql, con, dbTransaction));
            SqlCommandBuilder oDbCmdBuilder = new SqlCommandBuilder(oDbAdapter);

            oDbAdapter.FillSchema(oDs, SchemaType.Source, "HR_MONTH");
            // Insert the Data
            oOrderRow = oDs.Tables["HR_MONTH"].NewRow();
            oOrderRow["YR_ID"] = YrId;
            oOrderRow["MONTH_NAME"] = MonthName;
            oOrderRow["MONTH_VALUE"] = Monthvalue;
            oOrderRow["MONTH_START_DATE"] = startDt;
            oOrderRow["MONTH_END_DATE"] = endDt;
            oOrderRow["MONTH_TOTAL_DAYS"] = TotalDays;

            oDs.Tables["HR_MONTH"].Rows.Add(oOrderRow);
            oDbAdapter.Update(oDs, "HR_MONTH");
            dbTransaction.Commit();
            //con.Close();
        }
        catch (Exception ex)
        {

            return null;
        }

        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }

        return "Success";
    }

    public string SaveHRYear(string year, DateTime startDt, DateTime endDt, string weekenStart,string weekenEnd,string officeStart,string graceTime,string details,string status,string branchId)
    {


        try
        {
            string strSql = " SELECT  YR_YEAR, CMP_BRANCH_ID, YR_DETAILS, YR_WEEKEND_START, YR_WEEKEND_END, YR_OFFICE_HOUR,YR_STATUS,YR_OFFICE_HOUR_END,GRACE_TIME,YR_STARAT_DATE,YR_END_DATE,VIEW_TIME FROM HR_YEAR ";
            DataRow oOrderRow;

            con = new SqlConnection(cn);
            con.Open();
            dbTransaction = con.BeginTransaction();
            DataSet oDs = new DataSet();
            SqlDataAdapter oDbAdapter = new SqlDataAdapter(new SqlCommand(strSql, con, dbTransaction));
            SqlCommandBuilder oDbCmdBuilder = new SqlCommandBuilder(oDbAdapter);

            oDbAdapter.FillSchema(oDs, SchemaType.Source, "HR_YEAR");
            // Insert the Data
            oOrderRow = oDs.Tables["HR_YEAR"].NewRow();


            oOrderRow["YR_YEAR"] = year;
            oOrderRow["CMP_BRANCH_ID"] = branchId;
            oOrderRow["YR_DETAILS"] = details;
            oOrderRow["YR_WEEKEND_START"] = weekenStart;
            oOrderRow["YR_WEEKEND_END"] = weekenEnd;
            oOrderRow["YR_OFFICE_HOUR"] = officeStart;

            oOrderRow["YR_STATUS"] = status;
            oOrderRow["YR_OFFICE_HOUR_END"] = "";
            oOrderRow["GRACE_TIME"] = graceTime;

            oOrderRow["YR_STARAT_DATE"] = startDt;
            oOrderRow["YR_END_DATE"] = endDt;
            oOrderRow["VIEW_TIME"] = 0;



            oDs.Tables["HR_YEAR"].Rows.Add(oOrderRow);
            oDbAdapter.Update(oDs, "HR_YEAR");
            dbTransaction.Commit();
            //con.Close();
        }
        catch (Exception ex)
        {

            return null;
        }

        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }

        return "Success";
    }
    public DataSet getMonthInfo(string hrYearId, string MonthId)
    {
        string strQuery = " select * from HR_MONTH where YR_ID='" + hrYearId + "' AND MONTH_ID='" + MonthId + "' ";
        try
        {
            con = new SqlConnection(cn);
            con.Open();
            DataSet oDS = new DataSet();
            SqlDataAdapter odaData = new SqlDataAdapter(new SqlCommand(strQuery, con));
            odaData.Fill(oDS, "MonthInfo");
            return oDS;
        }
        catch (Exception ex)
        {
            return null;
        }
        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }
    }
    public DataSet getWkndInfo(string MonthId, string WkndName)
    {
        string strQuery = " select * from HR_WEEKENED where  MONTH_ID='" + MonthId + "' AND WEEKENED_NAME = '" + WkndName + "'  ";
        try
        {
            con = new SqlConnection(cn);
            con.Open();
            DataSet oDS = new DataSet();
            SqlDataAdapter odaData = new SqlDataAdapter(new SqlCommand(strQuery, con));
            odaData.Fill(oDS, "WkndInfo");
            return oDS;
        }
        catch (Exception ex)
        {
            return null;
        }
        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }
    }
    public string SaveHRweekend(string wkndName, string MonthId, DateTime startDt, DateTime endDt)
    {


        try
        {
            string strSql = "SELECT  MONTH_ID, WEEKENED_NAME, WEEKENED_START_DATE, WEEKENED_END_DATE FROM HR_WEEKENED ";
            DataRow oOrderRow;

            con = new SqlConnection(cn);
            con.Open();
            dbTransaction = con.BeginTransaction();
            DataSet oDs = new DataSet();
            SqlDataAdapter oDbAdapter = new SqlDataAdapter(new SqlCommand(strSql, con, dbTransaction));
            SqlCommandBuilder oDbCmdBuilder = new SqlCommandBuilder(oDbAdapter);

            oDbAdapter.FillSchema(oDs, SchemaType.Source, "HR_WEEKENED");
            // Insert the Data
            oOrderRow = oDs.Tables["HR_WEEKENED"].NewRow();


            oOrderRow["MONTH_ID"] = MonthId;
            oOrderRow["WEEKENED_NAME"] = wkndName;
            oOrderRow["WEEKENED_START_DATE"] = startDt;
            oOrderRow["WEEKENED_END_DATE"] = endDt;


            oDs.Tables["HR_WEEKENED"].Rows.Add(oOrderRow);
            oDbAdapter.Update(oDs, "HR_WEEKENED");
            dbTransaction.Commit();
            //con.Close();
        }
        catch (Exception ex)
        {

            return null;
        }

        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }

        return "Success";
    }
    public string RemoveRoster(string bId, string empId, string hrYearId, string MonthId, string rosterDate)
    {
        string strSql;

        strSql = "DELETE from PR_EMP_ROSTER "
                + "WHERE CMP_BRANCH_ID='" + bId + "' AND  EMP_ID='" + empId + "' AND "
                            + "YR_ID='" + hrYearId + "' AND MONTH_ID='" + MonthId + "' AND ROSTER_DATE='" + rosterDate + "'";

        try
        {

            con = new SqlConnection(cn);
            con.Open();
            dbTransaction = con.BeginTransaction();
            SqlCommand olcmd = new SqlCommand(strSql, con, dbTransaction);
            olcmd.Connection = con;
            olcmd.ExecuteNonQuery();
            dbTransaction.Commit();

            return "Deleted successfully";
        }
        catch (Exception ex)
        {
            return ex.Message.ToString();
        }
        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }
    }
    public DataSet ChkEmpRoster(string bId, string empId, string hrYearId, string MonthId, string rosterDate)
    {
        string strQuery = "SELECT * FROM PR_EMP_ROSTER "
                            + "WHERE CMP_BRANCH_ID='" + bId + "' AND  EMP_ID='" + empId + "' AND "
                            + "YR_ID='" + hrYearId + "' AND MONTH_ID='" + MonthId + "' AND ROSTER_DATE='" + rosterDate + "'";
        try
        {
            con = new SqlConnection(cn);
            con.Open();
            DataSet oDS = new DataSet();
            SqlDataAdapter odaData = new SqlDataAdapter(new SqlCommand(strQuery, con));
            odaData.Fill(oDS, "EMP_ROSTER");
            return oDS;
        }
        catch (Exception ex)
        {
            return null;
        }
        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }
    }
    //asad
    public DataSet GetRosterInfoReport(string RosterDate, string Branch, string EmpId) /* ASAD  +' ['+PR.PRR_TIME_FROM + '-->' + PR.PRR_TIME_TO+']'  */
    {
        string strQuery = " SELECT DISTINCT PR.PRR_TYPE  AS ROSTERNAME, LTRIM(CAST(ISNULL(EL.EMP_TITLE, '') AS VARCHAR(100)) + ' ' + CAST(EL.EMP_NAME AS VARCHAR(500))) + ' [' + EL.EMP_CODE + ']' AS ENAME "
        + " FROM PR_EMP_ROSTER ER, PR_ROSTER_SETUP PR,PR_EMPLOYEE_LIST EL  "
        + " WHERE PR.PRR_ID=ER.PRR_ID AND EL.EMP_ID=ER.EMP_ID  "
        + " AND ER.EMP_ID= '" + EmpId + "' AND ER.CMP_BRANCH_ID='" + Branch + "'  "
        + " AND convert(date, CONVERT(DATETIME, CONVERT(VARCHAR(23), ER.ROSTER_DATE))) = convert(date, CONVERT(DATETIME, CONVERT(VARCHAR(23), '" + RosterDate + "'))) ";
        var con = new SqlConnection(cn);
        try
        {
            DataSet oDS = new DataSet();
            SqlDataAdapter odaData = new SqlDataAdapter(new SqlCommand(strQuery, con));
            odaData.Fill(oDS, "RosterRpt");
            return oDS;
        }
        catch (Exception ex)
        {
            return null;
        }
        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }
    }
    public DataSet GetEmpIdAsDeptId(string DeptId)  /* ASAD */
    {
        string strQuery = " SELECT  EL.EMP_CODE, EL.EMP_ID, LTRIM(CAST(ISNULL(EL.EMP_TITLE, '') AS VARCHAR(100)) + ' ' + CAST(EL.EMP_NAME AS VARCHAR(500)))  AS ENAME, LTRIM(CAST(ISNULL(EL.EMP_TITLE, '') AS VARCHAR(100)) + ' ' + CAST(EL.EMP_NAME AS VARCHAR(500)))  AS EMPNAME FROM PR_EMPLOYEE_LIST EL, PR_DEPARTMENT PD WHERE PD.DPT_ID=EL.DPT_ID AND PD.DPT_ID = '" + DeptId + "' AND EL.EMP_STATUS  not in ('11','9') order by  convert(nvarchar,Ltrim(rtrim(isnull(EL.EMP_CODE,'0')))) ASC ";  //4=PARTTIME EMPLOYEE
        var con = new SqlConnection(cn);
        try
        {
            DataSet oDS = new DataSet();
            SqlDataAdapter odaData = new SqlDataAdapter(new SqlCommand(strQuery, con));
            odaData.Fill(oDS, "empId");
            return oDS;
        }
        catch (Exception ex)
        {
            return null;
        }
        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }
    }
    public DataSet GetEmpIdAsDeptId(string DeptId, string filterServiceEnd)  /* ASAD */
    {

        string strQuery = " SELECT PD.DPT_ID,PD.DPT_NAME,  EL.EMP_CODE, EL.EMP_ID, LTRIM(RTRIM(EMP_LATE_SHOW))EMP_LATE_SHOW, LTRIM(CAST(ISNULL(EL.EMP_TITLE, '') AS VARCHAR(100)) + ' ' + CAST(EL.EMP_NAME AS VARCHAR(500)))  AS ENAME, LTRIM(CAST(ISNULL(EL.EMP_TITLE, '') AS VARCHAR(100)) + ' ' + CAST(EL.EMP_NAME AS VARCHAR(500)))  AS EMPNAME FROM PR_EMPLOYEE_LIST EL, PR_DEPARTMENT PD WHERE PD.DPT_ID=EL.DPT_ID AND PD.DPT_ID in (" + DeptId + ") AND EL.EMP_STATUS !='4'  " + filterServiceEnd + " order by PD.DPT_ID  ";  //4=PARTTIME EMPLOYEE   convert(numeric(18,0),Ltrim(rtrim(isnull(EL.EMP_CODE,'0')))) ASC 
        var con = new SqlConnection(cn);
        try
        {
            DataSet oDS = new DataSet();
            SqlDataAdapter odaData = new SqlDataAdapter(new SqlCommand(strQuery, con));
            odaData.Fill(oDS, "empId");
            return oDS;
        }
        catch (Exception ex)
        {
            return null;
        }
        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }
    }
    public DataSet GetEmpIdAsDeptId(string DeptId, string empWise, string empId)  /* ASAD */
    {
        string filterEmp = "";
        if (empWise == "empwise")
        {
            filterEmp = " AND EL.EMP_ID='" + empId + "' ";
        }
        string strQuery = " SELECT  EL.EMP_CODE, EL.EMP_ID, LTRIM(CAST(ISNULL(EL.EMP_TITLE, '') AS VARCHAR(100)) + ' ' + CAST(EL.EMP_NAME AS VARCHAR(500)))  AS ENAME, LTRIM(CAST(ISNULL(EL.EMP_TITLE, '') AS VARCHAR(100)) + ' ' + CAST(EL.EMP_NAME AS VARCHAR(500)))  AS EMPNAME FROM PR_EMPLOYEE_LIST EL, PR_DEPARTMENT PD WHERE PD.DPT_ID=EL.DPT_ID AND PD.DPT_ID = '" + DeptId + "' AND EL.EMP_STATUS !='4'  " + filterEmp + "  order by  convert(numeric(18,0),Ltrim(rtrim(isnull(EL.EMP_CODE,'0')))) ASC ";  //4=PARTTIME EMPLOYEE
        var con = new SqlConnection(cn);
        try
        {
            DataSet oDS = new DataSet();
            SqlDataAdapter odaData = new SqlDataAdapter(new SqlCommand(strQuery, con));
            odaData.Fill(oDS, "empId");
            return oDS;
        }
        catch (Exception ex)
        {
            return null;
        }
        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }
    }
    public DataSet GetEmpIdAsDeptId(string DeptId, string empWise, string empId, string status)  /* ASAD */
    {
        string filterEmp = "";
        if (empWise == "empwise")
        {
            filterEmp = " AND EL.EMP_ID='" + empId + "' ";
        }
        string strQuery = " SELECT  EL.EMP_CODE, EL.EMP_ID, LTRIM(CAST(ISNULL(EL.EMP_TITLE, '') AS VARCHAR(100)) + ' ' + CAST(EL.EMP_NAME AS VARCHAR(500)))  AS EMPNAME,  LTRIM(RTRIM(EL.EMP_LATE_SHOW))EMP_LATE_SHOW  FROM PR_EMPLOYEE_LIST EL, PR_DEPARTMENT PD WHERE PD.DPT_ID=EL.DPT_ID AND PD.DPT_ID = '" + DeptId + "' AND EL.EMP_STATUS !='4'  " + filterEmp + "  " + status + "   order by  EL.EMP_CODE ASC ";  //4=PARTTIME EMPLOYEE
        var con = new SqlConnection(cn);
        try
        {
            DataSet oDS = new DataSet();
            SqlDataAdapter odaData = new SqlDataAdapter(new SqlCommand(strQuery, con));
            odaData.Fill(oDS, "empId");
            return oDS;
        }
        catch (Exception ex)
        {
            return null;
        }
        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }
    }
    #endregion

    public DataSet GetEmpBasicInfo(string empId, string branch)
    {
        string strQuery = " SELECT EL.DPT_ID,EL.EMP_CODE,LTRIM(CAST(ISNULL(EL.EMP_TITLE, '') AS VARCHAR(100)) + ' ' + CAST(EL.EMP_NAME AS VARCHAR(500)))  AS ENAME ,PD.DPT_NAME,DG.DSG_TITLE,LTRIM(RTRIM(EL.EMP_LATE_SHOW))EMP_LATE_SHOW "
                         + " FROM PR_EMPLOYEE_LIST EL, PR_DEPARTMENT PD, PR_DESIGNATION DG "
                         + " WHERE EL.DPT_ID=PD.DPT_ID AND EL.DSG_ID_MAIN=DG.DSG_ID  AND EL.EMP_ID='" + empId + "' and EL.CMP_BRANCH_ID='" + branch + "'  ";
        var con = new SqlConnection(cn);
        try
        {
            DataSet oDS = new DataSet();
            SqlDataAdapter odaData = new SqlDataAdapter(new SqlCommand(strQuery, con));
            odaData.Fill(oDS, "empId");
            return oDS;
        }
        catch (Exception ex)
        {
            return null;
        }
        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }
    }

    //saydur
    //7/7/2014
    //to save the holiday for dept
    //saydur
    //7/7/2014
    //to save the holiday for dept
    public bool saveHolidayForDept(string holiday, string dept, string branch, string yrId)
    {
        DataBaseClassSql db = new DataBaseClassSql();
        string qry = "select * from PR_HOLIDAY_FOR_DPT where DPT_ID='" + dept + "' and HOLIDAY_ID='" + holiday + "' and CMP_BRANCH_ID='" + branch + "' and YR_ID='" + yrId + "' ";
        DataTable dt = db.ConnectDataBaseReturnDT(qry);
        if (dt.Rows.Count == 0)
        {
            qry = "insert into PR_HOLIDAY_FOR_DPT (DPT_ID,HOLIDAY_ID,CMP_BRANCH_ID,YR_ID)values('" + dept + "','" + holiday + "','" + branch + "','" + yrId + "')";
            db.ConnectDataBaseToInsert(qry);
            return true;
        }
        else
            return false;

    }


    //saydur
    //6-7-2014 
    //to check weather the attandance is given before in a day or not
    public bool checkMultiAttandnc(string empId)
    {
        DataBaseClassSql db = new DataBaseClassSql();
        DataTable dt = new DataTable();
        string qry = "select * from PR_EMP_ATTENDENCE where EMP_ID='" + empId + "' and CONVERT(date,ATT_DATE_TIME)='" + DateTime.Now.ToString("yyyy-MM-dd") + "';";
        try
        {
            dt = db.ConnectDataBaseReturnDT(qry);
            if (dt.Rows.Count > 1)
            {
                return false;
            }
            else
                return true;
        }
        catch (Exception ex)
        {
            return false;
        }
    }
    public string getdeptNameById(string id)  /*  sayed  */
    {
        string result = "";
        try
        {
            DataBaseClassSql db = new DataBaseClassSql();
            string qry = "select * from PR_DEPARTMENT WHERE DPT_ID='" + id + "'";
            DataTable dt = db.ConnectDataBaseReturnDT(qry);
            result = dt.Rows[0]["DPT_NAME"].ToString();
        }
        catch (Exception)
        {

            return "";
        }
        return result;
    }

    //################################################### Notice board ########################################################
    public string InsertNotice(string SRC, string NOTICEDT, string AUTHORITY, string AUTHODESIG, string NOTICETITLE, string NOTICEDETAILS, string PHOTO, string BRID, string DEPTID)
    {
        try  /*  asad  */
        {
            string strSql = "SELECT NOT_SOURCE_NUM,NOT_DATE,NOT_AUTHORITY_NAME,NOT_AUTHORITY_DESIG,NOT_TITLE,NOT_DETAILS,NOT_PHOTO_ID,NOT_CREATION_DATE,DPT_ID,CMP_BRANCH_ID FROM HR_NOTICE_BOARD  ";
            DataRow oOrderRow;

            con = new SqlConnection(cn);
            con.Open();
            dbTransaction = con.BeginTransaction();
            DataSet oDs = new DataSet();
            SqlDataAdapter oDbAdapter = new SqlDataAdapter(new SqlCommand(strSql, con, dbTransaction));
            SqlCommandBuilder oDbCmdBuilder = new SqlCommandBuilder(oDbAdapter);
            oDbAdapter.FillSchema(oDs, SchemaType.Source, "HR_NOTICE_BOARD");
            // Insert Data
            oOrderRow = oDs.Tables["HR_NOTICE_BOARD"].NewRow();

            oOrderRow["NOT_SOURCE_NUM"] = SRC;
            oOrderRow["NOT_DATE"] = NOTICEDT;
            oOrderRow["NOT_AUTHORITY_NAME"] = AUTHORITY;
            oOrderRow["NOT_AUTHORITY_DESIG"] = AUTHODESIG;
            oOrderRow["NOT_TITLE"] = NOTICETITLE;
            oOrderRow["NOT_DETAILS"] = NOTICEDETAILS;
            oOrderRow["NOT_PHOTO_ID"] = PHOTO;
            oOrderRow["DPT_ID"] = DEPTID;
            oOrderRow["CMP_BRANCH_ID"] = BRID;
            oOrderRow["NOT_CREATION_DATE"] = DateTime.Now.ToShortDateString();

            oDs.Tables["HR_NOTICE_BOARD"].Rows.Add(oOrderRow);
            oDbAdapter.Update(oDs, "HR_NOTICE_BOARD");
            dbTransaction.Commit();
        }
        catch (Exception ex)
        {
            return "error";
        }

        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }

        return "Success";
    }
    public DataSet GetNoticeBoard(string branchId, string dptId)      /*** added by : asad ***/
    {
        string strSql = "SELECT * FROM HR_NOTICE_BOARD WHERE NOT_ACTIVE ='A' AND DPT_ID='" + dptId + "' AND CMP_BRANCH_ID='" + branchId + "'  ";

        DataSet oDS = new DataSet();
        try
        {
            con = new SqlConnection(cn);
            con.Open();
            // Create the DataTable "Orders" in the Dataset and the OrdersDataAdapter
            SqlDataAdapter oOrdersDataAdapter = new SqlDataAdapter(new SqlCommand(strSql, con));
            SqlCommandBuilder oOrdersCmdBuilder = new SqlCommandBuilder(oOrdersDataAdapter);
            oOrdersDataAdapter.Fill(oDS, "HR_NOTICE_BOARD");
            return oDS;
        }
        catch (Exception ex)
        {

            return null;
        }
        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }
    }
    public DataSet GetNoticeBoard(string noticeid)      /*** added by : asad ***/
    {
        string strSql = "SELECT * FROM HR_NOTICE_BOARD WHERE NOTICE_ID='" + noticeid + "'";

        DataSet oDS = new DataSet();
        try
        {
            con = new SqlConnection(cn);
            con.Open();
            // Create the DataTable "Orders" in the Dataset and the OrdersDataAdapter
            SqlDataAdapter oOrdersDataAdapter = new SqlDataAdapter(new SqlCommand(strSql, con));
            SqlCommandBuilder oOrdersCmdBuilder = new SqlCommandBuilder(oOrdersDataAdapter);
            oOrdersDataAdapter.Fill(oDS, "HR_NOTICE_BOARD");
            return oDS;
        }
        catch (Exception ex)
        {

            return null;
        }

        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }
    }
    public string getDptidByUserId(string empid)
    {
        DataBaseClassSql db = new DataBaseClassSql();
        string qry = "SELECT DPT_ID FROM PR_EMPLOYEE_LIST WHERE EMP_ID='" + empid + "'";
        string dptid;
        try
        {
            DataTable dt = new DataTable();
            dt = db.ConnectDataBaseReturnDT(qry);
            dptid = dt.Rows[0]["DPT_ID"].ToString();
        }
        catch (Exception ex)
        {
            dptid = "";
        }

        return dptid;
    }
    public string getCurrentHRyearAsBranch(string branchid)
    {
        DataBaseClassSql db = new DataBaseClassSql();
        string qry = " SELECT YR_YEAR FROM HR_YEAR HY, CM_CMP_BRANCH C WHERE HY.CMP_BRANCH_ID = C.CMP_BRANCH_ID and YR_STATUS='R' "
                    + " AND C.CMP_BRANCH_ID ='" + branchid + "' ";
        string year;
        try
        {
            DataTable dt = new DataTable();
            dt = db.ConnectDataBaseReturnDT(qry);
            year = dt.Rows[0]["YR_YEAR"].ToString();
        }
        catch (Exception ex)
        {
            year = "";
        }

        return year;
    }

    public DataTable getAppliedLeaveStautsEmp(string branchid, string empId)
    {
        DataBaseClassSql db = new DataBaseClassSql();
        DataTable dt = new DataTable();
        string qry = "SELECT  LVE_FROM_DATE,LVE_TO_DATE,LVE_STATUS =CASE LVE_STATUS "
                       + " WHEN 'A1' THEN 'FIRST-RECOMMENDED'  WHEN 'RC' THEN 'RECOMMENDED' "
                       + " WHEN 'R1' THEN 'INITIAL APPROVED' WHEN 'R' THEN 'APPROVED' WHEN 'A' THEN 'APPLIED' END "
                       + "FROM PR_LEAVE WHERE  LVE_ID = (SELECT MAX(LVE_ID) FROM PR_LEAVE WHERE EMP_ID='" + empId + "') "
                       + " AND CMP_BRANCH_ID = '" + branchid + "' and LVE_TO_DATE >= convert(date, getdate()-1) ";
        string lvStatus = "";
        try
        {

            dt = db.ConnectDataBaseReturnDT(qry);
            //if (dt.Rows.Count > 0)
            //{
            //    lvStatus = dt.Rows[0]["LVE_STATUS"].ToString();
            //}
        }
        catch (Exception ex)
        {

        }

        return dt;
    }
    public DataTable getAppliedAttendanceInfo(string branchid, string empId)
    {
        DataBaseClassSql db = new DataBaseClassSql();
        DataTable dt = new DataTable();
        string qry = " SELECT DISTINCT APPROVAL_STATUS = CASE AT.APPROVAL_STATUS "
                    + " WHEN 'AV' THEN 'HR APPROVED'  WHEN 'NAV' THEN 'INITIAL APPROVED' "
                    + " WHEN 'AP' THEN 'APPLIED' WHEN 'CN' THEN 'CANCELLED' END ,  "
                    + " convert(date, CONVERT(DATETIME, CONVERT(VARCHAR(23), AT.ATT_DATE_TIME))) DATEOSD  "
                    + " FROM PR_EMP_ATTENDENCE AT "
                    + "  WHERE  "
                    + " ( "
                    + " convert(date, CONVERT(DATETIME, CONVERT(VARCHAR(23), AT.ATT_DATE_TIME))) >= GETDATE() OR "
                    + " convert(date, CONVERT(DATETIME, CONVERT(VARCHAR(23), AT.ATT_DATE_TIME))) >= convert(date, getdate()-1) OR "
                    + " convert(date, CONVERT(DATETIME, CONVERT(VARCHAR(23), AT.ATT_DATE_TIME))) >= convert(date, getdate()-2) OR "
                    + " convert(date, CONVERT(DATETIME, CONVERT(VARCHAR(23), AT.ATT_DATE_TIME))) >= convert(date, getdate()-3) "
                    + " ) "
                    + " AND "
                    + " AT.ACCESS_METHOD='FD' AND AT.ATT_TYPE='OSD' AND AT.EMP_ID='" + empId + "' ";// AND AT.CMP_BRANCH_ID='" + branchid + "' ";
        string lvStatus = "";
        try
        {
            dt = db.ConnectDataBaseReturnDT(qry);
        }
        catch (Exception ex)
        {

        }

        return dt;
    }


    public DataTable getPerformanceInfo(string branchid, string empId)
    {
        DataBaseClassSql db = new DataBaseClassSql();
        DataTable dt = new DataTable();
        string qry = " select * from [dbo].[HR_ASSESSMENT_EMPLOYEE_MASTER] where  ";
        string lvStatus = "";
        try
        {
            dt = db.ConnectDataBaseReturnDT(qry);
        }
        catch (Exception ex)
        {

        }

        return dt;
    }

    //password request
    public bool AdminGroupCheck(string groupId)
    {
        DataBaseClassSql db = new DataBaseClassSql();
        string qry = " select SYS_USR_GRP_ID from CM_SYSTEM_USER_GROUP WHERE SYS_USR_GRP_ID='" + groupId + "' AND SYS_USR_GRP_TITLE='Administrator'  ";
        string grpId = "";
        bool adminYes = false;
        try
        {
            DataTable dt = new DataTable();
            dt = db.ConnectDataBaseReturnDT(qry);
            grpId = dt.Rows[0]["SYS_USR_GRP_ID"].ToString();
            if (grpId != "")
            {
                adminYes = true;
            }
        }
        catch (Exception ex)
        {
            adminYes = false;
        }

        return adminYes;
    }
    public string TotalPassRequest()
    {
        DataBaseClassSql db = new DataBaseClassSql();
        string qry = " SELECT COUNT(*)totalRequest FROM CM_SYSTEM_PASSWORD_REQUEST WHERE (PASS_REQ_SENT = 'N') ";

        string total;
        try
        {
            DataTable dt = new DataTable();
            dt = db.ConnectDataBaseReturnDT(qry);
            total = dt.Rows[0]["totalRequest"].ToString();
        }
        catch (Exception ex)
        {
            total = "";
        }

        return total;
    }
    public string getAttendanceTime(string Empid, string BranchId) //asad
    {
        string attTime = "";
        try
        {
            string qry = " select distinct ' ' + rs.PRR_TIME_FROM + ' --> ' +rs.PRR_TIME_TO + ' ]'  attendanceTime "
                        + " from PR_EMP_ROSTER r, PR_ROSTER_SETUP rs "
                        + " where convert(date, CONVERT(DATETIME, CONVERT(VARCHAR(23), ROSTER_DATE)))  =  convert(date, getdate()) "
                        + " and r.PRR_ID=rs.PRR_ID and r.EMP_ID='" + Empid + "' and r.CMP_BRANCH_ID='" + BranchId + "' ";
            DataBaseClassSql db = new DataBaseClassSql();
            DataTable dt = new DataTable();
            dt = db.ConnectDataBaseReturnDT(qry);
            attTime = dt.Rows[0]["attendanceTime"].ToString();

        }
        catch (Exception)
        {


        }
        return attTime;
    }
    public string getLVjoin_Holiday(string deptId, string BranchId, string JoinDt, string HRyrId) //asad
    {
        string LvJoinDt = "";
        try
        {
            string qry = " SELECT DISTINCT   DATEADD(day,HS.PRH_DATE_DURATION,convert(date, CONVERT(DATETIME, CONVERT(VARCHAR(23), PRH_FROM_DATE ))))LVJOIN_HOLIDAY "
                // --HS.PRH_FROM_DATE STARTDT,HS.PRH_TO_DATE ENDDT,HS.PRH_DATE_DURATION,
           + " FROM PR_HOLIDAY_SETUP HS,  PR_HOLIDAY_FOR_DPT HD "
           + " WHERE HS.PRH_ID = HD.HOLIDAY_ID AND  HD.DPT_ID='" + deptId + "' AND HD.CMP_BRANCH_ID='" + BranchId + "' AND YR_ID='" + HRyrId + "' "
           + " ( "
           + " convert(date,CONVERT(DATETIME, CONVERT(VARCHAR(23), '" + JoinDt + "')))>= convert(date, CONVERT(DATETIME, CONVERT(VARCHAR(23), PRH_FROM_DATE))) "
           + " AND convert(date,CONVERT(DATETIME, CONVERT(VARCHAR(23), '" + JoinDt + "'))) <= convert(date, CONVERT(DATETIME, CONVERT(VARCHAR(23), PRH_TO_DATE))) "
           + " ) ";

            DataBaseClassSql db = new DataBaseClassSql();
            DataTable dt = new DataTable();
            dt = db.ConnectDataBaseReturnDT(qry);
            LvJoinDt = dt.Rows[0]["LVJOIN_HOLIDAY"].ToString();
        }
        catch (Exception)
        {
        }
        return LvJoinDt;
    }
    public bool ChkWeekend(string Empid, string BranchId, string joinDt)
    {
        string attTime = "";
        bool weekend = false;
        try
        {
            string qry = " select distinct  rs.PRR_TIME_FROM   Weekend "
                        + " from PR_EMP_ROSTER r, PR_ROSTER_SETUP rs "
                        + " where convert(date, CONVERT(DATETIME, CONVERT(VARCHAR(23), ROSTER_DATE)))  =  convert(date, CONVERT(DATETIME, CONVERT(VARCHAR(23), '" + joinDt + "'))) "
                        + " and r.PRR_ID=rs.PRR_ID and r.EMP_ID='" + Empid + "' and r.CMP_BRANCH_ID='" + BranchId + "' ";
            DataBaseClassSql db = new DataBaseClassSql();
            DataTable dt = new DataTable();
            dt = db.ConnectDataBaseReturnDT(qry);
            attTime = dt.Rows[0]["Weekend"].ToString();
            if (attTime == "00:00")
            {
                weekend = true;
            }
        }
        catch (Exception)
        {

        }
        return weekend;
    }
    #region AddressSetup
    public string InsertDivision(string DivisionName)
    {
        string strSql;
        strSql = " SELECT CMP_DIVISION_NAME  FROM CM_DIVISION  ";
        var con = new SqlConnection(cn);
        try
        {
            DataRow oOrderRow;
            con.Open();
            dbTransaction = con.BeginTransaction();
            DataSet oDS = new DataSet();
            SqlDataAdapter oOrdersDataAdapter = new SqlDataAdapter(new SqlCommand(strSql, con, dbTransaction));
            SqlCommandBuilder oOrdersCmdBuilder = new SqlCommandBuilder(oOrdersDataAdapter);
            oOrdersDataAdapter.FillSchema(oDS, SchemaType.Source);
            //######################################################
            DataTable dtSchedule = oDS.Tables["Table"];
            dtSchedule.TableName = "CM_DIVISION";
            //######################################################
            oOrderRow = oDS.Tables["CM_DIVISION"].NewRow();
            oOrderRow["CMP_DIVISION_NAME"] = DivisionName;
            //#########################################################
            oDS.Tables["CM_DIVISION"].Rows.Add(oOrderRow);
            oOrdersDataAdapter.Update(oDS, "CM_DIVISION");
            dbTransaction.Commit();
            return "Saved Successfully.";
        }
        catch (Exception ex)
        {
            return ex.Message.ToString();
        }

        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }
    }
    public string InsertDistrict(string DistName, string DivId)
    {
        string strSql;
        strSql = "SELECT DISTRI_NAME, CMP_DIVISION_ID  FROM CM_DISTRICTS  ";
        var con = new SqlConnection(cn);
        try
        {
            DataRow oOrderRow;
            con.Open();
            dbTransaction = con.BeginTransaction();
            DataSet oDS = new DataSet();
            SqlDataAdapter oOrdersDataAdapter = new SqlDataAdapter(new SqlCommand(strSql, con, dbTransaction));
            SqlCommandBuilder oOrdersCmdBuilder = new SqlCommandBuilder(oOrdersDataAdapter);
            oOrdersDataAdapter.FillSchema(oDS, SchemaType.Source);
            //######################################################
            DataTable dtSchedule = oDS.Tables["Table"];
            dtSchedule.TableName = "CM_DISTRICTS";
            //######################################################
            oOrderRow = oDS.Tables["CM_DISTRICTS"].NewRow();
            oOrderRow["DISTRI_NAME"] = DistName;
            oOrderRow["CMP_DIVISION_ID"] = DivId;
            //#########################################################
            oDS.Tables["CM_DISTRICTS"].Rows.Add(oOrderRow);
            oOrdersDataAdapter.Update(oDS, "CM_DISTRICTS");
            dbTransaction.Commit();
            return "Saved Successfully.";
        }
        catch (Exception ex)
        {
            return ex.Message.ToString();
        }

        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }
    }
    public string InsertUpazilla(string Upazilla, string DistId)
    {
        string strSql;
        strSql = "SELECT CMP_UPAZILLA_NAME,DISTRI_ID  FROM CM_UPAZILLA  ";
        var con = new SqlConnection(cn);
        try
        {
            DataRow oOrderRow;
            con.Open();
            dbTransaction = con.BeginTransaction();
            DataSet oDS = new DataSet();
            SqlDataAdapter oOrdersDataAdapter = new SqlDataAdapter(new SqlCommand(strSql, con, dbTransaction));
            SqlCommandBuilder oOrdersCmdBuilder = new SqlCommandBuilder(oOrdersDataAdapter);
            oOrdersDataAdapter.FillSchema(oDS, SchemaType.Source);
            //######################################################
            DataTable dtSchedule = oDS.Tables["Table"];
            dtSchedule.TableName = "CM_UPAZILLA";
            //######################################################
            oOrderRow = oDS.Tables["CM_UPAZILLA"].NewRow();
            oOrderRow["CMP_UPAZILLA_NAME"] = Upazilla;
            oOrderRow["DISTRI_ID"] = DistId;
            //#########################################################
            oDS.Tables["CM_UPAZILLA"].Rows.Add(oOrderRow);
            oOrdersDataAdapter.Update(oDS, "CM_UPAZILLA");
            dbTransaction.Commit();
            return "Saved Successfully.";
        }
        catch (Exception ex)
        {
            return ex.Message.ToString();
        }
        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }
    }

    #endregion


    public DataSet LvApplicationDatewise(string empId, string FrmDt, string Branch)
    {
        string strQuery = " SELECT  LVE_FROM_DATE, LVE_TO_DATE,LVE_DURATION,LVE_PURPOSE,LVE_STATUS = CASE LVE_STATUS  "
                           + " WHEN 'A' THEN 'Applied'  WHEN 'A1' THEN '1st Recommended' WHEN 'RC' THEN 'Recommended' WHEN 'R1' THEN 'FIRST Approved' "
                           + " WHEN 'R' THEN 'HR Approved' ELSE 'Cancelled' END "
                           + " FROM PR_LEAVE  WHERE EMP_ID='" + empId + "' AND CMP_BRANCH_ID='" + Branch + "' AND "
                           + " convert(date, CONVERT(DATETIME, CONVERT(VARCHAR(23), LVE_FROM_DATE)))=convert(date, CONVERT(DATETIME, CONVERT(VARCHAR(23), '" + FrmDt + "'))) ";
        try
        {
            con = new SqlConnection(cn);
            con.Open();
            DataSet oDS = new DataSet();
            SqlDataAdapter odaData = new SqlDataAdapter(new SqlCommand(strQuery, con));
            odaData.Fill(oDS, "LVapplication");
            return oDS;
        }
        catch (Exception ex)
        {
            return null;
        }
        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }
    }
    public DataSet OSDAttendanceDatewise(string empId, string FrmDt, string Branch)
    {
        string strQuery = " SELECT   EA.ATT_DATE_TIME,EL.EMP_CODE,LTRIM(CAST(ISNULL(EL.EMP_TITLE, '') AS VARCHAR(100)) + ' ' + CAST(EL.EMP_NAME AS VARCHAR(500)))  AS ENAME, "
                           + " EA.OSD_TIME_IN + ' --> ' + EA.OSD_TIME_OUT  AS ROSTER, EA.ATT_COMMENTS  "
                           + " FROM PR_EMP_ATTENDENCE AS EA  INNER JOIN PR_EMPLOYEE_LIST AS EL ON EA.EMP_ID = EL.EMP_ID INNER JOIN CM_CMP_BRANCH AS CB ON EA.CMP_BRANCH_ID = CB.CMP_BRANCH_ID  "
                           + " WHERE EA.EMP_ID='" + empId + "' AND EA.CMP_BRANCH_ID='" + Branch + "'  AND EA.ACCESS_METHOD ='FD'  AND   EA.ATT_TYPE  = 'OSD'   "
                           + " AND EA.APPROVAL_STATUS='AV'  "
                           + " AND  convert(date, CONVERT(DATETIME, CONVERT(VARCHAR(23), EA.ATT_DATE_TIME)))=convert(date, CONVERT(DATETIME, CONVERT(VARCHAR(23), '" + FrmDt + "'))) ";
        try
        {
            con = new SqlConnection(cn);
            con.Open();
            DataSet oDS = new DataSet();
            SqlDataAdapter odaData = new SqlDataAdapter(new SqlCommand(strQuery, con));
            odaData.Fill(oDS, "OSD");
            return oDS;
        }
        catch (Exception ex)
        {
            return null;
        }
        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }
    }


    public string getOfficeExtension(string Empid, string BranchId) //asad
    {
        string extensionsNum = "";
        try
        {
            string qry = " SELECT OFC_EXTENSION_NUMBER  FROM PR_EMPLOYEE_LIST EL  "
                        + " where EL.OFC_EXTENSION_NUMBER !='' and EL.CMP_BRANCH_ID='" + BranchId + "'  and  EL.EMP_ID='" + Empid + "' ";
            DataBaseClassSql db = new DataBaseClassSql();
            DataTable dt = new DataTable();
            dt = db.ConnectDataBaseReturnDT(qry);
            if (dt.Rows.Count > 0)
            {
                extensionsNum = dt.Rows[0]["OFC_EXTENSION_NUMBER"].ToString();
            }
            if (extensionsNum == "")
            {
                extensionsNum = "No data";
            }

        }
        catch (Exception)
        {


        }
        return extensionsNum;
    }

    public string UpdateExtensions(string extensions, string empId, string branch)
    {
        string updateString;
        updateString = "UPDATE PR_EMPLOYEE_LIST SET OFC_EXTENSION_NUMBER = '" + extensions + "'  WHERE  EMP_ID='" + empId + "' and CMP_BRANCH_ID='" + branch + "' ";
        string strReturn = "Successfully data inserted";
        try
        {
            con = new SqlConnection(cn);
            con.Open();
            dbTransaction = con.BeginTransaction();
            SqlCommand cmd = new SqlCommand(updateString, con, dbTransaction);
            cmd.Connection = con;
            cmd.ExecuteNonQuery();
            dbTransaction.Commit();
            // con.Close();
        }
        catch (Exception ex)
        {
            strReturn = "Something wrong...";
        }

        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }
        return strReturn;
    }

    //################################ Read Excel file #####################################

    public DataTable Import_To_Grid(string FilePath, string Extension, string isHDR)
    {
        DataTable dt = new DataTable();
        try
        {
            string conStr = "";
            switch (Extension)
            {
                case ".xls": //Excel 97-03

                    //conStr = ConfigurationManager.ConnectionStrings["Excel03ConString"].ConnectionString;
                    conStr = "Provider=Microsoft.Jet.OLEDB.4.0; Data Source=" + FilePath + "; Extended Properties=\"Excel 8.0; HDR=Yes\";";
                    break;

                case ".xlsx": //Excel 07
                    //conStr = ConfigurationManager.ConnectionStrings["Excel07ConString"].ConnectionString;
                    conStr = "Provider=Microsoft.ACE.OLEDB.12.0; Data Source=" + FilePath + "; Extended Properties=\"Excel 12.0; HDR=YES\";";
                    break;

            }
            conStr = String.Format(conStr, FilePath, isHDR);
            OleDbConnection connExcel = new OleDbConnection(conStr);
            OleDbCommand cmdExcel = new OleDbCommand();
            OleDbDataAdapter oda = new OleDbDataAdapter();
            cmdExcel.Connection = connExcel;
            //Get the name of First Sheet
            connExcel.Open();
            DataTable dtExcelSchema;
            dtExcelSchema = connExcel.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
            string SheetName = dtExcelSchema.Rows[0]["TABLE_NAME"].ToString();
            connExcel.Close();
            //Read Data from First Sheet
            connExcel.Open();
            cmdExcel.CommandText = "SELECT * From [" + SheetName + "]";
            oda.SelectCommand = cmdExcel;
            oda.Fill(dt);
            connExcel.Close();
        }
        catch (Exception)
        {
        }
        return dt;
    }


    public string InsertSalaryInformation(params string[] strParam)
    {
        string strSql;
        strSql = "  SELECT [EMP_CODE],[Year],[Month],[Basic],[House_rent],[Mediacal_ allowance],[Convey_Allow],[Ent_ Allow],[Charge_ Allow] "
                  + " ,[Tele],[Special Allow] ,[Dearn Allow],[Other Allow],[Incentive Bonus],[Com PF Con],[Com Tax],[Gross Salary],[Arrear Salary] "
                  + " ,[Co'sLib  PF/WF],[Emp  PF Con],[Arrear  PF/WF],[PF/WF Loan],[EPS TML (Mobile) ],[EPS  TEL (Electronics)],[Advance Long] "
                  + " ,[Advance Short],[Others Deduction],[Salary (AIT)],[Net Salary/ Bank Transfer] "
                  + " FROM [PR_SalaryInformationSheet] ";
        var con = new SqlConnection(cn);
        try
        {
            DataRow oOrderRow;
            con.Open();
            dbTransaction = con.BeginTransaction();
            DataSet oDS = new DataSet();
            SqlDataAdapter oOrdersDataAdapter = new SqlDataAdapter(new SqlCommand(strSql, con, dbTransaction));
            SqlCommandBuilder oOrdersCmdBuilder = new SqlCommandBuilder(oOrdersDataAdapter);
            oOrdersDataAdapter.FillSchema(oDS, SchemaType.Source);
            //######################################################
            DataTable dtSchedule = oDS.Tables["Table"];
            dtSchedule.TableName = "PR_SalaryInformationSheet";
            //######################################################
            oOrderRow = oDS.Tables["PR_SalaryInformationSheet"].NewRow();
            oOrderRow[0] = strParam[0].ToString();
            oOrderRow[1] = strParam[1].ToString();
            oOrderRow[2] = strParam[2].ToString();
            oOrderRow[3] = Convert.ToDecimal(strParam[3].ToString());
            oOrderRow[4] = Convert.ToDecimal(strParam[4].ToString());
            oOrderRow[5] = Convert.ToDecimal(strParam[5].ToString());
            oOrderRow[6] = Convert.ToDecimal(strParam[6].ToString());
            oOrderRow[7] = Convert.ToDecimal(strParam[7].ToString());
            oOrderRow[8] = Convert.ToDecimal(strParam[8].ToString());
            oOrderRow[9] = Convert.ToDecimal(strParam[9].ToString());
            oOrderRow[10] = Convert.ToDecimal(strParam[10].ToString());
            oOrderRow[11] = Convert.ToDecimal(strParam[11].ToString());
            oOrderRow[12] = Convert.ToDecimal(strParam[12].ToString());
            oOrderRow[13] = Convert.ToDecimal(strParam[13].ToString());
            oOrderRow[14] = Convert.ToDecimal(strParam[14].ToString());
            oOrderRow[15] = Convert.ToDecimal(strParam[15].ToString());
            oOrderRow[16] = Convert.ToDecimal(strParam[16].ToString());
            oOrderRow[17] = Convert.ToDecimal(strParam[17].ToString());
            oOrderRow[18] = Convert.ToDecimal(strParam[18].ToString());
            oOrderRow[19] = Convert.ToDecimal(strParam[19].ToString());
            oOrderRow[20] = Convert.ToDecimal(strParam[20].ToString());
            oOrderRow[21] = Convert.ToDecimal(strParam[21].ToString());
            oOrderRow[22] = Convert.ToDecimal(strParam[22].ToString());
            oOrderRow[23] = Convert.ToDecimal(strParam[23].ToString());
            oOrderRow[24] = Convert.ToDecimal(strParam[24].ToString());
            oOrderRow[25] = Convert.ToDecimal(strParam[25].ToString());
            oOrderRow[26] = Convert.ToDecimal(strParam[26].ToString());
            oOrderRow[27] = Convert.ToDecimal(strParam[27].ToString());
            oOrderRow[28] = Convert.ToDecimal(strParam[28].ToString());
            //#########################################################
            oDS.Tables["PR_SalaryInformationSheet"].Rows.Add(oOrderRow);
            oOrdersDataAdapter.Update(oDS, "PR_SalaryInformationSheet");
            dbTransaction.Commit();
            return "Saved Successfully.";
        }
        catch (Exception ex)
        {
            return ex.Message.ToString();
        }

        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }
    }

    public DataSet GetSalaryInfo(string strSql)
    {
        DataSet oDS = new DataSet();
        try
        {
            con = new SqlConnection(cn);
            con.Open();
            SqlDataAdapter odaData = new SqlDataAdapter(new SqlCommand(strSql, con));
            odaData.Fill(oDS, "PR_SalaryInformationSheet");
        }
        catch (Exception ex)
        {

        }
        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }
        return oDS;
    }
    public bool ChkExistingSalaryInfo(string empcode, string yr, string mn)
    {
        DataSet oDS = new DataSet();
        bool exist = false;
        try
        {
            string strSql = " SELECT  DISTINCT *  FROM  PR_SalaryInformationSheet PS "
                           + " WHERE UPPER(LTRIM(RTRIM([EMP_CODE])))=UPPER('" + empcode + "') AND  UPPER(LTRIM(RTRIM([Year])))=UPPER('" + yr + "') AND UPPER(LTRIM(RTRIM([Month])))=UPPER('" + mn + "') ";
            con = new SqlConnection(cn);
            con.Open();
            SqlDataAdapter odaData = new SqlDataAdapter(new SqlCommand(strSql, con));
            odaData.Fill(oDS, "PR_SalaryInformationSheet");
            if (oDS.Tables[0].Rows.Count > 0)
            {
                exist = true;
            }
        }
        catch (Exception ex)
        {
        }
        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }
        return exist;
    }

    public DataSet ChkDeptHead(string empId)
    {
        DataSet oDS = new DataSet();
        string strSql = "SELECT  HEAD_OF_DEPT FROM PR_EMPLOYEE_LIST WHERE EMP_ID='" + empId + "' ";
        try
        {
            con = new SqlConnection(cn);
            con.Open();
            SqlDataAdapter odaData = new SqlDataAdapter(new SqlCommand(strSql, con));
            odaData.Fill(oDS, "PR_EMPLOYEE_LIST");
        }
        catch (Exception ex)
        {

        }

        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }
        return oDS;
    }
    // Time creation
    public void BindTime(DropDownList ddl)  //asad
    {
        // Set the start time (00:00 means 12:00 AM)
        DateTime StartTime = DateTime.ParseExact("00:00", "HH:mm", null);
        // Set the end time (23:55 means 11:55 PM)
        DateTime EndTime = DateTime.ParseExact("23:55", "HH:mm", null);
        //Set 5 minutes interval
        TimeSpan Interval = new TimeSpan(0, 5, 0); // Hr, min, seconds
        ddl.Items.Clear();
        while (StartTime <= EndTime)
        {
            ddl.Items.Add(StartTime.ToShortTimeString());
            StartTime = StartTime.Add(Interval);
        }
        ddl.Items.Insert(0, new ListItem("--Time--", "0"));
    }

    public void BindRoster(DropDownList ddl)  //Polash
    {

        string strQuery = "select * from PR_ROSTER_SETUP ";
        string TypeName = "";
        try
        {
            con = new SqlConnection(cn);
            con.Open();
            DataSet oDS = new DataSet();
            SqlDataAdapter odaData = new SqlDataAdapter(new SqlCommand(strQuery, con));
            DataTable dt = new DataTable();
            odaData.Fill(dt);
            ddl.DataTextField = "PRR_TYPE";
            ddl.DataValueField = "PRR_ID";
            ddl.DataSource = dt;
            ddl.DataBind();
        }
        catch (Exception ex)
        {
           
        }
        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }
        
    }

    // get applicationType
    public string GetApplicationType(string appTypeID)
    {
        string strQuery = "select RE_NAME "
                            + "from HR_APP_REPORT_TYPE  "
                            + "where RE_TYPE_ID='" + appTypeID + "'  ";
        string TypeName = "";
        try
        {
            con = new SqlConnection(cn);
            con.Open();
            DataSet oDS = new DataSet();
            SqlDataAdapter odaData = new SqlDataAdapter(new SqlCommand(strQuery, con));
            odaData.Fill(oDS, "HR_APP_REPORT_TYPE");
            if (oDS.Tables[0].Rows.Count > 0)
            {
                TypeName = oDS.Tables[0].Rows[0]["RE_NAME"].ToString();
            }
        }
        catch (Exception ex)
        {
            return null;
        }
        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }
        return TypeName;
    }

    public DataSet GetEmp(string empCode)
    {
        string strQuery = "SELECT EL.EMP_ID FROM PR_EMPLOYEE_LIST EL WHERE EL.EMP_CODE='" + empCode + "' ";
        var con = new SqlConnection(cn);
        try
        {
            DataSet oDS = new DataSet();
            SqlDataAdapter odaData = new SqlDataAdapter(new SqlCommand(strQuery, con));
            odaData.Fill(oDS, "EmployeeList");
            return oDS;
        }
        catch (Exception ex)
        {
            return null;
        }
        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }
    }

    public bool checkRosterManager(string empId)
    {
        bool RosterManager = false;
        string strQuery = "SELECT EL.EMP_ROSTER_MANAGER  FROM PR_EMPLOYEE_LIST EL WHERE EL.EMP_ID='" + empId + "' ";
        var con = new SqlConnection(cn);
        try
        {
            DataSet oDS = new DataSet();
            SqlDataAdapter odaData = new SqlDataAdapter(new SqlCommand(strQuery, con));
            odaData.Fill(oDS, "EmployeeList");
            if (oDS.Tables[0].Rows.Count > 0)
            {
                string RosterM = oDS.Tables[0].Rows[0]["EMP_ROSTER_MANAGER"].ToString();
                if (RosterM == "Y")
                {
                    RosterManager = true;
                }
            }
        }
        catch (Exception ex)
        {
            return false;
        }
        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }
        return RosterManager;
    }

    public DataSet GetAttData(string brid, string empId, string attType, string attDate, string attFor)
    {

        string strQuery = "SELECT  COUNT(*)Total FROM   PR_EMP_ATTENDENCE AT  WHERE "
                          + " convert(date, CONVERT(DATETIME, CONVERT(VARCHAR(23), AT.ATT_DATE_TIME)))  "
                          + " = convert(date, CONVERT(DATETIME, CONVERT(VARCHAR(23), '" + attDate + "'))) "
                          + " AND AT.EMP_ID='" + empId + "' AND AT.ATT_TYPE='" + attType + "'  and at.PREV_OUT_FLAG='" + attFor + "' AND CMP_BRANCH_ID ='" + brid + "' ";
        var con = new SqlConnection(cn);
        try
        {
            DataSet oDS = new DataSet();
            SqlDataAdapter odaData = new SqlDataAdapter(new SqlCommand(strQuery, con));
            odaData.Fill(oDS, "EmployeeList");
            return oDS;
        }
        catch (Exception ex)
        {
            return null;
        }
        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }
    }

    public string InsertAttendanceManually(string branch, string atDate, string EmpId, string Atttype, string AttFor, string AttLateStatus, string AttLateDeduct, string LateTime, string RosterType, string Modified, string comments)
    {
        string strSql;
        strSql = "SELECT AT.ATT_DATE_TIME,AT.ACCESS_METHOD,AT.APPLY_LATE_DEDUCT,AT.ATT_LATE_STATUS,ATT_LATE_TIME, "
                + "AT.ATT_TYPE,AT.CMP_BRANCH_ID,AT.EMP_ID,PREV_OUT_FLAG,ROSTER_TIME,Attendance_Modified,AT.ATT_COMMENTS FROM PR_EMP_ATTENDENCE AT ";

        var con = new SqlConnection(cn);
        try
        {
            DataRow oOrderRow;
            con.Open();
            dbTransaction = con.BeginTransaction();
            DataSet oDS = new DataSet();
            SqlDataAdapter oOrdersDataAdapter = new SqlDataAdapter(new SqlCommand(strSql, con, dbTransaction));
            SqlCommandBuilder oOrdersCmdBuilder = new SqlCommandBuilder(oOrdersDataAdapter);
            oOrdersDataAdapter.FillSchema(oDS, SchemaType.Source);
            //######################################################
            DataTable dtSchedule = oDS.Tables["Table"];
            dtSchedule.TableName = "PR_EMP_ATTENDENCE";
            //######################################################
            oOrderRow = oDS.Tables["PR_EMP_ATTENDENCE"].NewRow();

            oOrderRow["EMP_ID"] = EmpId;
            oOrderRow["ATT_TYPE"] = Atttype;
            oOrderRow["ATT_DATE_TIME"] = atDate;

            oOrderRow["ATT_LATE_TIME"] = LateTime;
            oOrderRow["APPLY_LATE_DEDUCT"] = AttLateDeduct;
            oOrderRow["ATT_LATE_STATUS"] = AttLateStatus;

            oOrderRow["ATT_COMMENTS"] = comments;
            oOrderRow["ACCESS_METHOD"] = "FP";
            oOrderRow["CMP_BRANCH_ID"] = branch;
            oOrderRow["ROSTER_TIME"] = RosterType;
            oOrderRow["PREV_OUT_FLAG"] = AttFor;
            oOrderRow["Attendance_Modified"] = Modified;
            //#########################################################
            oDS.Tables["PR_EMP_ATTENDENCE"].Rows.Add(oOrderRow);
            oOrdersDataAdapter.Update(oDS, "PR_EMP_ATTENDENCE");
            dbTransaction.Commit();
            return "Saved Successfully.";
        }
        catch (Exception ex)
        {
            return ex.Message.ToString();
        }
        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }
    }

    public string ExecuteProcedure(string Proc, string param1, string param2, string param3) /*  Md. Asaduzzaman Dated:12-Mar-2015       */
    {
        var con = new SqlConnection(cn);
        string strReturn = "";
        try
        {
            con.Open();
            if (Proc != "")
            {
                SqlCommand mycmd = new SqlCommand(Proc, con);
                mycmd.CommandTimeout = 0;
                mycmd.CommandType = CommandType.StoredProcedure;
                mycmd.Parameters.Add("@Date_From", SqlDbType.DateTime).Value = param1.Trim();
                mycmd.Parameters.Add("@Branch", SqlDbType.VarChar).Value = param2.Trim();
                if (param3.Trim() != "")
                {
                    mycmd.Parameters.Add("@EmpCode", SqlDbType.VarChar).Value = param3;
                }
                mycmd.ExecuteNonQuery();
            }
        }
        catch (Exception ex)
        {
            strReturn = ex.Message.ToString();
        }
        finally
        {
            con.Close();
            con = null;
        }
        return strReturn;
    }


    public string InsertGratuitySetup(string typecode, string reqMin, string reqMax, string graMin, string graMax, string considerMonth, string considerDays)
    {
        string strSql;
        strSql = "SELECT TYP_CODE ,GRA_MIN_REQ_YR_MIN ,GRA_MIN_REQ_YR_MAX ,GRA_MIN_COUNT ,GRA_MAX_COUNT ,GRA_MONTH_CONSIDER ,GRA_DAYS_CONSIDER  FROM HR_EMP_GRATUITY_SETUP  ";
        var con = new SqlConnection(cn);
        try
        {
            DataRow oOrderRow;
            con.Open();
            dbTransaction = con.BeginTransaction();
            DataSet oDS = new DataSet();
            SqlDataAdapter oOrdersDataAdapter = new SqlDataAdapter(new SqlCommand(strSql, con, dbTransaction));
            SqlCommandBuilder oOrdersCmdBuilder = new SqlCommandBuilder(oOrdersDataAdapter);
            oOrdersDataAdapter.FillSchema(oDS, SchemaType.Source);
            //######################################################
            DataTable dtSchedule = oDS.Tables["Table"];
            dtSchedule.TableName = "HR_EMP_GRATUITY_SETUP";
            //######################################################
            oOrderRow = oDS.Tables["HR_EMP_GRATUITY_SETUP"].NewRow();
            oOrderRow["TYP_CODE"] = typecode;
            oOrderRow["GRA_MIN_REQ_YR_MIN"] = reqMin.Trim();
            oOrderRow["GRA_MIN_REQ_YR_MAX"] = reqMax.Trim();
            oOrderRow["GRA_MIN_COUNT"] = graMin.Trim();
            oOrderRow["GRA_MAX_COUNT"] = graMax.Trim();
            oOrderRow["GRA_MONTH_CONSIDER"] = considerMonth.Trim();
            oOrderRow["GRA_DAYS_CONSIDER"] = considerDays.Trim();
            //#########################################################
            oDS.Tables["HR_EMP_GRATUITY_SETUP"].Rows.Add(oOrderRow);
            oOrdersDataAdapter.Update(oDS, "HR_EMP_GRATUITY_SETUP");
            dbTransaction.Commit();
            return "";
        }
        catch (Exception ex)
        {
            return ex.Message.ToString();
        }
        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }
    }


    public void DisableControls(System.Web.UI.Control control) /*  Disabled all Controls    Md. Asaduzzaman       */
    {
        foreach (System.Web.UI.Control c in control.Controls)
        {
            // Get the Enabled property by reflection.
            Type type = c.GetType();
            PropertyInfo prop = type.GetProperty("Enabled");

            // Set it to False to disable the control.
            if (prop != null)
            {
                prop.SetValue(c, false, null);
            }

            // Recurse into child controls.
            if (c.Controls.Count > 0)
            {
                this.DisableControls(c);
            }
        }
    }

    public bool CheckCurrentYrDt(string bId, string bIdAll, string dateCheck)                              /* asad */
    {
        bool allow = false;
        string strQuery = "SELECT COUNT(*)TOT  FROM HR_YEAR WHERE YR_STATUS in ('R','TC') and CMP_BRANCH_ID in ('" + bId + "', '" + bIdAll + "')  "
                         + " AND  convert(date, CONVERT(DATETIME, CONVERT(VARCHAR(23), '" + dateCheck + "')))   "
                         + " BETWEEN                                                                      "
                         + " convert(date, CONVERT(DATETIME, CONVERT(VARCHAR(23), YR_STARAT_DATE)))       "
                         + " AND convert(date, CONVERT(DATETIME, CONVERT(VARCHAR(23), YR_END_DATE)))      ";

        try
        {
            con = new SqlConnection(cn);
            con.Open();
            DataSet oDS = new DataSet();
            SqlDataAdapter odaData = new SqlDataAdapter(new SqlCommand(strQuery, con));
            odaData.Fill(oDS, "HR_YEAR");
            if (oDS.Tables[0].Rows.Count > 0)
            {
                if (int.Parse(oDS.Tables[0].Rows[0]["TOT"].ToString()) > 0)
                {
                    allow = true;
                }
            }
        }
        catch (Exception ex)
        {
            return false;
        }
        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }
        return allow;
    }

    //Added by Rokon; Date : 11-December, 2017
    public string GetEmployeeId(string strEmpCode)
    {
        string strAccountId = "";
        string strSql = " SELECT EMP_ID FROM PR_EMPLOYEE_LIST WHERE EMP_CODE = '" + strEmpCode + "' ";
        SqlConnection conn = new SqlConnection(cn);
        conn.Open();

        try
        {
            SqlCommand cmd = new SqlCommand(strSql, conn);
            SqlDataReader dr;
            dr = cmd.ExecuteReader();
            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    strAccountId = dr["EMP_ID"].ToString();
                }
                return strAccountId;
            }
            else
            {
                return strAccountId;

            }
            conn.Close();
        }
        catch (Exception ex)
        {
            return ex.Message.ToString();
            conn.Close();
        }
    }
    //---03/06/2018
    public DataSet GetEmpNameDesg(string empId)
    {
        string strSQL = "SELECT EL.EMP_ID,EL.EMP_NAME,D.DSG_TITLE FROM PR_EMPLOYEE_LIST EL, PR_DESIGNATION D WHERE EL.DSG_ID=D.DSG_ID AND  EMP_ID in('" + empId + "')";

        try
        {
            con = new SqlConnection(cn);
            con.Open();
            DataSet oDS = new DataSet();
            SqlDataAdapter odaData = new SqlDataAdapter(new SqlCommand(strSQL, con));
            odaData.Fill(oDS, "PR_DESIGNATION");
            return oDS;
        }
        catch (Exception ex)
        {
            return null;
        }
        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }
    }
    public DataSet GetRosterInfo(string RosterDate, string Branch, string EmpId) /* ASAD  +' ['+PR.PRR_TIME_FROM + '-->' + PR.PRR_TIME_TO+']'  */
    {
        string strQuery = " SELECT DISTINCT PR.DAY_MAX_HOUR, PR.PRR_ID,PR.PRR_TYPE AS ROSTERNAME,PR.PRR_TIME_FROM,PR.PRR_TIME_TO , LTRIM(CAST(ISNULL(EL.EMP_TITLE, '') AS VARCHAR(100)) + ' ' + CAST(EL.EMP_NAME AS VARCHAR(500))) + ' [' + EL.EMP_CODE + ']' AS ENAME "
        + " FROM PR_EMP_ROSTER ER, PR_ROSTER_SETUP PR,PR_EMPLOYEE_LIST EL  "
        + " WHERE PR.PRR_ID=ER.PRR_ID AND EL.EMP_ID=ER.EMP_ID  "
        + " AND ER.EMP_ID= '" + EmpId + "' AND ER.CMP_BRANCH_ID='" + Branch + "'  "
        + " AND convert(date, CONVERT(DATETIME, CONVERT(VARCHAR(23), ER.ROSTER_DATE))) = convert(date, CONVERT(DATETIME, CONVERT(VARCHAR(23), '" + RosterDate + "'))) ";
        var con = new SqlConnection(cn);
        try
        {
            DataSet oDS = new DataSet();
            SqlDataAdapter odaData = new SqlDataAdapter(new SqlCommand(strQuery, con));
            odaData.Fill(oDS, "RosterRpt");
            return oDS;
        }
        catch (Exception ex)
        {
            return null;
        }
        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }
    }

    public DataSet GetTotalHoliday(string frmDate, string toDate)
    {
        string strQuery = "select PRH_FROM_DATE,PRH_TO_DATE FROM  PR_HOLIDAY_SETUP where PRH_FROM_DATE >= cast('" + frmDate + "' as date)  and PRH_TO_DATE <= cast('" + toDate + "' as date) ";
        var con = new SqlConnection(cn);
        try
        {
            DataSet oDS = new DataSet();
            SqlDataAdapter odaData = new SqlDataAdapter(new SqlCommand(strQuery, con));
            odaData.Fill(oDS, "RosterRpt");
            return oDS;
        }
        catch (Exception ex)
        {
            return null;
        }
        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }
    }

    public DataSet GetWeekRoasterDate(string empId, string roasterId, string frmDate, string toDate)
    {
        string strQuery = "select * from   PR_EMP_ROSTER where ROSTER_DATE between cast('" + frmDate + "' as date)  and cast('" + toDate + "' as date) and EMP_ID='" + empId + "' and  PRR_ID='" + roasterId + "'";
        var con = new SqlConnection(cn);
        try
        {
            DataSet oDS = new DataSet();
            SqlDataAdapter odaData = new SqlDataAdapter(new SqlCommand(strQuery, con));
            odaData.Fill(oDS, "RosterRpt");
            return oDS;
        }
        catch (Exception ex)
        {
            return null;
        }
        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }
    }

    public DataSet GetEmployeeAccademicCredensial(string empId)
    {
        string strQuery = @" 
select (ACQ_LEVEL_OF_EDUCATION +' : '+ ACQ_CONCENTRATION +'- '+ACQ_RESULT+'- '+ACQ_INSTITUTE_NAME) as AccademicCredensial from [dbo].[HR_EMP_JOB_ACADEMIC_QUA]  "
                              + " where  "
                              + "  EMP_ID='" + empId + "'  order by ACQ_ORDER desc  ";
        try
        {
            con = new SqlConnection(cn);
            con.Open();
            DataSet oDS = new DataSet();
            SqlDataAdapter odaData = new SqlDataAdapter(new SqlCommand(strQuery, con));
            odaData.Fill(oDS, "attBasic");
            return oDS;
        }
        catch (Exception ex)
        {
            return null;
        }
        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }
    }


    public DataSet GetEmployeeAccademicDistinction(string empId)
    {
        string strQuery = @" 
select (ACC_DIST_DESCRIPTION +'  '+ ACC_DIST_DATE ) as ACC_DIST_DESCRIPTION from ACCADEMIC_DISTINCTION  "
                              + " where  "
                              + "  EMP_ID='" + empId + "' ";
        try
        {
            con = new SqlConnection(cn);
            con.Open();
            DataSet oDS = new DataSet();
            SqlDataAdapter odaData = new SqlDataAdapter(new SqlCommand(strQuery, con));
            odaData.Fill(oDS, "ACCADEMIC_DISTINCTION");
            return oDS;
        }
        catch (Exception ex)
        {
            return null;
        }
        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }
    }

    public DataSet GetEmployeePastExperianceDetails(string empId)
    {
        string strQuery = @" select (isnull(EMH_COMPANY_NAME,'') +' : '+ isnull( EMP_DEPARTMENT,'') +'- ( '+convert(nvarchar, EMH_FROM_DATE,107)+'- '+convert(nvarchar,EMH_TO_DATE,107)+' )')  as PastExperianceDetails from HR_EMP_JOB_EMPLOYMENT_HISTORY "
                              + " where  "
                              + "  EMP_ID='" + empId + "'  ";
        try
        {
            con = new SqlConnection(cn);
            con.Open();
            DataSet oDS = new DataSet();
            SqlDataAdapter odaData = new SqlDataAdapter(new SqlCommand(strQuery, con));
            odaData.Fill(oDS, "attBasic");
            return oDS;
        }
        catch (Exception ex)
        {
            return null;
        }
        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }
    }

    public DataSet GetEmployeeCarrerStartDate(string empId)
    {
        string strQuery = @" select convert(nvarchar, min(EMH_FROM_DATE),107)  as CareerStartDate from HR_EMP_JOB_EMPLOYMENT_HISTORY "
                              + " where  "
                              + "  EMP_ID='" + empId + "'  ";
        try
        {
            con = new SqlConnection(cn);
            con.Open();
            DataSet oDS = new DataSet();
            SqlDataAdapter odaData = new SqlDataAdapter(new SqlCommand(strQuery, con));
            odaData.Fill(oDS, "attBasic");
            return oDS;
        }
        catch (Exception ex)
        {
            return null;
        }
        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }
    }

    public DataSet GetEmployeeRewardList(string empId)
    {
        string strQuery = @"select REWARD_DATE +' - '+REWARD_DETAILS as Reward from [dbo].[HR_EMP_REWARD_LIST] "
                              + " where  "
                              + "  EMP_ID='" + empId + "'  ";
        try
        {
            con = new SqlConnection(cn);
            con.Open();
            DataSet oDS = new DataSet();
            SqlDataAdapter odaData = new SqlDataAdapter(new SqlCommand(strQuery, con));
            odaData.Fill(oDS, "HR_EMP_REWARD_LIST");
            return oDS;
        }
        catch (Exception ex)
        {
            return null;
        }
        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }
    }


    public DataSet GetEmployeeForeignTour(string empId)
    {
        string strQuery = @"select (TOUR_COUNTRY +' - '+TOUR_DETAILS +'('+TOUR_FROM_DATE + ' to '+ TOUR_TO_DATE +')') as OfficialTour from EMP_OFFICIAL_TOUR "
                              + " where  "
                              + "  EMP_ID='" + empId + "'  ";
        try
        {
            con = new SqlConnection(cn);
            con.Open();
            DataSet oDS = new DataSet();
            SqlDataAdapter odaData = new SqlDataAdapter(new SqlCommand(strQuery, con));
            odaData.Fill(oDS, "EMP_OFFICIAL_TOUR");
            return oDS;
        }
        catch (Exception ex)
        {
            return null;
        }
        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }
    }

    public DataSet GetEmployeeDisciplinaryAction(string empId)
    {
        string strQuery = @"select (DISCIPLINARY_DATE +' - '+DISCIPLINARY_DETAILS ) as DISCIPLINARY_DETAILS from HR_DISCIPLINARY_ACTION "
                              + " where  "
                              + "  EMP_ID='" + empId + "'  ";
        try
        {
            con = new SqlConnection(cn);
            con.Open();
            DataSet oDS = new DataSet();
            SqlDataAdapter odaData = new SqlDataAdapter(new SqlCommand(strQuery, con));
            odaData.Fill(oDS, "HR_DISCIPLINARY_ACTION");
            return oDS;
        }
        catch (Exception ex)
        {
            return null;
        }
        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }
    }


    public DataSet GetEmployeeBasic(string empCode)
    {
        string strQuery = @"select * from PR_SalaryInformationSheet as pp where pp.Year=year(GETDATE()) and pp.Month=DATENAME(month, GETDATE()) "
                              + " and  "
                              + "  PP.EMP_CODE='" + empCode + "'  ";
        try
        {
            con = new SqlConnection(cn);
            con.Open();
            DataSet oDS = new DataSet();
            SqlDataAdapter odaData = new SqlDataAdapter(new SqlCommand(strQuery, con));
            odaData.Fill(oDS, "PR_SalaryInformationSheet");
            return oDS;
        }
        catch (Exception ex)
        {
            return null;
        }
        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }
    }

    public DataSet GetEmployeeProfessionalQua(string empId)
    {
        string strQuery = @" select (isnull(PRQ_CERTIFICATION,'') +' : '+ isnull( PRQ_INSTITUTE,'')+'- ' + ISNULL( PRQ_LOCATION,'') +'- ( '+convert(nvarchar, PRQ_FROM_DATE,107)+'  to  '+convert(nvarchar,PRQ_TO_DATE,107)+' )')  as ProfessionalQualification from HR_EMP_JOB_PROFESSIONAL_QUA "
                              + " where  "
                              + "  EMP_ID='" + empId + "'  ";
        try
        {
            con = new SqlConnection(cn);
            con.Open();
            DataSet oDS = new DataSet();
            SqlDataAdapter odaData = new SqlDataAdapter(new SqlCommand(strQuery, con));
            odaData.Fill(oDS, "attBasic");
            return oDS;
        }
        catch (Exception ex)
        {
            return null;
        }
        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }
    }

    public string totalLeave(string empId, string frmDate, string toDate)
    {

        string strQuery = "select SUM(LVE_APPROVED_DAY) as totalLeave from PR_LEAVE where LVE_STATUS in ('R1','R') and  EMP_ID='" + empId + "' "
        + " and convert(date, CONVERT(DATETIME, CONVERT(VARCHAR(23), LVE_APPROVED_DATE))) between convert(date, CONVERT(DATETIME, CONVERT(VARCHAR(23), '" + frmDate + "'))) "
        + " and convert(date, CONVERT(DATETIME, CONVERT(VARCHAR(23), '" + toDate + "'))) ";
        DataSet oDS = new DataSet();
        string result = "";
        try
        {
            con = new SqlConnection(cn);
            con.Open();

            SqlDataAdapter odaData = new SqlDataAdapter(new SqlCommand(strQuery, con));
            odaData.Fill(oDS, "PR_LEAVE");
            if (oDS.Tables[0].Rows.Count > 0)
            {
                result = oDS.Tables[0].Rows[0]["totalLeave"].ToString();
            }
        }
        catch (Exception ex)
        {
            return null;
        }

        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }

        return result;
    }
    public string totalOSD(string empId, string frmDate, string toDate)
    {

        string strQuery = "SELECT count(*) as totalOSD FROM PR_EMP_ATTENDENCE WHERE ATT_TYPE='OSD' AND  EMP_ID='" + empId + "' "
        + " and convert(date, CONVERT(DATETIME, CONVERT(VARCHAR(23), ATT_DATE_TIME))) between convert(date, CONVERT(DATETIME, CONVERT(VARCHAR(23), '" + frmDate + "'))) "
        + " and convert(date, CONVERT(DATETIME, CONVERT(VARCHAR(23), '" + toDate + "'))) ";
        DataSet oDS = new DataSet();
        string result = "";
        try
        {
            con = new SqlConnection(cn);
            con.Open();

            SqlDataAdapter odaData = new SqlDataAdapter(new SqlCommand(strQuery, con));
            odaData.Fill(oDS, "PR_EMP_ATTENDENCE");
            if (oDS.Tables[0].Rows.Count > 0)
            {
                result = oDS.Tables[0].Rows[0]["totalOSD"].ToString();
            }
        }
        catch (Exception ex)
        {
            return null;
        }

        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }

        return result;
    }
    public string totalLate(string empId, string frmDate, string toDate)
    {

        string strQuery = "SELECT count(*) as totalLATE FROM PR_EMP_ATTENDENCE WHERE ATT_LATE_STATUS='YES' AND  EMP_ID='" + empId + "' "
        + " and convert(date, CONVERT(DATETIME, CONVERT(VARCHAR(23), ATT_DATE_TIME))) between convert(date, CONVERT(DATETIME, CONVERT(VARCHAR(23), '" + frmDate + "'))) "
        + " and convert(date, CONVERT(DATETIME, CONVERT(VARCHAR(23), '" + toDate + "'))) ";
        DataSet oDS = new DataSet();
        string result = "";
        try
        {
            con = new SqlConnection(cn);
            con.Open();

            SqlDataAdapter odaData = new SqlDataAdapter(new SqlCommand(strQuery, con));
            odaData.Fill(oDS, "PR_EMP_ATTENDENCE");
            if (oDS.Tables[0].Rows.Count > 0)
            {
                result = oDS.Tables[0].Rows[0]["totalLATE"].ToString();
            }
        }
        catch (Exception ex)
        {
            return null;
        }

        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }

        return result;
    }
    public string totalPresent(string empId, string frmInDate, string toInDate)
    {

        string strQuery = "SELECT count(*) as totalLATE FROM PR_EMP_ATTENDENCE WHERE att_type='IN' AND  EMP_ID='" + empId + "' "
      + " and convert(date, CONVERT(DATETIME, CONVERT(VARCHAR(23), ATT_DATE_TIME))) between convert(date, CONVERT(DATETIME, CONVERT(VARCHAR(23), '" + frmInDate + "'))) "
      + " and convert(date, CONVERT(DATETIME, CONVERT(VARCHAR(23), '" + toInDate + "'))) ";
        DataSet oDS = new DataSet();
        string result = "";
        try
        {
            con = new SqlConnection(cn);
            con.Open();

            SqlDataAdapter odaData = new SqlDataAdapter(new SqlCommand(strQuery, con));
            odaData.Fill(oDS, "PR_EMP_ATTENDENCE");
            if (oDS.Tables[0].Rows.Count > 0)
            {
                result = oDS.Tables[0].Rows[0]["totalIn"].ToString();
            }
        }
        catch (Exception ex)
        {
            return null;
        }

        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }

        return result;
    }

    public string totalIn(string empId, DateTime frmInDate, DateTime toInDate)
    {

        string strQuery = "select count(*) as totalIn from [PR_EMP_ATTENDENCE]  where att_type='IN' and emp_id='" + empId + "'  "
            + "and cast(ATT_DATE_TIME as date) between cast('" + frmInDate + "' as date) and  cast('" + toInDate + "' as date) "
            + "and cast(ATT_DATE_TIME as time) between cast('" + frmInDate + "' as time) and  cast('" + toInDate + "' as time) ";
        DataSet oDS = new DataSet();
        string result = "";
        try
        {
            con = new SqlConnection(cn);
            con.Open();

            SqlDataAdapter odaData = new SqlDataAdapter(new SqlCommand(strQuery, con));
            odaData.Fill(oDS, "PR_EMP_ATTENDENCE");
            if (oDS.Tables[0].Rows.Count > 0)
            {
                result = oDS.Tables[0].Rows[0]["totalIn"].ToString();
            }
        }
        catch (Exception ex)
        {
            return null;
        }

        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }

        return result;
    }
    public string totalOut(string empId, DateTime frmInDate, DateTime toInDate)
    {
        string strQuery = "select count(*) as totalIn from [PR_EMP_ATTENDENCE]  where att_type='OUT' and emp_id='" + empId + "'  "
           + "and cast(ATT_DATE_TIME as date) between cast('" + frmInDate + "' as date) and  cast('" + toInDate + "' as date) "
           + "and cast(ATT_DATE_TIME as time) between cast('" + frmInDate + "' as time) and  cast('" + toInDate + "' as time) ";
        //string strQuery = "select count(*) as totalIn from [PR_EMP_ATTENDENCE]  where att_type='OUT' and emp_id='" + empId + "' and ATT_DATE_TIME between '" + frmInDate + "' and '" + toInDate + "' ";
        DataSet oDS = new DataSet();
        string result = "";
        try
        {
            con = new SqlConnection(cn);
            con.Open();

            SqlDataAdapter odaData = new SqlDataAdapter(new SqlCommand(strQuery, con));
            odaData.Fill(oDS, "PR_EMP_ATTENDENCE");
            if (oDS.Tables[0].Rows.Count > 0)
            {
                result = oDS.Tables[0].Rows[0]["totalIn"].ToString();
            }
        }
        catch (Exception ex)
        {
            return null;
        }

        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }

        return result;
    }
    public string WeekDay(string empId, string branchId, string roasterId)
    {

        string strQuery = " SELECT DISTINCT isnull(DATENAME(dw,max(er.ROSTER_DATE)),'') WEEKDAY "
                       + " FROM PR_EMP_ROSTER ER, PR_ROSTER_SETUP PR,PR_EMPLOYEE_LIST EL WHERE PR.PRR_ID=ER.PRR_ID AND EL.EMP_ID=ER.EMP_ID "
                       + " AND ER.EMP_ID= '" + empId + "' AND ER.CMP_BRANCH_ID='" + branchId + "'  and PR.PRR_ID='" + roasterId + "'";
        DataSet oDS = new DataSet();
        string result = "";
        try
        {
            con = new SqlConnection(cn);
            con.Open();

            SqlDataAdapter odaData = new SqlDataAdapter(new SqlCommand(strQuery, con));
            odaData.Fill(oDS, "PR_EMP_ATTENDENCE");
            if (oDS.Tables[0].Rows.Count > 0)
            {
                result = oDS.Tables[0].Rows[0]["WEEKDAY"].ToString();
            }
        }
        catch (Exception ex)
        {
            return null;
        }

        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }

        return result;
    }
    public string InsertTempRoasterTime(string frmInTime, string toInTime, string frmOutTime, string toOutTime)
    {
        string strSql;
        strSql = "SELECT In_Frm_Time, In_To_Time, Out_Frm_Time, Out_To_Time FROM  HR_TEMP_Roster_Time ";
        var con = new SqlConnection(cn);
        try
        {
            DataRow oOrderRow;
            con.Open();
            dbTransaction = con.BeginTransaction();
            DataSet oDS = new DataSet();
            SqlDataAdapter oOrdersDataAdapter = new SqlDataAdapter(new SqlCommand(strSql, con, dbTransaction));
            SqlCommandBuilder oOrdersCmdBuilder = new SqlCommandBuilder(oOrdersDataAdapter);
            oOrdersDataAdapter.FillSchema(oDS, SchemaType.Source);
            //######################################################
            DataTable dtSchedule = oDS.Tables["Table"];
            dtSchedule.TableName = "HR_TEMP_Roster_Time";
            //######################################################
            oOrderRow = oDS.Tables["HR_TEMP_Roster_Time"].NewRow();
            oOrderRow["In_Frm_Time"] = frmInTime;
            oOrderRow["In_To_Time"] = toInTime.Trim();
            oOrderRow["Out_Frm_Time"] = frmOutTime.Trim();
            oOrderRow["Out_To_Time"] = toOutTime.Trim();
            //#########################################################
            oDS.Tables["HR_TEMP_Roster_Time"].Rows.Add(oOrderRow);
            oOrdersDataAdapter.Update(oDS, "HR_TEMP_Roster_Time");
            dbTransaction.Commit();
            return "";
        }
        catch (Exception ex)
        {
            return ex.Message.ToString();
        }
        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }
    }
    public string DeleteTempRoaster(string tempId)
    {
        string strMsg = "";
        con = new SqlConnection(cn);
        try
        {
            string strSql = "DELETE FROM HR_TEMP_Roster_Time WHERE Temp_id='" + tempId + "'";



            SqlCommand CMD = new SqlCommand();
            con.Open();
            CMD.Connection = con;
            CMD.CommandType = CommandType.Text;
            CMD.CommandText = strSql;
            CMD.ExecuteNonQuery();

            strMsg = "Success";
        }
        catch (Exception ex)
        {
            return "Err:" + ex.Message.ToString();
        }
        finally
        {
            con.Close();
            con = null;
        }
        return strMsg;
    }

    public DataSet GetEmpInfoById(string eId)
    {
        try
        {
            string strSql = "select SYS_USR_LOGIN_NAME from PR_EMPLOYEE_LIST EL, CM_SYSTEM_USERS SU where SU.SYS_USR_ID = EL.SYS_USR_ID AND EMP_ID='" + eId + "' ";
            // + "WHERE EMP_ID='" + eId + "' ";

            con = new SqlConnection(cn);
            con.Open();
            DataSet oDS = new DataSet();
            // Create the DataTable "Orders" in the Dataset and the OrdersDataAdapter
            SqlDataAdapter oOrdersDataAdapter = new SqlDataAdapter(new SqlCommand(strSql, con));
            SqlCommandBuilder oOrdersCmdBuilder = new SqlCommandBuilder(oOrdersDataAdapter);
            oOrdersDataAdapter.Fill(oDS, "PR_EMPLOYEE_LIST");
            return oDS;
        }
        catch (Exception ex)
        {
            return null;
        }

        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }
    }


    public DataSet GetBranchId(string eId) // whether alrady defined or not
    {
        string strQuery = "select CMP_BRANCH_ID from PR_EMPLOYEE_LIST WHERE EMP_ID='" + eId + "' ";
        var con = new SqlConnection(cn);
        try
        {

            con.Open();
            DataSet oDS = new DataSet();
            SqlDataAdapter odaData = new SqlDataAdapter(new SqlCommand(strQuery, con));
            odaData.Fill(oDS, "PR_EMPLOYEE_LIST");
            return oDS;
        }
        catch (Exception ex)
        {
            return null;
        }
        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }
    }
    public string GetEmpBranch(string empId)
    {
        string strQuery = " SELECT CMP_BRANCH_ID FROM PR_EMPLOYEE_LIST  WHERE  (EMP_ID = '" + empId + "') ";

        string branchId = "";
        try
        {
            con = new SqlConnection(cn);
            con.Open();
            DataSet oDS = new DataSet();
            SqlDataAdapter odaData = new SqlDataAdapter(new SqlCommand(strQuery, con));
            odaData.Fill(oDS, "PR_EMPLOYEE_LIST");
            branchId = oDS.Tables[0].Rows[0]["CMP_BRANCH_ID"].ToString();
        }
        catch (Exception ex)
        {
            return branchId = "";
        }
        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }
        return branchId;
    }
    public string InsertUserID(string eId, string userName)
    {
        string updateString;
        updateString = "UPDATE PR_EMPLOYEE_LIST SET SYS_USR_ID='" + userName + "'  WHERE EMP_ID='" + eId + "' ";
        string strReturn = "";
        try
        {
            con = new SqlConnection(cn);
            con.Open();
            dbTransaction = con.BeginTransaction();
            SqlCommand cmd = new SqlCommand(updateString, con, dbTransaction);
            cmd.Connection = con;
            cmd.ExecuteNonQuery();
            dbTransaction.Commit();
            con.Close();
        }
        catch (Exception ex)
        {
            strReturn = ex.Message.ToString();
        }
        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }
        return strReturn = "User Id Saved";
    }
    public string UpdateSysUserBranch(string bId, string usrId)
    {
        string updateString;
        updateString = "UPDATE [dbo].[CM_SYSTEM_USERS] SET CMP_BRANCH_ID='" + bId + "'  WHERE SYS_USR_ID='" + usrId + "' ";
        string strReturn = "";
        try
        {
            con = new SqlConnection(cn);
            con.Open();
            dbTransaction = con.BeginTransaction();
            SqlCommand cmd = new SqlCommand(updateString, con, dbTransaction);
            cmd.Connection = con;
            cmd.ExecuteNonQuery();
            dbTransaction.Commit();
            con.Close();
        }
        catch (Exception ex)
        {
            strReturn = ex.Message.ToString();
        }
        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }
        return strReturn = "User Id Saved";
    }

    public string InsertRegionInfo(string regionName, string compId, string regionCode)
    {
        string strSql;
        strSql = "SELECT [CMP_REG_NAME],[COMPANY_ID],[CMP_REG_CODE] FROM [dbo].[CM_REGION] ";

        try
        {
            DataRow oOrderRow;
            con = new SqlConnection(cn);
            con.Open();
            dbTransaction = con.BeginTransaction();
            DataSet oDS = new DataSet();
            SqlDataAdapter oOrdersDataAdapter = new SqlDataAdapter(new SqlCommand(strSql, con, dbTransaction));
            SqlCommandBuilder oOrdersCmdBuilder = new SqlCommandBuilder(oOrdersDataAdapter);
            //oOrdersDataAdapter.MissingSchemaAction = MissingSchemaAction.AddWithKey;
            //oOrdersDataAdapter.Fill(oDS, "PR_ASSIGNMENT_DETAIL");

            oOrdersDataAdapter.FillSchema(oDS, SchemaType.Source, "CM_REGION");
            oOrderRow = oDS.Tables["CM_REGION"].NewRow();

            oOrderRow["CMP_REG_NAME"] = regionName;
            oOrderRow["COMPANY_ID"] = compId;
            oOrderRow["CMP_REG_CODE"] = regionCode;

            oDS.Tables["CM_REGION"].Rows.Add(oOrderRow);
            oOrdersDataAdapter.Update(oDS, "CM_REGION");
            dbTransaction.Commit();
            // con.Close();
            return "Saved successfully";
        }
        catch (Exception ex)
        {
            return ex.Message.ToString();
        }
        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }
    }

    public string GetEmpBranchId(string eId)
    {
        string strSql = "SELECT CMP_BRANCH_ID FROM PR_EMPLOYEE_LIST WHERE EMP_ID='" + eId + "'";

        try
        {
            con = new SqlConnection(cn);
            con.Open();
            DataSet oDs = new DataSet();
            SqlDataAdapter oOrdersDataAdapter = new SqlDataAdapter(new SqlCommand(strSql, con));
            SqlCommandBuilder oOrdersCmdBuilder = new SqlCommandBuilder(oOrdersDataAdapter);
            oOrdersDataAdapter.Fill(oDs, "PR_EMPLOYEE_LIST");

            DataRow dRow = oDs.Tables["PR_EMPLOYEE_LIST"].Rows[0];
            return dRow["CMP_BRANCH_ID"].ToString();
        }
        catch (Exception ex)
        {
            return ex.Message.ToString();

        }
        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }
    }
    public string AddEmpRewardList(string empRewadrname, string empRewardDate, string empRewardDetails, string branchId, string departmentId)  ///oct 2018 Sajib Goswami
    {
        con = new SqlConnection(cn);
        string strTestRegSajib = "";
        strTestRegSajib = "SELECT [EMP_ID],[REWARD_DATE],[REWARD_DETAILS],CMP_BRANCH_ID,DPT_ID FROM HR_EMP_REWARD_LIST";
        try
        {
            DataRow oOrderRow;
            con.Open();
            dbTransaction = con.BeginTransaction();
            DataSet oDS = new DataSet();
            SqlDataAdapter oOrdersDataAdapter = new SqlDataAdapter(new SqlCommand(strTestRegSajib, con, dbTransaction));
            SqlCommandBuilder oOrdersCmdBuilder = new SqlCommandBuilder(oOrdersDataAdapter);
            oOrdersDataAdapter.FillSchema(oDS, SchemaType.Source);
            DataTable dtAddSchedule = oDS.Tables["Table"];
            dtAddSchedule.TableName = "HR_EMP_REWARD_LIST";

            oOrderRow = oDS.Tables["HR_EMP_REWARD_LIST"].NewRow();

            oOrderRow["EMP_ID"] = empRewadrname;
            oOrderRow["REWARD_DATE"] = empRewardDate;
            oOrderRow["REWARD_DETAILS"] = empRewardDetails;

            oOrderRow["CMP_BRANCH_ID"] = branchId;

            oOrderRow["DPT_ID"] = departmentId;

            oDS.Tables["HR_EMP_REWARD_LIST"].Rows.Add(oOrderRow);
            oOrdersDataAdapter.Update(oDS, "HR_EMP_REWARD_LIST");
            dbTransaction.Commit();
            return "Success";

        }
        catch (Exception ex)
        {
            return ex.Message.ToString();
        }
        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }
    }


    public string AddNotActiveDirectoryEmployee( string userLoginId,string branchId)  ///polash 5/5/2019
    {
        con = new SqlConnection(cn);
        string strTestRegSajib = "";
        strTestRegSajib = "SELECT LOGIN_NAME,CMP_BRANCH_ID FROM HR_NOT_ACTIVE_DIRECTORY_LOGIN";
        try
        {
            DataRow oOrderRow;
            con.Open();
            dbTransaction = con.BeginTransaction();
            DataSet oDS = new DataSet();
            SqlDataAdapter oOrdersDataAdapter = new SqlDataAdapter(new SqlCommand(strTestRegSajib, con, dbTransaction));
            SqlCommandBuilder oOrdersCmdBuilder = new SqlCommandBuilder(oOrdersDataAdapter);
            oOrdersDataAdapter.FillSchema(oDS, SchemaType.Source);
            DataTable dtAddSchedule = oDS.Tables["Table"];
            dtAddSchedule.TableName = "HR_NOT_ACTIVE_DIRECTORY_LOGIN";

            oOrderRow = oDS.Tables["HR_NOT_ACTIVE_DIRECTORY_LOGIN"].NewRow();

            oOrderRow["LOGIN_NAME"] = userLoginId;
            oOrderRow["CMP_BRANCH_ID"] = branchId;
           

            oDS.Tables["HR_NOT_ACTIVE_DIRECTORY_LOGIN"].Rows.Add(oOrderRow);
            oOrdersDataAdapter.Update(oDS, "HR_NOT_ACTIVE_DIRECTORY_LOGIN");
            dbTransaction.Commit();
            return "Success";

        }
        catch (Exception ex)
        {
            return ex.Message.ToString();
        }
        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }
    }



    public string AddEmpTourList(string empid, string tourFromDate, string tourToDate, string tourcntry, string tourdts, string cmbrnchid, string dptid)  //november 11,2018 Saib Goswami
    {
        string strSql;
        strSql = "SELECT [EMP_ID],[TOUR_FROM_DATE],[TOUR_TO_DATE],[TOUR_COUNTRY],[TOUR_DETAILS],[CMP_BRANCH_ID],[DPT_ID] FROM [EMP_OFFICIAL_TOUR] ";

        try
        {
            DataRow oOrderRow;
            con = new SqlConnection(cn);
            con.Open();
            dbTransaction = con.BeginTransaction();
            DataSet oDS = new DataSet();
            SqlDataAdapter oOrdersDataAdapter = new SqlDataAdapter(new SqlCommand(strSql, con, dbTransaction));
            SqlCommandBuilder oOrdersCmdBuilder = new SqlCommandBuilder(oOrdersDataAdapter);


            oOrdersDataAdapter.FillSchema(oDS, SchemaType.Source, "EMP_OFFICIAL_TOUR");
            oOrderRow = oDS.Tables["EMP_OFFICIAL_TOUR"].NewRow();

            oOrderRow["EMP_ID"] = empid;
            oOrderRow["TOUR_FROM_DATE"] = tourFromDate;
            oOrderRow["TOUR_TO_DATE"] = tourToDate;
            oOrderRow["TOUR_COUNTRY"] = tourcntry;
            oOrderRow["TOUR_DETAILS"] = tourdts;
            oOrderRow["CMP_BRANCH_ID"] = cmbrnchid;
            oOrderRow["DPT_ID"] = dptid;

            oDS.Tables["EMP_OFFICIAL_TOUR"].Rows.Add(oOrderRow);
            oOrdersDataAdapter.Update(oDS, "EMP_OFFICIAL_TOUR");
            dbTransaction.Commit();

            return "Saved successfully";
        }
        catch (Exception ex)
        {
            return ex.Message.ToString();
        }
        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }
    }
    public string InsertAccademicDistinct(string branchId, string emplyeeId, string date, string description)
    {
        string strSql;
        strSql = "SELECT [CMP_BRANCH_ID],[EMP_ID],[ACC_DIST_DATE],[ACC_DIST_DESCRIPTION] FROM [dbo].[ACCADEMIC_DISTINCTION]  ";

        try
        {
            DataRow oOrderRow;
            con = new SqlConnection(cn);
            con.Open();
            dbTransaction = con.BeginTransaction();
            DataSet oDS = new DataSet();
            SqlDataAdapter oOrdersDataAdapter = new SqlDataAdapter(new SqlCommand(strSql, con, dbTransaction));
            SqlCommandBuilder oOrdersCmdBuilder = new SqlCommandBuilder(oOrdersDataAdapter);


            oOrdersDataAdapter.FillSchema(oDS, SchemaType.Source, "ACCADEMIC_DISTINCTION");
            oOrderRow = oDS.Tables["ACCADEMIC_DISTINCTION"].NewRow();

            oOrderRow["CMP_BRANCH_ID"] = branchId;
            oOrderRow["EMP_ID"] = emplyeeId;
            oOrderRow["ACC_DIST_DATE"] = date;
            oOrderRow["ACC_DIST_DESCRIPTION"] = description;

            oDS.Tables["ACCADEMIC_DISTINCTION"].Rows.Add(oOrderRow);
            oOrdersDataAdapter.Update(oDS, "ACCADEMIC_DISTINCTION");
            dbTransaction.Commit();
            // con.Close();
            return "Saved successfully";
        }
        catch (Exception ex)
        {
            return ex.Message.ToString();
        }
        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }
    }

    public string InsertDispAct(string bId, string eId, string p1, string p2, string dptId)
    {
        string strSql;
        strSql = "SELECT DISCIPLINARY_DATE ,DISCIPLINARY_DETAILS ,EMP_ID ,CMP_BRANCH_ID,DPT_ID FROM [HR_DISCIPLINARY_ACTION] ";

        try
        {
            DataRow oOrderRow;
            con = new SqlConnection(cn);
            con.Open();
            dbTransaction = con.BeginTransaction();
            DataSet oDS = new DataSet();
            SqlDataAdapter oOrdersDataAdapter = new SqlDataAdapter(new SqlCommand(strSql, con, dbTransaction));
            SqlCommandBuilder oOrdersCmdBuilder = new SqlCommandBuilder(oOrdersDataAdapter);


            oOrdersDataAdapter.FillSchema(oDS, SchemaType.Source, "HR_DISCIPLINARY_ACTION");
            oOrderRow = oDS.Tables["HR_DISCIPLINARY_ACTION"].NewRow();

            oOrderRow["EMP_ID"] = eId;
            oOrderRow["CMP_BRANCH_ID"] = bId;
            oOrderRow["DISCIPLINARY_DATE"] = p1;
            oOrderRow["DPT_ID"] = dptId;

            oOrderRow["DISCIPLINARY_DETAILS"] = p2;




            oDS.Tables["HR_DISCIPLINARY_ACTION"].Rows.Add(oOrderRow);
            oOrdersDataAdapter.Update(oDS, "HR_DISCIPLINARY_ACTION");
            dbTransaction.Commit();

            return "Saved successfully";
        }
        catch (Exception ex)
        {
            return ex.Message.ToString();
        }
        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }
    }

    public DataSet GetAssestmentInfo(string empId, string branchId, string assistPerson,string condition)
    {
        string strQuery = " select * from [dbo].[HR_ASSESSMENT_EMPLOYEE] where EMP_ID='" + empId + "' and CMP_BRANCH_ID='" + branchId + "' and ASSIS_PERSON_TYPE='" + assistPerson + "' "+condition+" and DRAFT_STATUS=''";

        try
        {
            con = new SqlConnection(cn);
            con.Open();
            DataSet oDS = new DataSet();
            SqlDataAdapter odaData = new SqlDataAdapter(new SqlCommand(strQuery, con));
            odaData.Fill(oDS, "HR_ASSESSMENT_EMPLOYEE");
            return oDS;
        }
        catch (Exception ex)
        {
            return null;
        }
        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }
    }


    public DataSet GetAssestmentInfoDetails(string empId, string branchId, string assistPerson, string condition)
    {
        string strQuery = " select * from [dbo].[HR_ASSESSMENT_EMPLOYEE] where EMP_ID='" + empId + "' and CMP_BRANCH_ID='" + branchId + "' and ASSIS_PERSON_TYPE='" + assistPerson + "' " + condition + " and DRAFT_STATUS=''";

        try
        {
            con = new SqlConnection(cn);
            con.Open();
            DataSet oDS = new DataSet();
            SqlDataAdapter odaData = new SqlDataAdapter(new SqlCommand(strQuery, con));
            odaData.Fill(oDS, "HR_ASSESSMENT_EMPLOYEE");
            return oDS;
        }
        catch (Exception ex)
        {
            return null;
        }
        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }
    }

    public DataSet GetAssestmentPromotionInfo(string empId, string branchId, string assistPerson, string condition)
    {
        string strQuery = " select * from HR_ASSESSMENT_EMPLOYEE_MASTER as EM  inner join HR_PROMOTION_CATEGORY as CTG on CTG.CATEGORY_ID=EM.PROMOTION_CATEGORY_ID  where EM.EMP_ID='" + empId + "' and EM.CMP_BRANCH_ID='" + branchId + "' and EM.ASSIS_PERSON_TYPE='" + assistPerson + "' " + condition + " and EM.DRAFT_STATUS=''";

        try
        {
            con = new SqlConnection(cn);
            con.Open();
            DataSet oDS = new DataSet();
            SqlDataAdapter odaData = new SqlDataAdapter(new SqlCommand(strQuery, con));
            odaData.Fill(oDS, "HR_ASSESSMENT_EMPLOYEE");
            return oDS;
        }
        catch (Exception ex)
        {
            return null;
        }
        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }
    }

    public string UpdatePromotionEndDateStatus(string empId, DateTime endDate, string dsgStatus)
    {
        string message = "";
        string qry = "UPDATE HR_EMP_PROMOTION SET END_DATE='" + endDate + "', DSG_STATUS='" + dsgStatus + "' WHERE EMP_ID='" + empId + "' "
                    + "AND STR_DATE=(SELECT MAX(STR_DATE) FROM HR_EMP_PROMOTION WHERE EMP_ID='" + empId + "')";
        string strReturn = "";
        try
        {
            con = new SqlConnection(cn);
            con.Open();
            dbTransaction = con.BeginTransaction();
            SqlCommand cmd = new SqlCommand(qry, con, dbTransaction);
            cmd.Connection = con;
            cmd.ExecuteNonQuery();
            dbTransaction.Commit();
            con.Close();
            message = "Success";
        }
        catch (Exception ex)
        {
            message = ex.Message.ToString();
        }
        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }
        return message;
    }

    public string InsertEmpPromotionInfo(string empID, string dsgId, DateTime strDate, string reference, string remarks, string dsgStatus)
    {
        string tblTranfer = "HR_EMP_PROMOTION";
        string fldSlNo = "SL_NO";
        int slno = SlNo(tblTranfer, fldSlNo, empID);
        string strSql;
        strSql = "SELECT EMP_ID, DSG_ID, STR_DATE, END_DATE, SL_NO, REMARKS, REFERANCE, DSG_STATUS FROM HR_EMP_PROMOTION";
        try
        {
            DataRow oOrderRow;
            con = new SqlConnection(cn);
            con.Open();
            dbTransaction = con.BeginTransaction();
            DataSet oDS = new DataSet();
            SqlDataAdapter oOrdersDataAdapter = new SqlDataAdapter(new SqlCommand(strSql, con, dbTransaction));
            SqlCommandBuilder oOrdersCmdBuilder = new SqlCommandBuilder(oOrdersDataAdapter);
            oOrdersDataAdapter.FillSchema(oDS, SchemaType.Source);

            DataTable dtAddSchedule = oDS.Tables["Table"];
            dtAddSchedule.TableName = "HR_EMP_PROMOTION";

            oOrderRow = oDS.Tables["HR_EMP_PROMOTION"].NewRow();

            oOrderRow["EMP_ID"] = empID;
            oOrderRow["DSG_ID"] = dsgId;
            oOrderRow["STR_DATE"] = strDate;
            oOrderRow["SL_NO"] = slno + 1;
            oOrderRow["REMARKS"] = remarks;
            oOrderRow["REFERANCE"] = reference;
            oOrderRow["DSG_STATUS"] = dsgStatus;

            oDS.Tables["HR_EMP_PROMOTION"].Rows.Add(oOrderRow);
            oOrdersDataAdapter.Update(oDS, "HR_EMP_PROMOTION");
            dbTransaction.Commit();
            return "Saved Successfully.";
        }
        catch (Exception ex)
        {
            return ex.Message.ToString();
        }
        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }
    }

    public string UpdateDesignationId(string dsgId, string empId)
    {

        DateTime todayDate = new DateTime();
        todayDate = DateTime.Today;

        string message = "";
        string qry = "UPDATE PR_EMPLOYEE_LIST SET DSG_ID_MAIN='" + dsgId + "' WHERE EMP_ID ='" + empId + "'";
        string strReturn = "";
        try
        {
            con = new SqlConnection(cn);
            con.Open();
            dbTransaction = con.BeginTransaction();
            SqlCommand cmd = new SqlCommand(qry, con, dbTransaction);
            cmd.Connection = con;
            cmd.ExecuteNonQuery();
            dbTransaction.Commit();
            con.Close();
            message = "Success";
        }
        catch (Exception ex)
        {
            message = ex.Message.ToString();
        }
        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }
        return message;
    }
    public string UpdateGradeEndDateStatus(string empId, DateTime endDate, string dsgStatus)
    {
        string message = "";
        string qry = "UPDATE HR_EMP_GRADE_CHANGE SET END_DATE='" + endDate + "', DSG_STATUS='" + dsgStatus + "' WHERE EMP_ID='" + empId + "' "
                    + "AND STR_DATE=(SELECT MAX(STR_DATE) FROM HR_EMP_GRADE_CHANGE WHERE EMP_ID='" + empId + "')";
        string strReturn = "";
        try
        {
            con = new SqlConnection(cn);
            con.Open();
            dbTransaction = con.BeginTransaction();
            SqlCommand cmd = new SqlCommand(qry, con, dbTransaction);
            cmd.Connection = con;
            cmd.ExecuteNonQuery();
            dbTransaction.Commit();
            con.Close();
            message = "Success";
        }
        catch (Exception ex)
        {
            message = ex.Message.ToString();
        }
        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }
        return message;
    }
    public string InsertEmpGradeInfo(string empID, string dsgId, DateTime strDate, string reference, string remarks, string dsgStatus)
    {
        string tblTranfer = "HR_EMP_GRADE_CHANGE";
        string fldSlNo = "SL_NO";
        int slno = SlNo(tblTranfer, fldSlNo, empID);
        string strSql;
        strSql = "SELECT EMP_ID, DSG_ID, STR_DATE, END_DATE, SL_NO, REMARKS, REFERANCE, DSG_STATUS FROM HR_EMP_GRADE_CHANGE";
        try
        {
            DataRow oOrderRow;
            con = new SqlConnection(cn);
            con.Open();
            dbTransaction = con.BeginTransaction();
            DataSet oDS = new DataSet();
            SqlDataAdapter oOrdersDataAdapter = new SqlDataAdapter(new SqlCommand(strSql, con, dbTransaction));
            SqlCommandBuilder oOrdersCmdBuilder = new SqlCommandBuilder(oOrdersDataAdapter);
            oOrdersDataAdapter.FillSchema(oDS, SchemaType.Source);

            DataTable dtAddSchedule = oDS.Tables["Table"];
            dtAddSchedule.TableName = "HR_EMP_GRADE_CHANGE";

            oOrderRow = oDS.Tables["HR_EMP_GRADE_CHANGE"].NewRow();

            oOrderRow["EMP_ID"] = empID;
            oOrderRow["DSG_ID"] = dsgId;
            oOrderRow["STR_DATE"] = strDate;
            oOrderRow["SL_NO"] = slno + 1;
            oOrderRow["REMARKS"] = remarks;
            oOrderRow["REFERANCE"] = reference;
            oOrderRow["DSG_STATUS"] = dsgStatus;

            oDS.Tables["HR_EMP_GRADE_CHANGE"].Rows.Add(oOrderRow);
            oOrdersDataAdapter.Update(oDS, "HR_EMP_GRADE_CHANGE");
            dbTransaction.Commit();
            return "Saved Successfully.";
        }
        catch (Exception ex)
        {
            return ex.Message.ToString();
        }
        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }
    }
    public string UpdateGradeId(string dsgId, string empId)
    {

        DateTime todayDate = new DateTime();
        todayDate = DateTime.Today;

        string message = "";
        string qry = "UPDATE PR_EMPLOYEE_LIST SET DSG_ID='" + dsgId + "' WHERE EMP_ID ='" + empId + "'";
        string strReturn = "";
        try
        {
            con = new SqlConnection(cn);
            con.Open();
            dbTransaction = con.BeginTransaction();
            SqlCommand cmd = new SqlCommand(qry, con, dbTransaction);
            cmd.Connection = con;
            cmd.ExecuteNonQuery();
            dbTransaction.Commit();
            con.Close();
            message = "Success";
        }
        catch (Exception ex)
        {
            message = ex.Message.ToString();
        }
        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }
        return message;
    }
    public DataSet GetEmployeeLastGradeChangeDate(string empId)
    {
        string strQuery = @" select  convert(varchar, max(END_DATE),106) as Dates from HR_EMP_GRADE_CHANGE "
                              + " where DSG_STATUS='I' and "
                              + "  EMP_ID='" + empId + "'  ";
        try
        {
            con = new SqlConnection(cn);
            con.Open();
            DataSet oDS = new DataSet();
            SqlDataAdapter odaData = new SqlDataAdapter(new SqlCommand(strQuery, con));
            odaData.Fill(oDS, "attBasic");
            return oDS;
        }
        catch (Exception ex)
        {
            return null;
        }
        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }
    }
    public DataSet GetEmployeeTenureLastGradeChangeDate(string empId)
    {
        string strQuery = @" select dbo.CALCULATE_SERVICETIME(convert(datetime, END_DATE),GETDATE())
  as Dates from HR_EMP_GRADE_CHANGE "
                              + " where DSG_STATUS='I' and "
                              + "  EMP_ID='" + empId + "'  ";
        try
        {
            con = new SqlConnection(cn);
            con.Open();
            DataSet oDS = new DataSet();
            SqlDataAdapter odaData = new SqlDataAdapter(new SqlCommand(strQuery, con));
            odaData.Fill(oDS, "attBasic");
            return oDS;
        }
        catch (Exception ex)
        {
            return null;
        }
        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }
    }
    public DataSet GetEmployeeLastDesignationChangeDate(string empId)
    {
        string strQuery = @" select  convert(varchar, max(END_DATE),106) as Dates from HR_EMP_PROMOTION "
                              + " where DSG_STATUS='I' and "
                              + "  EMP_ID='" + empId + "'  ";
        try
        {
            con = new SqlConnection(cn);
            con.Open();
            DataSet oDS = new DataSet();
            SqlDataAdapter odaData = new SqlDataAdapter(new SqlCommand(strQuery, con));
            odaData.Fill(oDS, "attBasic");
            return oDS;
        }
        catch (Exception ex)
        {
            return null;
        }
        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }
    }
    public DataSet GetEmployeeTenureLastDesignationChangeDate(string empId)
    {
        string strQuery = @" select dbo.CALCULATE_SERVICETIME(convert(datetime, END_DATE),GETDATE())
  as Dates from HR_EMP_PROMOTION "
                              + " where DSG_STATUS='I' and "
                              + "  EMP_ID='" + empId + "'  ";
        try
        {
            con = new SqlConnection(cn);
            con.Open();
            DataSet oDS = new DataSet();
            SqlDataAdapter odaData = new SqlDataAdapter(new SqlCommand(strQuery, con));
            odaData.Fill(oDS, "attBasic");
            return oDS;
        }
        catch (Exception ex)
        {
            return null;
        }
        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }
    }
    public string AddPromotionCategory(string categoryName,string categoryDescription,string branchId)  ///oct 2018 polash
    {
        con = new SqlConnection(cn);
        string strTestRegSajib = "";
        strTestRegSajib = "SELECT CATEGORY_NAME,CATEGORY_DESCRIPTION,CMP_BRANCH_ID FROM HR_PROMOTION_CATEGORY";
        try
        {
            DataRow oOrderRow;
            con.Open();
            dbTransaction = con.BeginTransaction();
            DataSet oDS = new DataSet();
            SqlDataAdapter oOrdersDataAdapter = new SqlDataAdapter(new SqlCommand(strTestRegSajib, con, dbTransaction));
            SqlCommandBuilder oOrdersCmdBuilder = new SqlCommandBuilder(oOrdersDataAdapter);
            oOrdersDataAdapter.FillSchema(oDS, SchemaType.Source);
            DataTable dtAddSchedule = oDS.Tables["Table"];
            dtAddSchedule.TableName = "HR_PROMOTION_CATEGORY";

            oOrderRow = oDS.Tables["HR_PROMOTION_CATEGORY"].NewRow();

            oOrderRow["CATEGORY_NAME"] = categoryName;
            oOrderRow["CATEGORY_DESCRIPTION"] = categoryDescription;
            oOrderRow["CMP_BRANCH_ID"] = branchId;

            oDS.Tables["HR_PROMOTION_CATEGORY"].Rows.Add(oOrderRow);
            oOrdersDataAdapter.Update(oDS, "HR_PROMOTION_CATEGORY");
            dbTransaction.Commit();
            return "Success";

        }
        catch (Exception ex)
        {
            return ex.Message.ToString();
        }
        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }
    }
    public DataSet GetDataForWorkflow()
    {
        string strQuery = @"SELECT WM_ITEM_ID, WM_ITEM_NAME FROM CM_WORKFLOW_MANAGE_TYPE WHERE WM_ITEM_PARENT_CODE='04' ";
        try
        {
            con = new SqlConnection(cn);
            con.Open();
            DataSet oDS = new DataSet();
            SqlDataAdapter odaData = new SqlDataAdapter(new SqlCommand(strQuery, con));
            odaData.Fill(oDS, "attBasic");
            return oDS;
        }
        catch (Exception ex)
        {
            return null;
        }
        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }
    }
    public DataSet GetDataForWorkflowCheck( string empId)
    {
        string strQuery = @"SELECT  * FROM CM_WORKFLOW_SETUP WHERE  DESTINATION_EMP_ID='"+empId+"' and MAIN_TYPE_ID='04' and ITEM_SET_CODE='0404' ";
        try
        {
            con = new SqlConnection(cn);
            con.Open();
            DataSet oDS = new DataSet();
            SqlDataAdapter odaData = new SqlDataAdapter(new SqlCommand(strQuery, con));
            odaData.Fill(oDS, "attBasic");
            return oDS;
        }
        catch (Exception ex)
        {
            return null;
        }
        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }
    }
    public DataSet GetBasicInformationForEmployee(string branchId)
    {
        string strQuery = @"  select el.EMP_NAME_BANGLA, DPT.DPT_NAME, el.EMP_FATHER_NAME,el.EMP_MOTHER_NAME,el.FATHERS_NAME_BANGLA,el.MOTHERS_NAME_BANGLA,el.EMP_PER_ADDRESS,DE.DSG_TITLE as Grade,
       el.EMP_PRE_ADDRES, CAST(ISNULL(el.EMP_CODE, '') AS VARCHAR(100)) as empId,CAST(ISNULL(EMP_TITLE, '') AS VARCHAR(100)) + ' ' + CAST(EMP_NAME AS VARCHAR(500)) AS ENAME, 
       LTRIM(RTRIM(el.EMP_LATE_SHOW)) EMP_LATE_SHOW,  pd.CMP_BRANCH_NAME, 
       (SELECT DSG_TITLE FROM PR_DESIGNATION p WHERE p.SET_TYPE='D' AND p.DSG_ID=el.DSG_ID_MAIN)DSG_TITLE, (SELECT DSG_TITLE_BANGLA FROM PR_DESIGNATION p WHERE p.SET_TYPE='D' AND p.DSG_ID=el.DSG_ID_MAIN)DSG_TITLE_BANGLA,  CONVERT(varchar, EMP_JOINING_DATE, 107) EMP_JOINING_DATE ,
       CONVERT(varchar, el.EMP_BIRTHDAY, 107) EMP_BIRTHDAY ,el.EMP_BLOODGRP, CONVERT(varchar, el.EMP_CONFIR_DATE, 107) EMP_CONFIR_DATE,el.EMP_CONTACT_NUM,el.EMP_EMAIL,el.EMP_EMAIL_PERSONAL ,
       [dbo].[CALCULATE_SERVICETIME](el.EMP_JOINING_DATE,GETDATE()) as ExperianceInPalo,[dbo].[CALCULATE_SERVICETIME](el.EMP_BIRTHDAY,GETDATE()) Age

       from PR_EMPLOYEE_LIST el left join PR_DESIGNATION as DE on DE.DSG_ID=el.DSG_ID and DE.SET_TYPE='G'  
       left join PR_DEPARTMENT as DPT on DPT.DPT_ID=el.DPT_ID ,CM_CMP_BRANCH pd "
                              + " where  "
                              + " el.CMP_BRANCH_ID=pd.CMP_BRANCH_ID and el.CMP_BRANCH_ID='" + branchId + "'   AND EMP_STATUS NOT IN (9,11) and EMP_EMAIL is not null  ";
        try
        {
            con = new SqlConnection(cn);
            con.Open();
            DataSet oDS = new DataSet();
            SqlDataAdapter odaData = new SqlDataAdapter(new SqlCommand(strQuery, con));
            odaData.Fill(oDS, "attBasic");
            return oDS;
        }
        catch (Exception ex)
        {
            return null;
        }
        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }
    }
    public string InsertJoiningMailStatus(string employeeName, string empId, string branchId,string date,string status)  ///jan 2019 polash
    {
        con = new SqlConnection(cn);
        string strTestRegSajib = "";
        strTestRegSajib = "SELECT EMP_ID,EMP_NAME,MAIL_DATE,MAIL_STATUS,CMP_BRANCH_ID FROM HR_JOINING_MAIL";
        try
        {
            DataRow oOrderRow;
            con.Open();
            dbTransaction = con.BeginTransaction();
            DataSet oDS = new DataSet();
            SqlDataAdapter oOrdersDataAdapter = new SqlDataAdapter(new SqlCommand(strTestRegSajib, con, dbTransaction));
            SqlCommandBuilder oOrdersCmdBuilder = new SqlCommandBuilder(oOrdersDataAdapter);
            oOrdersDataAdapter.FillSchema(oDS, SchemaType.Source);
            DataTable dtAddSchedule = oDS.Tables["Table"];
            dtAddSchedule.TableName = "HR_JOINING_MAIL";

            oOrderRow = oDS.Tables["HR_JOINING_MAIL"].NewRow();

            oOrderRow["EMP_ID"] = empId;
            oOrderRow["EMP_NAME"] = employeeName;
            oOrderRow["MAIL_DATE"] = date;
            oOrderRow["MAIL_STATUS"] = status;
            oOrderRow["CMP_BRANCH_ID"] = branchId;

            oDS.Tables["HR_JOINING_MAIL"].Rows.Add(oOrderRow);
            oOrdersDataAdapter.Update(oDS, "HR_JOINING_MAIL");
            dbTransaction.Commit();
            return "Success";

        }
        catch (Exception ex)
        {
            return ex.Message.ToString();
        }
        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }
    }

    public string InsertLateMailStatus(string employeeName, string empId, string branchId, string date, string status,string forDate,string toDate)  ///jan 2019 polash
    {
        con = new SqlConnection(cn);
        string strTestRegSajib = "";
        strTestRegSajib = "SELECT [FORM_DATE],[TO_DATE],[EMP_ID],[SEND_DATE],CMP_BRANCH_ID,[MAIL_STATUS],[EMP_NAME] FROM HR_LATE_MAIL_SEND";
        try
        {
            DataRow oOrderRow;
            con.Open();
            dbTransaction = con.BeginTransaction();
            DataSet oDS = new DataSet();
            SqlDataAdapter oOrdersDataAdapter = new SqlDataAdapter(new SqlCommand(strTestRegSajib, con, dbTransaction));
            SqlCommandBuilder oOrdersCmdBuilder = new SqlCommandBuilder(oOrdersDataAdapter);
            oOrdersDataAdapter.FillSchema(oDS, SchemaType.Source);
            DataTable dtAddSchedule = oDS.Tables["Table"];
            dtAddSchedule.TableName = "HR_LATE_MAIL_SEND";

            oOrderRow = oDS.Tables["HR_LATE_MAIL_SEND"].NewRow();

            oOrderRow["EMP_ID"] = empId;
            oOrderRow["EMP_NAME"] = employeeName;
            oOrderRow["SEND_DATE"] = date;
            oOrderRow["MAIL_STATUS"] = status;
            oOrderRow["CMP_BRANCH_ID"] = branchId;
            oOrderRow["FORM_DATE"] = forDate;
            oOrderRow["TO_DATE"] = toDate;

            oDS.Tables["HR_LATE_MAIL_SEND"].Rows.Add(oOrderRow);
            oOrdersDataAdapter.Update(oDS, "HR_LATE_MAIL_SEND");
            dbTransaction.Commit();
            return "Success";

        }
        catch (Exception ex)
        {
            return ex.Message.ToString();
        }
        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }
    }


    public DataSet GetWorkflowTypeForDashboard(string empId, string dptId)
    {
        string strQuery = @" select distinct CAST(el.EMP_NAME AS VARCHAR) + ' (' + CAST(el.EMP_CODE AS VARCHAR)+')' EMP_NAME
,WP.ITEM_SET_CODE from [dbo].[CM_WORKFLOW_SETUP] as WP
inner join PR_EMPLOYEE_LIST as EL on EL.EMP_ID=wp.SOURCE_EMP_ID  where DESTINATION_EMP_ID='" + empId + "' and WP.DPT_ID='"+dptId+"' and MAIN_TYPE_ID='01' and ITEM_SET_CODE in (101,102,103) order by ITEM_SET_CODE";

        try
        {
            con = new SqlConnection(cn);
            con.Open();
            DataSet oDS = new DataSet();
            SqlDataAdapter odaData = new SqlDataAdapter(new SqlCommand(strQuery, con));
            odaData.Fill(oDS, "CM_WORKFLOW_MANAGE_TYPE");
            return oDS;
        }
        catch (Exception ex)
        {
            return null;
        }
        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }
    }

    public DataSet GetWorkflowTypeForDashboardReportedTo(string empId, string dptId)
    {
        string strQuery = @" select distinct
CAST(el.EMP_NAME AS VARCHAR) + ' (' + CAST(el.EMP_CODE AS VARCHAR)+')' EMP_NAME
,WP.ITEM_SET_CODE from [dbo].[CM_WORKFLOW_SETUP] as WP
inner join PR_EMPLOYEE_LIST as EL on EL.EMP_ID=wp.DESTINATION_EMP_ID  where SOURCE_EMP_ID='" + empId + "'  and MAIN_TYPE_ID='01' and ITEM_SET_CODE in (101,102,103) order by ITEM_SET_CODE";

        try
        {
            con = new SqlConnection(cn);
            con.Open();
            DataSet oDS = new DataSet();
            SqlDataAdapter odaData = new SqlDataAdapter(new SqlCommand(strQuery, con));
            odaData.Fill(oDS, "CM_WORKFLOW_MANAGE_TYPE");
            return oDS;
        }
        catch (Exception ex)
        {
            return null;
        }
        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }
    }


    public DataSet GetLeaveApprovedData(string yearid, string strFilter, string strFilter1, string strFilter2)
    {
        string strQuery = @"select *, (select LTRIM(CAST(ISNULL(E.EMP_TITLE, '') AS VARCHAR(100)) + ' ' + CAST(E.EMP_NAME AS VARCHAR(500))) + '  [' + E.EMP_CODE + ']' as ENAME from  PR_EMPLOYEE_LIST E where L.EMP_ID=E.EMP_ID) ENAME,

                                (select CASE NATURE_TYPE WHEN 'NL' THEN 'General Leave' WHEN 'SL' THEN 'Special Leave' END NATURE_TYPE from PR_LEAVE_TYPE LT where NATURE_TYPE='NL' and  LT.PRLT_ID=L.PRLT_ID)  NATURE_TYPE,
                                (select PRLT_TITLE from PR_LEAVE_TYPE LT where NATURE_TYPE='NL' and  LT.PRLT_ID=L.PRLT_ID)  PRLT_TITLE
                                 from PR_LEAVE L
                                  WHERE DATEPART(YYYY, LVE_FROM_DATE)  = '" + yearid + "' "
                                                                                + " " + strFilter + " " + strFilter1 + " " + strFilter2 + " order by L.EMP_ID";
        var con = new SqlConnection(cn);
        try
        {

            DataSet oDS = new DataSet();
            SqlDataAdapter odaData = new SqlDataAdapter(new SqlCommand(strQuery, con));
            odaData.Fill(oDS, "EmpList");
            return oDS;
        }
        catch (Exception ex)
        {
            return null;
        }

        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }
    }
    public DataSet GetMissingData_att(string Emp_Id, string date, string type)   ///september 2019 polash
    {
        string strQuery = @"select  EMP_ID,  "
        + " from PR_EMP_ROSTER EA,PR_EMPLOYEE_LIST EL where EL.EMP_CODE='" + Emp_Id + "'    ";    //and ATT_TYPE = '"+ type +"'
        var con = new SqlConnection(cn);
        try
        {

            DataSet oDS = new DataSet();
            SqlDataAdapter odaData = new SqlDataAdapter(new SqlCommand(strQuery, con));
            odaData.Fill(oDS, "EmpList");
            return oDS;
        }
        catch (Exception ex)
        {
            return null;
        }

        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }
    }
    public string InsertINOUTmiss(string Emp_Id, string date, string type)  ///september 2019 polash
    {
        con = new SqlConnection(cn);
        string strTestRegSajib = "";
        strTestRegSajib = "SELECT [FORM_DATE],[TO_DATE],[EMP_ID],[SEND_DATE],CMP_BRANCH_ID,[MAIL_STATUS],[EMP_NAME] FROM PR_EMP_ATTENDENCE";
        try
        {
            DataRow oOrderRow;
            con.Open();
            dbTransaction = con.BeginTransaction();
            DataSet oDS = new DataSet();
            SqlDataAdapter oOrdersDataAdapter = new SqlDataAdapter(new SqlCommand(strTestRegSajib, con, dbTransaction));
            SqlCommandBuilder oOrdersCmdBuilder = new SqlCommandBuilder(oOrdersDataAdapter);
            oOrdersDataAdapter.FillSchema(oDS, SchemaType.Source);
            DataTable dtAddSchedule = oDS.Tables["Table"];
            dtAddSchedule.TableName = "HR_LATE_MAIL_SEND";

            oOrderRow = oDS.Tables["HR_LATE_MAIL_SEND"].NewRow();

            //oOrderRow["EMP_ID"] = empId;
            //oOrderRow["EMP_NAME"] = employeeName;
            //oOrderRow["SEND_DATE"] = date;
            //oOrderRow["MAIL_STATUS"] = status;
            //oOrderRow["CMP_BRANCH_ID"] = branchId;
            //oOrderRow["FORM_DATE"] = forDate;
            //oOrderRow["TO_DATE"] = toDate;

            oDS.Tables["HR_LATE_MAIL_SEND"].Rows.Add(oOrderRow);
            oOrdersDataAdapter.Update(oDS, "HR_LATE_MAIL_SEND");
            dbTransaction.Commit();
            return "Success";

        }
        catch (Exception ex)
        {
            return ex.Message.ToString();
        }
        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }
    }

    public int ApprovedWorkDaysDailyChk_excepRoster(string FrmDt, string EmpId) // asad
    {
        //string strQuery = " set deadlock_priority low; select COUNT(*)total from PR_EMP_ATTENDENCE at  with(nolock) where (at.ATT_TYPE= 'IN' OR at.ATT_TYPE= 'OUT' OR at.ATT_TYPE= 'OSD')  and EMP_ID='" + EmpId + "' and "
        //                 + " convert(date, CONVERT(DATETIME, CONVERT(VARCHAR(23), at.ATT_DATE_TIME))) =  convert(date, CONVERT(DATETIME, CONVERT(VARCHAR(23), '" + FrmDt + "'))) AND at.PREV_OUT_FLAG='PRE' ";
        string strQuery = " set deadlock_priority low; select COUNT(*)total from PR_EMP_ATTENDENCE at  with(nolock) where (at.ATT_TYPE= 'IN' OR at.ATT_TYPE= 'OUT' OR at.ATT_TYPE= 'OSD')  and EMP_ID='" + EmpId + "' and "
                        + " convert(date, at.ATT_DATE_TIME) =  convert(date,  '" + FrmDt + "') AND at.PREV_OUT_FLAG='PRE' ";
        int totalPresent = 0;
        try
        {
            con = new SqlConnection(cn);
            con.Open();
            DataSet oDS = new DataSet();
            SqlDataAdapter odaData = new SqlDataAdapter(new SqlCommand(strQuery, con));
            odaData.Fill(oDS, "workingDays");
            string chkWDays = oDS.Tables[0].Rows[0]["total"].ToString();
            if (int.Parse(chkWDays) > 0)
            {
                totalPresent = 1;
            }
        }
        catch (Exception ex)
        {
            return 0;
        }
        finally
        {
            con.Close();
            con.Dispose();
            SqlConnection.ClearPool(con);
            con = null;
        }
        return totalPresent;
    }
}
