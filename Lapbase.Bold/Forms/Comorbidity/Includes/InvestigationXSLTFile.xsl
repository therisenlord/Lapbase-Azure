<?xml version="1.0" encoding="utf-8"?>

<xsl:stylesheet version="1.0"
    xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
	xmlns:msxsl="urn:schemas-microsoft-com:xslt" >

<xsl:template match="/">
    <html>
    <body>
		<table class="scrollTable" id="scrollTable" border="0" cellpadding="0" cellspacing="0">
		<tbody>
			<tr>
				<td>
					<div id="corner" class="corner">
						<table border="0" cellpadding="0" cellspacing="0">
						<tbody>
							<tr>
								<th id="cornDate">
									<div style="width: 54px; height: 40px; vertical-align: bottom;" id="dateDiv"><br/><br/>Date</div>
								</th>
								<th id="cornWgt">
									<div style="width: 21px;"><br/><br/>Wgt.</div>
								</th>
								<th id="cornEWL">
									<div style="width: 31px;"><br/><br/>%EWL</div>
								</th>
							</tr>
						</tbody>
						</table>
					</div>
				</td>
				<td>
					<div style="overflow: hidden; width: 756px;" id="headerRow" class="headerRow">
						<table border="0" cellpadding="0" cellspacing="0">
							<tbody>
								<tr>
									<th id="hbc"><div style="height: 40px; width: 23px;">BMR</div></th>
									<th id="hbc"><div style="width: 21px;">Imp.</div></th>
									<th id="hbc"><div style="width: 24px;">Fat%</div></th>
									<th id="hbc"><div style="width: 27px;">Fat<br/>Mass</div></th>
									<th id="hbc"><div style="width: 31px;">Fr. Fat<br/>Mass</div></th>
									<th id="hbc"><div style="width: 28px;">T.B.W<br/>Water</div></th>
									<th id="hdiab">
										<div style="width: 50px;">F. Bl. Sug.</div>
										<div class="range">
											<xsl:for-each select="dsSchema/tblSystemNormals">
												<xsl:if test="Code = 'FBloodGlucose'">
													<xsl:value-of select="MetricLow"/><br/><xsl:value-of select="MetricHigh"/>
												</xsl:if>
											</xsl:for-each>
										</div>
									</th>
									<th id="hdiab">
										<div style="width: 32px;">HbA1c</div>
										<div class="range">
											<xsl:for-each select="dsSchema/tblSystemNormals">
												<xsl:if test="Code = 'HBA1C'">
													<xsl:value-of select="MetricLow"/><br/><xsl:value-of select="MetricHigh"/>
												</xsl:if>
											</xsl:for-each>
										</div>
									</th>
									<th id="hdiab">
										<div style="width: 53px;">F. Ser. Ins.</div>
										<div class="range">
											<br/>
											<xsl:for-each select="dsSchema/tblSystemNormals">
												<xsl:if test="Code = 'FSerumInsulin'">
													<xsl:value-of select="MetricLow"/><xsl:value-of select="MetricHigh"/>
												</xsl:if>
											</xsl:for-each>
										</div>
									</th>

									<th id="hlip">
										<div style="width: 24px;">Trigl.</div>
										<div class="range"><br/>
											<xsl:for-each select="dsSchema/tblSystemNormals">
												<xsl:if test="Code = 'Triglycerides'">
													<xsl:value-of select="MetricLow"/><xsl:value-of select="MetricHigh"/>
												</xsl:if>
											</xsl:for-each>
										</div>
									</th>
									<th id="hlip">
										<div style="width: 45px;">Tot. Chol.</div>
										<div class="range"><br/>
											<xsl:for-each select="dsSchema/tblSystemNormals">
												<xsl:if test="Code = 'TotalCholesterol'">
													<xsl:value-of select="MetricLow"/>
													<xsl:value-of select="MetricHigh"/>
												</xsl:if>
											</xsl:for-each>
										</div>
									</th>
									<th id="hlip">
										<div style="width: 48px;">HDL Chol.</div>
										<div class="range">
											<xsl:for-each select="dsSchema/tblSystemNormals">
												<xsl:if test="Code = 'HDLCholesterol'">
													<xsl:value-of select="MetricLow"/><br/><xsl:value-of select="MetricHigh"/>
												</xsl:if>
											</xsl:for-each>
										</div>
									</th>
									<th id="hlip">
										<div style="width: 47px;">LDL Chol.</div>
										<div class="range"></div>
									</th>

									<th id="hhema">
										<div style="width: 40px;">Hemogl.</div>
										<div class="range">
											<xsl:for-each select="dsSchema/tblSystemNormals">
												<xsl:if test="Code = 'Hemoglobin'">
													<xsl:value-of select="MetricLow"/><br/><xsl:value-of select="MetricHigh"/>
												</xsl:if>
											</xsl:for-each>
										</div>
									</th>
									<th id="hhema">
										<div style="width: 24px;">WCC</div>
										<div class="range">
											<xsl:for-each select="dsSchema/tblSystemNormals">
												<xsl:if test="Code = 'WCC'">
													<xsl:value-of select="MetricLow"/><br/><xsl:value-of select="MetricHigh"/>
												</xsl:if>
											</xsl:for-each>
										</div>
									</th>
									<th id="hhema">
										<div style="width: 43px;">Platelets</div>
										<div class="range">
											<xsl:for-each select="dsSchema/tblSystemNormals">
												<xsl:if test="Code = 'Platelets'">
													<xsl:value-of select="MetricLow"/><br/><xsl:value-of select="MetricHigh"/>
												</xsl:if>
											</xsl:for-each>
										</div>
									</th>
									<th id="hhema">
										<div style="width: 19px;">Iron</div>
										<div class="range">
											<xsl:for-each select="dsSchema/tblSystemNormals">
												<xsl:if test="Code = 'Iron'">
													<xsl:value-of select="MetricLow"/><br/><xsl:value-of select="MetricHigh"/>
												</xsl:if>
											</xsl:for-each>
										</div>
									</th>
									<th id="hhema">
										<div style="width: 35px;">Ferritin</div>
										<div class="range">
											<xsl:for-each select="dsSchema/tblSystemNormals">
												<xsl:if test="Code = 'Ferritin'">
													<xsl:value-of select="MetricLow"/><br/><xsl:value-of select="MetricHigh"/>
												</xsl:if>
											</xsl:for-each>
										</div>
									</th>
									<th id="hhema">
										<div style="width: 47px;">Transferr.</div>
										<div class="range">
											<xsl:for-each select="dsSchema/tblSystemNormals">
												<xsl:if test="Code = 'Transferrin'">
													<xsl:value-of select="MetricLow"/><br/><xsl:value-of select="MetricHigh"/>
												</xsl:if>
											</xsl:for-each>
										</div>
									</th>
									<th id="hhema">
										<div style="width: 17px;">IBC</div>
										<div class="range">
											<xsl:for-each select="dsSchema/tblSystemNormals">
												<xsl:if test="Code = 'IBC'">
													<xsl:value-of select="MetricLow"/><br/><xsl:value-of select="MetricHigh"/>
												</xsl:if>
											</xsl:for-each>
										</div>
									</th>
									<th id="hhema">
										<div style="width: 30px;">Folate</div>
										<div class="range">
											<xsl:for-each select="dsSchema/tblSystemNormals">
												<xsl:if test="Code = 'Folate'">
													<xsl:value-of select="MetricLow"/><br/><xsl:value-of select="MetricHigh"/>
												</xsl:if>
											</xsl:for-each>
										</div>
									</th>
									<th id="hhema">
										<div style="width: 19px;">B12</div>
										<div class="range">
											<xsl:for-each select="dsSchema/tblSystemNormals">
												<xsl:if test="Code = 'B12'">
													<xsl:value-of select="MetricLow"/>
													<br/>
													<xsl:value-of select="MetricHigh"/>
												</xsl:if>
											</xsl:for-each>
										</div>
									</th>

									<th id="hlfts">
										<div style="width: 41px;">Bilirubin</div>
										<div class="range"></div>
									</th>
									<th id="hlfts">
										<div style="width: 50px;">Alk. Phos.</div>
										<div class="range">
											<xsl:for-each select="dsSchema/tblSystemNormals">
												<xsl:if test="Code = 'AlkPhos'">
													<xsl:value-of select="MetricLow"/><br/><xsl:value-of select="MetricHigh"/>
												</xsl:if>
											</xsl:for-each>
										</div>
									</th>
									<th id="hlfts">
										<div style="width: 18px;">ALT</div>
										<div class="range"><br/>
											<xsl:for-each select="dsSchema/tblSystemNormals">
												<xsl:if test="Code = 'ALT'">
													<xsl:value-of select="MetricLow"/><xsl:value-of select="MetricHigh"/>
												</xsl:if>
											</xsl:for-each>
										</div>
									</th>
									<th id="hlfts">
										<div style="width: 19px;">AST</div>
										<div class="range"><br/>
											<xsl:for-each select="dsSchema/tblSystemNormals">
												<xsl:if test="Code = 'AST'">
													<xsl:value-of select="MetricLow"/>
													<xsl:value-of select="MetricHigh"/>
												</xsl:if>
											</xsl:for-each>
										</div>
									</th>
									<th id="hlfts">
										<div style="width: 21px;">GGT</div>
										<div class="range"><br/>
											<xsl:for-each select="dsSchema/tblSystemNormals">
												<xsl:if test="Code = 'GGT'">
													<xsl:value-of select="MetricLow"/>
													<xsl:value-of select="MetricHigh"/>
												</xsl:if>
											</xsl:for-each>
										</div>
									</th>
									<th id="hlfts">
										<div style="width: 40px;">TProtein</div>
										<div class="range">
											<xsl:for-each select="dsSchema/tblSystemNormals">
												<xsl:if test="Code = 'Tprotein'">
													<xsl:value-of select="MetricLow"/><br/><xsl:value-of select="MetricHigh"/>
												</xsl:if>
											</xsl:for-each>
										</div>
									</th>
									<th id="hlfts">
										<div style="width: 40px;">Albumin</div>
										<div class="range">
											<xsl:for-each select="dsSchema/tblSystemNormals">
												<xsl:if test="Code = 'Albumin'">
													<xsl:value-of select="MetricLow"/><br/><xsl:value-of select="MetricHigh"/>
												</xsl:if>
											</xsl:for-each>
										</div>
									</th>

									<th id="helec">
										<div style="width: 18px;">Na</div>
										<div class="range">
											<xsl:for-each select="dsSchema/tblSystemNormals">
												<xsl:if test="Code = 'Sodium'">
													<xsl:value-of select="MetricLow"/>
													<br/>
													<xsl:value-of select="MetricHigh"/>
												</xsl:if>
											</xsl:for-each>
										</div>
									</th>
									<th id="helec">
										<div style="width: 15px;">K</div>
										<div class="range">
											<xsl:for-each select="dsSchema/tblSystemNormals">
												<xsl:if test="Code = 'Potassium'">
													<xsl:value-of select="MetricLow"/>
													<br/>
													<xsl:value-of select="MetricHigh"/>
												</xsl:if>
											</xsl:for-each>
										</div>
									</th>
									<th id="helec">
										<div style="width: 18px;">Cl</div>
										<div class="range">
											<xsl:for-each select="dsSchema/tblSystemNormals">
												<xsl:if test="Code = 'Calcium'">
													<xsl:value-of select="MetricLow"/>
													<br/>
													<xsl:value-of select="MetricHigh"/>
												</xsl:if>
											</xsl:for-each>
										</div>
									</th>
									<th id="helec">
										<div style="width: 28px;">HCO3</div>
										<div class="range">
											<xsl:for-each select="dsSchema/tblSystemNormals">
												<xsl:if test="Code = 'Bicarbonate'">
													<xsl:value-of select="MetricLow"/>
													<br/>
													<xsl:value-of select="MetricHigh"/>
												</xsl:if>
											</xsl:for-each>
										</div>
									</th>
									
									<th id="helec">
										<div style="width: 23px;">Urea</div>
										<div class="range">
											<xsl:for-each select="dsSchema/tblSystemNormals">
												<xsl:if test="Code = 'Urea'">
													<xsl:value-of select="MetricLow"/><br/><xsl:value-of select="MetricHigh"/>
												</xsl:if>
											</xsl:for-each>
										</div>
									</th>
									<th id="helec">
										<div style="width: 50px;">Creatinine</div>
										<div class="range">
											<xsl:for-each select="dsSchema/tblSystemNormals">
												<xsl:if test="Code = 'Creatinine'">
													<xsl:value-of select="MetricLow"/>
													<br/>
													<xsl:value-of select="MetricHigh"/>
												</xsl:if>
											</xsl:for-each>
										</div>
									</th>
									<th id="helec">
										<div style="width: 52px;">H.Cysteine</div>
										<div class="range">
											<xsl:for-each select="dsSchema/tblSystemNormals">
												<xsl:if test="Code = 'Homocysteine'">
													<xsl:value-of select="MetricLow"/><br/><xsl:value-of select="MetricHigh"/>
												</xsl:if>
											</xsl:for-each>
										</div>
									</th>

									<th id="htfts">
										<div style="width: 19px;">TSH</div>
										<div class="range">
											<xsl:for-each select="dsSchema/tblSystemNormals">
												<xsl:if test="Code = 'TSH'">
													<xsl:value-of select="MetricLow"/><br/><xsl:value-of select="MetricHigh"/>
												</xsl:if>
											</xsl:for-each>
										</div>
									</th>
									<th id="htfts">
										<div style="width: 12px;">T4</div>
										<div class="range">
											<xsl:for-each select="dsSchema/tblSystemNormals">
												<xsl:if test="Code = 'T4'">
													<xsl:value-of select="MetricLow"/><br/><xsl:value-of select="MetricHigh"/>
												</xsl:if>
											</xsl:for-each>
										</div>
									</th>
									<th id="htfts">
										<div style="width: 15px;">T3</div>
										<div class="range">
											<xsl:for-each select="dsSchema/tblSystemNormals">
												<xsl:if test="Code = 'T3'">
													<xsl:value-of select="MetricLow"/><br/><xsl:value-of select="MetricHigh"/>
												</xsl:if>
											</xsl:for-each>
										</div>
									</th>

									<th id="hcalc">
										<div style="width: 40px;">Calcium</div>
										<div class="range">
											<xsl:for-each select="dsSchema/tblSystemNormals">
												<xsl:if test="Code = 'Calcium'">
													<xsl:value-of select="MetricLow"/><br/><xsl:value-of select="MetricHigh"/>
												</xsl:if>
											</xsl:for-each>
										</div>
									</th>
									<th id="hcalc">
										<div style="width: 40px;">Phosph.</div>
										<div class="range">
											<xsl:for-each select="dsSchema/tblSystemNormals">
												<xsl:if test="Code = 'Phosphate'">
													<xsl:value-of select="MetricLow"/><br/><xsl:value-of select="MetricHigh"/>
												</xsl:if>
											</xsl:for-each>
										</div>
									</th>
									<th id="hcalc">
										<div style="width: 26px;">Vit. D</div>
										<div class="range">
											<xsl:for-each select="dsSchema/tblSystemNormals">
												<xsl:if test="Code = 'VitD'">
													<xsl:value-of select="MetricLow"/><br/><xsl:value-of select="MetricHigh"/>
												</xsl:if>
											</xsl:for-each>
										</div>
									</th>
								</tr>
							</tbody>
						</table>
					</div>
				</td>
			</tr>
			<tr>
				<td valign="top">
					<div style="overflow: hidden; height: 350px;" id="headerColumn" class="headerColumn">
						<table border="0" cellpadding="0" cellspacing="0">
						<xsl:for-each select="dsSchema/tblInvestigations">
							<tr>
								<td>
									<div style="width: 54px; height: 18px;"><xsl:value-of select="tempDateSeen"/></div>
								</td>
								<td>
									<div style="width: 21px;">
										<xsl:value-of select="round(Weight)"/>
									</div>
								</td>
								<td>
									<div style="width: 31px;">
										<xsl:value-of select="round(EWL)"/>
									</div>
								</td>
							</tr>
						</xsl:for-each>
						</table>
					</div>
				</td>

				<td>
					<div style="overflow: scroll; width: 773px; height: 367px;" id="body" class="body">
						<table border="0" cellpadding="0" cellspacing="0">
						<xsl:for-each select="dsSchema/tblInvestigations">
							<tr>
								<td>
									<div style="width: 23px; height: 18px;">
										<xsl:value-of select="BMR"/>
									</div>
								</td>
								<td>
									<div style="width: 21px;">
										<xsl:value-of select="Impedance"/></div>
								</td>
								<td>
									<div style="width: 24px;">
										<xsl:value-of select="FatPerCent"/></div>
								</td>
								<td>
									<div style="width: 27px;">??</div>
								</td>
								<td>
									<div style="width: 31px;">
										<xsl:value-of select="FreeFatMass"/></div>
								</td>
								<td>
									<div style="width: 28px;">
										<xsl:value-of select="TotalBodyWater"/></div>
								</td>
								<td>
									<div style="width: 50px;">
										<xsl:value-of select="FBloodGlucose"/></div>
								</td>
								<td>
									<div style="width: 32px;">
										<xsl:value-of select="HBA1C"/></div>
								</td>
								<td>
									<div style="width: 53px;">
										<xsl:value-of select="FSerumInsulin"/></div>
								</td>
								<td>
									<div style="width: 24px;">
										<xsl:value-of select="Triglycerides"/></div>
								</td>
								<td>
									<div style="width: 45px;">
										<xsl:value-of select="TotalCholesterol"/></div>
								</td>
								<td>
									<div style="width: 48px;">
										<xsl:value-of select="HDLCholesterol"/></div>
								</td>
								<td>
									<div style="width: 47px;">
										<xsl:value-of select="LDLCholesterol"/></div>
								</td>
								<td>
									<div style="width: 40px;">
										<xsl:value-of select="Hemoglobin"/></div>
								</td>
								<td>
									<div style="width: 24px;">
										<xsl:value-of select="WCC"/></div>
								</td>
								<td>
									<div style="width: 43px;">
										<xsl:value-of select="Platelets"/></div>
								</td>
								<td>
									<div style="width: 19px;">
										<xsl:value-of select="Iron"/></div>
								</td>
								<td>
									<div style="width: 35px;">
										<xsl:value-of select="Ferritin"/></div>
								</td>
								<td>
									<div style="width: 47px;">
										<xsl:value-of select="Transferrin"/></div>
								</td>
								<td>
									<div style="width: 17px;">
										<xsl:value-of select="IBC"/></div>
								</td>
								<td>
									<div style="width: 30px;">
										<xsl:value-of select="Folate"/></div>
								</td>
								<td>
									<div style="width: 19px;">
										<xsl:value-of select="B12"/></div>
								</td>
								<td>
									<div style="width: 41px;">
										<xsl:value-of select="Bilirubin"/></div>
								</td>
								<td>
									<div style="width: 50px;">
										<xsl:value-of select="AlkPhos"/></div>
								</td>
								<td>
									<div style="width: 18px;">
										<xsl:value-of select="ALT"/></div>
								</td>
								<td>
									<div style="width: 19px;">
										<xsl:value-of select="AST"/></div>
								</td>
								<td>
									<div style="width: 21px;">
										<xsl:value-of select="GGT"/></div>
								</td>
								<td>
									<div style="width: 40px;">
										<xsl:value-of select="TProtein"/></div>
								</td>
								<td>
									<div style="width: 40px;">
										<xsl:value-of select="Albumin"/></div>
								</td>
								<td>
									<div style="width: 18px;">
										<xsl:value-of select="Potassium"/></div>
								</td>
								<td>
									<div style="width: 15px;">
										<xsl:value-of select="Chloride"/></div>
								</td>
								<td>
									<div style="width: 18px;">
										<xsl:value-of select="Calcium"/></div>
								</td>
								<td>
									<div style="width: 28px;">
										<xsl:value-of select="Bicarbonate"/></div>
								</td>
								<td>
									<div style="width: 23px;">
										<xsl:value-of select="Urea"/></div>
								</td>
								<td>
									<div style="width: 50px;">
										<xsl:value-of select="Creatinine"/></div>
								</td>
								<td>
									<div style="width: 52px;">
										<xsl:value-of select="Homocysteine"/></div>
								</td>
								<td>
									<div style="width: 19px;">
										<xsl:value-of select="TSH"/></div>
								</td>
								<td>
									<div style="width: 12px;">
										<xsl:value-of select="T4"/></div>
								</td>
								<td>
									<div style="width: 15px;">
										<xsl:value-of select="T3"/></div>
								</td>
								<td>
									<div style="width: 40px;">
										<xsl:value-of select="Calcium"/></div>
								</td>
								<td>
									<div style="width: 40px;">
										<xsl:value-of select="Phosphate"/></div>
								</td>
								<td>
									<div style="width: 26px;">
										<xsl:value-of select="VitD"/></div>
								</td>
							</tr>
						</xsl:for-each>
						</table>
					</div>
				</td>
			</tr>
		</tbody>
		</table>
    </body>
    </html>
</xsl:template>

</xsl:stylesheet> 

