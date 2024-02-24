using DocumentFormat.OpenXml.Drawing.Diagrams;
using Reveal.Sdk;
using Reveal.Sdk.Data;
using Reveal.Sdk.Data.Microsoft.SqlServer;

namespace RevealSdk.Server.Reveal

{
    internal class DataSourceProvider : IRVDataSourceProvider
    {
        public Task<RVDataSourceItem> ChangeDataSourceItemAsync
                (IRVUserContext userContext, string dashboardId, RVDataSourceItem dataSourceItem)
        {
            if (dataSourceItem is RVSqlServerDataSourceItem sqlDsi)
            {
                ChangeDataSourceAsync(userContext, sqlDsi.DataSource);


                string idPrefix = dataSourceItem.Id.Substring(0, 2).ToLower();
                string storedProcName = dataSourceItem.Id.Substring(2);

                if (idPrefix == "sp")
                {
                    switch (storedProcName)
                    {
                        case "OrdersByCustomer":
                            sqlDsi.Procedure = "OrdersByCustomer";
                            sqlDsi.ProcedureParameters = new Dictionary<string, object>
                                 {
                                     { "@CustomerID", userContext.UserId }
                                 };
                            break;

                        case "OrderDetails":
                            int orderID;
                            if (int.TryParse(userContext.Properties["OrderId"].ToString(), out orderID))
                            {
                                sqlDsi.Procedure = "OrderDetails";
                                sqlDsi.ProcedureParameters = new Dictionary<string, object>
                                     {
                                         { "@OrderID", orderID }
                                     };
                            }
                            break;

                        case "OrdersByEmployee":
                            int employeeID;
                            if (int.TryParse(userContext.Properties["EmployeeId"].ToString(), out employeeID))
                            {
                                sqlDsi.Procedure = "OrdersByEmployee";
                                sqlDsi.ProcedureParameters = new Dictionary<string, object>
                                     {
                                         { "@EmployeeID", employeeID }
                                     };
                            }
                            break;

                        default:
                            // Handle unknown stored procedure
                            break;
                    }

                }
                else if (idPrefix == "cq")
                {
                    int employeeID;
                    if (int.TryParse(userContext.Properties["EmployeeId"].ToString(), out employeeID))
                    {
                        sqlDsi.CustomQuery = "Select * from OrderAnalysis where EmployeeID = " + employeeID;
                    }                    
                }
                else if (idPrefix == "tb")
                {
                    // Handle Tables or Views
                }
                else
                {
                    return null;
                }



            }
            return Task.FromResult(dataSourceItem);
        }

        public Task<RVDashboardDataSource> ChangeDataSourceAsync(IRVUserContext userContext, RVDashboardDataSource dataSource)
        {
            if (dataSource is RVAzureSqlDataSource sqlDs)
            {
                sqlDs.Host = "server";
                sqlDs.Database = "database";
            }
            return Task.FromResult(dataSource);
        }
    }
}