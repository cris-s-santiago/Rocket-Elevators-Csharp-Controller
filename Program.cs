using System;
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
        public List<FloorRequestButton> floorRequestButtonsList  = new List<FloorRequestButton>();        

        public Battery(int _id, int _amountOfColumns, String _status, int _amountOfFloors, int _amountOfBasements, int _amountOfElevatorPerColumn)
        {
            this.ID = _id;
            this.amountOfColumns = _amountOfColumns;
            this.status = _status;
            this.amountOfFloors = _amountOfFloors;
            this.amountOfBasements = _amountOfBasements;
            
            if(_amountOfBasements > 0)
            {
                this.createBasementFloorRequestButtons(_amountOfBasements);
                this.createBasementColumn(_amountOfBasements, _amountOfElevatorPerColumn);
                _amountOfColumns --;
            }
            
            this.createFloorRequestButtons(_amountOfFloors);
            this.createColumns(_amountOfColumns, _amountOfFloors, _amountOfElevatorPerColumn);            
        }

        public void createBasementColumn(int _amountOfBasements, int _amountOfElevatorPerColumn)
        {
           List<int> servedFloorsList = new List<int>();
           servedFloorsList.Add(1);
           int floor = -1;           

           for(int i = 1; i <= _amountOfBasements; i++)
           {
              servedFloorsList.Add(floor);
              floor --;
           }
            Column column = new Column(Buttons.columnID, "A", "online", _amountOfBasements, _amountOfElevatorPerColumn, servedFloorsList, true);
            this.columnsList.Add(column);
            Buttons.columnID ++;
        }

        public void createColumns(int _amountOfColumns, int _amountOfFloors, int _amountOfElevatorPerColumn)
        {
            List<String> columnNameList = new List<String>(new []{ "B", "C", "D" });
            
            float floorsPerColumn = (_amountOfFloors/_amountOfColumns);
            int amountOfFloorsPerColumn = Convert.ToInt32(Math.Ceiling(floorsPerColumn));            
            int floor = 1;
             
            for(int i = 1; i <= _amountOfColumns; i++)
            {        
                List<int> servedFloorsList = new List<int>();                     
                for(int x = 1; x <= amountOfFloorsPerColumn; x++)
                {                                       
                    if(floor <= _amountOfFloors)
                    {
                        servedFloorsList.Add(floor);
                        floor ++;
                    }
                }
                bool searchedFloor = servedFloorsList.Exists(element => element == 1);
                if(searchedFloor == false)
                {
                    servedFloorsList.Add(1);
                }
                servedFloorsList.Sort((a, b) => a.CompareTo(b));
                Column column = new Column(Buttons.columnID, columnNameList[i-1], "online", amountOfFloorsPerColumn, _amountOfElevatorPerColumn, servedFloorsList, false); 
                this.columnsList.Add(column);
                Buttons.columnID ++;
            }   
            
        }

        public void createFloorRequestButtons(int _amountOfFloors)
        {
           for(int buttonFloor = 1; buttonFloor <= _amountOfFloors; buttonFloor++)
           {
              FloorRequestButton floorRequestButton = new FloorRequestButton(Buttons.floorRequestButtonID, "off", buttonFloor, "up");
              this.floorRequestButtonsList.Add(floorRequestButton);
              Buttons.floorRequestButtonID ++;
           }
        }
        
        public void createBasementFloorRequestButtons(int _amountOfBasements)
        {
            int buttonFloor = -1;           

           for(int i = 1; i <= _amountOfBasements; i++)
           {
              FloorRequestButton floorRequestButton = new FloorRequestButton(Buttons.floorRequestButtonID, "off", buttonFloor, "down");
              this.floorRequestButtonsList.Add(floorRequestButton);
              buttonFloor --;
              Buttons.floorRequestButtonID ++;
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
            Console.WriteLine("- Selected Column: " + column.name);
            Elevator elevator = column.findElevator(1, _direction);
            Console.WriteLine("- Selected Elevator: " + elevator.name);
            elevator.floorRequestList.Add(_requestedFloor);
            elevator.move();            
        }
    }

    static class Buttons
    {
        public static int callButtonID = 1;
        public static int buttonFloor = 1;
        public static int elevatorID = 1;
        public static int columnID = 1;
        public static int floorRequestButtonID = 1; 
    }

    class Column
    {
        public int ID;
        public String name;
        public String status;
        public int amountOfElevators;
        public List<Elevator> elevatorsList = new List<Elevator>();
        public List<CallButton> callButtonList = new List<CallButton>();
        public List<int> servedFloors = new List<int>();

        public Column(int _id, String _name, String _status, int _amountOfFloors, int _amountOfElevators, List<int> _servedFloors, Boolean _isBasement)
        {
            this.ID = _id;
            this.name = _name;
            this.status = _status;
            this.amountOfElevators = _amountOfElevators;
            this.servedFloors = _servedFloors;

            this.createElevators(_amountOfFloors, _amountOfElevators);
            this.createCallButtons(_amountOfFloors, _isBasement);
        }
               

        public void createCallButtons(int _amountOfFloors, Boolean _isBasement)
        {
            if(_isBasement)
            {
                int buttonBasement = -1;
                for(int i = 1; i <= _amountOfFloors; i ++)
                {
                    CallButton callButton = new CallButton (Buttons.callButtonID, "off", buttonBasement, "up");
                    this.callButtonList.Add(callButton);
                    buttonBasement --;
                    Buttons.callButtonID ++;
                }
            }
            else
            {                
                for(int i = 1; i <= _amountOfFloors; i++)
                {
                    CallButton callButton = new CallButton (Buttons.callButtonID, "off", Buttons.buttonFloor, "down");
                    this.callButtonList.Add(callButton);
                    Buttons.buttonFloor ++;
                    Buttons.callButtonID ++;
                }
            }
        }

        public void createElevators (int _amountOfFloors, int _amountOfElevators)
        {            
            for(int i = 1; i <= _amountOfElevators; i ++)
            {
                Elevator elevator = new Elevator(Buttons.elevatorID, this.name + (i), "idle", _amountOfFloors, 1);
                this.elevatorsList.Add(elevator);
                Buttons.elevatorID ++;
            }
        }

        public Elevator requestElevator(int _requestedFloor, String _direction)
        {        
            Console.WriteLine("- Current column: " + this.name);
            Elevator elevator = this.findElevator(_requestedFloor, _direction);
            Console.WriteLine("- Selected Elevator: " + elevator.name);
            elevator.floorRequestList.Add(_requestedFloor);
            elevator.sortFloorList();
            elevator.move();

            return elevator;
        }

        public Elevator findElevator(int _requestedFloor, String _direction)
        {
            BestElevatorInfo bestElevatorInfo = new BestElevatorInfo(null, 7, 10000000);

            if (_requestedFloor == 1)
            {
                foreach(Elevator elevator in this.elevatorsList)
                {
                    //The elevator is at the lobby and already has some requests. It is about to leave but has not yet departed
                    if (_requestedFloor == elevator.currentFloor && elevator.status == "stopped")
                    {
                        bestElevatorInfo = checkIfElevatorISBetter(1, elevator, bestElevatorInfo, _requestedFloor);
                    }
                    //The elevator is at the lobby and has no requests
                    else if (_requestedFloor == elevator.currentFloor && elevator.status == "idle")
                    {
                        bestElevatorInfo = checkIfElevatorISBetter(2, elevator, bestElevatorInfo, _requestedFloor);
                        //The elevator is lower than me and is coming up. It means that I am requesting an elevator to go to a basement, and the elevator is on it's way to me.
                    } else if (_requestedFloor > elevator.currentFloor && elevator.direction == "up")
                    {
                        bestElevatorInfo = checkIfElevatorISBetter(3, elevator, bestElevatorInfo, _requestedFloor);
                    }
                    //The elevator is above me and is coming down. It means that I'm requesting an elevator to go to a floor, and the elevator is on it's way to me
                    else if (_requestedFloor < elevator.currentFloor && elevator.direction == "down")
                    {
                        bestElevatorInfo = checkIfElevatorISBetter(3, elevator, bestElevatorInfo, _requestedFloor);
                    }//The elevator is not at the first floor, but doesn't have any request
                    else if (elevator.status == "idle")
                    {
                        bestElevatorInfo = checkIfElevatorISBetter(4, elevator, bestElevatorInfo, _requestedFloor);
                    } //The elevator is not available, but still could take the call if nothing better is found
                    else
                    {
                        bestElevatorInfo = checkIfElevatorISBetter(5, elevator, bestElevatorInfo, _requestedFloor);
                    }
                }
            } else
            {
                foreach(Elevator elevator in this.elevatorsList)
                {
                    //The elevator is at the same level as me, and is about to depart to the first floor
                    if (_requestedFloor == elevator.currentFloor && elevator.status == "stopped" && _direction == elevator.direction)
                    {
                        bestElevatorInfo = checkIfElevatorISBetter(1, elevator, bestElevatorInfo, _requestedFloor);
                    }
                    //The elevator is lower than me and is going up. I'm on a basement, and the elevator can pick me up on it's way
                    else if (_requestedFloor > elevator.currentFloor && elevator.direction == "up" && _direction == "up")
                    {
                        bestElevatorInfo = checkIfElevatorISBetter(2, elevator, bestElevatorInfo, _requestedFloor);
                    } //The elevator is higher than me and is going down. I'm on a floor, and the elevator can pick me up on it's way
                    else if (_requestedFloor < elevator.currentFloor && elevator.direction == "down" && _direction == "down")
                    {
                        bestElevatorInfo = checkIfElevatorISBetter(2, elevator, bestElevatorInfo, _requestedFloor);
                    }//The elevator is idle and has no requests
                    else if (elevator.status == "idle")
                    {
                        bestElevatorInfo = checkIfElevatorISBetter(4, elevator, bestElevatorInfo, _requestedFloor);
                    }//The elevator is not available, but still could take the call if nothing better is found
                    else
                    {
                        bestElevatorInfo = checkIfElevatorISBetter(5, elevator, bestElevatorInfo, _requestedFloor);
                    }
                }        
            }
            return bestElevatorInfo.bestElevator;
        }

        public BestElevatorInfo checkIfElevatorISBetter(int scoreToCheck, Elevator newElevator, BestElevatorInfo bestElevatorInfo, int _requestedFloor){
            if(scoreToCheck < bestElevatorInfo.bestScore)
            {
                bestElevatorInfo.bestScore = scoreToCheck;
                bestElevatorInfo.bestElevator = newElevator;
                bestElevatorInfo.referanceGap = Math.Abs(newElevator.currentFloor - _requestedFloor); 
            }else if(bestElevatorInfo.bestScore == scoreToCheck)
            {
                int gap = Math.Abs(newElevator.currentFloor - _requestedFloor);
                if(bestElevatorInfo.referanceGap > gap)
                {
                    bestElevatorInfo.bestScore = scoreToCheck;
                    bestElevatorInfo.bestElevator = newElevator;
                    bestElevatorInfo.referanceGap = gap;
                }
            }
            return bestElevatorInfo;
        }
    }

    class Elevator
    {
        public int ID;
        public String name;
        public String status;
        public int amountOfFloors;
        public int currentFloor;
        public Door door;
        public List<int> floorRequestList = new List<int>();
        public String direction;
        public int screenDisplay;

        public Elevator(int _id, String _name, String _status, int _amountOfFloors, int _currentFloor)
        {
            this.ID = _id;
            this.name = _name;
            this.status = _status;
            this.amountOfFloors = _amountOfFloors;
            this.currentFloor = _currentFloor;
            this.door = new Door(this.ID, "closed");
            this.direction = null;
            this.screenDisplay = _currentFloor;
        }

        public void move()
        {   
            while(this.floorRequestList.Count != 0)
            {
                int destination = this.floorRequestList[0];
                this.operateDoors("closed");
                if(this.door.status == "closed")
                {
                    Console.WriteLine("Status door:" + this.door.status + "\n");
                    this.status = "moving";
                    this.screenDisplay = this.currentFloor;
                    Console.WriteLine("Elevator Status: " + this.status + " ||  Elevator Display: " + this.screenDisplay);
                    if(this.currentFloor < destination)
                    {
                        this.direction = "up";
                        while(this.currentFloor < destination)
                        {
                            this.currentFloor ++;
                            if(this.currentFloor != 0)
                            {
                                this.screenDisplay = this.currentFloor;
                                Console.WriteLine("Elevator Status: " + this.status + " ||  Elevator Display: " + this.screenDisplay);
                            }                            
                        }
                    }
                    else if(this.currentFloor > destination)
                    {
                        this.direction = "down";
                        while(this.currentFloor > destination)
                        {
                            this.currentFloor --;
                            this.screenDisplay = this.currentFloor;
                            Console.WriteLine("Elevator Status: " + this.status + " ||  Elevator Display: " + this.screenDisplay);
                        }
                    }
                    this.status = "stopped";
                    Console.WriteLine("Elevator Status: " + this.status + "\n");
                    this.operateDoors("openned");
                    Console.WriteLine("Status door:" + this.door.status + "\n");
                }
                this.floorRequestList.RemoveAt(0);
            }
            this.status = "idle";
        }

        public void sortFloorList()
        {
            if (this.direction == "up"){
                this.floorRequestList.Sort((a, b) => a.CompareTo(b));
            }else{
                this.floorRequestList.Sort((a, b) => b.CompareTo(a));
            }
        }

        public void operateDoors(String _command)
        {
        var sensorDoor = false; // External data
            if(sensorDoor == false){
                this.door.status = _command;
            }else
            {
                Console.WriteLine("Blocked door");
            }
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

        // public override string ToString() {
        //     return this.ID + ", " + this.status + ", " + floor + ", " + direction;
        // }

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

    class BestElevatorInfo
    {
        public Elevator bestElevator;
        public int bestScore;
        public int referanceGap;

        public BestElevatorInfo(Elevator _bestElevator, int _bestScore, int _referanceGap)
        {
            this.bestElevator = _bestElevator;
            this.bestScore = _bestScore;
            this.referanceGap = _referanceGap;
        }
    }

    class Program
    {        
        static void Main(string[] args)
        {
            Battery battery1 = null;

            scenario1();
            scenario2();
            scenario3();
            scenario4();

            //-------------------------------------"    Battery Initialization   "-------------------------------------
            void createBattery()
            {
                battery1 = new Battery(1, 4, "OnLine", 60, 6, 5);
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("=======================| Creating the Battery |=======================");
                Console.ResetColor();
                Console.WriteLine("New  Battery ID = " + battery1.ID + " || Status =  " + battery1.status + " || Number of Columns =  " + battery1.amountOfColumns + " || Number of Floors =  " + battery1.amountOfFloors + " || Number of Basements =  " + battery1.amountOfBasements);
                Console.WriteLine("\n");
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("==================| Creating the Columns |=======================");
                Console.ResetColor();
                foreach (Column column in battery1.columnsList)
                {
                    Console.WriteLine("Column: " + column.name + "  ||  " 
                    + "Status: " + column.status 
                    + " || Floors served =  " + String.Join(", ", column.servedFloors));
                }           
                Console.WriteLine("\n");
            }

            //-------------------------------------"    Scenario 1   "-------------------------------------
            void scenario1()
            {              
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("=============================================| Scenario 1 |=============================================");
                Console.ForegroundColor = ConsoleColor.Yellow;                
                Console.ResetColor();
                Console.WriteLine("\n");

                createBattery();
                
                Console.WriteLine("Someone at RC wants to go to the 20th floor");
                Console.WriteLine("\n");

                battery1.columnsList[1].elevatorsList[0].direction = "down";
                battery1.columnsList[1].elevatorsList[0].status = "moving";
                battery1.columnsList[1].elevatorsList[0].currentFloor = 20;
                battery1.columnsList[1].elevatorsList[0].floorRequestList.Add(5);

                battery1.columnsList[1].elevatorsList[1].direction = "up";
                battery1.columnsList[1].elevatorsList[1].status = "moving";
                battery1.columnsList[1].elevatorsList[1].currentFloor = 3;
                battery1.columnsList[1].elevatorsList[1].floorRequestList.Add(15);

                battery1.columnsList[1].elevatorsList[2].direction = "down";
                battery1.columnsList[1].elevatorsList[2].status = "moving";
                battery1.columnsList[1].elevatorsList[2].currentFloor = 13;
                battery1.columnsList[1].elevatorsList[2].floorRequestList.Add(1);

                battery1.columnsList[1].elevatorsList[3].direction = "down";
                battery1.columnsList[1].elevatorsList[3].status = "moving";
                battery1.columnsList[1].elevatorsList[3].currentFloor = 15;
                battery1.columnsList[1].elevatorsList[3].floorRequestList.Add(2);

                battery1.columnsList[1].elevatorsList[4].direction = "down";
                battery1.columnsList[1].elevatorsList[4].status = "moving";
                battery1.columnsList[1].elevatorsList[4].currentFloor = 6;
                battery1.columnsList[1].elevatorsList[4].floorRequestList.Add(1);

                battery1.assignElevator(20, "up"); 
            }

            //-------------------------------------"    Scenario 2   "-------------------------------------
            void scenario2()
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("=============================================| Scenario 2 |=============================================");
                Console.ForegroundColor = ConsoleColor.Yellow;                
                Console.ResetColor();
                Console.WriteLine("\n");

                createBattery(); 

                Console.WriteLine("Someone at RC wants to go to the 36th floor");
                Console.WriteLine("\n");

                battery1.columnsList[2].elevatorsList[0].direction = "up";
                battery1.columnsList[2].elevatorsList[0].status = "stopped";
                battery1.columnsList[2].elevatorsList[0].currentFloor = 1;
                battery1.columnsList[2].elevatorsList[0].floorRequestList.Add(21);

                battery1.columnsList[2].elevatorsList[1].direction = "up";
                battery1.columnsList[2].elevatorsList[1].status = "moving";
                battery1.columnsList[2].elevatorsList[1].currentFloor = 23;
                battery1.columnsList[2].elevatorsList[1].floorRequestList.Add(28);

                battery1.columnsList[2].elevatorsList[2].direction = "down";
                battery1.columnsList[2].elevatorsList[2].status = "moving";                
                battery1.columnsList[2].elevatorsList[2].currentFloor = 33;
                battery1.columnsList[2].elevatorsList[2].floorRequestList.Add(1);

                battery1.columnsList[2].elevatorsList[3].direction = "down";
                battery1.columnsList[2].elevatorsList[3].status = "moving";                
                battery1.columnsList[2].elevatorsList[3].currentFloor = 40;
                battery1.columnsList[2].elevatorsList[3].floorRequestList.Add(24);

                battery1.columnsList[2].elevatorsList[4].direction = "down";
                battery1.columnsList[2].elevatorsList[4].status = "moving";                
                battery1.columnsList[2].elevatorsList[4].currentFloor = 39;
                battery1.columnsList[2].elevatorsList[4].floorRequestList.Add(1);

                battery1.assignElevator(36, "up"); 
            }

            //-------------------------------------"    Scenario 3   "-------------------------------------
            void scenario3()
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("=============================================| Scenario 3 |=============================================");
                Console.ForegroundColor = ConsoleColor.Yellow;                
                Console.ResetColor();
                Console.WriteLine("\n");

                createBattery();
                
                Console.WriteLine("Someone at 54e floor wants to go to RC");
                Console.WriteLine("\n");

                battery1.columnsList[3].elevatorsList[0].direction = "down";
                battery1.columnsList[3].elevatorsList[0].status = "moving";
                battery1.columnsList[3].elevatorsList[0].currentFloor = 58;
                battery1.columnsList[3].elevatorsList[0].floorRequestList.Add(1);

                battery1.columnsList[3].elevatorsList[1].direction = "up";
                battery1.columnsList[3].elevatorsList[1].status = "moving";
                battery1.columnsList[3].elevatorsList[1].currentFloor = 50;
                battery1.columnsList[3].elevatorsList[1].floorRequestList.Add(60);

                battery1.columnsList[3].elevatorsList[2].direction = "up";
                battery1.columnsList[3].elevatorsList[2].status = "moving";
                battery1.columnsList[3].elevatorsList[2].currentFloor = 46;
                battery1.columnsList[3].elevatorsList[2].floorRequestList.Add(58);

                battery1.columnsList[3].elevatorsList[3].direction = "up";
                battery1.columnsList[3].elevatorsList[3].status = "moving";
                battery1.columnsList[3].elevatorsList[3].currentFloor = 1;
                battery1.columnsList[3].elevatorsList[3].floorRequestList.Add(54);

                battery1.columnsList[3].elevatorsList[4].direction = "down";
                battery1.columnsList[3].elevatorsList[4].status = "moving";
                battery1.columnsList[3].elevatorsList[4].currentFloor = 60;
                battery1.columnsList[3].elevatorsList[4].floorRequestList.Add(1);

                battery1.columnsList[3].requestElevator(54, "down"); 
            }

            //-------------------------------------"    Scenario 4   "-------------------------------------
            void scenario4()
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("=============================================| Scenario 4 |=============================================");
                Console.ForegroundColor = ConsoleColor.Yellow;                
                Console.ResetColor();
                Console.WriteLine("\n");

                createBattery();
                
                Console.WriteLine("Someone at SS3 wants to go to RC");
                Console.WriteLine("\n");

                battery1.columnsList[0].elevatorsList[0].status = "idle";
                battery1.columnsList[0].elevatorsList[0].currentFloor = -4;

                battery1.columnsList[0].elevatorsList[1].status = "idle";
                battery1.columnsList[0].elevatorsList[1].currentFloor = 1;

                battery1.columnsList[0].elevatorsList[2].direction = "down";
                battery1.columnsList[0].elevatorsList[2].status = "moving";
                battery1.columnsList[0].elevatorsList[2].currentFloor = -3;
                battery1.columnsList[0].elevatorsList[2].floorRequestList.Add(-5);

                battery1.columnsList[0].elevatorsList[3].direction = "up";
                battery1.columnsList[0].elevatorsList[3].status = "moving";
                battery1.columnsList[0].elevatorsList[3].currentFloor = -6;
                battery1.columnsList[0].elevatorsList[3].floorRequestList.Add(1);

                battery1.columnsList[0].elevatorsList[4].direction = "down";
                battery1.columnsList[0].elevatorsList[4].status = "moving";
                battery1.columnsList[0].elevatorsList[4].currentFloor = -1;
                battery1.columnsList[0].elevatorsList[4].floorRequestList.Add(-6);

                battery1.columnsList[0].requestElevator(-3, "up"); 
            }
        }      
    }
}

