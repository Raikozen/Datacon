using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using App.Models;

namespace App.Datalayer
{
    public interface IUserContext
    {
        User Login(string email, string password);
        void UpdateUserRole(User user, Role role);
        List<User> GetUserList();
        User GetUser(int userId);

        void RegisterInfix(string email, string password, string firstName, string infix, string lastName, string telnr);
        void RegisterNoInfix(string email, string password, string firstName, string lastName, string telnr);
		void Register(string email, string hashedPassword, byte[] salt, string firstName, string lastName, string telNr, string infix);

        void ReportSick(int userID);
        bool IsSick(int userID);
        void SicknessRestored(int userID);
        List<SickReport> GetSickReportsUser(int userID);
        List<SickReport> GetSickReportsAll();

        List<HolidayRequest> GetUnapprovedHolidayRequests();
        List<HolidayRequest> GetAllHolidayRequests();
        void AddHolidayRequest(HolidayRequest holidayRequest);
        void ApproveHolidayRequest(int Id);
    }
}
