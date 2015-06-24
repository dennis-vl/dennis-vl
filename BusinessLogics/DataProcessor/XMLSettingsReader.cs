using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using VanLeeuwen.Projects.WebPortal.DataAccess;

namespace VanLeeuwen.Projects.WebPortal.BusinessLogics.DataProcessor
{
	//public class XMLSettingsReader
	//{
	//	private String xmlFilePath;
	//	private List<CompanySettings> companySettingsList;

	//	public List<CompanySettings>  CompanySettingsList
	//	{
	//		get { return this.companySettingsList; }
	//	}

	//	public XMLSettingsReader(String xmlFilePath)
	//	{
	//		this.xmlFilePath = xmlFilePath;
	//	}

	//	public void ReadXML()
	//	{
	//		companySettingsList = new List<CompanySettings>();
      
	//		XDocument xmlDoc = XDocument.Load(this.xmlFilePath);
	//		//XElement serviceSettings = xmlDoc.Element("ServiceSettings");

	//		List<XElement> companies = xmlDoc.Element("Settings").Element("companies").Elements("company").ToList();
	//		foreach (XElement company in companies)
	//		{
	//			ReadCompany(company);
	//		}
	//	}

	//	private void ReadCompany(XElement company)
	//	{
	//		CompanySettings settings = new CompanySettings();

	//		settings.SQLServerName = company.Element("dbserver").Value;
	//		settings.SQLUserName = company.Element("dbuser").Value;
	//		settings.SQLPassword = company.Element("dbpassword").Value;
	//		settings.SBODatabaseName = company.Element("database").Value;
	//		settings.SBOUserName = company.Element("user").Value;
	//		settings.SBOPassword = company.Element("password").Value;

	//		this.companySettingsList.Add(settings);
	//	}
	//}
}
