export interface BaseResponse {
  success: boolean;
  message: string;
  responseCode: number;
}

export interface GetPeopleResponse extends BaseResponse {
  people: import('./person.model').Person[];
}

export interface GetPersonResponse extends BaseResponse {
  person: import('./person.model').Person;
}

export interface GetAstronautDutiesResponse extends BaseResponse {
  person: import('./person.model').Person;
  astronautDuties: import('./astronaut-duty.model').AstronautDuty[];
}

export interface CreatePersonResponse extends BaseResponse {
  id: number;
}

export interface CreateAstronautDutyResponse extends BaseResponse {
  id: number;
}
