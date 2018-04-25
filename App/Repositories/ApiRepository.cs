using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using App.Models;

namespace App.Repositories
{
    public class ApiRepository
    {
		private static HttpClient client = null;

		//Client singleton
		private static HttpClient Client
		{
			get
			{
				if (client == null)
				{
					//Create the Http client
					client = new HttpClient();
					//Set baseaddress URI
					client.BaseAddress = new Uri("https://test.connect.boomi.com/ws/rest/fontys/");
					//Add required headers
					client.DefaultRequestHeaders.Accept.Clear();
					client.DefaultRequestHeaders.Accept.Add(
						new MediaTypeWithQualityHeaderValue("application/json")
					);
					//Authorization
					var byteArray = Encoding.ASCII.GetBytes("fontys@dataconbv-LXAOAC.ROJKE4:4f437fbf-0ffe-4ebf-82bf-b9caf4d8d404");
					client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));
				}

				return client;
			}
		}

		/// <summary>
		/// Get all sickreports async
		/// </summary>
		/// <returns></returns>
		public static async Task<List<ApiSickReport>> GetAllSickReportsAsync()
		{
			//Make a http request to the API
			HttpResponseMessage response = await Client.GetAsync("AlleZiekteMeldingen/");

			string jsonString = response.Content.ReadAsStringAsync().Result;
			JObject jObj = (JObject)JsonConvert.DeserializeObject(jsonString);

			List<ApiSickReport> listSickReports = new List<ApiSickReport>();

			//Iterate through the results
			foreach (var item in jObj["ziektedagen"])
			{
				foreach (var aanmelding in item["aanmeldingen"])
				{
					ApiSickReport report = new ApiSickReport(
						aanmelding["id"].Value<Int32>(),
						item["email"].Value<string>(),
						aanmelding["beschrijving"].Value<string>(),
						aanmelding["datum"].Value<string>()
					);

					listSickReports.Add(report);
				}
			}

			return listSickReports;
		}


		/// <summary>
		/// Get all agenda appointments async
		/// </summary>
		/// <returns></returns>
		public static async Task<List<ApiAgendaAppointment>> GetAllAgendaAppointmentsAsync()
		{
			//Make a http request to the API
			HttpResponseMessage response = await Client.GetAsync("AlleAgendaAfspraken/");

			string jsonString = response.Content.ReadAsStringAsync().Result;
			JObject jObj = (JObject)JsonConvert.DeserializeObject(jsonString);

			List<ApiAgendaAppointment> agendaAppointments = new List<ApiAgendaAppointment>();

			//Iterate through the results
			foreach (var item in jObj["agenda"])
			{
				foreach (var afspraak in item["afspraken"])
				{
					agendaAppointments.Add(new ApiAgendaAppointment(
						afspraak["id"].Value<int>(),
						item["email"].Value<string>(),
						afspraak["ruimte"].Value<string>(),
						afspraak["beschrijving"].Value<string>(),
						afspraak["datum"].Value<string>()
					));
				}
			}

			return agendaAppointments;
		}

		/// <summary>
		/// Get all availble rooms async
		/// </summary>
		/// <returns></returns>
		public static async Task<List<ApiRoom>> GetAllAvailableRoomsAsync()
		{
			//Make a http request to the API
			HttpResponseMessage response = await Client.GetAsync("AlleReserveerbareRuimtes/");

			string jsonString = response.Content.ReadAsStringAsync().Result;
			JObject jObj = (JObject)JsonConvert.DeserializeObject(jsonString);

			List<ApiRoom> availableRooms = new List<ApiRoom>();

			//Iterate through the results
			foreach (var item in jObj["ruimtes"])
			{
				availableRooms.Add(new ApiRoom(
					item["id"].Value<int>(),
					item["naam"].Value<string>()
				));
			}

			return availableRooms;
		}
	}
}
