using BlazorWebApp.Server.Services;
using BlazorWebApp.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorWebApp.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {
        private readonly IEmployeeRepository employeeRepository;

        public EmployeesController(IEmployeeRepository employeeRepository)
        {
            this.employeeRepository = employeeRepository;
        }

        [HttpGet]
        public async Task<ActionResult> GetEmployees()
        {
            try
            {
                return Ok(await employeeRepository.GetEmployees());
            }
            catch (Exception)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, "Error retrivieving all data from database");
            }

        }

        [HttpGet("{Search}")]
        public async Task<ActionResult<IEnumerable<Employee>>> Search(string name, Gender gender)
        {
            try
            {
                var result = await employeeRepository.SearchEmployee(name, gender);
                if (result.Any()) return Ok(result);
                return NotFound();

            }
            catch (Exception)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, "Error while retrieving data by search criteria");
            }
        }
        [HttpGet("{Id:int}")]
        public async Task<ActionResult<Employee>> GetEmployee(int Id)
        {
            try
            {
                var result = await employeeRepository.GetEmployee(Id);
                if (result == null) return NotFound();
                return result;
            }
            catch (Exception ex)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, "Error retrivieving data by ID from database");
            }

        }
        [HttpPost]
        public async Task<ActionResult<Employee>> CreateEmployee(Employee employee)
        {
            try
            {
                if (employee == null) BadRequest();
                if(employee!= null)
                {
                    var duplicateEmail = await employeeRepository.GetEmployeeByEmail(employee.Email);
                    if (duplicateEmail != null)
                    {
                        ModelState.AddModelError("Email", "Email already exists and it is already in use");
                        return BadRequest(ModelState);
                    }
                }
                var createdEmployee = await employeeRepository.AddEmployee(employee);
                return CreatedAtAction(nameof(GetEmployee), new { Id = createdEmployee.EmployeeId}, createdEmployee);
            }
            catch (Exception)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, "Error creating new employee");
            }

        }
        [HttpPut("{Id:int}")]
        public async Task<ActionResult<Employee>> UpdateEmployee(int Id, Employee employee)
        {
            try
            {
                if (Id != employee.EmployeeId) return BadRequest("Employee Id is not matching");
                var employeeToUpdate = await employeeRepository.GetEmployee(Id);
                if (employeeToUpdate == null) return NotFound($"Employee with Id = {Id} not found");
                return await employeeRepository.UpdateEmployee(employee);
            }
            catch (Exception)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, "Error updating employee");
            }

        }

        [HttpDelete("{Id:int}")]
        public async Task<ActionResult> DeleteEmployee(int Id)
        {
            try
            {
                var employeeToDelete = await employeeRepository.GetEmployee(Id);
                if (employeeToDelete == null) return NotFound($"Employee with Id = {Id} not found");
                await employeeRepository.DeleteEmployee(Id);
                return Ok($"Employee with Id = {Id} deleted");
            }
            catch (Exception)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, "Error deleting employee");
            }

        }


    }
}
