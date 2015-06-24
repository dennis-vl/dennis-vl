using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace VanLeeuwen.Projects.WebPortal.BusinessLogics.DataTransfer
{
  public class XMLSettingsReader
  {
    private String xmlFilePath;
    private List<DataAccess.DataTransfer.CompanySettings> companySettingsList;

    public List<DataAccess.DataTransfer.CompanySettings> CompanySettingsList
    {
      get { return this.companySettingsList; }
    }

    public XMLSettingsReader(String xmlFilePath)
    {
      this.xmlFilePath = xmlFilePath;
    }

    public void ReadXML()
    {
      companySettingsList = new List<DataAccess.DataTransfer.CompanySettings>();

      XDocument xmlDoc = XDocument.Load(this.xmlFilePath);
      //XElement serviceSettings = xmlDoc.Element("ServiceSettings");

      List<XElement> companies = xmlDoc.Element("Settings").Element("companies").Elements("company").ToList();
      foreach (XElement company in companies)
      {
        ReadCompany(company);
      }
    }

    private void ReadCompany(XElement company)
    {
      DataAccess.DataTransfer.CompanySettings settings = new DataAccess.DataTransfer.CompanySettings();

      settings.DatabaseName = company.Element("databasename").Value;
      settings.XMLOutboxPath = company.Element("xmloutboxpath").Value;

      this.companySettingsList.Add(settings);
    }
  }
}
