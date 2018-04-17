using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarDealership.Data
{
    public interface ICarRepo
    {
        List<Car> GetAllCars();
        List<Car> GetAllNewCars();
        List<Car> GetAllUsedCars();
        Car Get(string vin);
        void AddCar(Car car);
        void EditCar(Car car);
        void DeleteCar(string vin);
        List<Make> GetAllMakes();
        List<Model> GetAllModels(Make make);
        void AddContact(Contact contact);
        void AddPurchase(Purchase purchase);
        void AddMake(Make make);
        void AddModel(Model model);
        void AddSpecial(Special special);
        void EditSpecial(Special special);
        void DeleteSpecial(int id);
        List<Special> GetAllSpecials();
        Special GetSpecialById(int id);
        List<string> GetExtColors();
        List<string> GetIntColors();
        List<string> GetBodyStyles();
        List<string> GetTransmissions();
        BodyStyle GetBodyStyle(string selectedBodyStyle);
        List<Model> GetAllModels();
        ExteriorColor GetExtColor(string color, string type);
        InteriorColor GetIntColor(string color, string type);
        Transmission GetTransmission(string selectedTransmision);
    }
}
