//
//  Photo_Information.h
//  iSanta
//
//  Created by  on 5/3/12.
//  Copyright (c) 2012 __MyCompanyName__. All rights reserved.
//

#import <Foundation/Foundation.h>
#import <CoreData/CoreData.h>

@class Test_Report;

@interface Photo_Information : NSManagedObject

@property (nonatomic, retain) NSData * image;
@property (nonatomic, retain) NSData * points;
@property (nonatomic, retain) NSNumber * target_Height;
@property (nonatomic, retain) NSNumber * target_Width;
@property (nonatomic, retain) Test_Report *test;

@end
