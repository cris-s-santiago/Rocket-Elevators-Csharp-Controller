using System;
using System.Collections; 
using System.Collections.Generic;

namespace Commercial_Controller
{
    class Battery 
    {
        public int ID;
        public int amountOfColumns;
        public String status;
        public int amountOfFloors;
        public int amountOfBasements;
        //public List<Column> columnsList = new List<Column>();
        //public List<FloorRequestButton> floorRequestButtonsList = new List<FloorRequestButton>();    

        public Battery(int _id, int _amountOfColumns, String _status, int _amountOfFloors, int _amountOfBasements, int _amountOfElevatorPerColumn)
            {
                this.ID = _id;
                this.amountOfColumns = _amountOfColumns;
                this.status = _status;
                this.amountOfFloors = _amountOfFloors;
                this.amountOfBasements = _amountOfBasements;

                if(_amountOfBasements > 0){
                //this.createBasementFloorRequestButtons(_amountOfBasements);
                //this.createBasementColumn(_amountOfBasements, _amountOfElevatorPerColumn);
                _amountOfColumns --;
                }

                //this.createBasementFloorRequestButtons(_amountOfFloors);
                //this.createBasementColumn(_amountOfBasements, _amountOfElevatorPerColumn);            
            }
    }
    
    
    class Column
    {
        public int ID;
        public String status;
        public int amountOfFloors;
        public int amountOfElevators;
        public List<Elevator> elevatorsList = new List<Elevator>();
        public List<CallButton> callButtonsList = new List<CallButton>();
        public List<int> servedFloorsList = new List<int>();

        public Column(int _id, String _status, int _amountOfFloors, int _amountOfElevators, List<int> _servedFloors, Boolean _isBasement)
        {
            this.ID = _id;
            this.status = _status;
            this.amountOfFloors = _amountOfFloors;
            this.amountOfElevators = _amountOfElevators;
            this.servedFloorsList = _servedFloors;
        }  
    }


    class Elevator
    {
        public int ID;
        public String status;
        public int amountOfFloors;
        public int currentFloor;
        public object Door;
        public List<int> floorRequestList = new List<int>();
        public String direction;
        public int screenDisplay;

        public Elevator(int _id, String _status, int _amountOfFloors, int _currentFloor)
        {
            this.ID = _id;
            this.status = _status;
            this.amountOfFloors = _amountOfFloors;
            this.currentFloor = _currentFloor;
            this.Door = new Door(_id, "closed");
            this.direction = null;
            this.screenDisplay = _currentFloor;
        }
    }


    class CallButton
    {
        public int ID;
        public String status;
        public int floor;
        public String direction;

        public CallButton(int _id, String _status, int _floor, String _direction)
        {
            this.ID = _id;
            this.status = _status;
            this.floor = _floor;
            this.direction = _direction;
        }
    }


    class FloorRequestButton
    {
        public int ID;
        public String status;
        public int floor;
        public String direction;

        public FloorRequestButton(int _id, String _status, int _floor, String _direction)
        {
            this.ID = _id;
            this.status = _status;
            this.floor = _floor;
            this.direction = _direction;
        }
    }


    class Door
    {
        public int ID;
        public String status;

        public Door(int _id, string _status)
        {
            this.ID = _id;
            this.status = _status;
        }
    }


    class Program
    {
        static void Main(string[] args)
        {
        //Console.WriteLine("Hello World!");    
        }
    }
}

