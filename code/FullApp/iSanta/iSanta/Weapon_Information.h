//
//  Weapon_Information.h
//  iSanta
//
//  Created by Jack Hall on 1/9/12.
//  Copyright (c) 2012 __MyCompanyName__. All rights reserved.
//

#import <Foundation/Foundation.h>
#import <CoreData/CoreData.h>

@class Test_Report;

@interface Weapon_Information : NSManagedObject

@property (nonatomic, retain) NSString * weapon_Notes;
@property (nonatomic, retain) NSNumber * serial_Number;
@property (nonatomic, retain) NSString * weapon_Nomenclature;
@property (nonatomic, retain) Test_Report *test;

@end