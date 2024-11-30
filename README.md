# Operations Research (I) Midterm Project

**Course:** Operations Research (I)  
**Instructor:** David Martens  
**Semester:** Academic Year 2021, First Semester (Sep. 2021 - Jan. 2022)

This repository contains the implementation of a linear programming model to assist in creating a master production schedule for a semiconductor manufacturing company. The project focuses on maximizing profits while meeting production constraints, inventory requirements, and customer demands.

---

## Project Overview
### Problem Statement
The goal is to develop a linear programming model to determine the monthly production quantities for over 3,000 products across six factories with distinct capacities and technologies. The objective is to maximize total profit, calculated as:  
**Total Sales - Production Costs - Inventory Costs - Backorder Costs**
##### Key Assumptions
1. Initial inventory is zero.
2. Backorders are allowed.
3. Monthly production cannot exceed factory capacities (no overtime or outsourcing).
4. No depreciation for finished goods.
### Sets and Variables
##### Sets:
* **I:** Factories $\lbrace1, 2, ..., 6\rbrace$
* **J:** Products $\lbrace1,2,...,3412\rbrace$
* **T:** Months in the planning horizon $\lbrace1,2,...,24\rbrace$
##### Parameters:
* $C_{i}$: Installed capacity for factory $i$
* $S_{j}$: Sales price per unit for product $j$
* $D_{jt}$: Demand for product $j$ in month $t$
##### Decision Variables:
* $X_{ijt}$: Production quantity of product $j$ using factory $i$ in month $t$
* $K_{jt}$: Sales quantity of product $j$ in month $t$
* $B_{jt}$: Backorder quantity of product $j$ in month $t$
* $I_{jt}$: Inventory of product $j$ in month $t$
### Linear Programming Model
##### Objective Function:
$$
Z = \sum_{j \in J} \sum_{t \in T} S_j K_{jt} 
    - 0.5 \sum_{i \in I} \sum_{j \in J} \sum_{t \in T} S_j X_{ijt} 
    - \sum_{j \in J} \sum_{t \in T} I_{jt} 
    - 2 \sum_{j \in J} \sum_{t \in T} S_j B_{jt}
$$
##### Constraints:
1. Inventory Balance:
$$I_{jt} = I_{j, t-1} + \sum_{i \in I} X_{ijt} - B_{j, t-1} - D_{jt} + B_{jt}, \quad \forall j \in J, \forall t \in T$$
2. Capacity Limitation:
$$\sum_{j \in J} X_{ijt} \leq C_i, \quad \forall i \in I, \forall t \in T$$
3. Backorder Definition:
$$B_{jt} = D_{jt} - \sum_{i \in I} X_{ijt}, \quad \forall j \in J, \forall t \in T$$
4. Sales Limitation:
$$K_{jt} \leq I_{j, t-1} + \sum_{i \in I} X_{ijt}, \quad \forall j \in J, \forall t \in T$$
5. Initial Inventory and Backorders:
$$I_{j0} = 0, \quad B_{j0} = 0, \quad \forall j \in J$$
6. Non-Negativity:
$$X_{ijt}, K_{jt}, B_{jt}, I_{jt} \geq 0, \quad \forall i \in I, \forall j \in J, \forall t \in T$$

## Code Explanation
The code is implemented in **C#** using the **IBM CPLEX Optimization Studio**. It includes the following steps:
1. **Data Input:** Reads CSV files (`Demand.csv`, `Sales.csv`, and `Capacity.csv`) into lists for processing.
2. **Variable Definition:** Uses `INumVar` and `ILinearNumExpr` to define decision variables for production, inventory, sales, and backorders.
3. **Objective Function:** Constructs the objective function by iterating through data and applying the relevant formulas.
4. **Constraints:** Implements constraints (1) through (6) using loops and CPLEX APIs.
5. **Model Solving:** Solves the linear programming model and exports it to `Result.lp`.

## Files in This Repository
* `Program.cs`: Source code for the linear programming model.
* `practice.csproj`: Project configuration file.
* `Demand.csv`: Forecasted demand data.
* `Sales.csv`: Product sales price data.
* `Capacity.csv`: Factory capacity data.
* `Result.lp`: Exported CPLEX model (generated upon execution).
* `README.md`: Project documentation (this file).

## Output Analysis
The solution includes:
* **Total Profit:** Optimized profit value.
* **Production Quantities($X_{ijt}$):** Monthly production plans.
* **Sales Quantities($K_{jt}$):** Monthly sales numbers.
* **Backorders($B_{jt}$):** Monthly backlog numbers.
* **Backorders($I_{jt}$):** Monthly inventory details.