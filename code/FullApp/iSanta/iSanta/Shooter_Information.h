//
//  Shooter_Information.h
//  iSanta
//
//  Created by Jack Hall on 1/9/12.
//  Copyright (c) 2012 __MyCompanyName__. All rights reserved.
//

#import <Foundation/Foundation.h>
#import <CoreData/CoreData.h>

@class Test_Report;

@interface Shooter_Information : NSManagedObject

@property (nonatomic, retain) NSString * first_Name;
@property (nonatomic, retain) NSString * last_Name;
@property (nonatomic, retain) Test_Report *test;

@end
