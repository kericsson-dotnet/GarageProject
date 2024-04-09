using Core;

var ui = new ConsoleUI.UI();
var garage = ui.CreateGarage();
var handler = new GarageHandler(garage);

ui.PopulateGarage(handler);
while (true)
{
    ui.MainMenu(handler);
}