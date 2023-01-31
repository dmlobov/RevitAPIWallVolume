using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RevitAPIWallVolume
{
    [Transaction(TransactionMode.Manual)]
    public class Main : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIApplication uiapp = commandData.Application;
            UIDocument uidoc = uiapp.ActiveUIDocument;
            Document doc = uidoc.Document;

            IList<Reference> selectedElementRefList = uidoc.Selection.PickObjects(ObjectType.Element, new WallFilter(), "Выберете элементы по грани");
            var wallList = new List<double>();
            double sumVolume = 0;
            foreach (var selectedElement in selectedElementRefList)
            {
                Wall owall = doc.GetElement(selectedElement) as Wall;
                Parameter volume = owall.get_Parameter(BuiltInParameter.HOST_VOLUME_COMPUTED);
                double volumeValue = UnitUtils.ConvertFromInternalUnits(volume.AsDouble(), DisplayUnitType.DUT_CUBIC_METERS);
                sumVolume += volumeValue;
            }
            TaskDialog.Show("Объем стен", sumVolume.ToString());

            return Result.Succeeded;
        }
    }
}
