def findpal(n):
    org=n
    rev=0
    while(n>0):
        r=n%10
        rev=(rev*10)+r
        n=n//10
    if(org==rev):
        print("it is a palindrom")
    else:
        print("not a palindrom")
n=int(input("enter a number:"))
findpal(n)