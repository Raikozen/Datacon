using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using App.Models;

namespace App.ViewModels
{
    public class IndexViewModel
    {
		public List<NewsfeedPost> NewsFeedPosts { get; set; }
		public List<ApiSickReport> Sickreports { get; set; }
		public List<ApiAgendaAppointment> AgendaAppointments { get; set; }
		public List<ApiRoom> Rooms { get; set; }


		public IndexViewModel(List<NewsfeedPost> newsFeedPosts, List<ApiSickReport> sickReports, List<ApiAgendaAppointment> agendaAppointments, List<ApiRoom> rooms)
		{
			this.NewsFeedPosts = newsFeedPosts;
			this.Sickreports = sickReports;
			this.AgendaAppointments = agendaAppointments;
			this.Rooms = rooms;
		}
    }
}
