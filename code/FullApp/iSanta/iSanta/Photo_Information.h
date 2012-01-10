//
//  Photo_Information.h
//  iSanta
//
//  Created by Jack Hall on 1/9/12.
//  Copyright (c) 2012 __MyCompanyName__. All rights reserved.
//

#import <Foundation/Foundation.h>
#import <CoreData/CoreData.h>

@class Test_Report;

@interface Photo_Information : NSManagedObject

@property (nonatomic, retain) NSData * image;
@property (nonatomic, retain) NSSet *point;
@property (nonatomic, retain) Test_Report *test;
@end

@interface Photo_Information (CoreDataGeneratedAccessors)

- (void)addPointObject:(NSManagedObject *)value;
- (void)removePointObject:(NSManagedObject *)value;
- (void)addPoint:(NSSet *)values;
- (void)removePoint:(NSSet *)values;

@end
