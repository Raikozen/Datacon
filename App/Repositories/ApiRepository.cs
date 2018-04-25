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

		/*
		/// ||==========================================
		/// || THIS FUNCTION IS CURRENTLY BROKEN!!
		/// || SERIOUSLY, DO NOT USE IT!
		/// || 
		/// || (╯°□°）╯︵ ┻━┻
		/// ||==========================================
		/// <summary>
		/// Get holidays by email adress async
		/// </summary>
		/// <param name="email"></param>
		/// <returns></returns>
		public static async Task<List<Holiday>> GetHolidaysByEmailAsync(string email)
		{
			var input = new FormUrlEncodedContent(new[]
			{
				new KeyValuePair<string, string>("email", email)
			});

			//Make a http request to the API
			HttpResponseMessage response = await Client.PostAsync("VakantieDagenPerPersoon/", input);

			string jsonString = response.Content.ReadAsStringAsync().Result;
			JObject jObj = (JObject)JsonConvert.DeserializeObject(jsonString);

			List<Holiday> holidays = new List<Holiday>();

			foreach(var item in jObj["gebruikers"])
			{
				foreach(var holiday in item["geplanned"])
				{
					holidays.Add(new Holiday(
						holiday["id"].Value<int>(),
						holiday["datum_van"].Value<string>(),
						holiday["datum_tot"].Value<string>(),
						holiday["omschrijving"].Value<string>(),
						holiday["totaal_aantal_uren"].Value<int>(),
						holiday["status"].Value<string>()
					));
				}
			}

			return holidays;
		}
		*/


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
