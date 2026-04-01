import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, of, delay, throwError, tap } from 'rxjs';
import { Person } from '../models/person.model';
import { AstronautDuty } from '../models/astronaut-duty.model';
import {
  GetPeopleResponse,
  GetPersonResponse,
  GetAstronautDutiesResponse,
  CreatePersonResponse,
  CreateAstronautDutyResponse
} from '../models/api-response.model';
import { MOCK_PEOPLE, MOCK_DUTIES, MOCK_PROCESS_LOGS } from './mock-data';

@Injectable({ providedIn: 'root' })
export class StargateService {
  // Toggle to switch between mock and live API
  private useMock = true;
  private apiUrl = 'https://localhost:5001';

  private people = [...MOCK_PEOPLE];
  private duties = JSON.parse(JSON.stringify(MOCK_DUTIES)) as Record<string, AstronautDuty[]>;
  private logs = [...MOCK_PROCESS_LOGS];
  private nextPersonId = 13;
  private nextDutyId = 32;
  private nextLogId = 11;

  constructor(private http: HttpClient) {}

  getPeople(): Observable<GetPeopleResponse> {
    if (this.useMock) {
      return of({
        success: true,
        message: 'Successful',
        responseCode: 200,
        people: this.people
      }).pipe(delay(400));
    }
    return this.http.get<GetPeopleResponse>(`${this.apiUrl}/Person`);
  }

  getPersonByName(name: string): Observable<GetPersonResponse> {
    if (this.useMock) {
      const person = this.people.find(
        p => p.name.toLowerCase() === name.toLowerCase()
      );
      if (!person) {
        return throwError(() => ({
          success: false,
          message: `Person "${name}" not found.`,
          responseCode: 404
        }));
      }
      return of({
        success: true,
        message: 'Successful',
        responseCode: 200,
        person
      }).pipe(delay(300));
    }
    return this.http.get<GetPersonResponse>(
      `${this.apiUrl}/Person/${encodeURIComponent(name)}`
    );
  }

  createPerson(name: string): Observable<CreatePersonResponse> {
    if (this.useMock) {
      const trimmed = name.trim();
      if (!trimmed) {
        return throwError(() => ({
          success: false,
          message: 'Name is required.',
          responseCode: 400
        }));
      }
      const exists = this.people.find(
        p => p.name.toLowerCase() === trimmed.toLowerCase()
      );
      if (exists) {
        return throwError(() => ({
          success: false,
          message: `Person "${trimmed}" already exists.`,
          responseCode: 400
        }));
      }
      const newPerson: Person = {
        personId: this.nextPersonId++,
        name: trimmed,
        currentRank: '',
        currentDutyTitle: '',
        careerStartDate: null,
        careerEndDate: null
      };
      this.people.push(newPerson);
      this.addLog(`Person created: ${trimmed}`, 'Information', 'CreatePersonHandler');
      return of({
        success: true,
        message: 'Successful',
        responseCode: 200,
        id: newPerson.personId
      }).pipe(delay(500));
    }
    return this.http.post<CreatePersonResponse>(`${this.apiUrl}/Person`, { name });
  }

  getAstronautDutiesByName(name: string): Observable<GetAstronautDutiesResponse> {
    if (this.useMock) {
      const person = this.people.find(
        p => p.name.toLowerCase() === name.toLowerCase()
      );
      if (!person) {
        return throwError(() => ({
          success: false,
          message: `Person "${name}" not found.`,
          responseCode: 404
        }));
      }
      const duties = this.duties[person.name] || [];
      return of({
        success: true,
        message: 'Successful',
        responseCode: 200,
        person,
        astronautDuties: duties
      }).pipe(delay(400));
    }
    return this.http.get<GetAstronautDutiesResponse>(
      `${this.apiUrl}/AstronautDuty/${encodeURIComponent(name)}`
    );
  }

  createAstronautDuty(payload: {
    name: string;
    rank: string;
    dutyTitle: string;
    dutyStartDate: string;
  }): Observable<CreateAstronautDutyResponse> {
    if (this.useMock) {
      const person = this.people.find(
        p => p.name.toLowerCase() === payload.name.toLowerCase()
      );
      if (!person) {
        return throwError(() => ({
          success: false,
          message: `Person "${payload.name}" not found.`,
          responseCode: 404
        }));
      }

      const personDuties = this.duties[person.name] || [];
      const currentDuty = personDuties.find(d => d.dutyEndDate === null);
      if (currentDuty) {
        const startDate = new Date(payload.dutyStartDate);
        const endDate = new Date(startDate);
        endDate.setDate(endDate.getDate() - 1);
        currentDuty.dutyEndDate = endDate.toISOString().split('T')[0];
      }

      const newDuty: AstronautDuty = {
        id: this.nextDutyId++,
        personId: person.personId,
        rank: payload.rank,
        dutyTitle: payload.dutyTitle,
        dutyStartDate: payload.dutyStartDate,
        dutyEndDate: null
      };

      if (!this.duties[person.name]) {
        this.duties[person.name] = [];
      }
      this.duties[person.name].unshift(newDuty);

      person.currentRank = payload.rank;
      person.currentDutyTitle = payload.dutyTitle;
      if (!person.careerStartDate) {
        person.careerStartDate = payload.dutyStartDate;
      }

      if (payload.dutyTitle.toUpperCase() === 'RETIRED') {
        const startDate = new Date(payload.dutyStartDate);
        const endDate = new Date(startDate);
        endDate.setDate(endDate.getDate() - 1);
        person.careerEndDate = endDate.toISOString().split('T')[0];
      }

      this.addLog(
        `Astronaut duty assigned: ${person.name} — ${payload.rank} / ${payload.dutyTitle}`,
        'Information',
        'CreateAstronautDutyHandler'
      );
      const dutyType = payload.dutyTitle.toUpperCase() === 'RETIRED' ? 'info' : 'success';

      return of({
        success: true,
        message: 'Successful',
        responseCode: 200,
        id: newDuty.id
      }).pipe(delay(600));
    }
    return this.http.post<CreateAstronautDutyResponse>(
      `${this.apiUrl}/AstronautDuty`,
      payload
    );
  }

  getProcessLogs(): Observable<any[]> {
    return of([...this.logs].reverse()).pipe(delay(350));
  }

  private addLog(message: string, logLevel: string, source: string) {
    this.logs.push({
      id: this.nextLogId++,
      message,
      logLevel,
      timestamp: new Date().toISOString(),
      source,
      stackTrace: null
    });
  }
}
