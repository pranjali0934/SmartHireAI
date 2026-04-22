import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { HttpClientModule } from '@angular/common/http';
import { ReactiveFormsModule, FormsModule } from '@angular/forms';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';

import { MatToolbarModule } from '@angular/material/toolbar';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatTableModule } from '@angular/material/table';
import { MatSelectModule } from '@angular/material/select';

import { DashboardComponent } from './components/dashboard/dashboard.component';
import { UploadResumeComponent } from './components/upload-resume/upload-resume.component';
import { JobsComponent } from './components/jobs/jobs.component';
import { CandidateRankingComponent } from './components/candidate-ranking/candidate-ranking.component';
import { AnalysisComponent } from './components/analysis/analysis.component';

@NgModule({
  declarations: [
    AppComponent,
    DashboardComponent,
    UploadResumeComponent,
    JobsComponent,
    CandidateRankingComponent,
    AnalysisComponent
  ],
  imports: [
    BrowserModule,
    BrowserAnimationsModule,
    HttpClientModule,
    ReactiveFormsModule,
    FormsModule,
    AppRoutingModule,
    MatToolbarModule,
    MatButtonModule,
    MatCardModule,
    MatFormFieldModule,
    MatInputModule,
    MatTableModule,
    MatSelectModule
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
