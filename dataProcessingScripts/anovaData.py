import numpy as np
import matplotlib.pyplot as plt
import matplotlib.patches as mpatches
import pandas as pd
from os import listdir
from collections import defaultdict

dir = 'C:/KatseAndmed/'

experiments = [f for f in listdir(dir) if(f.endswith('trial_data.csv'))]

allTrials = None
data = []
data.append(['SUBJECT_ID', 'CONTRAST', 'TEST_SIDE', 'MEAN'])
#Loen kõikide katsete andmed ühte objekti
for i in range(0, len(experiments)):
    e = experiments[i]
    allTrials = pd.read_csv(dir + e, sep=';', index_col=False);
    allTrials = allTrials.loc[allTrials['CPD']=="4cpd"]
    #allTrials = pd.concat([allTrials, fail], ignore_index=True)

    standardLeft = allTrials.loc[allTrials['LEFT_STIMULUS_CONTRAST'] == 0.245000]
    standardLeft = standardLeft.loc[standardLeft['TARGET_SHOWN']==True]
    standardRight = allTrials.loc[allTrials['RIGHT_STIMULUS_CONTRAST'] == 0.245000]
    standardRight = standardRight.loc[standardRight['TARGET_SHOWN']==True]

    testContrasts = standardLeft['RIGHT_STIMULUS_CONTRAST'].unique() #kasutatud testkontrasti väärtused

    #Mitmel protsendil kordadest, kui standardkontrast oli vasakul, vastas katseisik, et parempoolne on kontrastsem
    #Iga erineva testkontrasti väärtuse jaoks arvutan, mitmel protsendil juhtudest kui seda testkontrasti näidati vastas katseisik et test on kontrastsem

    countLeft = defaultdict(int)
    x = standardLeft.loc[standardLeft['LEFT_STIMULUS_ORIENTATION'] == 'left']
    x2 = standardLeft.loc[standardLeft['LEFT_STIMULUS_ORIENTATION'] == 'right']
    y = x.loc[x['QUESTION_2_ANS'] == 'right']
    y2 = x2.loc[x2['QUESTION_2_ANS'] == 'left']
    for cont in testContrasts:
        countLeft[cont] = len(y.loc[y['RIGHT_STIMULUS_CONTRAST'] == cont]) / len(
            standardLeft.loc[standardLeft['RIGHT_STIMULUS_CONTRAST'] == cont])
        countLeft[cont] = countLeft[cont] + len(y2.loc[y2['RIGHT_STIMULUS_CONTRAST'] == cont]) / len(
            standardLeft.loc[standardLeft['RIGHT_STIMULUS_CONTRAST'] == cont])

    for key in countLeft.keys():
        data.append([i, key, 'right', countLeft.get(key)])

    #Mitmel protsendil kordadest, kui standardkontrast oli paremal, vastas katseisik, et vasakpoolne on kontrastsem
    countRight = defaultdict(int)
    x = standardRight.loc[standardRight['LEFT_STIMULUS_ORIENTATION'] == 'left']
    x2 = standardRight.loc[standardRight['LEFT_STIMULUS_ORIENTATION'] == 'right']
    y = x.loc[x['QUESTION_2_ANS'] == 'left']
    y2 = x2.loc[x2['QUESTION_2_ANS'] == 'right']
    for cont in testContrasts:
        countRight[cont] = len(y.loc[y['LEFT_STIMULUS_CONTRAST'] == cont]) / len(
            standardRight.loc[standardRight['LEFT_STIMULUS_CONTRAST'] == cont])
        countRight[cont] = countRight[cont] + len(y2.loc[y2['LEFT_STIMULUS_CONTRAST'] == cont]) / len(
            standardRight.loc[standardRight['LEFT_STIMULUS_CONTRAST'] == cont])

    for key in countRight.keys():
        data.append([i, key, 'left', countRight.get(key)])

np.savetxt("anova_data_Orientation_question_4cpd.csv", data, delimiter=';', fmt='%s') #All of the data for anova was extracted using similar methods