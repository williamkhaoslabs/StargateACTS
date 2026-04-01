import { Person } from '../models/person.model';
import { AstronautDuty } from '../models/astronaut-duty.model';

export const MOCK_PEOPLE: Person[] = [
  {
    personId: 1,
    name: 'Neil Armstrong',
    currentRank: 'Commander',
    currentDutyTitle: 'Lunar Mission Pilot',
    careerStartDate: '1966-03-01',
    careerEndDate: null
  },
  {
    personId: 2,
    name: 'Buzz Aldrin',
    currentRank: 'Colonel',
    currentDutyTitle: 'Lunar Module Pilot',
    careerStartDate: '1963-10-18',
    careerEndDate: null
  },
  {
    personId: 3,
    name: 'Sally Ride',
    currentRank: 'Mission Specialist',
    currentDutyTitle: 'RETIRED',
    careerStartDate: '1978-01-12',
    careerEndDate: '1987-06-30'
  },
  {
    personId: 4,
    name: 'John Glenn',
    currentRank: 'Colonel',
    currentDutyTitle: 'RETIRED',
    careerStartDate: '1959-04-09',
    careerEndDate: '1998-11-06'
  },
  {
    personId: 5,
    name: 'Mae Jemison',
    currentRank: '',
    currentDutyTitle: '',
    careerStartDate: null,
    careerEndDate: null
  },
  {
    personId: 6,
    name: 'Chris Hadfield',
    currentRank: 'Colonel',
    currentDutyTitle: 'Station Commander',
    careerStartDate: '1992-06-01',
    careerEndDate: null
  },
  {
    personId: 7,
    name: 'Valentina Tereshkova',
    currentRank: 'Major General',
    currentDutyTitle: 'RETIRED',
    careerStartDate: '1962-03-12',
    careerEndDate: '1997-04-27'
  },
  {
    personId: 8,
    name: 'Yuri Gagarin',
    currentRank: 'Colonel',
    currentDutyTitle: 'RETIRED',
    careerStartDate: '1960-03-07',
    careerEndDate: '1968-03-26'
  },
  {
    personId: 9,
    name: 'Peggy Whitson',
    currentRank: 'Colonel',
    currentDutyTitle: 'Station Commander',
    careerStartDate: '1996-04-26',
    careerEndDate: null
  },
  {
    personId: 10,
    name: 'Scott Kelly',
    currentRank: 'Captain',
    currentDutyTitle: 'Long Duration Mission Specialist',
    careerStartDate: '1996-08-01',
    careerEndDate: null
  },
  {
    personId: 11,
    name: 'Alan Shepard',
    currentRank: 'Rear Admiral',
    currentDutyTitle: 'RETIRED',
    careerStartDate: '1959-04-09',
    careerEndDate: '1974-08-01'
  },
  {
    personId: 12,
    name: 'Christina Koch',
    currentRank: 'Mission Specialist',
    currentDutyTitle: 'Artemis Crew Member',
    careerStartDate: '2013-06-17',
    careerEndDate: null
  }
];

