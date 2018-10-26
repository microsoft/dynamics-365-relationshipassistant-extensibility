# RA Card Extensibility Example

## Extending Card Type
1. Extensibility solution is placed inside ExtPkgDeployer/PkgFolder/extensibility_example_sol.zip, we will discuss about this solution later. First we will extend RA cards.

2. Build this solution from root

    ````bash
      msbuild ExtPkgDeployer.sln
    ````

3. After build is successful use *PkgFolder* and *ExtPkgDeployer.dll* from *ExtPkgDeployer\bin\Debug*
or *ExtPkgDeployer\bin\Release* and follow below 

    [documentation for package deployment](https://docs.microsoft.com/en-us/dynamics365/customer-engagement/admin/deploy-packages-using-package-deployer-windows-powershell#PD_tool)

    ![Deployment Preview](/images/extensibility_deployer.gif)

4. Verify new card type is created
**{URL}/api/data/v9.0/cardtype(2183dfc0-3c1c-45b7-a331-1943880c25c6)**

    ![Card Type Verification](/images/cardtype_verification.PNG)

5. Now its time to create Action card for new card type we just created, send a POST request

    ````code
    {URL}/api/data/v9.0/actioncards
    ````
    
    ```javascript
    {
    "cardtypeid@odata.bind": "/cardtype(2183dfc0-3c1c-45b7-a331-1943880c25c6)",
    "startdate": "2018-10-20T01:01:01Z",
    "expirydate": "2018-10-25T01:01:01Z",
    "visibility": true,
    "priority": 2000,
    "description": "This card should be visible between start and end date mentioned above",
    "title" : "Extensibility Example",
    "cardtype": 11000,
    "ownerid@odata.bind" : "/systemusers({USER_ID})"
    }
    ```

    ![Create Request](/images/actioncard_create.PNG)

6. Clear session cache, open developer console and run
    ````javascript
     sessionStorage.clear();
    ````

7. Refresh browser and you should see Action Card

    ![Action Card](/images/ActionCard.PNG)

8. Go to admin and you will see extended cards grouped by publishers.

    ![Admin Screen](/images/admin.PNG)

## Understanding Solution
* Unzip solution placed inside 
 ExtPkgDeployer/PkgFolder/extensibility_example_sol.zip, you will see 3 resources required for extensibility

 * new_cardtype_schemaxml9dd7e039-33a1-4778-9972-66536dc5e829
    * This contains schema definition for new cardtype
    ````xml
    <?xml version="1.0" encoding="utf-8" ?>
     <entity name="cardtype" displayname="Action Card Type">
       <cardname>Extensibility Example</cardname>
       <cardtypeid>2183dfc0-3c1c-45b7-a331-1943880c25c6</cardtypeid>
       <cardtype>11000</cardtype>
       <cardtypeicon>webresources/new_msicon</cardtypeicon>
       <softtitle>Extensibility Example</softtitle>
       <summarytext>RA Card Extensibility Example</summarytext> 
       <actions>{"WebClient":{"Actions":{"Open":"Mscrm.HomepageGrid.actioncard.CardCommand"},"Default":{"Open":"Mscrm.HomepageGrid.actioncard.CardCommand"}}, "Mobile":{"Actions":{"Open":"Mscrm.HomepageGrid.actioncard.CardCommand"}}}</actions>
    </entity>
     ````
 * new_commands8db43275-0291-401d-923a-90a6c373cc18
    * This contains command for Action Card
        ````javascript
        function CardCommand() {
	    window.open("https://aka.ms/salesai-raext");
        }
       ````
    * This command is invoked through RibbonDiff defined in customization.xml on Entity *ActionCard*
       ````xml
         <Entity>
         <Name LocalizedName="ActionCard" OriginalName="ActionCard">ActionCard</Name>
         <ObjectTypeCode>9962</ObjectTypeCode>
         <RibbonDiffXml>
           <CustomActions>
             <CustomAction Id="Mscrm.HomepageGrid.actioncard.CardCommand.CustomAction" Location="Mscrm.HomepageGrid.actioncard.MainTab.Actions.Controls._children" Sequence="12">
                <CommandUIDefinition>
                 <Button Id="Mscrm.HomepageGrid.actioncard.CardCommand" ToolTipTitle="Open" ToolTipDescription="Open" Command="Mscrm.HomepageGrid.actioncard.CardCommand" Sequence="12" LabelText="Open" Alt="Open" Image16by16="/WebResources/new_msicon" Image32by32="/WebResources/new_msicon" TemplateAlias="o1" ModernImage="new_msicon" />
               </CommandUIDefinition>
           </CustomAction>
           <CustomAction Id="Mscrm.SubGrid.actioncard.CardCommand.CustomAction" Location="Mscrm.SubGrid.actioncard.MainTab.Actions.Controls._children" Sequence="57">
              <CommandUIDefinition>
               <Button Id="Mscrm.HomepageGrid.actioncard.CardCommand" ToolTipTitle="Open" ToolTipDescription="Open" Command="Mscrm.HomepageGrid.actioncard.CardCommand" Sequence="29" LabelText="Open" Alt="Open" Image16by16="/WebResources/new_msicon" Image32by32="/WebResources/new_msicon" TemplateAlias="o1" ModernImage="new_msicon" />
            </CommandUIDefinition>
         </CustomAction>
      </CustomActions>
      <CommandDefinitions>
         <CommandDefinition Id="Mscrm.HomepageGrid.actioncard.CardCommand">
            <EnableRules>
               <EnableRule Id="Mscrm.SelectionCountExactlyOne" />
               <EnableRule Id="Mscrm.NotOffline" />
            </EnableRules>
            <DisplayRules />
            <Actions>
               <JavaScriptFunction FunctionName="CardCommand" Library="$webresource:new_commands">
                  <CrmParameter Value="SelectedControl" />
                  <CrmParameter Value="SelectedControlSelectedItemReferences" />
               </JavaScriptFunction>
            </Actions>
         </CommandDefinition>
      </CommandDefinitions>
      <RuleDefinitions>
         <TabDisplayRules />
         <DisplayRules />
         <EnableRules />
      </RuleDefinitions>
       </RibbonDiffXml>
      </Entity>
      ````
 * new_msicon6aa7c568-4830-4da6-89f9-18a8fd9c2285
    * This is our icon displayed with Action Card 
    
    ![ICON](/images/msicon.PNG)
## ImportCardTypeSchema Api
* This Api is being called by package deployer after solution install in *ExtPkgDeployer\SchemaImporter.cs*

    ```csharp
       public override bool AfterPrimaryImport()
		{
			OrganizationRequest request = new OrganizationRequest() { RequestName = "ImportCardTypeSchema" };
			request["SolutionUniqueName"] = "ExtensibilityExample";
			CrmSvc.Execute(request);
			return true;
		}
    ```
    
# Contributing

This project welcomes contributions and suggestions.  Most contributions require you to agree to a
Contributor License Agreement (CLA) declaring that you have the right to, and actually do, grant us
the rights to use your contribution. For details, visit https://cla.microsoft.com.

When you submit a pull request, a CLA-bot will automatically determine whether you need to provide
a CLA and decorate the PR appropriately (e.g., label, comment). Simply follow the instructions
provided by the bot. You will only need to do this once across all repos using our CLA.

This project has adopted the [Microsoft Open Source Code of Conduct](https://opensource.microsoft.com/codeofconduct/).
For more information see the [Code of Conduct FAQ](https://opensource.microsoft.com/codeofconduct/faq/) or
contact [opencode@microsoft.com](mailto:opencode@microsoft.com) with any additional questions or comments.
