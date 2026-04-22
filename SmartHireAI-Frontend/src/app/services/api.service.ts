import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';

export interface DashboardStats {
  totalResumes: number;
  totalJobs: number;
  totalCandidates: number;
  topCandidates: RankedCandidate[];
}

export interface RankedCandidate {
  candidateId: number;
  candidateName: string;
  email: string;
  resumeId: number;
  jobId: number;
  matchScore: number;
  matchingSkills: string;
  missingSkills: string;
  aiAnalysis?: string;
}

export interface Job {
  id: number;
  title: string;
  description: string;
  requiredSkills: string;
  createdAt: string;
}

export interface CreateJobRequest {
  title: string;
  description: string;
  requiredSkills: string;
}

export interface ResumeUploadResponse {
  resumeId: number;
  candidateId: number;
  fileName: string;
  extractedText?: string;
  uploadedAt: string;
}

export interface ResumeListItem {
  id: number;
  candidateId: number;
  candidateName: string;
  fileName: string;
  uploadedAt: string;
}

export interface MatchAnalysisRequest {
  resumeId: number;
  jobId: number;
}

export interface MatchAnalysisResponse {
  resumeId: number;
  jobId: number;
  matchScore: number;
  matchingSkills: string;
  missingSkills: string;
  aiAnalysis?: string;
}

@Injectable({ providedIn: 'root' })
export class ApiService {
  private baseUrl = environment.apiUrl;

  constructor(private http: HttpClient) {}

  getDashboardStats(): Observable<DashboardStats> {
    return this.http.get<DashboardStats>(`${this.baseUrl}/dashboard/stats`);
  }

  uploadResume(file: File, candidateName: string, candidateEmail: string): Observable<ResumeUploadResponse> {
    const formData = new FormData();
    formData.append('file', file);
    formData.append('candidateName', candidateName);
    formData.append('candidateEmail', candidateEmail);
    return this.http.post<ResumeUploadResponse>(`${this.baseUrl}/resume/upload`, formData);
  }

  getJobs(): Observable<Job[]> {
    return this.http.get<Job[]>(`${this.baseUrl}/jobs`);
  }

  createJob(job: CreateJobRequest): Observable<Job> {
    return this.http.post<Job>(`${this.baseUrl}/jobs`, job);
  }

  analyzeMatch(request: MatchAnalysisRequest): Observable<MatchAnalysisResponse> {
    return this.http.post<MatchAnalysisResponse>(`${this.baseUrl}/analysis/match`, request);
  }

  getResumes(): Observable<ResumeListItem[]> {
    return this.http.get<ResumeListItem[]>(`${this.baseUrl}/resume`);
  }

  getRankedCandidates(jobId: number): Observable<RankedCandidate[]> {
    return this.http.get<RankedCandidate[]>(`${this.baseUrl}/candidates/ranked/${jobId}`);
  }
}
