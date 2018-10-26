namespace ExtensibilityExample
{
	using System;
	using System.ComponentModel.Composition;
	using Microsoft.Uii.Common.Entities;
	using Microsoft.Xrm.Tooling.PackageDeployment.CrmPackageExtentionBase;
	using Microsoft.Xrm.Sdk;
	/// <summary>
	/// This creates cardtype schema from solution after import
	/// </summary>
	[Export(typeof(IImportExtensions))]
	public class SchemaImporter : ImportExtension
	{
		/// <summary>
		/// Called When the package is initialized. 
		/// </summary>
		public override void InitializeCustomExtension()
		{
			// Do nothing. 
		}

		/// <summary>
		/// Called Before Import Completes. 
		/// </summary>
		/// <returns>true if card type created successfully, false otherwise</returns>
		public override bool BeforeImportStage()
		{
			return true;
		}


		/// <summary>
		/// Called for each UII record imported into the system
		/// This is UII Specific and is not generally used by Package Developers
		/// </summary>
		/// <param name="app">App Record</param>
		/// <returns></returns>
		public override ApplicationRecord BeforeApplicationRecordImport(ApplicationRecord app)
		{
			return app;  // do nothing here. 
		}

		/// <summary>
		/// Called during a solution upgrade while both solutions are present in the target CRM instance. 
		/// This function can be used to provide a means to do data transformation or upgrade while a solution update is occurring. 
		/// </summary>
		/// <param name="solutionName">Name of the solution</param>
		/// <param name="oldVersion">version number of the old solution</param>
		/// <param name="newVersion">Version number of the new solution</param>
		/// <param name="oldSolutionId">Solution ID of the old solution</param>
		/// <param name="newSolutionId">Solution ID of the new solution</param>
		public override void RunSolutionUpgradeMigrationStep(string solutionName, string oldVersion, string newVersion, Guid oldSolutionId, Guid newSolutionId)
		{
			base.RunSolutionUpgradeMigrationStep(solutionName, oldVersion, newVersion, oldSolutionId, newSolutionId);
		}

		/// <summary>
		/// Called after Import completes. 
		/// </summary>
		/// <returns>true, if solution deployed and schema imported successfully, exception otherwise</returns>
		public override bool AfterPrimaryImport()
		{
			OrganizationRequest request = new OrganizationRequest() { RequestName = "ImportCardTypeSchema" };
			request["SolutionUniqueName"] = "ExtensibilityExample";
			CrmSvc.Execute(request);
			return true;
		}

		#region Properties

		/// <summary>
		/// Name of the Import Package to Use
		/// </summary>
		/// <param name="plural">if true, return plural version</param>
		/// <returns></returns>
		public override string GetNameOfImport(bool plural)
		{
			return "Extensibility Example.";
		}

		/// <summary>
		/// Folder Name for the Package data. 
		/// </summary>
		public override string GetImportPackageDataFolderName
		{
			get
			{
				// WARNING this value directly correlates to the folder name in the Solution Explorer where the ImportConfig.xml and sub content is located. 
				// Changing this name requires that you also change the correlating name in the Solution Explorer 
				return "PkgFolder";
			}
		}

		/// <summary>
		/// Description of the package, used in the package selection UI
		/// </summary>
		public override string GetImportPackageDescriptionText
		{
			get { return "Extensibility package deployer"; }
		}

		/// <summary>
		/// Long name of the Import Package. 
		/// </summary>
		public override string GetLongNameOfImport
		{
			get { return "Extensibility Example"; }
		}
		#endregion

	}
}
