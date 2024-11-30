# Operations Research (I) Midterm Project

**Course:** Operations Research (I)  
**Instructor:** David Martens  

This repository contains the implementation of a linear programming model to assist in creating a master production schedule for a semiconductor manufacturing company. The project focuses on maximizing profits while meeting production constraints, inventory requirements, and customer demands.

---

## Project Overview
### Problem Statement
The goal is to develop a linear programming model to determine the monthly production quantities for over 3,000 products across six factories with distinct capacities and technologies. The objective is to maximize total profit, calculated as:
Total Sales - Production Costs - Inventory Costs - Backorder Costs

I 
jt
​
 =I 
j,t−1
​
 + 
i∈I
∑
​
 X 
ijt
​
 −B 
j,t−1
​
 −D 
jt
​
 +B 
jt
​
 ,∀j∈J,∀t∈T