export const MOCK_DUTIES: Record<string, AstronautDuty[]> = {
  'Neil Armstrong': [
    {
      id: 1,
      personId: 1,
      rank: 'Commander',
      dutyTitle: 'Lunar Mission Pilot',
      dutyStartDate: '1968-01-15',
      dutyEndDate: null
    },
    {
      id: 2,
      personId: 1,
      rank: 'Senior Pilot',
      dutyTitle: 'Orbital Mission Specialist',
      dutyStartDate: '1966-03-01',
      dutyEndDate: '1968-01-14'
    }
  ],
  'Buzz Aldrin': [
    {
      id: 3,
      personId: 2,
      rank: 'Colonel',
      dutyTitle: 'Lunar Module Pilot',
      dutyStartDate: '1966-11-11',
      dutyEndDate: null
    },
    {
      id: 4,
      personId: 2,
      rank: 'Major',
      dutyTitle: 'Gemini Pilot',
      dutyStartDate: '1963-10-18',
      dutyEndDate: '1966-11-10'
    }
  ],
  'Sally Ride': [
    {
      id: 5,
      personId: 3,
      rank: 'Mission Specialist',
      dutyTitle: 'RETIRED',
      dutyStartDate: '1987-07-01',
      dutyEndDate: null
    },
    {
      id: 6,
      personId: 3,
      rank: 'Mission Specialist',
      dutyTitle: 'Shuttle Crew Member',
      dutyStartDate: '1983-06-18',
      dutyEndDate: '1987-06-30'
    },
    {
      id: 7,
      personId: 3,
      rank: 'Candidate',
      dutyTitle: 'Astronaut Trainee',
      dutyStartDate: '1978-01-12',
      dutyEndDate: '1983-06-17'
    }
  ],
  'John Glenn': [
    {
      id: 8,
      personId: 4,
      rank: 'Colonel',
      dutyTitle: 'RETIRED',
      dutyStartDate: '1998-11-07',
      dutyEndDate: null
    },
    {
      id: 9,
      personId: 4,
      rank: 'Colonel',
      dutyTitle: 'Payload Specialist',
      dutyStartDate: '1998-01-16',
      dutyEndDate: '1998-11-06'
    },
    {
      id: 10,
      personId: 4,
      rank: 'Lieutenant Colonel',
      dutyTitle: 'Orbital Pilot',
      dutyStartDate: '1959-04-09',
      dutyEndDate: '1998-01-15'
    }
  ],
  'Chris Hadfield': [
    {
      id: 11,
      personId: 6,
      rank: 'Colonel',
      dutyTitle: 'Station Commander',
      dutyStartDate: '2012-12-19',
      dutyEndDate: null
    },
    {
      id: 12,
      personId: 6,
      rank: 'Colonel',
      dutyTitle: 'Mission Specialist',
      dutyStartDate: '2001-04-19',
      dutyEndDate: '2012-12-18'
    },
    {
      id: 13,
      personId: 6,
      rank: 'Major',
      dutyTitle: 'Shuttle Mission Specialist',
      dutyStartDate: '1992-06-01',
      dutyEndDate: '2001-04-18'
    }
  ],
  'Valentina Tereshkova': [
    {
      id: 14,
      personId: 7,
      rank: 'Major General',
      dutyTitle: 'RETIRED',
      dutyStartDate: '1997-04-28',
      dutyEndDate: null
    },
    {
      id: 15,
      personId: 7,
      rank: 'Colonel',
      dutyTitle: 'Cosmonaut Corps Instructor',
      dutyStartDate: '1963-07-01',
      dutyEndDate: '1997-04-27'
    },
    {
      id: 16,
      personId: 7,
      rank: 'Junior Lieutenant',
      dutyTitle: 'Vostok Pilot',
      dutyStartDate: '1962-03-12',
      dutyEndDate: '1963-06-30'
    }
  ],
  'Yuri Gagarin': [
    {
      id: 17,
      personId: 8,
      rank: 'Colonel',
      dutyTitle: 'RETIRED',
      dutyStartDate: '1968-03-27',
      dutyEndDate: null
    },
    {
      id: 18,
      personId: 8,
      rank: 'Major',
      dutyTitle: 'Soyuz Mission Commander',
      dutyStartDate: '1964-11-01',
      dutyEndDate: '1968-03-26'
    },
    {
      id: 19,
      personId: 8,
      rank: 'Senior Lieutenant',
      dutyTitle: 'Vostok Pilot',
      dutyStartDate: '1960-03-07',
      dutyEndDate: '1964-10-31'
    }
  ],
  'Peggy Whitson': [
    {
      id: 20,
      personId: 9,
      rank: 'Colonel',
      dutyTitle: 'Station Commander',
      dutyStartDate: '2016-11-17',
      dutyEndDate: null
    },
    {
      id: 21,
      personId: 9,
      rank: 'Colonel',
      dutyTitle: 'ISS Science Officer',
      dutyStartDate: '2002-06-05',
      dutyEndDate: '2016-11-16'
    },
    {
      id: 22,
      personId: 9,
      rank: 'Lieutenant Colonel',
      dutyTitle: 'Research Astronaut',
      dutyStartDate: '1996-04-26',
      dutyEndDate: '2002-06-04'
    }
  ],
  'Scott Kelly': [
    {
      id: 23,
      personId: 10,
      rank: 'Captain',
      dutyTitle: 'Long Duration Mission Specialist',
      dutyStartDate: '2015-03-27',
      dutyEndDate: null
    },
    {
      id: 24,
      personId: 10,
      rank: 'Captain',
      dutyTitle: 'Station Commander',
      dutyStartDate: '2010-10-07',
      dutyEndDate: '2015-03-26'
    },
    {
      id: 25,
      personId: 10,
      rank: 'Commander',
      dutyTitle: 'Shuttle Pilot',
      dutyStartDate: '1996-08-01',
      dutyEndDate: '2010-10-06'
    }
  ],
  'Alan Shepard': [
    {
      id: 26,
      personId: 11,
      rank: 'Rear Admiral',
      dutyTitle: 'RETIRED',
      dutyStartDate: '1974-08-02',
      dutyEndDate: null
    },
    {
      id: 27,
      personId: 11,
      rank: 'Captain',
      dutyTitle: 'Apollo Commander',
      dutyStartDate: '1969-05-01',
      dutyEndDate: '1974-08-01'
    },
    {
      id: 28,
      personId: 11,
      rank: 'Commander',
      dutyTitle: 'Mercury Pilot',
      dutyStartDate: '1959-04-09',
      dutyEndDate: '1969-04-30'
    }
  ],
  'Christina Koch': [
    {
      id: 29,
      personId: 12,
      rank: 'Mission Specialist',
      dutyTitle: 'Artemis Crew Member',
      dutyStartDate: '2024-01-15',
      dutyEndDate: null
    },
    {
      id: 30,
      personId: 12,
      rank: 'Flight Engineer',
      dutyTitle: 'ISS Long Duration Crew',
      dutyStartDate: '2019-03-14',
      dutyEndDate: '2024-01-14'
    },
    {
      id: 31,
      personId: 12,
      rank: 'Candidate',
      dutyTitle: 'Astronaut Trainee',
      dutyStartDate: '2013-06-17',
      dutyEndDate: '2019-03-13'
    }
  ]
};

