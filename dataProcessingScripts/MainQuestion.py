import numpy as np
import matplotlib.pyplot as plt
import matplotlib.patches as mpatches
import pandas as pd
from os import listdir
from collections import defaultdict

dir = 'C:/KatseAndmed/'

experiments = [f for f in listdir(dir) if(f.endswith('trial_data.csv'))]

allTrials = None

#Loen kõikide katsete andmed ühte objekti
for e in experiments:
    fail = pd.read_csv(dir+e, sep=';', index_col=False);
    allTrials = pd.concat([allTrials, fail], ignore_index=True)


#Kogu täpsus
acc = allTrials.get('TARGET_SHOWN').sum()/len(allTrials.get('TARGET_SHOWN'))
print('Kogu täpsus = ' + str(allTrials.get('TARGET_SHOWN').sum()))

#Valin eraldi 2cpd ja 4cpd trialid kus näidati stiimulit (st kysiti kysimusi ka)
_2cpdTrials = allTrials.loc[allTrials['CPD']=='2cpd']
_2cpdTrials = _2cpdTrials.loc[_2cpdTrials['TARGET_SHOWN']==True]

_4cpdTrials = allTrials.loc[allTrials['CPD']=='4cpd']
_4cpdTrials = _4cpdTrials.loc[_4cpdTrials['TARGET_SHOWN']==True]

_2cpdStandardLeft = _2cpdTrials.loc[_2cpdTrials['LEFT_STIMULUS_CONTRAST'] == 0.245000]

_2cpdStandardRight = _2cpdTrials.loc[_2cpdTrials['RIGHT_STIMULUS_CONTRAST'] == 0.245000]

_4cpdStandardLeft = _4cpdTrials.loc[_4cpdTrials['LEFT_STIMULUS_CONTRAST'] == 0.245000]

_4cpdStandardRight = _4cpdTrials.loc[_4cpdTrials['RIGHT_STIMULUS_CONTRAST'] == 0.245000]

testContrasts = _2cpdStandardLeft['RIGHT_STIMULUS_CONTRAST'].unique() #kasutatud testkontrasti väärtused

#Mitmel protsendil kordadest, kui standardkontrast oli vasakul, vastas katseisik, et parempoolne on kontrastsem
#Iga erineva testkontrasti väärtuse jaoks arvutan, mitmel protsendil juhtudest kui seda testkontrasti näidati vastas katseisik et test on kontrastsem
countLeft = defaultdict(int)
rightAnswers = _4cpdStandardLeft.loc[_4cpdStandardLeft['WHAT_SIDE_WAS_MORE_CONTRASTY']=='right']
for cont in testContrasts:
    countLeft[cont] = len(rightAnswers.loc[rightAnswers['RIGHT_STIMULUS_CONTRAST']==cont])/len(_4cpdStandardLeft.loc[_4cpdStandardLeft['RIGHT_STIMULUS_CONTRAST']==cont])

#Mitmel protsendil kordadest, kui standardkontrast oli paremal, vastas katseisik, et vasakpoolne on kontrastsem
countRight = defaultdict(int)
leftAnswers = _4cpdStandardRight  .loc[_4cpdStandardRight['WHAT_SIDE_WAS_MORE_CONTRASTY']=='left']
for cont in testContrasts:
    countRight[cont] = len(leftAnswers.loc[leftAnswers['LEFT_STIMULUS_CONTRAST']==cont])/len(_4cpdStandardRight.loc[_4cpdStandardRight['LEFT_STIMULUS_CONTRAST']==cont])

#Plotin väärtused, enne muudan protsentideks
xValues = []
yValues=[]
keys = sorted(countLeft.keys())
for k in keys:
    xValues.append(k*100)
    yValues.append(countLeft[k]*100)
plt.plot(xValues, yValues, 'bo--')

xValues = []
yValues=[]
keys = sorted(countRight.keys())
for k in keys:
    xValues.append(k * 100)
    yValues.append(countRight[k] * 100)
plt.plot(xValues, yValues, 'ro--')

#Joonise atribuudid
red = mpatches.Patch(color='red', label='Test contrast behind hand')
blue = mpatches.Patch(color='blue', label='Test contrast mirrored')
plt.legend(handles=[blue, red])
plt.title("4 cpd")
plt.ylabel("Perceived contrast of test > standard (%)")
plt.xlabel("Contrast of test stimulus (%)")
plt.axis([0, 100, 0, 100])
plt.show()

#Teen sama asja aga 2cpd'ga. Copy-paste
countLeft = defaultdict(int)
rightAnswers = _2cpdStandardLeft.loc[_2cpdStandardLeft['WHAT_SIDE_WAS_MORE_CONTRASTY']=='right']
for cont in testContrasts:
    countLeft[cont] = len(rightAnswers.loc[rightAnswers['RIGHT_STIMULUS_CONTRAST']==cont])/len(_2cpdStandardLeft.loc[_2cpdStandardLeft['RIGHT_STIMULUS_CONTRAST']==cont])

countRight = defaultdict(int)
leftAnswers = _2cpdStandardRight  .loc[_2cpdStandardRight['WHAT_SIDE_WAS_MORE_CONTRASTY']=='left']
for cont in testContrasts:
    countRight[cont] = len(leftAnswers.loc[leftAnswers['LEFT_STIMULUS_CONTRAST']==cont])/len(_2cpdStandardRight.loc[_2cpdStandardRight['LEFT_STIMULUS_CONTRAST']==cont])


xValues = []
yValues=[]
keys = sorted(countLeft.keys())
for k in keys:
    xValues.append(k*100)
    yValues.append(countLeft[k]*100)
plt.plot(xValues, yValues, 'bo--')

xValues = []
yValues=[]
keys = sorted(countRight.keys())
for k in keys:
    xValues.append(k * 100)
    yValues.append(countRight[k] * 100)
plt.plot(xValues, yValues, 'ro--')

red = mpatches.Patch(color='red', label='Test contrast behind hand')
blue = mpatches.Patch(color='blue', label='Test contrast mirrored')
plt.legend(handles=[blue, red])
plt.title("2 cpd")
plt.ylabel("Perceived contrast of test > standard (%)")
plt.xlabel("Contrast of test stimulus (%)")
plt.axis([0, 100, 0, 100])
plt.show()