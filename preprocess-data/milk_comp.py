import pandas as pd

# Cornell University - Dairy Market Watch
data = {
    pd.Period('2017-10', 'M'): [2.66],
    pd.Period('2017-11', 'M'): [2.34],
    pd.Period('2017-12', 'M'): [2.03],
    pd.Period('2018-1', 'M'): [1.66],
    pd.Period('2018-2', 'M'): [1.62],
    pd.Period('2018-3', 'M'): [1.80],
    pd.Period('2018-4', 'M'): [1.78],
    pd.Period('2018-5', 'M'): [1.86],
    pd.Period('2018-6', 'M'): [1.74],
    pd.Period('2018-7', 'M'): [1.48],
    pd.Period('2018-8', 'M'): [1.62],
    pd.Period('2018-9', 'M'): [2.00],
    pd.Period('2018-10', 'M'): [1.72],
}

pd.DataFrame(data).to_csv("milk_component_price.csv", index=False)