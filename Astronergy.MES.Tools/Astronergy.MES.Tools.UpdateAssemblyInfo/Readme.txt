UpdateAssemblyInfo.exe是用来自动向相关文件写入版本信息的工具。
它通过使用全局应用程序模板文件、自定义模板文件和源代码控制向需要包含版本信息的相关文件写入版本信息。

（1）全局应用程序模板必须是Templates/GlobalAssemblyInfo.template的格式。
（2）自定义模板文件可以根据自己需要进行定制。
	您可以参考Templates目录下的模板文件制定自己的模板文件，模板文件中可使用的替换标识如下：

	$INSERTVERSION$					：	版本号，完整版本号
	$INSERTMAJORVERSION$			：	主版本号
	$INSERTREVISION$				：	修订版版本号
	$INSERTCOMMITHASH$				：	源代码控制中的提交标识符
	$INSERTSHORTCOMMITHASH$			：	源代码控制中的提交标识符短码
	$INSERTDATE$					：	编译时日期
	$INSERTYEAR$					：  编译时年份
	$INSERTBRANCHNAME$				：  分支名
	$INSERTBRANCHPOSTFIX$			：  分支名作为后缀，"-分支名"
	$INSERTVERSIONNAME$				：  版本名
	$INSERTVERSIONNAMEPOSTFIX$		：  版本名作为后缀，"-版本名"

（3）源代码控制目前只支持svn和git
