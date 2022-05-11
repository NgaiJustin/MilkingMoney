import os
import pandas

src_file = 'simulate_data/original-2/daily-data.csv'
out_dir = 'simulate_data/processed-2'

# Output seperate CSV for each farm (indicated by groupID)
df = pandas.read_csv(src_file)
groupIDs = df["Group_ID"].unique()
grouped = df.groupby(["Group_ID"])

for groupID in groupIDs:
    group_df = grouped.get_group(groupID)
    group_df = group_df.fillna(0)
    out_path = os.path.join(out_dir, "farm" + str(groupID) + ".csv")
    group_df.to_csv(out_path)