export const MOCK_PROCESS_LOGS = [
  {
    id: 1,
    message: 'Person created: Neil Armstrong',
    logLevel: 'Information',
    timestamp: '2024-06-15T09:00:00',
    source: 'CreatePersonHandler',
    stackTrace: null
  },
  {
    id: 2,
    message: 'Astronaut duty assigned: Neil Armstrong — Senior Pilot / Orbital Mission Specialist',
    logLevel: 'Information',
    timestamp: '2024-06-15T09:01:00',
    source: 'CreateAstronautDutyHandler',
    stackTrace: null
  },
  {
    id: 3,
    message: 'Astronaut duty assigned: Neil Armstrong — Commander / Lunar Mission Pilot',
    logLevel: 'Information',
    timestamp: '2024-06-15T09:02:00',
    source: 'CreateAstronautDutyHandler',
    stackTrace: null
  },
  {
    id: 4,
    message: 'Person created: Sally Ride',
    logLevel: 'Information',
    timestamp: '2024-06-15T09:03:00',
    source: 'CreatePersonHandler',
    stackTrace: null
  },
  {
    id: 5,
    message: 'Astronaut duty assigned: Sally Ride — RETIRED',
    logLevel: 'Information',
    timestamp: '2024-06-15T09:05:00',
    source: 'CreateAstronautDutyHandler',
    stackTrace: null
  },
  {
    id: 6,
    message: 'Person not found: Unknown Person',
    logLevel: 'Error',
    timestamp: '2024-06-15T09:10:00',
    source: 'GetPersonByNameHandler',
    stackTrace: 'BadHttpRequestException: Person not found.\n   at GetPersonByNamePreProcessor.Process()'
  },
  {
    id: 7,
    message: 'Duplicate person rejected: Neil Armstrong',
    logLevel: 'Warning',
    timestamp: '2024-06-15T09:12:00',
    source: 'CreatePersonPreProcessor',
    stackTrace: null
  },
  {
    id: 8,
    message: 'Person created: Chris Hadfield',
    logLevel: 'Information',
    timestamp: '2024-06-15T09:15:00',
    source: 'CreatePersonHandler',
    stackTrace: null
  },
  {
    id: 9,
    message: 'Astronaut duty assigned: Chris Hadfield — Colonel / Station Commander',
    logLevel: 'Information',
    timestamp: '2024-06-15T09:16:00',
    source: 'CreateAstronautDutyHandler',
    stackTrace: null
  },
  {
    id: 10,
    message: 'Astronaut duty assigned: Christina Koch — Mission Specialist / Artemis Crew Member',
    logLevel: 'Information',
    timestamp: '2024-06-16T14:30:00',
    source: 'CreateAstronautDutyHandler',
    stackTrace: null
  }
];
