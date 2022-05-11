import os
import pandas

src_dir = 'simulate_data/original'
out_dir = 'simulate_data/processed'
total_files = len(os.listdir(src_dir))

# Iterate over CSV in "simulate_data/original"
for f_index, filename in enumerate(os.listdir(src_dir)):
    file_path = os.path.join(src_dir, filename)
    
    # If "animal_activity" is missing, remove entry (known error in that occured during merge)
    # this was in the raw data given, thus the best we can do is preprocess them first.
    # other missing data is fine.
    df = pandas.read_csv(file_path, encoding="ISO-8859-1")
    df.drop(df[df['animal_activity'].isna()].index, inplace=True)
    print("processing ", f_index, " out of ", total_files, " files...")
    
    processed_csv_path = os.path.join(out_dir, "p" + filename)
    df.to_csv(processed_csv_path)
    