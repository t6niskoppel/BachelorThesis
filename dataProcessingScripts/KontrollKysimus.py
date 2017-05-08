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
for i in range(0, len(experiments)):
    e = experiments[i]
    fail = pd.read_csv(dir+e, sep=';', index_col=False);
    fail['SUBJECT_ID'] = i
    allTrials = pd.concat([allTrials, fail], ignore_index=True)

#print(allTrials)
katse = {}
katse['STANDARD_BEHIND_HAND'] = allTrials['LEFT_STIMULUS_CONTRAST']== 0.245000
katse['LEFT_STIMULUS_CONTRAST'] = allTrials['LEFT_STIMULUS_CONTRAST']
katse['RIGHT_STIMULUS_CONTRAST'] = allTrials['RIGHT_STIMULUS_CONTRAST']

tulp = []
test_side = []
values = katse['STANDARD_BEHIND_HAND'].tolist()
vasak = katse['LEFT_STIMULUS_CONTRAST'].tolist()
parem = katse['RIGHT_STIMULUS_CONTRAST'].tolist()
for i in range(0,len(values)):
    if values[i]:
        tulp.append(parem[i])
        test_side.append("right")
    else:
        tulp.append(vasak[i])
        test_side.append("left")

outPut = pd.DataFrame()
outPut['SUBJECT_ID'] = allTrials['SUBJECT_ID'].tolist()
outPut['TEST_CONT'] = tulp
outPut['TEST_SIDE'] = test_side

#grupp = outPut.groupby(['SUBJECT_ID', 'TEST_CONT', 'TEST_SIDE'])

#for key, item in grupp:
   # print(grupp.get_group(key),"\n\n")
#np.asarray([test_side, tulp]).tofile('kellad.csv', sep=';', format='%10.5f')
#np.savetxt("tulbad.csv", np.c_[test_side, tulp], delimiter=';', fmt='%s')
#print(outPut)


#outPut.to_csv('all_trials.csv', sep=';')

#Kõik trialid kus küsiti kontrollküsimus
allTrials = allTrials.loc[allTrials['QUESTION_2_ASKED']=='orientationQuestion']
allTrials = allTrials.loc[allTrials['TARGET_SHOWN']==True]
print('Kogu täpsus = ' + str(len(allTrials)))

#Valin eraldi 2cpd ja 4cpd trialid kus näidati stiimulit (st kysiti kysimusi ka)
_2cpdTrials = allTrials.loc[allTrials['CPD']=='2cpd']

_4cpdTrials = allTrials.loc[allTrials['CPD']=='4cpd']

_2cpdStandardLeft = _2cpdTrials.loc[_2cpdTrials['LEFT_STIMULUS_CONTRAST'] == 0.245000]

_2cpdStandardRight = _2cpdTrials.loc[_2cpdTrials['RIGHT_STIMULUS_CONTRAST'] == 0.245000]

_4cpdStandardLeft = _4cpdTrials.loc[_4cpdTrials['LEFT_STIMULUS_CONTRAST'] == 0.245000]

_4cpdStandardRight = _4cpdTrials.loc[_4cpdTrials['RIGHT_STIMULUS_CONTRAST'] == 0.245000]

testContrasts = _2cpdStandardLeft['RIGHT_STIMULUS_CONTRAST'].unique() #kasutatud testkontrasti väärtused

#Mitmel protsendil kordadest, kui standardkontrast oli vasakul, vastas katseisik, et parempoolne on kontrastsem
#Iga erineva testkontrasti väärtuse jaoks arvutan, mitmel protsendil juhtudest kui seda testkontrasti näidati vastas katseisik et test on kontrastsem
countLeft = defaultdict(int)
x = _4cpdStandardLeft.loc[_4cpdStandardLeft['LEFT_STIMULUS_ORIENTATION']=='left']
x2 = _4cpdStandardLeft.loc[_4cpdStandardLeft['LEFT_STIMULUS_ORIENTATION']=='right']
y = x.loc[x['QUESTION_2_ANS']=='right']
y2 = x2.loc[x2['QUESTION_2_ANS']=='left']
for cont in testContrasts:
    countLeft[cont] = len(y.loc[y['RIGHT_STIMULUS_CONTRAST']==cont])/len(_4cpdStandardLeft.loc[_4cpdStandardLeft['RIGHT_STIMULUS_CONTRAST']==cont])
    countLeft[cont] = countLeft[cont] + len(y2.loc[y2['RIGHT_STIMULUS_CONTRAST']==cont])/len(_4cpdStandardLeft.loc[_4cpdStandardLeft['RIGHT_STIMULUS_CONTRAST']==cont])



