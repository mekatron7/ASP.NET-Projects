using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CarDealership.Data
{
    public class InMemoryRepo : ICarRepo
    {
        public void AddCar(Car car)
        {
            throw new NotImplementedException();
        }

        public void AddContact(Contact contact)
        {
            throw new NotImplementedException();
        }

        public void AddMake(Make make)
        {
            throw new NotImplementedException();
        }

        public void AddModel(Model model)
        {
            throw new NotImplementedException();
        }

        public void AddPurchase(Purchase purchase)
        {
            throw new NotImplementedException();
        }

        public void AddSpecial(Special special)
        {
            throw new NotImplementedException();
        }

        public void DeleteCar(string vin)
        {
            throw new NotImplementedException();
        }

        public void DeleteSpecial(int id)
        {
            throw new NotImplementedException();
        }

        public void EditCar(Car car)
        {
            throw new NotImplementedException();
        }

        public void EditSpecial(Special special)
        {
            throw new NotImplementedException();
        }

        public Car Get(string vin)
        {
            throw new NotImplementedException();
        }

        public List<Car> GetAllCars()
        {
            throw new NotImplementedException();
        }

        public List<Make> GetAllMakes()
        {
            throw new NotImplementedException();
        }

        public List<Model> GetAllModels(Make make)
        {
            throw new NotImplementedException();
        }

        public List<Model> GetAllModels()
        {
            throw new NotImplementedException();
        }

        public List<Car> GetAllNewCars()
        {
            throw new NotImplementedException();
        }

        public List<Special> GetAllSpecials()
        {
            throw new NotImplementedException();
        }

        public List<Car> GetAllUsedCars()
        {
            throw new NotImplementedException();
        }

        public BodyStyle GetBodyStyle(string selectedBodyStyle)
        {
            throw new NotImplementedException();
        }

        public List<string> GetBodyStyles()
        {
            throw new NotImplementedException();
        }

        public Make GetByModelEdition(string model, string edition)
        {
            throw new NotImplementedException();
        }

        public ExteriorColor GetExtColor(string color, string type)
        {
            throw new NotImplementedException();
        }

        public List<string> GetExtColors()
        {
            throw new NotImplementedException();
        }

        public InteriorColor GetIntColor(string color, string type)
        {
            throw new NotImplementedException();
        }

        public List<string> GetIntColors()
        {
            throw new NotImplementedException();
        }

        public Special GetSpecialById(int id)
        {
            throw new NotImplementedException();
        }

        public Transmission GetTransmission(string selectedTransmision)
        {
            throw new NotImplementedException();
        }

        public List<string> GetTransmissions()
        {
            throw new NotImplementedException();
        }
    }
}