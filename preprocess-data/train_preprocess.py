import random
import pandas

col_list = ['datesql', 'Animal_ID', 'Group_ID', 'Lactation_Num', 'DIM',
            'AnimalStatus', 'Gynecology_Status', 'Yield(gr)', 'ProdRate(gr/hr)',
            'Fat(%)', 'Avg_Fat(%)', 'Protein(%)', 'Avg_Protein(%)', 'Lactose',
            'Avg_Lactose(%)', 'Conductivity', 'Avg_Conductivity', 'Milking_Time(seconds)', 
            'Avg_Milking_Time(seconds)', 'Activity(steps/hr)', 'ActivityDeviation(%)', 
            'RestBout(#)', 'RestPerBout(min)', 'RestRatio(%)', 'RestRestlessness', 
            'RestTime(min)', 'Weight(gr)'
            # 'SCC(*1000/ml)', 'Avg_SCC(*1000/ml)', 'Blood(%)', 'Avg_Blood(%)'
            ]
df = pandas.read_csv("../data/Milk Daily Data.csv", usecols=col_list, parse_dates=['datesql'])

# Label cost estimation
df['monetary_worth'] = df.apply (lambda row: row["ProdRate(gr/hr)"] * row["Avg_Protein(%)"], axis=1)
nonzero_worth = df['monetary_worth'] > 0
df = df[nonzero_worth]

df.to_csv('processed_data.csv')