#Mitmel protsendil kordadest, kui standardkontrast oli paremal, vastas katseisik, et vasakpoolne on kontrastsem
countRight = defaultdict(int)
x = _4cpdStandardRight.loc[_4cpdStandardRight['LEFT_STIMULUS_ORIENTATION']=='left']
x2 = _4cpdStandardRight.loc[_4cpdStandardRight['LEFT_STIMULUS_ORIENTATION']=='right']
y = x.loc[x['QUESTION_2_ANS']=='left']
y2 = x2.loc[x2['QUESTION_2_ANS']=='right']
for cont in testContrasts:
    countRight[cont] = len(y.loc[y['LEFT_STIMULUS_CONTRAST']==cont])/len(_4cpdStandardRight.loc[_4cpdStandardRight['LEFT_STIMULUS_CONTRAST']==cont])
    countRight[cont] = countRight[cont] + len(y2.loc[y2['LEFT_STIMULUS_CONTRAST']==cont])/len(_4cpdStandardRight.loc[_4cpdStandardRight['LEFT_STIMULUS_CONTRAST']==cont])

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
x = _2cpdStandardLeft.loc[_2cpdStandardLeft['LEFT_STIMULUS_ORIENTATION']=='left']
x2 = _2cpdStandardLeft.loc[_2cpdStandardLeft['LEFT_STIMULUS_ORIENTATION']=='right']
y = x.loc[x['QUESTION_2_ANS']=='right']
y2 = x2.loc[x2['QUESTION_2_ANS']=='left']
for cont in testContrasts:
    countLeft[cont] = len(y.loc[y['RIGHT_STIMULUS_CONTRAST']==cont])/len(_2cpdStandardLeft.loc[_2cpdStandardLeft['RIGHT_STIMULUS_CONTRAST']==cont])
    countLeft[cont] = countLeft[cont] + len(y2.loc[y2['RIGHT_STIMULUS_CONTRAST']==cont])/len(_2cpdStandardLeft.loc[_2cpdStandardLeft['RIGHT_STIMULUS_CONTRAST']==cont])



#Mitmel protsendil kordadest, kui standardkontrast oli paremal, vastas katseisik, et vasakpoolne on kontrastsem
countRight = defaultdict(int)
x = _2cpdStandardRight.loc[_2cpdStandardRight['LEFT_STIMULUS_ORIENTATION']=='left']
x2 = _2cpdStandardRight.loc[_2cpdStandardRight['LEFT_STIMULUS_ORIENTATION']=='right']
y = x.loc[x['QUESTION_2_ANS']=='left']
y2 = x2.loc[x2['QUESTION_2_ANS']=='right']
for cont in testContrasts:
    countRight[cont] = len(y.loc[y['LEFT_STIMULUS_CONTRAST']==cont])/len(_2cpdStandardRight.loc[_2cpdStandardRight['LEFT_STIMULUS_CONTRAST']==cont])
    countRight[cont] = countRight[cont] + len(y2.loc[y2['LEFT_STIMULUS_CONTRAST']==cont])/len(_2cpdStandardRight.loc[_2cpdStandardRight['LEFT_STIMULUS_CONTRAST']==cont])

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
plt.title("2 cpd")
plt.ylabel("Perceived contrast of test > standard (%)")
plt.xlabel("Contrast of test stimulus (%)")
plt.axis([0, 100, 0, 100])
plt.show()

