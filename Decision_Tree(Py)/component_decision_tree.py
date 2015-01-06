import math

class  emptyTree(object):
	"""docstring for  emptyTree"""
	def isEmpty(self):
		return True
	def inorder(self):
		return
	def __iter__(self):
		return iter([])


class  tree(object):
	"""docstring for  tree"""
	emptyNode = emptyTree()
	def __init__(self, item):
		self._root = item
		self._left = tree.emptyNode
		self._right = tree.emptyNode
	def isEmpty(self):
		return False
	def getRoot(self):
		return self._root
	def getLeft(self):
		return self._left
	def getRight(self):
		return self._right
	def setRoot(self,item):
		self._root = item
	def setLeft(self,item):
		self._left = tree(item)
	def setRight(self,item):
		self._right = tree(item)
	def linkLeft(self,tree):
		self._left = tree
	def linkRight(self,tree):
		self._right = tree
	def removeLeft(self):
		tempLeft = self._left
		self._left = tree.emptyNode
		return tempLeft
	def removeRight(self):
		tempRight = self._right
		self._right = tree.emptyNode
		return tempRight
	def __iter__(self):
		lyst = []
		self.inorder(lyst)
		return lyst
	def inorder(self,lyst):
		if not self.getLeft().isEmpty():
			self.getLeft().inorder(lyst)
		lyst.append(self.getRoot())
		if not self.getRight().isEmpty():
			self.getRight().inorder(lyst)
	def __getAllLeaf__(self):
		listOfLeaf = []
		self.getLeaf(listOfLeaf)
		return listOfLeaf
	def getLeaf(self,listOfLeaf):
		if not self.getLeft().isEmpty():
			self.getLeft().getLeaf(listOfLeaf)
		if self.getLeft().isEmpty() and self.getRight().isEmpty():
			listOfLeaf.append(self.getRoot())
		if not self.getRight().isEmpty():
			self.getRight().getLeaf(listOfLeaf)


# a cycle of decision tree usually do
def classify(propertyName,sample):
	if not sample[0].has_key(propertyName):
		print " no property named like " + str(propertyName)
		return 0
	classifyTree = tree(sample)
	leftSpace = []
	rightSpace = []
	for single in sample:
		if single[propertyName] == 1:
			leftSpace.append(single)
		elif single[propertyName] == 0:
			rightSpace.append(single)
	#print leftSpace
	#print 'left'
	#print rightSpace
	#print 'right' ###test
	leftSonTree = tree(leftSpace)
	rightSonTree = tree(rightSpace)
	classifyTree.linkLeft(leftSonTree)
	classifyTree.linkRight(rightSonTree)
	return classifyTree

def getPropList_PrePro(dataInput):
	exampleData = dataInput[0] # exampleData is a dictionary like {'result':0,'prop1':1,...}
	exampleDataList = exampleData.keys()
	propList  = exampleDataList[1:] #always put the example result as first place
	return propList

def entropy(sample):#entropy calculate
	sampleEntropy = 0
	sampleNum = len(sample)
	sampleResultSpace = {0:0,1:0}
	for x in xrange(0,sampleNum):
		if sample[x]['result'] == 1:
			sampleResultSpace[1] += 1
		else:
			sampleResultSpace[0] += 1
	if sampleResultSpace[1] == 0 or sampleResultSpace[1] == sampleNum:
		return 0.0
	for singResult in sampleResultSpace.keys():
		singETemp = float(sampleResultSpace[singResult])/float(sampleNum)
		singTemp = singETemp * math.log(singETemp,2)
		sampleEntropy -= singTemp
	return sampleEntropy

def gainInformation(propertyName,sample):#gainInf calculate
	GainInf = entropy(sample)
	propertySpace = {}
	for x in sample:
		if x[propertyName] not in propertySpace:
			propertySpace[x[propertyName]] = 1
		else:
			propertySpace[x[propertyName]] += 1
	newSample = []
	for a in propertySpace.keys():
		for x in sample:
			if x[propertyName] == a:
				newSample.append(x)
		newSETemp = entropy(newSample) * float(propertySpace[a]) / float(len(sample))
		newSample = []
		GainInf -= newSETemp
	return GainInf

def bestPropChoose_PrePro(propList,dataInput):
	gainInfMax = 0.0
	bestProp = propList[0];
	for prop in propList:
		if gainInformation(prop,dataInput) >= gainInfMax:
			bestProp = prop

	return bestProp


def classifyPart(sample,propList):
	bestProperty = bestPropChoose_PrePro(propList,sample)
	#print bestProperty # a test
	classifyTree = classify(bestProperty,sample)
	return classifyTree



def recyclePart(dataTree,limitEntropy,propList):
	if not dataTree.getLeft().isEmpty():
		recyclePart(dataTree.getLeft(),limitEntropy,propList)
	if dataTree.getLeft().isEmpty() and dataTree.getRight().isEmpty():
		sample = dataTree.getRoot()
		entropyOfSample = entropy(sample)
		if entropyOfSample >= limitEntropy:
			newTree = classifyPart(sample,propList)
			dataTree.linkLeft(newTree.getLeft())
			dataTree.linkRight(newTree.getRight())
		else:
			return 0
	if not dataTree.getRight().isEmpty():
		recyclePart(dataTree.getRight(),limitEntropy,propList)



