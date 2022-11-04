using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Configuration;
using System.Data.SqlClient;
using System.Xml.Linq;
using Api_WebApplication1.Model;
using companyinfo.Model;
using Microsoft.Extensions.Configuration;

namespace Api_WebApplication1.Model.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        public EmployeController(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        [HttpGet]
        public JsonResult GetEmploye()
        {
            List<Employe1> employe = new List<Employe1>();
            using (SqlConnection con = new SqlConnection(_configuration.GetConnectionString("TestDatabase")))
                try
                {
                    SqlCommand cmd = con.CreateCommand();
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = "SELECT * FROM VW_SelectEmployeInfo";
                    con.Open();
                    SqlDataReader sdr = cmd.ExecuteReader();
                    while (sdr.Read())
                    {
                        employe.Add(new Employe1
                        {
                            EmployeId = Convert.ToInt32(sdr["EmployeId"]),
                            EmployeName = Convert.ToString(sdr["EmployeName"]),
                            EmailId = Convert.ToString(sdr["EmailId"])
                        });

                    }
                }
                catch (Exception ex)
                {
                    return new JsonResult(ex.Message);
                }
                finally
                {
                    con.Close();
                }
            return new JsonResult(employe);
        }
        [HttpPost]
        public JsonResult AddEmployee(Employe1 employe)
        {
            using (SqlConnection con = new SqlConnection(_configuration.GetConnectionString("TestDatabase")))
            {
                try
                {
                    SqlCommand cmd = con.CreateCommand();
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = "exec  SP_InsertEmploye " + employe.EmployeId + ",' " + employe.EmployeName + "','" + employe.EmailId + "'";
                    con.Open();
                    cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    return new JsonResult(ex.Message);
                }
                finally
                {
                    con.Close();
                }
                return new JsonResult(employe);
            }
        }
        [HttpPut]
        public JsonResult UpdateEmployee(int EmployeId, Employe1 employe)
        {
            using (SqlConnection con = new SqlConnection(_configuration.GetConnectionString("TestDatabase")))

                try
                {
                SqlCommand cmd = con.CreateCommand();
                cmd.CommandType = CommandType.Text;
                    cmd.CommandText = "EXEC SP_UpdateEmployeInfo "+employe.EmployeId+" ,'" + employe.EmployeName + "','" + employe.EmailId + "'";
                    con.Open();
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                return new JsonResult(ex.Message);
            }
            finally
            {
                con.Close();
            }
            return new JsonResult(employe);
        }
        [HttpDelete]
        public JsonResult DeleteEmployee(int EmployeId,Employe1 employe)
        {
            using (SqlConnection con = new SqlConnection(_configuration.GetConnectionString("TestDatabase")))
                try
            {
                SqlCommand cmd = con.CreateCommand();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "Exec SP_DeleteEmployeInfo " + employe.EmployeId + " ";
                con.Open();
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                    return new JsonResult(ex.Message);
                }
            finally
            {
                con.Close();
            }
            return new JsonResult(employe);
        }
    }
}
