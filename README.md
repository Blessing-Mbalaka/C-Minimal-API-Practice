
    SA SALARY CALCULATOR API - FRONTEND DEVELOPER GUIDE
================================================================================
Good day. This is a long document, so I summarise everything here. hope all is good. So here is an API guide. This code is deployed
and we need to basically need to use 2 urls for the main logic, which is display employee by ID and
to also display

To do list: Add max hours for work and overtime hours per week
            Add logic for sunday pay and holiday pay

API BASE URL: https://localhost:7233

Interactive API Docs: https://localhost:7233/scalar/v1


TABLE OF CONTENTS
================================================================================
1. Quick Start
2. Core Endpoints (Required)
3. Bonus Endpoint (Optional)
4. Common Use Cases with Examples
5. Error Handling
6. Tips & Best Practices

1. QUICK START
================================================================================

What this API does:
Calculates South African salary with SARS tax deductions (PAYE, UIF, SDL)
Stores employee records
Shows complete breakdown: Gross → Deductions → Net Pay

Technologies you can use:
React, Vue, Angular (JavaScript frameworks)
Plain HTML/CSS/JavaScript (like the included index.html)
Mobile apps (React Native, Flutter)

2. CORE ENDPOINTS (REQUIRED)
================================================================================

You only need 2 endpoints for a working salary calculator!


ENDPOINT 1: Calculate Salary and Save Employee
--------------------------------------------------------------------------------

METHOD: POST
URL: /api/employee/calculate-and-save

What it does:
- Calculates salary with all SARS deductions
- Saves/updates employee in database
- Returns complete salary breakdown

REQUEST BODY (JSON):
{
  "name": "Thandi",
  "surname": "Ndlovu",
  "email": "thandi.ndlovu@BlessingisAwesome.com",
  "jobTitle": "Software Developer",
  "employmentType": "Full-time",
  "hourlyRate": 250.00,
  "regularHours": 160,
  "overtimeHours": 10,
  "variableAmount": 5000.00,
  "period": "January 2025"
}

FIELD EXPLANATIONS:
- name: Employee's first name (required)
- surname: Employee's last name (required)
- email: Employee's email (required, unique)
- jobTitle: Job position (e.g., "Developer", "Accountant")
- employmentType: "Full-time", "Part-time", "Contract", or "Temporary"
- hourlyRate: How much they earn per hour in Rands
- regularHours: Normal working hours (typically 160-176 per month)
- overtimeHours: Extra hours worked (paid at 1.5x rate)
- variableAmount: Bonuses, commissions, allowances (0 if none)
- period: Month/year for this calculation (e.g., "January 2025")

RESPONSE (JSON):
{
  "id": 1,
  "name": "Thandi",
  "surname": "Ndlovu",
  "email": "Thandi@BlessingisAwesome.com",
  "jobTitle": "Software Developer",
  "employmentType": "Full-time",
  "hourlyRate": 250.00,
  "calculation": {
    "period": "January 2025",
    "hourlyRate": 250.00,
    "regularHours": 160,
    "overtimeHours": 10,
    "variableAmount": 5000.00,
    
    //EARNINGS
    "basicSalary": 40000.00,        // hourlyRate × regularHours
    "overtimePay": 3750.00,         // hourlyRate × 1.5 × overtimeHours
    "grossSalary": 48750.00,        // basicSalary + overtimePay + variableAmount
    
    //EMPLOYEE DEDUCTIONS (taken from salary)
    "deductions": {
      "uif": 177.12,                // Employee UIF (1%, max R177.12)
      "paye": 8562.50,              // Income tax (SARS 2024/2025 brackets)
      "total": 8739.62              // Total deducted from employee
    },
    
    //EMPLOYER COSTS (NOT deducted from employee)
    "employerCosts": {
      "uifEmployer": 177.12,        // Employer UIF (1%, max R177.12)
      "sdl": 487.50,                // Skills Development Levy (1%)
      "total": 664.62               // Total employer pays to SARS
    },
    
    // FINAL AMOUNTS
    "netSalary": 40010.38,          // Take-home pay (what employee receives)
    "costToCompany": 49414.62       // Total cost to employer
  }
}

