# Operations Research (I) Midterm Project

**Course:** Operations Research (I)  
**Instructor:** David Martens  

This repository contains the implementation of a linear programming model to assist in creating a master production schedule for a semiconductor manufacturing company. The project focuses on maximizing profits while meeting production constraints, inventory requirements, and customer demands.

---

## Project Overview
### Problem Statement
The goal is to develop a linear programming model to determine the monthly production quantities for over 3,000 products across six factories with distinct capacities and technologies. The objective is to maximize total profit, calculated as:
Total Sales - Production Costs - Inventory Costs - Backorder Costs
$\[
Z = \sum_{j \in J} \sum_{t \in T} S_j K_{jt} 
    - 0.5 \sum_{i \in I} \sum_{j \in J} \sum_{t \in T} S_j X_{ijt} 
    - \sum_{j \in J} \sum_{t \in T} I_{jt} 
    - 2 \sum_{j \in J} \sum_{t \in T} S_j B_{jt}
\]$
