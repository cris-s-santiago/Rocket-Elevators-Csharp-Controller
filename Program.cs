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
        public List<Column> columnsList = new List<Column>();
        public List<CallButton> callButtonsList = new List<CallButton>();

        public Battery(int _id, int _amountOfColumns, String _status, int _amountOfFloors, int _amountOfBasements, int _amountOfElevatorPerColumn)
        {
            this.ID = _id;
            this.amountOfColumns = _amountOfColumns;
            this.status = _status;
            this.amountOfFloors = _amountOfFloors;
            this.amountOfBasements = _amountOfBasements;
            if(_amountOfBasements > 0){
            this.createBasementCallButtonsList(_amountOfBasements);
            this.createBasementColumn(_amountOfBasements, _amountOfElevatorPerColumn);
            _amountOfColumns --;
            }
            _amountOfFloors = _amountOfFloors-_amountOfBasements;
            this.createCallButtonsList(_amountOfFloors);
            this.createColumns(_amountOfColumns, _amountOfFloors, _amountOfElevatorPerColumn);            
        }

        int columnID = 1;
        int callButtonID = 1;

        public void createBasementColumn(int _amountOfBasements, int _amountOfElevatorPerColumn)
        {
           List<int> servedFloors = new List<int>();
           int floor = -1;           

           for(int i = 1; i <= _amountOfBasements; i++){
              servedFloors.Add(floor);
              floor --;
           }

            this.columnsList.Add(new Column(columnID, "oline", _amountOfElevatorPerColumn, servedFloors, true));
            columnID ++;
        }

        public void createColumns(int _amountOfColumns, int _amountOfFloors, int _amountOfElevatorPerColumn)
        {
            float FloorsPerColumn = (_amountOfFloors/_amountOfColumns);
            int amountOfFloorsPerColumn = Convert.ToInt32(Math.Ceiling(FloorsPerColumn));            
            List<int> servedFloors = new List<int>();
            int floor = 1;
             
            for(int i = 1; i <= _amountOfColumns; i++){                
                for(int x = 1; x <= amountOfFloorsPerColumn; x++){                    
                    if(floor <= _amountOfFloors){
                        servedFloors.Add(floor);
                        floor ++;
                    }
                }                                   
                this.columnsList.Add(new Column(columnID, "oline", _amountOfElevatorPerColumn, servedFloors, false));
                columnID ++;
                servedFloors.Clear();
            }   
            
        }

        public void createBasementCallButtonsList(int _amountOfBasements)
        {
            int buttonFloor = -1;           

           for(int i = 1; i <= _amountOfBasements; i++){
              this.callButtonsList.Add(new CallButton(callButtonID, "off", buttonFloor, "up"));
              buttonFloor --;
              callButtonID ++;
           }
        }

        public void createCallButtonsList(int _amountOfFloors)
        {
           for(int buttonFloor = 1; buttonFloor <= _amountOfFloors; buttonFloor++){
              this.callButtonsList.Add(new CallButton(callButtonID, "off", buttonFloor, "down"));
              callButtonID ++;
           }
        }

        public Column findBestColumn (int _requestedFloor)
        {
            Column foundColumn = null;
            foreach( Column column in this.columnsList)
            {
                bool searchedColumn = column.servedFloors.Exists(element => element == _requestedFloor);
                if(searchedColumn)
                {
                    foundColumn = column;                    
                }                
            }
            return foundColumn;
        }

        public void assignElevator(int _requestedFloor, String _direction)
        {
            Column column = this.findBestColumn(_requestedFloor);
            //Elevator elevator = column.bestElevator(1, _direction);
            //elevator.floorRequestList.Add(_requestedFloor);
            // elevator.sortFloorList();
            // elevator.capacityCalculate();
            // elevator.movElev(_requestedFloor, _direction);

        }
    }
    
    
    class Column
    {
        public int ID;
        public String status;
        public int amountOfElevators;
        public List<Elevator> elevatorsList = new List<Elevator>();
        public List<FloorRequestButton> floorRequestButtonsList = new List<FloorRequestButton>();
        public List<int> servedFloors = new List<int>();

        public Column(int _id, String _status, int _amountOfElevators, List<int> _servedFloors, Boolean _isBasement)
        {
            this.ID = _id;
            this.status = _status;
            this.amountOfElevators = _amountOfElevators;
            this.servedFloors = _servedFloors;
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
        Battery mainBattery = new Battery(1, 4, "OnLine", 66, 6, 5);
        //Console.WriteLine("Hello World!");    
        }
    }
}

