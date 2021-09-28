using LHSAPI.Common.ApiResponse;
using LHSAPI.Domain.Entities;
using LHSAPI.Persistence.DbContext;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static LHSAPI.Common.Enums.ResponseEnums;
using LHSAPI.Application.Employee.Models;
using LHSAPI.Application.Interface;
using System.Net.Http;
using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using System.Data.OleDb;
using Microsoft.Data.SqlClient;

namespace LHSAPI.Application.Administration.Commands.Create.UploadServicePrice
{
    public class UploadServicePriceHandler : IRequestHandler<UploadServicePriceCommand, ApiResponse>
    {
        private readonly LHSDbContext _context;
        private readonly ISessionService _ISessionService;
        private IHostingEnvironment _hostingEnvironment;
        private readonly IConfiguration _configuration;
        public UploadServicePriceHandler(LHSDbContext context, ISessionService ISessionService, IHostingEnvironment hostingEnvironment, IConfiguration configuration)
        {
            _context = context;
            _ISessionService = ISessionService;
            _hostingEnvironment = hostingEnvironment;
            _configuration = configuration;
        }

        public async Task<ApiResponse> Handle(UploadServicePriceCommand request, CancellationToken cancellationToken)
        {
            ApiResponse response = new ApiResponse();
            try
            {
                if (!String.IsNullOrEmpty(request.files.FileName))
                {
                   
                    string path = Path.Combine(_hostingEnvironment.WebRootPath, _configuration["ClientDocumentPath"]);
                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }

                    //Save the uploaded Excel file.
                    string fileName = Path.GetFileName(request.files.FileName);
                    string filePath = Path.Combine(path, fileName);
                    using (FileStream stream = new FileStream(filePath, FileMode.Create))
                    {
                        request.files.CopyTo(stream);
                    }

                    var serviceList = (from service in _context.ServiceDetails select service).ToList();
                   //if(serviceList != null && serviceList.Count > 0)
                   // foreach (var item in serviceList)
                   // {
                   //     item.IsActive = false;
                   //     item.IsDeleted = true;
                   // }
                   // _context.SaveChanges();
                    //Read the connection string for the Excel file.
                    string conString = this._configuration.GetConnectionString("ExcelConString");
                    DataTable dt = new DataTable();
                    conString = string.Format(conString, filePath);

                    using (OleDbConnection connExcel = new OleDbConnection(conString))
                    {
                        using (OleDbCommand cmdExcel = new OleDbCommand())
                        {
                            using (OleDbDataAdapter odaExcel = new OleDbDataAdapter())
                            {
                                cmdExcel.Connection = connExcel;
                                connExcel.Open();
                                DataTable dtExcelSchema;
                                dtExcelSchema = connExcel.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
                                string sheetName = dtExcelSchema.Rows[0]["TABLE_NAME"].ToString();
                                connExcel.Close();
                                connExcel.Open();
                                cmdExcel.CommandText = "SELECT * From [" + sheetName + "]";
                                odaExcel.SelectCommand = cmdExcel;
                                odaExcel.Fill(dt);
                                connExcel.Close();
                            }
                        }
                    }
                    SqlConnection conn = GetConnection();
                    //Insert the Data read from the Excel file to Database Table.
                    conString = this._configuration.GetConnectionString("SqlConnection");
                    using (SqlConnection con = new SqlConnection(conString))
                    {
                        using (SqlBulkCopy sqlBulkCopy = new SqlBulkCopy(con))
                        {
                            //Set the database table name.
                            sqlBulkCopy.DestinationTableName = "dbo.ServiceDetails";

                            //[OPTIONAL]: Map the Excel columns with that of the database table.
                            sqlBulkCopy.ColumnMappings.Add("Id", "Id");
                            sqlBulkCopy.ColumnMappings.Add("SupportItemNumber", "SupportItemNumber");
                            sqlBulkCopy.ColumnMappings.Add("SupportItemName", "SupportItemName");
                            sqlBulkCopy.ColumnMappings.Add("Rate", "Rate");
                            sqlBulkCopy.ColumnMappings.Add("IsActive", "IsActive");
                            con.Open();
                            sqlBulkCopy.WriteToServer(dt);
                            con.Close();
                            response.Success(sqlBulkCopy);
                        }
                    }
                }
                else
                {

                }



            }
            catch (Exception ex)
            {
                response.Failed(ex.Message);

            }
            return response;

        }
        private SqlConnection GetConnection()
        {
            string connectionString = _configuration.GetConnectionString("SqlConnection");
            return new SqlConnection(connectionString);
        }
    }
}
