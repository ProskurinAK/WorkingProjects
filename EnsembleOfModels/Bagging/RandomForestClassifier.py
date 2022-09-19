import os
import numpy as np
import pandas as pd
from sklearn.model_selection import train_test_split
from sklearn.preprocessing import StandardScaler
from sklearn.ensemble import RandomForestClassifier
from sklearn import metrics
import matplotlib.pyplot as plt
import seaborn as sns

# функция чтения данных из файла
def read_data(dir_name, first_file, last_file):

    data = []

    files_name = os.listdir(dir_name)
    full_name = []

    for i in np.arange(0, len(files_name)):
        full_name.append(dir_name + '\\' + files_name[i])

    first_row = 157

    for i in np.arange(first_file, last_file):
        file = open(full_name[i])

        count_of_lines = len(open(full_name[i]).readlines())

        for j in np.arange(count_of_lines):
            if (j >= first_row and j < count_of_lines - 2):
                line = file.readline().replace(',', '.')
                line = line.split()
                line.pop(0)

                for k in np.arange(0, len(line)):
                    try:
                        line[k] = float(line[k])
                    except ValueError:
                        line[k] = 0

                data.append(line)
            else:
                line = file.readline()

    return data


first_dir_name = r'D:\Работа\EnsembleOfModels\Bagging\RandomForest\Data\2020'
second_dir_name = r'D:\Работа\EnsembleOfModels\Bagging\RandomForest\Data\2021'

X_train = read_data(first_dir_name, 0, 17)   # 0, 17
zero_train_targets_size = len(X_train)     # первая часть обучающей выборки, где значения скора равны 0
X_train = np.concatenate((X_train, read_data(second_dir_name, 0, 18)), axis=0)    # 0, 18
Y_train = np.concatenate((np.zeros(zero_train_targets_size), np.ones(len(X_train) - zero_train_targets_size)))

# print(X_train)
# print(len(X_train))
# print(type(X_train))
# print(Y_train)
# print(len(Y_train))
# print(type(Y_train))
# print('count of zero target objects - ' + str(zero_train_targets_size))

X_test = read_data(first_dir_name, 17, 35)  # 17, 35
zero_test_targets_size = len(X_test)
X_test = np.concatenate((X_test, read_data(second_dir_name, 18, 36)), axis=0)   # 18,36
Y_test = np.concatenate((np.zeros(zero_test_targets_size), np.ones(len(X_test) - zero_test_targets_size)))

# print(X_test)
# print(len(X_test))
# print(type(X_test))
# print(Y_test)
# print(len(Y_test))
# print(type(Y_test))
# print('count of zero target objects - ' + str(zero_test_targets_size))

# Первичный анализ данных
# ------------------------------------------------------------------------------------------------------
# data_set = pd.DataFrame(data_set)


# предварительный анализ данных
# print(data_set)
# print('--------------------------------------')
# print(data_set.info())
# print('--------------------------------------')
# print(data_set.isna().sum())
# print('--------------------------------------')
# print(data_set.describe())
# ------------------------------------------------------------------------------------------------------
# train_set = X_train.join(Y_train, rsuffix='B')
# train_set = train_set.rename(columns={'0B': 145})
# print(train_set)
#
# X = train_set.drop(145, axis=1)
# Y = train_set[145]
#
# X_train, X_test, Y_train, Y_test = train_test_split(X, Y, test_size=0.5, random_state=0)
# ------------------------------------------------------------------------------------------------------

# масштабирование данных
# --------------------------------------------------------------------------------------
# ss = StandardScaler()
#
# X_train_scaled = ss.fit_transform(X_train)
#
# Y_train = np.array(Y_train).astype(int).ravel()
#
# print(type(X_train_scaled))
# print(type(Y_train))
# --------------------------------------------------------------------------------------

# обучение модели
# --------------------------------------------------------------------------------------
# rfc = RandomForestClassifier(n_estimators=7, random_state=0)
# rfc.fit(X_train_scaled, Y_train)
# print(rfc.score(X_train_scaled, Y_train))
# print(rfc.predict(X_train_scaled))

rfc = RandomForestClassifier(n_estimators=15, random_state=0)
rfc.fit(X_train, Y_train)
print(rfc.score(X_train, Y_train))
print(rfc.predict(X_train))
#
# print('-------------------------------')
#
score = rfc.score(X_test, Y_test)
Y_predict = rfc.predict(X_test)
print('predict - ' + str(Y_predict))
print('Final counter = ' + str(metrics.accuracy_score(Y_test, Y_predict, normalize=False)))
print('Final percent = {}%'.format(score * 100))