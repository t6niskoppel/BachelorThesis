df <- read.csv(file="c:/anova/anova_data_Side_question_4cpd.csv", header=TRUE, sep=";")

colnames(df)
df <- within(df, {
  SUBJECT_ID <- factor(SUBJECT_ID)
  CONTRAST <- factor(CONTRAST)
  TEST_SIDE <- factor(TEST_SIDE)
})

df <- df[order(df$SUBJECT_ID),]

head(df)

results.aov = with(df, aov(MEAN ~ (CONTRAST * TEST_SIDE) + Error(SUBJECT_ID / (CONTRAST * TEST_SIDE))))

summary(results.aov)
#results.aov

#boxplot(df$MEAN ~ df$TEST_SIDE, ylab = "Perceived contrast of test>standard (%)",
#        xlab="Test contrast side, left is behind hand", col=rainbow(2))
#means<- round(tapply(df$MEAN, df$TEST_SIDE, mean), digits=2) 
#points(means, col="black", pch=15)