JAVASCRIPT EXAMPLE (Fetch API):
```javascript
async function calculateSalary() {
  const employeeData = {
    name: "Thandi",
    surname: "Ndlovu",
    email: "thandi.ndlovu@example.com",
    jobTitle: "Software Developer",
    employmentType: "Full-time",
    hourlyRate: 250.00,
    regularHours: 160,
    overtimeHours: 10,
    variableAmount: 5000.00,
    period: "January 2025"
  };

  try {
    const response = await fetch('https://localhost:7233/api/employee/calculate-and-save', {
      method: 'POST',
      headers: {
        'Content-Type': 'application/json'
      },
      body: JSON.stringify(employeeData)
    });

    if (!response.ok) {
      throw new Error('Failed to calculate salary');
    }

    const result = await response.json();
    console.log('Net Salary:', result.calculation.netSalary);
    console.log('PAYE Tax:', result.calculation.deductions.paye);
    
    // Display result to user...
  } catch (error) {
    console.error('Error:', error);
    alert('Something went wrong!');
  }
}
```

REACT EXAMPLE:
```javascript
const [result, setResult] = useState(null);

const handleSubmit = async (formData) => {
  const response = await fetch('https://localhost:7233/api/employee/calculate-and-save', {
    method: 'POST',
    headers: { 'Content-Type': 'application/json' },
    body: JSON.stringify(formData)
  });
  
  const data = await response.json();
  setResult(data);
};

// Display: {result?.calculation.netSalary}
```


--------------------------------------------------------------------------------
ENDPOINT 2: Get All Employees
--------------------------------------------------------------------------------

METHOD: GET
URL: /api/employee

What it does:
- Returns list of all saved employees
- Good for displaying employee directory

NO REQUEST BODY NEEDED!

RESPONSE (JSON):
[
  {
    "id": 1,
    "name": "Thandi",
    "surname": "Ndlovu",
    "email": "thandi.ndlovu@BlessingisAwesome.com",
    "jobTitle": "Software Developer",
    "employmentType": "Full-time",
    "hourlyRate": 250.00,
    "createdAt": "2025-01-15T10:30:00Z",
    "salaryCalculations": []
  },
  {
    "id": 2,
    "name": "John",
    "surname": "Smith",
    "email": "john.smith@BlessingisAwesome.com",
    "jobTitle": "Accountant",
    "employmentType": "Full-time",
    "hourlyRate": 180.00,
    "createdAt": "2025-01-16T14:20:00Z",
    "salaryCalculations": []
  }
]

JAVASCRIPT EXAMPLE:
```javascript
async function loadAllEmployees() {
  try {
    const response = await fetch('https://localhost:7233/api/employee');
    const employees = await response.json();
    
    console.log('Total employees:', employees.length);
    
    // Display in a list or table
    employees.forEach(emp => {
      console.log(`${emp.name} ${emp.surname} - R${emp.hourlyRate}/hr`);
    });
  } catch (error) {
    console.error('Error loading employees:', error);
  }
}
```


================================================================================
3. BONUS ENDPOINT (OPTIONAL)
================================================================================

This endpoint is NOT required, but useful for educational/info pages!

--------------------------------------------------------------------------------
ENDPOINT 3: View SARS Tax Brackets
--------------------------------------------------------------------------------

METHOD: GET
URL: /api/employee/tax-brackets

What it does:
- Shows current SARS 2024/2025 tax brackets
- Useful for "Tax Info" page or calculator explainer

NO REQUEST BODY NEEDED!

RESPONSE (JSON):
{
  "taxYear": "2024/2025 (1 March 2024 - 28 February 2025)",
  "brackets": [
    {
      "minIncome": 0,
      "maxIncome": 237100,
      "baseTax": 0,
      "rate": 0.18,
      "ratePercentage": "18%",
      "description": "R0 - R237,100: R0.00 + 18% of income above R0"
    },
    {
      "minIncome": 237101,
      "maxIncome": 370500,
      "baseTax": 42678,
      "rate": 0.26,
      "ratePercentage": "26%",
      "description": "R237,101 - R370,500: R42,678.00 + 26% of income above R237,101"
    }
    // ... more brackets
  ]
}

USE CASE: Display tax information
```javascript
async function showTaxInfo() {
  const response = await fetch('https://localhost:7233/api/employee/tax-brackets');
  const data = await response.json();
  
  console.log('Current Tax Year:', data.taxYear);
  data.brackets.forEach(bracket => {
    console.log(bracket.description);
  });
}
```


================================================================================
4. COMMON USE CASES WITH EXAMPLES
================================================================================

--------------------------------------------------------------------------------
USE CASE 1: Simple Salary Calculator (No Bonus/Overtime)
--------------------------------------------------------------------------------

SCENARIO: Employee works regular hours, no overtime or bonuses

const simpleCalculation = {
  name: "Sarah",
  surname: "Mokoena",
  email: "sarah@example.com",
  jobTitle: "HR Manager",
  employmentType: "Full-time",
  hourlyRate: 200.00,
  regularHours: 160,        // Standard month
  overtimeHours: 0,         // No overtime
  variableAmount: 0,        // No bonus
  period: "January 2025"
};

