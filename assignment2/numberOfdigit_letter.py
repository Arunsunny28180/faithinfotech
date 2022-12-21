str=input("enter a string")
digit=letter=0
for ch in str:
    if ch.isdigit():
        digit+=1
    elif ch.isalpha():
        letter+=1
    else:
       continue
print("letter:",letter)
print("digit:",digit)