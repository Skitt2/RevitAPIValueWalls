using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RevitAPIValueWalls
{
    [Transaction(TransactionMode.Manual)]
    public class Main : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIApplication uiapp = commandData.Application;
            UIDocument uidoc = uiapp.ActiveUIDocument;
            Document doc = uidoc.Document;

            IList<Reference> selectedRefs = null;
            try
            {
                selectedRefs = uidoc.Selection.PickObjects(ObjectType.Element, "Выберите элемент");
            }
            catch (Autodesk.Revit.Exceptions.OperationCanceledException)
            {

            }
            if (selectedRefs == null)
                return Result.Cancelled;

            List<Wall> selectedElements = new List<Wall>();
            double volumeParameter = 0;
            foreach (Reference selectedRef in selectedRefs)
            {
                Element selectedElement = doc.GetElement(selectedRef);
                if (selectedElement is Wall)
                {
                    selectedElements.Add((Wall)selectedElement);
                    volumeParameter += selectedElement.get_Parameter(BuiltInParameter.HOST_VOLUME_COMPUTED).AsDouble();
                }
            }

            double volumeValue = UnitUtils.ConvertFromInternalUnits(volumeParameter, UnitTypeId.CubicMeters);
            TaskDialog.Show("Объем выбранных стен", $"Объем выбранных стен {volumeValue} м3");
            return Result.Succeeded;
        }
    }
}