RESULT: 
- Gross Salary: R32,000
- Net Salary: ~R27,500 (after PAYE + UIF)


--------------------------------------------------------------------------------
USE CASE 2: High Earner with Overtime and Bonus
--------------------------------------------------------------------------------

SCENARIO: Senior employee with overtime and performance bonus

const seniorEmployeeCalc = {
  name: "Michael",
  surname: "van der Merwe",
  email: "michael@example.com",
  jobTitle: "Senior Developer",
  employmentType: "Full-time",
  hourlyRate: 400.00,
  regularHours: 160,
  overtimeHours: 20,        // 20 hours OT
  variableAmount: 10000,    // R10k bonus
  period: "December 2024"   // Bonus month
};

RESULT:
- Basic: R64,000
- Overtime: R12,000 (20h × R400 × 1.5)
- Variable: R10,000
- Gross: R86,000
- Net: ~R60,000 (higher tax bracket)


--------------------------------------------------------------------------------
USE CASE 3: Part-Time Employee
--------------------------------------------------------------------------------

SCENARIO: Part-time worker with fewer hours

const partTimeCalc = {
  name: "Zanele",
  surname: "Dlamini",
  email: "zanele@example.com",
  jobTitle: "Receptionist",
  employmentType: "Part-time",
  hourlyRate: 80.00,
  regularHours: 80,         // Half-time
  overtimeHours: 0,
  variableAmount: 0,
  period: "January 2025"
};

RESULT:
- Gross: R6,400
- Net: ~R6,336 (minimal tax, below threshold)


--------------------------------------------------------------------------------
USE CASE 4: View Employee Salary History
--------------------------------------------------------------------------------

SCENARIO: Show all calculations for one employee

Since the API returns salaryCalculations array, you can:

1. GET /api/employee → Get all employees
2. Filter by email or ID
3. Access their salaryCalculations array to show history

Example:
```javascript
async function getEmployeeHistory(email) {
  const response = await fetch('https://localhost:7233/api/employee');
  const employees = await response.json();
  
  const employee = employees.find(e => e.email === email);
  
  if (employee && employee.salaryCalculations.length > 0) {
    employee.salaryCalculations.forEach(calc => {
      console.log(`${calc.period}: Net Pay R${calc.netSalary}`);
    });
  }
}
```


--------------------------------------------------------------------------------
USE CASE 5: Update Existing Employee
--------------------------------------------------------------------------------

SCENARIO: Employee gets a raise or changes job title

If you POST with the SAME email, it updates the employee record:

// First calculation
POST /api/employee/calculate-and-save
{
  "email": "thandi@example.com",
  "hourlyRate": 200.00,
  "jobTitle": "Junior Developer"
  // ... other fields
}

// Later, after promotion
POST /api/employee/calculate-and-save
{
  "email": "thandi@example.com",    // Same email
  "hourlyRate": 300.00,              // New rate!
  "jobTitle": "Senior Developer"     // New title!
  // ... other fields
}

Result: Employee record is updated, new salary calculation is added to history!


================================================================================
5. ERROR HANDLING
================================================================================

COMMON ERRORS:

--------------------------------------------------------------------------------
400 Bad Request - Missing Required Fields
--------------------------------------------------------------------------------
{
  "error": "Name and Email are required"
}

FIX: Make sure name and email are filled in your form!


--------------------------------------------------------------------------------
500 Internal Server Error - Database Issue
--------------------------------------------------------------------------------
Could happen if database isn't set up.

FIX: Contact backend developer to run:
  dotnet ef migrations add InitialCreate
  dotnet ef database update


--------------------------------------------------------------------------------
CORS Error (in browser console)
--------------------------------------------------------------------------------
Error: "Access to fetch has been blocked by CORS policy"

FIX: Backend needs to add CORS support. Tell backend developer to add this to Program.cs:

builder.Services.AddCors(options => {
  options.AddDefaultPolicy(policy => {
    policy.AllowAnyOrigin()
          .AllowAnyMethod()
          .AllowAnyHeader();
  });
});

// And before app.MapControllers():
app.UseCors();


--------------------------------------------------------------------------------
Network Error / Connection Refused
--------------------------------------------------------------------------------

FIX: Make sure the API is running! Check https://localhost:7233


================================================================================
6. TIPS & BEST PRACTICES
================================================================================

DO:
- Always use try/catch for API calls
- Show loading spinners while fetching
- Validate form inputs before sending (email format, positive numbers)
- Display errors to users in a friendly way
- Use the interactive docs at /scalar/v1 to test endpoints first
- Format currency properly (R32,500.50 not 32500.5)

DON'T:
- Hard-code the API URL - use environment variables
- Forget to handle errors
- Send empty strings for numeric fields (use 0 instead)
- Assume API is always available

RECOMMENDED: Test in Scalar UI first!
1. Open https://localhost:7233/scalar/v1
2. Try the endpoints with sample data
3. See the exact request/response format
4. Then copy the working examples to your code


================================================================================
EXAMPLE: COMPLETE REACT COMPONENT
================================================================================

```javascript
import React, { useState } from 'react';

function SalaryCalculator() {
  const [formData, setFormData] = useState({
    name: '',
    surname: '',
    email: '',
    jobTitle: '',
    employmentType: 'Full-time',
    hourlyRate: 0,
    regularHours: 160,
    overtimeHours: 0,
    variableAmount: 0,
    period: ''
  });
  
  const [result, setResult] = useState(null);
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState(null);

  const handleSubmit = async (e) => {
    e.preventDefault();
    setLoading(true);
    setError(null);

    try {
      const response = await fetch('https://localhost:7233/api/employee/calculate-and-save', {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify(formData)
      });

      if (!response.ok) {
        const errorData = await response.json();
        throw new Error(errorData.error || 'Failed to calculate');
      }

      const data = await response.json();
      setResult(data);
    } catch (err) {
      setError(err.message);
    } finally {
      setLoading(false);
    }
  };

  return (
    <div>
      <h1>SA Salary Calculator</h1>
      
      <form onSubmit={handleSubmit}>
        <input 
          placeholder="Name" 
          value={formData.name}
          onChange={(e) => setFormData({...formData, name: e.target.value})}
          required 
        />
        
        <input 
          placeholder="Email" 
          type="email"
          value={formData.email}
          onChange={(e) => setFormData({...formData, email: e.target.value})}
          required 
        />
        
        <input 
          placeholder="Hourly Rate" 
          type="number"
          value={formData.hourlyRate}
          onChange={(e) => setFormData({...formData, hourlyRate: parseFloat(e.target.value)})}
          required 
        />
        
        {/* Add more fields... */}
        
        <button type="submit" disabled={loading}>
          {loading ? 'Calculating...' : 'Calculate Salary'}
        </button>
      </form>

      {error && <div className="error">{error}</div>}

      {result && (
        <div className="result">
          <h2>Results for {result.name} {result.surname}</h2>
          <p><strong>Gross Salary:</strong> R{result.calculation.grossSalary.toFixed(2)}</p>
          <p><strong>PAYE Tax:</strong> -R{result.calculation.deductions.paye.toFixed(2)}</p>
          <p><strong>UIF:</strong> -R{result.calculation.deductions.uif.toFixed(2)}</p>
          <h3><strong>Net Salary:</strong> R{result.calculation.netSalary.toFixed(2)}</h3>
          
          <h4>Employer Costs (not deducted from employee):</h4>
          <p>UIF Employer: R{result.calculation.employerCosts.uifEmployer.toFixed(2)}</p>
          <p>SDL: R{result.calculation.employerCosts.sdl.toFixed(2)}</p>
          <p><strong>Total Cost to Company:</strong> R{result.calculation.costToCompany.toFixed(2)}</p>
        </div>
      )}
    </div>
  );
}

export default SalaryCalculator;
```


================================================================================
QUICK REFERENCE CARD
================================================================================

ENDPOINTS:
┌─────────────────────────────────────────────────────────────────────────┐
│ POST /api/employee/calculate-and-save   :  Calculate & save salary     │
│ GET  /api/employee                       : Get all employees          │
│ GET  /api/employee/tax-brackets          : View SARS tax info  │
└─────────────────────────────────────────────────────────────────────────┘

KEY RESPONSE FIELDS:
- calculation.grossSalary      → Total before deductions
- calculation.deductions.paye  → Income tax
- calculation.deductions.uif   → Employee UIF
- calculation.netSalary        → Take-home pay 
- calculation.costToCompany    → Total employer cost 

MINIMUM REQUIRED FIELDS:
- name (string)
- surname (string)
- email (string, unique)
- hourlyRate (number)
- regularHours (number)


================================================================================
NEED HELP?
================================================================================
Interactive API Docs: https://localhost:7233/scalar/v1
Backend Team: Blessing Mbalaka, and Matthews Sethusa
SARS Tax Info: https://www.sars.gov.za

================================================================================
